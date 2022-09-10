using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace BotcRoles.Models
{
    public class Game
    {
        public Game()
        { }

        public long GameId { get; set; }
        public Module Module { get; set; }
        public DateTime CreationDate { get; set; }
        public Player StoryTeller { get; set; }
        public string? Notes { get; set; }

        public List<PlayerRoleGame> PlayerRoleGames { get; set; }
    }



    public class GameEntityTypeConfiguration : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> builder)
        {
            builder
                .HasKey(g => g.GameId);

            builder
                .HasOne(g => g.Module)
                .WithMany(m => m.Games);

            builder
                .HasOne(g => g.StoryTeller)
                .WithMany(p => p.GamesStoryTelling);
        }
    }
}
