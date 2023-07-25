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

        public Game(Edition edition, Player storyTeller, DateTime datePlayed, string notes, Alignment winningAlignment, DateTime? dateCreated = null)
        {
            Edition = edition;
            StoryTeller = storyTeller;
            DateCreated = dateCreated ?? DateTime.Now;
            DatePlayed = datePlayed;
            Notes = notes;
            WinningAlignment = winningAlignment;
            IsHidden = false;
        }

        public long GameId { get; set; }

        public long EditionId { get; set; }
        public Edition Edition { get; set; }


        public long StoryTellerId { get; set; }
        public Player StoryTeller { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DatePlayed { get; set; }
        public string? Notes { get; set; }
        public bool IsHidden { get; set; }

        public List<PlayerRoleGame> PlayerRoleGames { get; set; }
        public List<DemonBluff> DemonBluffs { get; set; }

        public Alignment WinningAlignment { get; set; }
    }



    public class GameEntityTypeConfiguration : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> builder)
        {
            builder
                .HasKey(g => g.GameId);

            builder
                .HasOne(g => g.Edition)
                .WithMany(m => m.Games)
                .HasForeignKey(g => g.EditionId);

            builder
                .HasOne(g => g.StoryTeller)
                .WithMany(p => p.GamesStoryTelling)
                .HasForeignKey(g => g.StoryTellerId);
        }
    }
}
