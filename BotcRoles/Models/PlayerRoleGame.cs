using BotcRoles.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BotcRoles.Models
{
    /// <summary>
    /// Data about a Role played by a Player in a Game
    /// </summary>
    public class PlayerRoleGame
    {
        public PlayerRoleGame() { }

        public PlayerRoleGame(Player player, Game game)
        {
            Player = player;
            Game = game;
        }

        public long PlayerRoleGameId { get; set; }

        public long PlayerId { get; set; }
        public Player Player { get; set; }

        public long RoleId { get; set; }
        public Role Role { get; set; }

        public long GameId { get; set; }
        public Game Game { get; set; }

        public Alignment FinalAlignment { get; set; }
    }



    public class PlayerRoleGameEntityTypeConfiguration : IEntityTypeConfiguration<PlayerRoleGame>
    {
        public void Configure(EntityTypeBuilder<PlayerRoleGame> builder)
        {
            builder
                .HasKey(g => g.PlayerRoleGameId);

            builder
                .HasOne(prg => prg.Player)
                .WithMany(p => p.PlayerRoleGames)
                .HasForeignKey(prg => prg.PlayerId);

            builder
                .HasOne(prg => prg.Role)
                .WithMany(r => r.PlayerRoleGames)
                .HasForeignKey(prg => prg.RoleId);

            builder
                .HasOne(prg => prg.Game)
                .WithMany(g => g.PlayerRoleGames)
                .HasForeignKey(prg => prg.GameId);
        }
    }
}
