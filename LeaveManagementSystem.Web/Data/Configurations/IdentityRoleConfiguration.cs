using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeaveManagementSystem.Web.Data.Configurations
{
    public class IdentityRoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(

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
        }

        
    }
}
