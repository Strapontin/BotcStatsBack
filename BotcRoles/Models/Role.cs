using BotcRoles.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace BotcRoles.Models
{
    /// <summary>
    /// Datas about a Role (Name, Type and Alignment)
    /// </summary>
    public class Role
    {
        public Role() { }

        public Role(string name, CharacterType characterType, Alignment defaultAlignment)
        {
            Name = name;
            CharacterType = characterType;
            DefaultAlignment = defaultAlignment;
            IsHidden = false;
        }

        public long RoleId { get; set; }
        public string Name { get; set; }
        public CharacterType CharacterType { get; set; }
        public Alignment DefaultAlignment { get; set; }
        public bool IsHidden { get; set; }

        public List<RoleEdition> RolesEdition { get; set; }
        public List<PlayerRoleGame> PlayerRoleGames { get; set; }
    }



    public class RoleEntityTypeConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder
                .HasKey(r => r.RoleId);

            builder
                .HasIndex(r => r.Name)
                .IsUnique();

            builder
                .Property(r => r.Name)
                .IsRequired();

            builder
                .Property(r => r.CharacterType)
                .IsRequired();

            builder
                .Property(r => r.DefaultAlignment)
                .IsRequired();
        }
    }
}
