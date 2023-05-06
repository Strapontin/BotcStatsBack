using BotcRoles.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace BotcRoles.Models
{
    /// <summary>
    /// Datas about a game played
    /// </summary>
    public class Game
    {
        public Game() { }

        public Game(Module module, Player storyTeller)
        {
            CreationDate = DateTime.Now;

            Module = module;
            StoryTeller = storyTeller;
        }

        public long GameId { get; set; }

        public long ModuleId { get; set; }
        public Module Module { get; set; }


        public long StoryTellerId { get; set; }
        public Player StoryTeller { get; set; }

        public DateTime CreationDate { get; set; }
        public string? Notes { get; set; }

        public List<PlayerRoleGame> PlayerRoleGames { get; set; }

        public Alignment WinningAlignment { get; set; }
    }



    public class GameEntityTypeConfiguration : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> builder)
        {
            builder
                .HasKey(g => g.GameId);

            builder
                .HasOne(g => g.Module)
                .WithMany(m => m.Games)
                .HasForeignKey(g => g.ModuleId);

            builder
                .HasOne(g => g.StoryTeller)
                .WithMany(p => p.GamesStoryTelling)
                .HasForeignKey(g => g.StoryTellerId);
        }
    }
}
