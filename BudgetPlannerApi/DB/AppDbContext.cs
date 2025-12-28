using BudgetPlannerApi.DB.Models.Address;
using BudgetPlannerApi.DB.Models.User;
using BudgetPlannerApplication_2025.Models;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using BudgetPlannerApi.DB.Models;

namespace Bpst.API.DB
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.EngagementRoles();
            modelBuilder.SeedCountry();
            modelBuilder.SeedState();
            modelBuilder.SeedCities();
            modelBuilder.SeedCategory();
            //        modelBuilder.Entity<ExpensePlan>()
            // .HasOne(c => c.ParentExpensePlan)
            // .WithMany(c => c.SubExpensePlans)
            //                .HasForeignKey(c => c.ParentId)
            //              .OnDelete(DeleteBehavior.Restrict); // avoids circular delete errors


            // 👉 Relationship with AppUser - keep cascade if you want
            modelBuilder.Entity<BudgetPlan>()
                .HasOne(bp => bp.AppUser)
                .WithMany()
                .HasForeignKey(bp => bp.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // 👉 Relationship with Category - STOP cascade here
            modelBuilder.Entity<BudgetPlan>()
                .HasOne(bp => bp.Category)
                .WithMany()
                .HasForeignKey(bp => bp.CategoryId)
                .OnDelete(DeleteBehavior.Restrict); // 🔥 IMPORTANT: This removes the cascade delete

            modelBuilder.Entity<Expense>()
                .HasOne(e => e.AppUser)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Keep cascade here

            modelBuilder.Entity<Expense>()
                .HasOne(e => e.ExpensePlan)
                .WithMany()
                .HasForeignKey(e => e.ExpensePlanId)
                .OnDelete(DeleteBehavior.Restrict); // Remove cascade here like BudgetPlan

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Support design-time and environment overrides via environment variables
                var conn = Environment.GetEnvironmentVariable("ConnectionStrings__LiveDB")
                           ?? Environment.GetEnvironmentVariable("LiveDB")
                           ?? Environment.GetEnvironmentVariable("MSSQL_CONN");

                if (!string.IsNullOrEmpty(conn))
                {
                    // If the connection string looks like MySQL (contains Uid/Uid= or Pwd or 'Server=MYSQL'), use MySQL provider
                    var lower = conn.ToLowerInvariant();
                    if (lower.Contains("uid=") || lower.Contains("user id=") || lower.Contains("pwd=") || lower.Contains("mysql"))
                    {
                        optionsBuilder.UseMySql(conn, ServerVersion.AutoDetect(conn));
                    }
                    else
                    {
                        optionsBuilder.UseSqlServer(conn);
                    }
                }
            }
        }

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Roles> Roles { get; set; }

        public DbSet<Country> Country { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Address> Addresses { get; set; }

        public DbSet<BudgetPlan> BudgetPlans { get; set; }
        public DbSet<ExpensePlan> ExpensePlans { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<WishList> WishLists { get; set; }
        public DbSet<IncomeSource> IncomeSource { get; set; } = default!;
    }
}
