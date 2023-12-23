using Microsoft.EntityFrameworkCore;

namespace BotcRoles.Models
{
    public class ModelContext : DbContext
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<PlayerRoleGame> PlayerRoleGames { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Edition> Editions { get; set; }
        public DbSet<RoleEdition> RolesEdition { get; set; }
        public DbSet<DemonBluff> DemonBluffs { get; set; }


        public string DbPath { get; }
        public EnvironmentBuild EnvironmentBuild { get; }

        public ModelContext(DbContextOptions<ModelContext> options, IConfiguration config, bool initData = true) : base(options)
        {
            switch (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"))
            {
                case "Production":
                    EnvironmentBuild = EnvironmentBuild.Production;
                    break;

                case "Recette":
                    EnvironmentBuild = EnvironmentBuild.Recette;
                    break;

                case "Development":
                    EnvironmentBuild = EnvironmentBuild.Development;
                    break;

                default:
                    EnvironmentBuild = EnvironmentBuild.Tests;
                    break;
            }


            if (EnvironmentBuild == EnvironmentBuild.Tests)
            {
                DbPath = config["Db_Path"];
            }
            else if (EnvironmentBuild == EnvironmentBuild.Production ||
                EnvironmentBuild == EnvironmentBuild.Recette)
            {
                DbPath = Environment.GetEnvironmentVariable("CONNECTION_STRING");
            }
            else if (EnvironmentBuild == EnvironmentBuild.Development)
            {
                DbPath = "Host=localhost; Database=botc_stats_db; Username=postgres; Password=postgres;";
            }

            this.Database.Migrate();

            if (initData)
            {
                InitDatabase();
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseNpgsql(DbPath);
        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new GameEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new EditionEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PlayerEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PlayerRoleGameEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new RoleEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new RoleEditionEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new DemonBluffEntityTypeConfiguration());
        }

        DateTime _dtCreated1 = DateTime.Now;
        DateTime _dtCreated2 = DateTime.Now.AddMinutes(1);
        private void InitDatabase()
        {
            if (!Players.Any())
                InitPlayers();
            if (!Roles.Any())
                InitRoles();
            if (!Editions.Any())
                InitEditions();
            if (!RolesEdition.Any())
                InitRolesEdition();
            // if (!Games.Any())
            //     InitGames();
            // if (!PlayerRoleGames.Any())
            //     InitPlayerRoleGames();
        }

        private void InitPlayers()
        {
            Players.AddRange(new List<Player> {
                new Player("Anthony", "Strapontin"),
                new Player("Pras"),
                new Player("Gil"),
                new Player("Anthony", "Zariko"),
                new Player("Jonathan", "Stashmou"),
                new Player("Alfonse", "Dura"),
                new Player("Eloise"),
                new Player("Alexandre", "Pacha"),
                new Player("Lauriane"),
                new Player("Alexandre", "Jila"),
                new Player("Florian", "Goratschin"),
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
                Roles.Add(new Role(townfolk, Enums.CharacterType.Townsfolk));
            }

            List<string> outsidersNames = new() { "Acrobate", "Balance", "Barbier", "Bete de foire", "Brute", "Demoiselle", "Dulcinée", "Gitane", "Golem",
                "Hérétique", "Inventeur", "Lunatique", "Majordome", "Maître des puzzles", "Maladroit", "Politicien", "Reclus", "Soûlard", "Vertueux" };
            foreach (var outsider in outsidersNames)
            {
                Roles.Add(new Role(outsider, Enums.CharacterType.Outsider));
            }

            List<string> minionNames = new() { "Assassin", "Avocat du Diable", "Baron", "Boomdandy", "Conspirateur", "Croqueuse d'hommes", "Empoisonneur",
                "Espion", "Gobelin", "Jumeau Maléfique", "Manipulateur", "Marionnette", "Mezepheles", "Parrain", "Prophète de l'effroi", "Psychopathe", "Sorcière",
                "Veuve Noire", "Vieille Chouette" };
            foreach (var minion in minionNames)
            {
                Roles.Add(new Role(minion, Enums.CharacterType.Minion));
            }

            List<string> demonNames = new() { "Al-Hadikhia", "Emeutier", "Fang Gu", "Imp", "Légion", "Léviathan", "Po", "Pukka", "No Dashii", "Sangsue", "Shabaloth",
                "Ptit Monstre", "Vigormortis", "Vortox", "Zombuul" };
            foreach (var demon in demonNames)
            {
                Roles.Add(new Role(demon, Enums.CharacterType.Demon));
            }

            List<string> travellerNames = new() { "Bouc Emissaire", "Bureaucrate", "Mendiant", "Vengeur", "Voleur", "Apprenti", "Archevêque", "Magistrat", "Matrone",
                "Necromant", "Barista", "Boucher", "Collecteur d'os", "Déviant", "Fille de joie", "Gangster" };
            foreach (var traveller in travellerNames)
            {
                Roles.Add(new Role(traveller, Enums.CharacterType.Traveller));
            }
            this.SaveChanges();
        }

        private void InitEditions()
        {
            Editions.Add(new Edition("Trouble Brewing"));
            Editions.Add(new Edition("Bad Moon Rising"));
            Editions.Add(new Edition("Sects And Violets"));
            this.SaveChanges();
        }

        private void InitRolesEdition()
        {
            List<string> rolesTB = new() { "Lavandière", "Archiviste", "Enquêteur", "Cuistot", "Empathique", "Voyante", "Croque-Mort", "Moine", "Gardien","Pucelle",
                "Mercenaire", "Soldat", "Maire", "Majordome", "Soûlard", "Reclus", "Vertueux", "Empoisonneur", "Espion", "Croqueuse d'hommes", "Baron", "Imp" };

            Edition editionTb = Editions.First(m => m.Name == "Trouble Brewing");


            List<string> rolesBMR = new() { "Mamie", "Marin", "Femme de Chambre", "Exorciste", "Aubergiste", "Parieur", "Commère", "Courtisane", "Professeur", "Ménestrel",
                "Herboriste", "Pacifiste", "Fou", "Inventeur", "Gitane", "Brute", "Lunatique", "Parrain", "Avocat du Diable", "Assassin", "Conspirateur",
                "Pukka", "Po", "Shabaloth", "Zombuul", "Apprenti", "Archevêque", "Magistrat", "Matrone", "Necromant" };

            Edition editionBMR = Editions.First(m => m.Name == "Bad Moon Rising");


            List<string> rolesSnV = new() { "Horloger", "Reveur", "Charmeur de serpent", "Mathematicien", "Fleuriste", "Crieur", "Oracle", "Savant", "Couturiere", "Faussaire",
                "Artiste", "Jongleur", "Sage", "Bete de foire", "Barbier", "Dulcinée", "Maladroit", "Jumeau Maléfique", "Manipulateur", "Sorcière", "Vieille Chouette",
                "Fang Gu", "No Dashii", "Vigormortis", "Vortox", "Barista", "Boucher", "Collecteur d'os", "Déviant", "Fille de joie" };


            Edition editionSnV = Editions.First(m => m.Name == "Sects And Violets");


            List<RoleEdition> rolesEdition = new();
            foreach (var role in rolesTB)
            {
                rolesEdition.Add(new RoleEdition(Roles.First(r => r.Name == role), editionTb));
            }

            foreach (var role in rolesBMR)
            {
                rolesEdition.Add(new RoleEdition(Roles.First(r => r.Name == role), editionBMR));
            }

            foreach (var role in rolesSnV)
            {
                rolesEdition.Add(new RoleEdition(Roles.First(r => r.Name == role), editionSnV));
            }
            RolesEdition.AddRange(rolesEdition);
            this.SaveChanges();
        }

        private void InitGames()
        {
            Games.Add(new Game(Editions.First(), Players.First(), _dtCreated1, "some notes", Enums.Alignment.Good));
            Games.Add(new Game(Editions.First(), Players.Skip(1).First(), _dtCreated2, "some other notes", Enums.Alignment.Evil));
            this.SaveChanges();
        }

        private void InitPlayerRoleGames()
        {
            var listPRG1 = new List<PlayerRoleGame>
            {
                new PlayerRoleGame(Players.Skip(1).First(), Editions.First().RolesEdition.First().Role, Games.First()),
                new PlayerRoleGame(Players.Skip(2).First(), Roles.First(r => r.Name.Contains("roqueuse")), Games.First()),
                new PlayerRoleGame(Players.Skip(3).First(), Editions.First().RolesEdition.Skip(2).First().Role, Games.First()),
                new PlayerRoleGame(Players.Skip(4).First(), Editions.First().RolesEdition.Skip(3).First().Role, Games.First()),
                new PlayerRoleGame(Players.Skip(5).First(), Editions.First().RolesEdition.Skip(4).First().Role, Games.First()),
                new PlayerRoleGame(Players.Skip(6).First(), Editions.First().RolesEdition.Skip(5).First().Role, Games.First()),
                new PlayerRoleGame(Players.Skip(7).First(), Editions.First().RolesEdition.Skip(6).First().Role, Games.First()),
                new PlayerRoleGame(Players.Skip(8).First(), Editions.First().RolesEdition.Skip(7).First().Role, Games.First()),
                new PlayerRoleGame(Players.Skip(9).First(), Editions.First().RolesEdition.Skip(8).First().Role, Games.First()),
                new PlayerRoleGame(Players.Skip(10).First(), Editions.First().RolesEdition.Last().Role, Games.First()),
            };
            var listPRG2 = new List<PlayerRoleGame>
            {
                new PlayerRoleGame(Players.Skip(1).First(), Editions.First().RolesEdition.First().Role, Games.First()),
                new PlayerRoleGame(Players.Skip(2).First(), Editions.First().RolesEdition.Skip(1+1).First().Role, Games.First()),
                new PlayerRoleGame(Players.Skip(3).First(), Editions.First().RolesEdition.Skip(2+1).First().Role, Games.First()),
                new PlayerRoleGame(Players.Skip(4).First(), Editions.First().RolesEdition.Skip(3+1).First().Role, Games.First()),
                new PlayerRoleGame(Players.Skip(5).First(), Editions.First().RolesEdition.Skip(4+1).First().Role, Games.First()),
                new PlayerRoleGame(Players.Skip(6).First(), Editions.First().RolesEdition.Skip(5+1).First().Role, Games.First()),
                new PlayerRoleGame(Players.Skip(7).First(), Editions.First().RolesEdition.Skip(6+1).First().Role, Games.First()),
                new PlayerRoleGame(Players.Skip(8).First(), Editions.First().RolesEdition.Skip(7+1).First().Role, Games.First()),
                new PlayerRoleGame(Players.Skip(9).First(), Editions.First().RolesEdition.Skip(8+1).First().Role, Games.First()),
                new PlayerRoleGame(Players.Skip(10).First(), Editions.First().RolesEdition.Last().Role, Games.First()),
            };

            Games.First().PlayerRoleGames = listPRG1;

            Games.Skip(1).First().PlayerRoleGames = listPRG2;
            this.SaveChanges();
        }
    }
}
