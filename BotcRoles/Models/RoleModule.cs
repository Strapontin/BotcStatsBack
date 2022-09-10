using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BotcRoles.Models
{
    public class RoleModule
    {
        public long RoleModuleId { get; set; }

        public long RoleId { get; set; }
        public Role Role { get; set; }

        public long ModuleId { get; set; }
        public Module Module { get; set; }
    }



    public class RoleModuleEntityTypeConfiguration : IEntityTypeConfiguration<RoleModule>
    {
        public void Configure(EntityTypeBuilder<RoleModule> builder)
        {
            builder
                .HasKey(rm => rm.RoleModuleId);

            builder
                .HasKey(rm => new { rm.RoleId, rm.ModuleId });

            builder
                .HasOne(rm => rm.Role)
                .WithMany(r => r.RoleModules)
                .HasForeignKey(rm => rm.RoleId);

            builder
                .HasOne(rm => rm.Module)
                .WithMany(m => m.RoleModules)
                .HasForeignKey(rm => rm.ModuleId);
        }
    }
}
