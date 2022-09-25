using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace BotcRoles.Models
{
    public class ModelContext : DbContext
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<PlayerRoleGame> PlayerRoleGames { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<RoleModule> RoleModules { get; set; }


        public string DbPath { get; }

        public ModelContext(DbContextOptions<ModelContext> options, IConfiguration config) : base(options)
        {
            var path = config["Db_Path"];
            var name = config["Db_Name"];
            DbPath = System.IO.Path.Join(path, name);

            this.Database.Migrate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite($"Data Source={DbPath}");



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new GameEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ModuleEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PlayerEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PlayerRoleGameEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new RoleEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new RoleModuleEntityTypeConfiguration());
        }
    }
}
