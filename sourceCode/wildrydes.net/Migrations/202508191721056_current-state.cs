namespace wildrydes.net.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class currentstate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Rides",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UnicornId = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                        DateTime = c.DateTime(nullable: false),
                        PickupLocation_Latitude = c.Double(nullable: false),
                        PickupLocation_Longitude = c.Double(nullable: false),
                        PickupLocation_Address = c.String(),
                        DestinationLocation_Latitude = c.Double(nullable: false),
                        DestinationLocation_Longitude = c.Double(nullable: false),
                        DestinationLocation_Address = c.String(),
                        NumberOfPassengers = c.Int(nullable: false),
                        SpecialRequests = c.String(),
                        EstimatedArrival = c.DateTime(nullable: false),
                        EstimatedDistance = c.Double(nullable: false),
                        EstimatedDuration = c.Int(nullable: false),
                        Status = c.String(),
                        Rating = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Unicorns", t => t.UnicornId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UnicornId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Unicorns",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false),
                        Color = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        Rating = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Email = c.String(nullable: false),
                        Password = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Rides", "UserId", "dbo.Users");
            DropForeignKey("dbo.Rides", "UnicornId", "dbo.Unicorns");
            DropIndex("dbo.Rides", new[] { "UserId" });
            DropIndex("dbo.Rides", new[] { "UnicornId" });
            DropTable("dbo.Users");
            DropTable("dbo.Unicorns");
            DropTable("dbo.Rides");
        }
    }
}
