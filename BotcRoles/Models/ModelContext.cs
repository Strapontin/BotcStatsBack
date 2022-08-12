using Microsoft.EntityFrameworkCore;

namespace BotcRoles.Models
{
    public class ModelContext : DbContext
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<PlayerRole> PlayerRoles { get; set; }
        public DbSet<Role> Roles { get; set; }

        public string DbPath { get; }

        public ModelContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "test.db");// TODO : Change the path
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite($"Data Source={DbPath}");
    }

}
