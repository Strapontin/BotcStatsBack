using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BotcRoles.Models
{
    /// <summary>
    /// Datas about what Roles are in a Edition
    /// </summary>
    public class RoleEdition
    {
        public RoleEdition() { }

        public RoleEdition(Role role, Edition edition)
        {
            Role = role;
            Edition = edition;
        }


        public long RoleId { get; set; }
        public Role Role { get; set; }

        public long EditionId { get; set; }
        public Edition Edition { get; set; }
    }



    public class RoleEditionEntityTypeConfiguration : IEntityTypeConfiguration<RoleEdition>
    {
        public void Configure(EntityTypeBuilder<RoleEdition> builder)
        {
            builder
                .HasKey(rm => new { rm.RoleId, rm.EditionId });

            builder
                .HasOne(rm => rm.Role)
                .WithMany(r => r.RolesEdition)
                .HasForeignKey(rm => rm.RoleId);

            builder
                .HasOne(rm => rm.Edition)
                .WithMany(m => m.RolesEdition)
                .HasForeignKey(rm => rm.EditionId);

            builder
                .HasIndex(rm => new { rm.RoleId, rm.EditionId })
                .IsUnique();
        }
    }
}
