using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace BotcRoles.Models
{
    /// <summary>
    /// Data about a module to play (mostly defines what roles are in it)
    /// </summary>
    public class Module
    {
        public Module() { }

        public Module(string name)
        {
            Name = name;
        }

        public long ModuleId { get; set; }
        public string Name { get; set; }

        public List<RoleModule> RolesModule { get; set; }
        public List<Game> Games { get; set; }
    }


    public class ModuleEntityTypeConfiguration : IEntityTypeConfiguration<Module>
    {
        public void Configure(EntityTypeBuilder<Module> builder)
        {
            builder
                .HasKey(m => m.ModuleId);

            builder
                .HasIndex(m => m.Name)
                .IsUnique();
        }
    }
}
