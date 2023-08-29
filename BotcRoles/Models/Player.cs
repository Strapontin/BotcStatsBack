using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BotcRoles.Models
{
    /// <summary>
    /// Datas about a Player
    /// </summary>
    public class Player
    {
        public Player() { }

        public Player(string name, string pseudo = null)
        {
            Name = name;
            Pseudo = pseudo ?? "";
            IsHidden = false;
        }

        public long PlayerId { get; set; }
        public string Name { get; set; }
        public string Pseudo { get; set; }
        public bool IsHidden { get; set; }

        public List<PlayerRoleGame> PlayerRoleGames { get; set; }
        public List<Game> GamesStoryTelling { get; set; }
    }



    public class PlayerEntityTypeConfiguration : IEntityTypeConfiguration<Player>
    {
        public void Configure(EntityTypeBuilder<Player> builder)
        {
            builder
                .HasKey(p => p.PlayerId);

            builder
                .Property(p => p.Name)
                .IsRequired();

            builder
                .Property(p => p.Pseudo)
                .IsRequired(false);

            builder
                .HasIndex(p => new { p.Name, p.Pseudo })
                .IsUnique()
                .HasFilter(null);

            builder
                .Property(p => p.IsHidden)
                .HasConversion(Helper.Converter.GetConverterBoolToInt());
        }
    }
}
