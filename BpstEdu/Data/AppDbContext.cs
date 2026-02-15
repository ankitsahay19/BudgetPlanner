using BpstEdu.DBModels;
using BpstEdu.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BpstEdu.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder); 
            modelBuilder.Entity<AppUser>().ToTable("AppUser");
            modelBuilder.SeedRoles();
            modelBuilder.SeedCourseCategory(); 
            modelBuilder.SeedCountry();
            modelBuilder.SeedState();
            modelBuilder.SeedCities();
        }
        public DbSet<Application> Applications { get; set; } = null!;

        public DbSet<Contact> Contacts { get; set; } = null!;
        public DbSet<Course> Courses { get; set; } = null!;
        public DbSet<ApplicationStatus> ApplicationStatus { get; set; } = null!;
        public DbSet<State> States { get; set; }
        public DbSet<City> Cities { get; set; }

    }
}
