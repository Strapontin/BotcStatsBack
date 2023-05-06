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
    public static class Helper
    {
        private static readonly string _dbPath = "E:\\Anthony\\Devs\\Devs\\BotcRoles\\BotcRoles\\DB\\BotcRoles_TestDatabases";

        public static string GetCurrentMethodName()
        {
            StackTrace stackTrace = new();
            StackFrame stackFrame = stackTrace.GetFrame(1)!;

            return stackFrame.GetMethod()!.Name;
        }

        public static ModelContext GetContext(string methodName)
        {
            var settings = new Dictionary<string, string>
            {
                { "Db_Path", _dbPath },
                { "Db_Name", methodName },
            };

            IConfiguration config = new ConfigurationBuilder()
                .AddInMemoryCollection(settings)
                .Build();

            ModelContext modelContext = new(new DbContextOptions<ModelContext>(), config);
            return modelContext;
        }

        public static void DeleteCreatedDatabase(ModelContext modelContext)
        {
            modelContext.Database.EnsureDeleted(); 
        }
    }
}
