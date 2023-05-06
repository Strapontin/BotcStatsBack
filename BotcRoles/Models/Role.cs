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

        public Role(string name, Enums.Type type, Alignment defaultAlignment)
        {
            Name = name;
            Type = type;
            DefaultAlignment = defaultAlignment;
        }

        public long RoleId { get; set; }
        public string Name { get; set; }
        public Enums.Type Type { get; set; }
        public Alignment DefaultAlignment { get; set; }

        public List<RoleModule> RolesModule { get; set; }
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
                .Property(r => r.Type)
                .IsRequired();

            builder
                .Property(r => r.DefaultAlignment)
                .IsRequired();
        }
    }
}
