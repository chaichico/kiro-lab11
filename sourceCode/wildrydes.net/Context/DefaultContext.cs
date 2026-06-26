using Microsoft.EntityFrameworkCore;
using wildrydes.net.Models;

namespace wildrydes.net.Context;

public class DefaultContext : DbContext
{
    public DefaultContext(DbContextOptions<DefaultContext> options) : base(options) { }

    public DbSet<UserModel> Users { get; set; }
    public DbSet<UnicornModel> Unicorns { get; set; }
    public DbSet<RideModel> Rides { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RideModel>(entity =>
        {
            entity.OwnsOne(r => r.PickupLocation);
            entity.OwnsOne(r => r.DestinationLocation);
        });

        // Seed data
        modelBuilder.Entity<UnicornModel>().HasData(
            new UnicornModel
            {
                Id = Guid.Parse("d4bb97cd-f5e9-4e98-bc59-a21241aefdbc"),
                Name = "Bucephalus",
                Color = "Gold",
                Description = "Bucephalus joined Wild Rydes in February 2016 and has been giving rydes almost daily. He says he most enjoys getting to know each of his ryders, which makes the job more interesting for him. In his spare time, Bucephalus enjoys watching sunsets and playing Pokemon Go.",
                Rating = 4
            },
            new UnicornModel
            {
                Id = Guid.Parse("eef1efdd-552a-4e1d-8287-d104c2925572"),
                Name = "Shadowfox",
                Color = "White",
                Description = "Shadowfox joined Wild Rydes after completing a distinguished career in the military, where he toured the world in many critical missions. Shadowfox enjoys impressing his ryders with magic tricks that he learned from his previous owner.",
                Rating = 5
            },
            new UnicornModel
            {
                Id = Guid.Parse("5dcf9469-588f-4d80-9183-eca28c8d0e2a"),
                Name = "Rocinante",
                Color = "Brown",
                Description = "Rocinante recently joined the Wild Rydes team in Madrid, Spain. She was instrumental in forming Wild Rydes' Spanish operations after a long, distinguished acting career in windmill shadow-jousting.",
                Rating = 4
            }
        );

        modelBuilder.Entity<UserModel>().HasData(
            new UserModel
            {
                Id = Guid.Parse("4aff9469-588f-4a80-9183-eca28c8d0f7d"),
                Email = "user@unicornrides.aws",
                Password = "b03ddf3ca2e714a6548e7495e2a03f5e824eaac9837cd7f159c67b90fb4b7342" // SHA256 of "Passw0rd"
            }
        );
    }
}
