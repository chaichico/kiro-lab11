using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Amazon.LocationService;
using Amazon.LocationService.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using wildrydes.net.Context;
using wildrydes.net.Models;

namespace wildrydes.net.Controllers;

public class RideController : Controller
{
    private readonly DefaultContext _db;
    private readonly IAmazonLocationService _locationClient;
    private readonly AwsSettings _awsSettings;

    public RideController(DefaultContext db, IAmazonLocationService locationClient, IOptions<AwsSettings> awsSettings)
    {
        _db = db;
        _locationClient = locationClient;
        _awsSettings = awsSettings.Value;
    }

    [Authorize]
    public IActionResult Map()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateRide(double pickupLat, double pickupLng, string pickupAddress,
                               double destLat, double destLng, string destAddress, int passengers)
    {
        try
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(userEmail))
            {
                return Json(new { success = false, error = "User not logged in" });
            }

            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
            if (user == null)
            {
                return Json(new { success = false, error = "User not found" });
            }

            var unicorn = await _db.Unicorns
                .OrderBy(x => EF.Functions.Random())
                .FirstOrDefaultAsync();

            var ride = new RideModel
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                UnicornId = unicorn!.Id,
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

            _db.Rides.Add(ride);
            await _db.SaveChangesAsync();
            return Json(new { success = true, rideId = ride.Id, unicornName = unicorn.Name, unicornDesc = unicorn.Description, unicornColor = unicorn.Color });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, error = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> ReverseGeocode(double lat, double lng)
    {
        try
        {
            var placeIndexName = _awsSettings.LocationService.PlaceIndexName;
            if (string.IsNullOrEmpty(placeIndexName))
                placeIndexName = "WildRydesPlaceIndex";

            var request = new SearchPlaceIndexForPositionRequest
            {
                IndexName = placeIndexName,
                Position = new List<double> { lng, lat },
                MaxResults = 1
            };

            var response = await _locationClient.SearchPlaceIndexForPositionAsync(request);

            if (response.Results?.Count > 0)
            {
                var result = response.Results[0];
                var address = result.Place.Label ?? $"{lat:F4}, {lng:F4}";
                return Json(new { success = true, address });
            }
            else
            {
                return Json(new { success = false, address = $"{lat:F4}, {lng:F4}" });
            }
        }
        catch (AccessDeniedException)
        {
            return Json(new { success = false, address = $"{lat:F4}, {lng:F4}", error = "Access denied to AWS Location Service" });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, address = $"{lat:F4}, {lng:F4}", error = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetMapTile(int z, int x, int y)
    {
        try
        {
            var request = new GetMapTileRequest
            {
                MapName = _awsSettings.LocationService.MapName,
                X = x.ToString(),
                Y = y.ToString(),
                Z = z.ToString()
            };
            var response = await _locationClient.GetMapTileAsync(request);

            byte[] tileData;
            await using (var memoryStream = response.Blob)
            {
                tileData = memoryStream.ToArray();
            }

            Response.Headers.CacheControl = "public, max-age=3600";
            return File(tileData, "image/png");
        }
        catch (AccessDeniedException)
        {
            return StatusCode(403);
        }
        catch
        {
            return NotFound();
        }
    }

    [Authorize]
    public async Task<IActionResult> Index()
    {
        var userEmail = User.FindFirstValue(ClaimTypes.Email);
        if (string.IsNullOrEmpty(userEmail))
        {
            return RedirectToAction("Login", "User");
        }

        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
        if (user == null)
        {
            return RedirectToAction("Login", "User");
        }

        var rideModels = await _db.Rides
            .Include(r => r.Unicorn)
            .Include(r => r.User)
            .Where(r => r.UserId == user.Id)
            .OrderBy(r => r.DateTime)
            .ToListAsync();

        return View(rideModels);
    }
}
