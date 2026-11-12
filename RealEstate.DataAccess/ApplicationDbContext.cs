using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RealEstate.Models;

namespace RealEstate.DataAccess
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<City> Cities { get; set; }
        public DbSet<PropertyType> PropertyTypes { get; set; }
        public DbSet<Amenity> Amenities { get; set; }
        public DbSet<Neighbourhood> Neighbourhoods { get; set; }
        public DbSet<ContactInfo> ContactInfos { get; set; }
        public DbSet<Property> Properties { get; set; }

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            base.OnModelCreating(modelbuilder);

            modelbuilder.Entity<Neighbourhood>().HasOne(n => n.City)
                                                .WithMany()
                                                .HasForeignKey(n => n.CityId)
                                                .OnDelete(DeleteBehavior.Restrict);
            modelbuilder.Entity<Property>().HasOne(p => p.PropertyType)
                                           .WithMany()
                                           .HasForeignKey(p => p.PropertyTypeId)
                                           .OnDelete(DeleteBehavior.Restrict);

            modelbuilder.Entity<Property>().HasOne(p => p.City)
                                           .WithMany()
                                           .HasForeignKey(p => p.CityId)
                                           .OnDelete(DeleteBehavior.Restrict);

            modelbuilder.Entity<Property>().HasOne(p => p.Neighbourhood)
                                           .WithMany()
                                           .HasForeignKey(p => p.NeighbourhoodId)
                                           .OnDelete(DeleteBehavior.Restrict);

            modelbuilder.Entity<Property>().HasIndex(p => p.ReferenceCode).IsUnique();
        }


    }
}
