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

            modelBuilder.SeedRoles();
            modelBuilder.SeedApplicationStatus();
            modelBuilder.SeedCourseCategory();
            modelBuilder.Entity<AppUser>().ToTable("AppUser");

            modelBuilder.SeedCountry();
            modelBuilder.SeedState();
            modelBuilder.SeedCities();
 
        }
        public DbSet<Application> Applications { get; set; } = null!;

        public DbSet<Contact> Contacts { get; set; } = null!;
       // public DbSet<Batch> Batchs { get; set; }
        //public DbSet<BatchStudent> BatchStudents { get; set; }
        //public DbSet<Qualification> Qualification { get; set; }
        //public DbSet<Course> Courses { get; set; } 
        //public DbSet<StudentApplication> Applications { get; set; }
        public DbSet<ApplicationStatus> ApplicationStatus { get; set; } = null!;
        //public DbSet<Subject> Subjects { get; set; }
        //public DbSet<Question> Questions { get; set; }
        //public DbSet<CodeHelper> CodeHelpers { get; set; }
        //public DbSet<StudentFee> Fees { get; set; }
        //public DbSet<Employees> Employees { get; set; }
        //public DbSet<Student> Students { get; set; }


        /////////////////////////////////////////////////////////////////////////////////////
        //public DbSet<EducationBoardsMaster> EducationBoards { get; set; }
        //public DbSet<Country> Countries { get; set; }
        //public DbSet<State> States { get; set; }
        //public DbSet<City> Cities { get; set; }
        //public DbSet<Address> Addresses { get; set; }
        //public DbSet<Enquiry> Enquiries { get; set; }

    }
}
