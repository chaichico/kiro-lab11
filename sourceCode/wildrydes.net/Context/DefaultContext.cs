using wildrydes.net.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace wildrydes.net.Context
{
    public class DefaultContext : DbContext
    {

        // default constructor
        public DefaultContext() : base("name=DefaultConnection") {}

        // data sets
        public DbSet<UserModel> Users { get; set; }
        public DbSet<UnicornModel> Unicorns { get; set; }
        public DbSet<RideModel> Rides { get; set; }

        // can override and customize settings on creations of a model
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}