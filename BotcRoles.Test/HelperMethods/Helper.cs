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
        private static readonly string _dbPath = "Host=localhost; Database={0}; Username=postgres; Password=postgres;";

        public static string GetCurrentMethodName()
        {
            StackTrace stackTrace = new();
            StackFrame stackFrame = stackTrace.GetFrame(1)!;

            return stackFrame.GetMethod()!.Name;
        }

        public static ModelContext GetCleanContext(string methodName, bool initData = true)
        {
            DeleteCreatedDatabase(GetContext(methodName, initData));
            return GetContext(methodName, initData);
        }

        private static ModelContext GetContext(string methodName, bool initData)
        {
            var settings = new Dictionary<string, string>
            {
                { "Db_Path", _dbPath.Replace("{0}", methodName.ToLower()) },
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
