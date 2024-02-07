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

        public override bool Equals(object? roleEdition)
        {
            if (roleEdition == null || !this.GetType().Equals(roleEdition.GetType()))
                return false;

            var re = (RoleEdition)roleEdition;
            return re.RoleId == this.RoleId &&
                re.EditionId == this.EditionId;
        }

        public override int GetHashCode()
        {
            return
                RoleId.GetHashCode() ^
                EditionId.GetHashCode();
        }
    }



    public class RoleEditionEntityTypeConfiguration : IEntityTypeConfiguration<RoleEdition>
    {
        public void Configure(EntityTypeBuilder<RoleEdition> builder)
        {
            builder
                .HasKey(re => new { re.RoleId, re.EditionId });

            builder
                .HasOne(re => re.Role)
                .WithMany(r => r.RolesEdition)
                .HasForeignKey(re => re.RoleId);

            builder
                .HasOne(re => re.Edition)
                .WithMany(e => e.RolesEdition)
                .HasForeignKey(re => re.EditionId);

            builder
                .HasIndex(re => new { re.RoleId, re.EditionId })
                .IsUnique();
        }
    }
}
