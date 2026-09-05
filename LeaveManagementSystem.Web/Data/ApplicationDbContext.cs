using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<IdentityRole>().HasData(

                new IdentityRole
                {
                    Id = "8edd273b-c9dd-4849-8892-240fd61a9dca",
                    Name = "Employee",
                    NormalizedName = "EMPLOYEE",
                    ConcurrencyStamp = "172ad0f4-02c2-422a-9896-94490cd1a0c2"
                },

                new IdentityRole 
                {
                    Id = "418e3361-c56a-4d22-b825-8953e615762a",
                    Name = "Supervisor", 
                    NormalizedName = "SUPERVISOR",
                    ConcurrencyStamp = "88a2a3a2-842d-4552-a48c-fba06715158b"
                },

                new IdentityRole
                {
                    Id = "b84229b8-ca2e-4423-9610-52c3c7e177bf",
                    Name = "Administrator",
                    NormalizedName = "ADMINISTRATOR",
                    ConcurrencyStamp = "9fc2c7e6-2bb4-4974-8f5e-cf90b16f09b4"
                }


            );

            //var hasher = new PasswordHasher<ApplicationUser>();
            builder.Entity<ApplicationUser>().HasData(new ApplicationUser
                {
                    Id = "f8e1c3b0-5d4e-4f9a-9c6b-2e1f3c4d5e6f",
                    UserName = "admin@example.com",
                    NormalizedUserName = "ADMIN@EXAMPLE.COM",
                    Email = "admin@example.com",
                    NormalizedEmail = "ADMIN@EXAMPLE.COM",
                    EmailConfirmed = true,
                    PasswordHash = "AQAAAAIAAYagAAAAEIxW2R98V3x5AWuqNCMJhOm+/COLTAQSPrQLMy43XLclF00+E0827xHmWLTH2E9LeQ==",
                    //PasswordHash = hasher.HashPassword(null, "Admin@123"),
                    SecurityStamp = "AQAAAAEAACcQAAAAEJ2a8b9c0d1e2f3g4h5i6j7k8l9m0n1o2p3q4r5s6t7u8v9w0x1y2z3",
                    ConcurrencyStamp = "AQAAAAEAACcQAAAAEJ2a8b9c0d1e2f3g4h5i6j7k8l9m0n1o2p3q4r5s6t7u8v9w0x1y2z3",
                    FirstName = "Default",
                    LastName = "Admin",
                    DateOfBirth = new DateOnly(1990, 1, 1)
            }

            );

            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    UserId = "f8e1c3b0-5d4e-4f9a-9c6b-2e1f3c4d5e6f",
                    RoleId = "b84229b8-ca2e-4423-9610-52c3c7e177bf"
                }
            );
        }

        public DbSet<LeaveType> LeaveTypes { get; set; }
    }
} 
