using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BotcRoles.Models
{
    /// <summary>
    /// Datas about what Roles are in a Module
    /// </summary>
    public class RoleModule
    {
        public RoleModule() { }

        public RoleModule(Role role, Module module)
        {
            Role = role;
            Module = module;
        }


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
                .HasKey(rm => new { rm.RoleId, rm.ModuleId });

            builder
                .HasOne(rm => rm.Role)
                .WithMany(r => r.RolesModule)
                .HasForeignKey(rm => rm.RoleId);

            builder
                .HasOne(rm => rm.Module)
                .WithMany(m => m.RolesModule)
                .HasForeignKey(rm => rm.ModuleId);

            builder
                .HasIndex(rm => new { rm.RoleId, rm.ModuleId })
                .IsUnique();
        }
    }
}
