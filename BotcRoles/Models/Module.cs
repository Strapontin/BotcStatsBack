using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace BotcRoles.Models
{
    public class Module
    {
        public long ModuleId { get; set; }
        public string Name { get; set; }

        public List<RoleModule> RoleModules { get; set; }
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
