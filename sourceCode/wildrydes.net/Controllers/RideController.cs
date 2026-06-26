using System;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Amazon;
using Amazon.CognitoIdentity;
using Amazon.LocationService;
using Amazon.LocationService.Model;
using wildrydes.net.Context;
using wildrydes.net.Models;
using wildrydes.net.Decorators;
using System.Web;
using System.Collections.Generic;

namespace wildrydes.net.Controllers
{
    public class RideController : Controller
    {
        private DefaultContext db = new DefaultContext();
        private AmazonLocationServiceClient _locationClient;
        private string _mapName;

        public RideController()
        {
            var region = RegionEndpoint.GetBySystemName(ConfigurationManager.AppSettings["AWS.Region"]);
            var identityPoolId = ConfigurationManager.AppSettings["AWS.CognitoIdentityPoolId"];
            var cognitoCredentials = new CognitoAWSCredentials(identityPoolId, region);
            _locationClient = new AmazonLocationServiceClient(cognitoCredentials, region);
            _mapName = ConfigurationManager.AppSettings["AWS.LocationService.MapName"];
        }

        [Protected]
        public ActionResult Map()
        {
            return View();
        }

        [HttpPost]
        public JsonResult CreateRide(double pickupLat, double pickupLng, string pickupAddress, 
                                   double destLat, double destLng, string destAddress, int passengers)
        {
            try
            {
                var userEmail = Session["user"] as string;
                if (string.IsNullOrEmpty(userEmail))
                {
                    return Json(new { success = false, error = "User not logged in" });
                }

                var user = db.Users.FirstOrDefault(u => u.Email == userEmail);
                if (user == null)
                {
                    return Json(new { success = false, error = "User not found" });
                }

                Random rand = new Random();
                var skip = rand.Next(0, db.Unicorns.Count());
                var unicorn = db.Unicorns.OrderBy(x => Guid.NewGuid())
                                .Skip(skip)
                                .FirstOrDefault();

                var ride = new RideModel
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    UnicornId = unicorn.Id,
                    DateTime = DateTime.Now,
                    PickupLocation = new Location
                    {
                        Latitude = pickupLat,
                        Longitude = pickupLng,
                        Address = pickupAddress
                    },
                    DestinationLocation = new Location
                    {
                        Latitude = destLat,
                        Longitude = destLng,
                        Address = destAddress
                    },
                    NumberOfPassengers = passengers,
                    EstimatedArrival = DateTime.Now.AddMinutes(15),
                    EstimatedDistance = 0,
                    EstimatedDuration = 15,
                    Status = "Requested"
                };

                db.Rides.Add(ride);
                db.SaveChanges();
                return Json(new { success = true, rideId = ride.Id, unicornName = unicorn.Name, unicornDesc = unicorn.Description, unicornColor = unicorn.Color });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"DB Insert error: {ex.Message} - {ex.StackTrace}");
                return Json(new { success = false, error = ex.Message });
            }
        }


        [HttpPost]
        public async Task<JsonResult> ReverseGeocode(double lat, double lng)
        {
            try
            {
                var placeIndexName = ConfigurationManager.AppSettings["AWS.LocationService.PlaceIndexName"] ?? "WildRydesPlaceIndex";
                
                var request = new SearchPlaceIndexForPositionRequest
                {
                    IndexName = placeIndexName,
                    Position = new List<double> { lng, lat }, // AWS expects [longitude, latitude]
                    MaxResults = 1
                };
                
                var response = await _locationClient.SearchPlaceIndexForPositionAsync(request);
                
                if (response.Results?.Count > 0)
                {
                    var result = response.Results[0];
                    var address = result.Place.Label ?? $"{lat:F4}, {lng:F4}";
                    return Json(new { success = true, address = address });
                }
                else
                {
                    return Json(new { success = false, address = $"{lat:F4}, {lng:F4}" });
                }
            }
            catch (Amazon.LocationService.Model.AccessDeniedException ex)
            {
                return Json(new { success = false, address = $"{lat:F4}, {lng:F4}", error = "Access denied to AWS Location Service" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, address = $"{lat:F4}, {lng:F4}", error = ex.Message });
            }
        }
        


        [HttpGet]
        public async Task<ActionResult> GetMapTile(int z, int x, int y)
        {
            try
            {
                var request = new GetMapTileRequest
                {
                    MapName = _mapName,
                    X = x.ToString(),
                    Y = y.ToString(),
                    Z = z.ToString()
                };
                var response = await _locationClient.GetMapTileAsync(request);
                
                byte[] tileData;
                using (var memoryStream = response.Blob)
                {
                    tileData = memoryStream.ToArray();
                }
                
                // AWS Location Service returns application/octet-stream but it's actually image data
                // Force the content type to image/png for browser compatibility
                Response.ContentType = "image/png";
                Response.Cache.SetCacheability(HttpCacheability.Public);
                Response.Cache.SetMaxAge(TimeSpan.FromHours(1));
                
                return File(tileData, "image/png");
            }
            catch (Amazon.LocationService.Model.AccessDeniedException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
        }

        // GET: Ride
        [Protected]
        public ActionResult Index()
        {
            var userEmail = Session["user"] as string;
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Login", "User");
            }
            var user = db.Users.FirstOrDefault(u => u.Email == userEmail);
            var rideModels = db.Rides.Include(r => r.Unicorn)
                                .Include(r => r.User)
                                .Where(r => r.UserId == user.Id)
                                .OrderBy(r => r.DateTime);
            return View(rideModels.ToList());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
                _locationClient?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
