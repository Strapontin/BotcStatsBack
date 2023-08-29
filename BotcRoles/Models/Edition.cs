using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
            IsHidden = false;
        }

        public long EditionId { get; set; }
        public string Name { get; set; }
        public bool IsHidden { get; set; }

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

            builder
                .Property(p => p.IsHidden)
                .HasConversion(Helper.Converter.GetConverterBoolToInt());
        }
    }
}
