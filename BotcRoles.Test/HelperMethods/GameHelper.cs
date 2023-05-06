using BotcRoles.Controllers;
using BotcRoles.Enums;
using BotcRoles.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotcRoles.Test.HelperMethods
{
    public static class GameHelper
    {
        public static IActionResult PostGame(ModelContext modelContext, long moduleId, long storyTellerId)
        {
            GamesController gameController = new(null!, modelContext);

            var res = gameController.Post(moduleId, storyTellerId);
            return res;
        }

        public static IEnumerable<Game> GetGames(ModelContext modelContext)
        {
            GamesController gameController = new(null!, modelContext);

            return gameController.Get();
        }

        public static Game GetGame(ModelContext modelContext, long gameId)
        {
            GamesController gameController = new(null!, modelContext);

            var res = gameController.Get(gameId);
            return res!;
        }

        public static IActionResult DeleteGame(ModelContext modelContext, long gameId)
        {
            GamesController gameController = new(null!, modelContext);

            var res = gameController.Delete(gameId);
            return res;
        }

        public static void CreateModuleAndStoryTellerForGame(ModelContext modelContext, string moduleName, out long moduleId, string storyTellerName, out long storyTellerId)
        {
            ModuleHelper.PostModule(modelContext, moduleName);
            moduleId = ModuleHelper.GetModules(modelContext).First().ModuleId;

            PlayerHelper.PostPlayer(modelContext, storyTellerName);
            storyTellerId = PlayerHelper.GetPlayers(modelContext).First().PlayerId;
        }

        public static IActionResult AddPlayerInGame(ModelContext modelContext, long gameId, long? playerId)
        {
            GamesController gameController = new(null!, modelContext);

            var res = gameController.AddPlayerInGame(gameId, playerId);
            return res;
        }

        public static IEnumerable<Player>? GetPlayersInGame(ModelContext modelContext, long gameId)
        {
            GamesController gameController = new(null!, modelContext);

            var res = gameController.GetPlayers(gameId);
            return res;
        }

        public static IActionResult ChangePlayerRoleAndAlignmentInGame(ModelContext modelContext, long gameId, long playerId, long roleId, Alignment finalAlignment)
        {
            GamesController gameController = new(null!, modelContext);

            var res = gameController.ChangePlayerRoleAndAlignmentInGame(gameId, playerId, roleId, finalAlignment);
            return res;
        }

        public static IEnumerable<PlayerRoleGame>? GetPlayerRoleFromGame(ModelContext modelContext, long gameId, long playerId)
        {
            GamesController gameController = new(null!, modelContext);

            var res = gameController.GetPlayerRoleFromGame(gameId, playerId);
            return res;
        }
    }
}
