using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace BotcRoles.Models
{
    public class Player
    {
        public Player(string name)
        {
            Name = name;
        }

        public long PlayerId { get; set; }
        public string Name { get; set; }

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
                .HasIndex(p => p.Name)
                .IsUnique();

            builder
                .Property(p => p.Name)
                .IsRequired();
        }
    }
}
