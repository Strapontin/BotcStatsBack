using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace BotcRoles.Models
{
    /// <summary>
    /// Data about an edition to play (mostly defines what roles are in it)
    /// </summary>
    public class Edition
    {
        public Edition() { }

        public Edition(string name)
        {
            Name = name;
        }

        public long EditionId { get; set; }
        public string Name { get; set; }

        public List<RoleEdition> RolesEdition { get; set; }
        public List<Game> Games { get; set; }
    }


    public class EditionEntityTypeConfiguration : IEntityTypeConfiguration<Edition>
    {
        public void Configure(EntityTypeBuilder<Edition> builder)
        {
            builder
                .HasKey(m => m.EditionId);

            builder
                .HasIndex(m => m.Name)
                .IsUnique();
        }
    }
}
