using BotcRoles.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotcRoles.Test.HelperMethods
{
    public static class DBHelper
    {
        //private static readonly string _dbPath = @"E:\Anthony\Devs\Devs\BotcStatsBack\BotcRoles\DB\BotcRoles_TestDatabases";

        // TODO : a voir pour mettre le nom du test dans 'Database=', et drop du coup la bdd correctement à la fin des tests
        private static readonly string _dbPath = "Host=localhost; Database=botc_stats_db_test; Username=postgres; Password=admin;";

        public static string GetCurrentMethodName()
        {
            StackTrace stackTrace = new();
            StackFrame stackFrame = stackTrace.GetFrame(1)!;

            return stackFrame.GetMethod()!.Name;
        }

        public static ModelContext GetCleanContext(string methodName, bool initData = true)
        {
            DeleteCreatedDatabase(GetContext(methodName, true));
            return GetContext(methodName, initData);
        }

        private static ModelContext GetContext(string methodName, bool initData)
        {
            if (!Directory.Exists(_dbPath))
            {
                Directory.CreateDirectory(_dbPath);
            }

            var settings = new Dictionary<string, string>
            {
                { "Db_Path", _dbPath },
                { "Db_Name", methodName },
            };

            IConfiguration config = new ConfigurationBuilder()
                .AddInMemoryCollection(settings)
                .Build();

            ModelContext modelContext = new(new DbContextOptions<ModelContext>(), config, initData);

            return modelContext;
        }

        public static void DeleteCreatedDatabase(ModelContext modelContext)
        {
            modelContext.Database.EnsureDeleted();
        }

        public static void CreateBasicDataInAllTables(ModelContext modelContext)
        {
            modelContext.Players.Add(new Player("playerName", "pseudo"));
            modelContext.Roles.Add(new Role("roleName", Enums.CharacterType.Townsfolk, Enums.Alignment.Good));
            modelContext.Editions.Add(new Edition("editionName"));
            modelContext.SaveChanges();

            modelContext.Games.Add(new Game(modelContext.Editions.First(),
                modelContext.Players.First(),
                DateTime.Now,
                "",
                Enums.Alignment.Good));
            modelContext.SaveChanges();
        }

        public static void DeleteAllPlayerRoleGame(ModelContext modelContext)
        {
            modelContext.PlayerRoleGames.RemoveRange(modelContext.PlayerRoleGames);
        }
    }
}
