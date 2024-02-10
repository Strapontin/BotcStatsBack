using BotcRoles.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BotcRoles.Models
{
    /// <summary>
    /// Datas about a game draft
    /// </summary>
    public class GameDraft
    {
        public GameDraft() { }

        public GameDraft(Edition edition, Player storyteller, DateTime datePlayed, string notes)
        {
            Edition = edition;
            Storyteller = storyteller;
            DatePlayed = datePlayed;
            Notes = notes;
        }

        public long GameDraftId { get; set; }

        public long EditionId { get; set; }
        public Edition Edition { get; set; }


        public long StorytellerId { get; set; }
        public Player Storyteller { get; set; }

        public DateTime DatePlayed { get; set; }
        public string? Notes { get; set; }
    }



    public class GameDraftEntityTypeConfiguration : IEntityTypeConfiguration<GameDraft>
    {
        public void Configure(EntityTypeBuilder<GameDraft> builder)
        {
            builder
                .HasKey(g => g.GameDraftId);

            builder
                .HasOne(g => g.Edition)
                .WithMany(m => m.GamesDraft)
                .HasForeignKey(g => g.EditionId);

            builder
                .HasOne(g => g.Storyteller)
                .WithMany(p => p.GamesDraftStoryTelling)
                .HasForeignKey(g => g.StorytellerId);

            builder
                .Property(p => p.DatePlayed)
                .HasConversion(Helper.Converter.GetConverterDateTimeToString());
        }
    }
}
