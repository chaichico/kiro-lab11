using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace wildrydes.net.Models;

[Table("Rides")]
public class RideModel
{
    public Guid Id { get; set; }

    [ForeignKey("Unicorn")]
    public Guid UnicornId { get; set; }
    public virtual UnicornModel Unicorn { get; set; } = null!;

    [ForeignKey("User")]
    public Guid UserId { get; set; }
    public virtual ApplicationUser User { get; set; } = null!;

    public DateTime DateTime { get; set; }

    [Display(Name = "Pickup Location")]
    public Location PickupLocation { get; set; } = null!;

    [Display(Name = "Destination Location")]
    public Location DestinationLocation { get; set; } = null!;

    [Display(Name = "Passengers")]
    public int NumberOfPassengers { get; set; } = 1;

    [Display(Name = "Special Requests")]
    public string? SpecialRequests { get; set; }

    [Display(Name = "Estimated Arrival")]
    public DateTime EstimatedArrival { get; set; }

    [Display(Name = "Estimated Distance")]
    public double EstimatedDistance { get; set; }

    [Display(Name = "Estimated Duration")]
    public int EstimatedDuration { get; set; }

    public string Status { get; set; } = "Pending";

    public Rating? Rating { get; set; }
}

public enum Rating
{
    One = 1,
    Two = 2,
    Three = 3,
    Four = 4,
    Five = 5
}

public class Location
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string? Address { get; set; }
}
