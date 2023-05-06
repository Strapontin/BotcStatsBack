using Microsoft.EntityFrameworkCore;

namespace BotcRoles.Models
{
    public class ModelContext : DbContext
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<PlayerRoleGame> PlayerRoleGames { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<RoleModule> RolesModule { get; set; }


        public string DbPath { get; }

        public ModelContext(DbContextOptions<ModelContext> options, IConfiguration config) : base(options)
        {
            var path = config["Db_Path"];
            var name = config["Db_Name"];
            DbPath = Path.Join(path, name);

            this.Database.Migrate();

            InitDatabase();
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

        private void InitDatabase()
        {
            if (!Players.Any())
                InitPlayers();
            if (!Roles.Any())
                InitRoles();
            if (!Modules.Any())
                InitModules();
            if (!RolesModule.Any())
                InitRolesModule();
            if (!Games.Any())
                InitGames();
            if (!PlayerRoleGames.Any())
                InitPlayerRoleGames();
        }

        private void InitPlayers()
        {
            Players.AddRange(new List<Player> {
                new Player("Strapontin"),
                new Player("Pras"),
                new Player("Mika"),
                new Player("Gil"),
                new Player("Zariko"),
                new Player("Stashmou"),
                new Player("Dura"),
                new Player("Eloise"),
                new Player("Pacha"),
                new Player("Lauriane"),
                new Player("Marwanne"),
                new Player("Jila"),
                new Player("Florian"),
                new Player("Florine"),
            });

            this.SaveChanges();
        }

        private void InitRoles()
        {
            List<string> townfolksNames = new() { "Aéronaute", "Alchimiste", "Amnésique", "Archiviste", "Artiste", "Athée", "Aubergiste", "Cannibale", "Charmeur de serpent",
                "Chasseur", "Chasseur de primes", "Chef de Secte", "Commère", "Courtisane", "Couturiere", "Crieur", "Croque-Mort", "Cuistot", "Cultivateur de pavot",
                "Empathique", "Enfant de choeur", "Enquêteur", "Exorciste", "Faussaire", "Femme de Chambre", "Fermier", "Fleuriste", "Fou", "Gardien", "Général",
                "Herboriste", "Horloger", "Ingénieur", "Jongleur", "Lavandière", "Lycanthrope", "Magicien", "Maire", "Mamie", "Marin", "Mathematicien", "Ménestrel",
                "Mercenaire", "Moine", "Noble", "Oracle", "Pacifiste", "Parieur", "Pêcheur", "Pixie", "Predicateur", "Professeur", "Pucelle", "Reveur", "Roi", "Sage",
                "Savant", "Soldat", "Veilleur de nuit", "Voyante" };
            foreach (var townfolk in townfolksNames)
            {
                Roles.Add(new Role(townfolk, Enums.Type.Townsfolk, Enums.Alignment.Good));
            }

            List<string> outsidersNames = new() { "Acrobate", "Balance", "Barbier", "Bete de foire", "Brute", "Demoiselle", "Dulcinée", "Gitane", "Golem",
                "Hérétique", "Inventeur", "Lunatique", "Majordome", "Maître des puzzles", "Maladroit", "Politicien", "Reclus", "Soûlard", "Vertueux" };
            foreach (var outsider in outsidersNames)
            {
                Roles.Add(new Role(outsider, Enums.Type.Outsider, Enums.Alignment.Good));
            }

            List<string> minionNames = new() { "Assassin", "Avocat du Diable", "Baron", "Boomdandy", "Conspirateur", "Croqueuse d'hommes", "Empoisonneur",
                "Espion", "Gobelin", "Jumeau Maléfique", "Manipulateur", "Marionnette", "Mezepheles", "Parrain", "Prophète de l'effroi", "Psychopathe", "Sorcière",
                "Veuve Noire", "Vieille Chouette" };
            foreach (var minion in minionNames)
            {
                Roles.Add(new Role(minion, Enums.Type.Minion, Enums.Alignment.Good));
            }

            List<string> demonNames = new() { "Al-Hadikhia", "Emeutier", "Fang Gu", "Imp", "Légion", "Léviathan", "Po", "Pukka", "No Dashii", "Sangsue", "Shabaloth",
                "P'tit Monstre", "Vigormortis", "Vortox", "Zombuul" };
            foreach (var demon in demonNames)
            {
                Roles.Add(new Role(demon, Enums.Type.Demon, Enums.Alignment.Good));
            }

            List<string> travellerNames = new() { "Bouc Emissaire", "Bureaucrate", "Mendiant", "Vengeur", "Voleur", "Apprenti", "Archevêque", "Magistrat", "Matrone",
                "Necromant", "Barista", "Boucher", "Collecteur d'os", "Déviant", "Fille de joie", "Gangster" };
            foreach (var traveller in travellerNames)
            {
                Roles.Add(new Role(traveller, Enums.Type.Traveller, Enums.Alignment.Good));
            }
            this.SaveChanges();
        }

        private void InitModules()
        {
            Modules.Add(new Module("Trouble Brewing"));
            this.SaveChanges();
        }

        private void InitRolesModule()
        {
            List<string> rolesTB = new() { "Lavandière", "Archiviste", "Enquêteur", "Cuistot", "Empathique", "Voyante", "Croque-Mort", "Moine", "Gardien",
                "Pucelle", "Mercenaire", "Soldat", "Maire", "Majordome", "Soûlard", "Reclus", "Vertueux", "Empoisonneur", "Espion", "Croqueuse d'hommes", "Baron", "Imp" };

            Module moduleTb = Modules.First(m => m.Name == "Trouble Brewing");

            List<RoleModule> rolesModule = new();
            foreach (var role in rolesTB)
            {
                rolesModule.Add(new RoleModule(Roles.First(r => r.Name == role), moduleTb));
            }

            RolesModule.AddRange(rolesModule);
            this.SaveChanges();
        }

        private void InitGames()
        {
            Games.Add(new Game(Modules.First(), Players.First()));
            this.SaveChanges();
        }

        private void InitPlayerRoleGames()
        {
            var listPRG = new List<PlayerRoleGame>
            {
                new PlayerRoleGame(Players.Skip(1).First(), Roles.First(), Games.First()),
                new PlayerRoleGame(Players.Skip(2).First(), Roles.Skip(1).First(), Games.First()),
                new PlayerRoleGame(Players.Skip(3).First(), Roles.Skip(2).First(), Games.First()),
                new PlayerRoleGame(Players.Skip(4).First(), Roles.Skip(3).First(), Games.First()),
                new PlayerRoleGame(Players.Skip(5).First(), Roles.Skip(4).First(), Games.First()),
                new PlayerRoleGame(Players.Skip(6).First(), Roles.Skip(5).First(), Games.First()),
                new PlayerRoleGame(Players.Skip(7).First(), Roles.Skip(6).First(), Games.First()),
                new PlayerRoleGame(Players.Skip(8).First(), Roles.Skip(7).First(), Games.First()),
                new PlayerRoleGame(Players.Skip(9).First(), Roles.Skip(8).First(), Games.First()),
                new PlayerRoleGame(Players.Skip(10).First(), Roles.Skip(9).First(), Games.First()),
            };

            Games.First().PlayerRoleGames = listPRG;
            this.SaveChanges();
        }
    }
}
