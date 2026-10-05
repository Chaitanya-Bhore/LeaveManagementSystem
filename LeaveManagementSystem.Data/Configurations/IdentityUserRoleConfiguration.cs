using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeaveManagementSystem.Data.Configurations
{
    public class IdentityUserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            builder.HasData(
                new IdentityUserRole<string>
                {
                    UserId = "f8e1c3b0-5d4e-4f9a-9c6b-2e1f3c4d5e6f",
                    RoleId = "b84229b8-ca2e-4423-9610-52c3c7e177bf"
                }
            );
        }
    }
}
