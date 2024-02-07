using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BotcRoles.Models
{
    /// <summary>
    /// Datas about what Roles are in a Edition
    /// </summary>
    public class DemonBluff
    {
        public DemonBluff() { }

        public DemonBluff(Role role, Game game)
        {
            Role = role;
            Game = game;
        }


        public long RoleId { get; set; }
        public Role Role { get; set; }

        public long GameId { get; set; }
        public Game Game { get; set; }

        public override bool Equals(object? demonBluff)
        {
            if (demonBluff == null || !this.GetType().Equals(demonBluff.GetType()))
                return false;

            var db = (DemonBluff)demonBluff;
            return db.RoleId == this.RoleId &&
                db.GameId == this.GameId;
        }

        public override int GetHashCode()
        {
            return RoleId.GetHashCode() ^
                GameId.GetHashCode();
        }
    }



    public class DemonBluffEntityTypeConfiguration : IEntityTypeConfiguration<DemonBluff>
    {
        public void Configure(EntityTypeBuilder<DemonBluff> builder)
        {
            builder
                .HasKey(demonBluff => new { demonBluff.RoleId, demonBluff.GameId });

            builder
                .HasOne(demonBluff => demonBluff.Role)
                .WithMany(r => r.DemonBluffs)
                .HasForeignKey(demonBluff => demonBluff.RoleId);

            builder
                .HasOne(demonBluff => demonBluff.Game)
                .WithMany(game => game.DemonBluffs)
                .HasForeignKey(demonBluff => demonBluff.GameId);

            builder
                .HasIndex(demonBluff => new { demonBluff.RoleId, demonBluff.GameId })
                .IsUnique();
        }
    }
}
