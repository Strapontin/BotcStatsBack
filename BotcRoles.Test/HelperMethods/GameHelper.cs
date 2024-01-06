using BotcRoles.Controllers;
using BotcRoles.Entities;
using BotcRoles.Enums;
using BotcRoles.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotcRoles.Test.HelperMethods
{
    public static class GameHelper
    {
        public static IActionResult PostGame(ModelContext modelContext, long? editionId, long? storytellerId, DateTime? datePlayed,
            Alignment? winningAlignment, List<PlayerIdRoleId> playersIdRolesId, List<long> demonBluffsId, string notes = null)
        {
            GamesController gamesController = new(null!, modelContext);

            var data = new
            {
                editionId,
                storytellerId,
                datePlayed,
                notes,
                winningAlignment,
                playersIdRolesId,
                demonBluffsId,
            };

            var res = gamesController.CreateGame(JObject.FromObject(data));
            return res;
        }

        public static IActionResult UpdateGame(ModelContext modelContext, long? gameId, long? editionId, long? storytellerId, DateTime? datePlayed,
            Alignment? winningAlignment, List<PlayerIdRoleId> playersIdRolesId, List<long> demonBluffsId, string notes = null)
        {
            GamesController gamesController = new(null!, modelContext);

            var data = new
            {
                gameId,
                editionId,
                storytellerId,
                datePlayed,
                notes,
                winningAlignment,
                playersIdRolesId,
                demonBluffsId,
            };

            var res = gamesController.UpdateGame(JObject.FromObject(data));
            return res;
        }

        public static IEnumerable<GameEntities> GetGames(ModelContext modelContext)
        {
            GamesController gamesController = new(null!, modelContext);

            return gamesController.GetGames().Value;
        }

        public static GameEntities GetGame(ModelContext modelContext, long gameId)
        {
            GamesController gamesController = new(null!, modelContext);

            var res = gamesController.GetGameById(gameId);
            return res.Value;
        }

        public static IEnumerable<GameEntities> GetGamesByPlayer(ModelContext modelContext, long playerId)
        {
            GamesController gamesController = new(null!, modelContext);

            return gamesController.GetGamesByPlayerId(playerId).Value;
        }

        public static IEnumerable<GameEntities> GetGamesByStoryteller(ModelContext modelContext, long storytellerId)
        {
            GamesController gamesController = new(null!, modelContext);

            return gamesController.GetGamesByStorytellerId(storytellerId).Value;
        }


        public static List<PlayerIdRoleId> GetCorrectPlayersIdRolesId(ModelContext modelContext)
        {
            var result = new List<PlayerIdRoleId>()
            {
                new (modelContext.Players.First().PlayerId, modelContext.Roles.First().RoleId, Alignment.Good),
                new (modelContext.Players.Skip(1).First().PlayerId, modelContext.Roles.Skip(1).First().RoleId, Alignment.Good),
                new (modelContext.Players.Skip(2).First().PlayerId, modelContext.Roles.Skip(2).First().RoleId, Alignment.Good),
                new (modelContext.Players.Skip(3).First().PlayerId, modelContext.Roles.Skip(3).First().RoleId, Alignment.Good),
                new (modelContext.Players.Skip(4).First().PlayerId, modelContext.Roles.Skip(4).First().RoleId, Alignment.Good),
            };

            return result;
        }

        public static IActionResult DeleteGame(ModelContext modelContext, long gameId)
        {
            GamesController gamesController = new(null!, modelContext);
            var res = gamesController.DeleteGame(gameId);

            return res;
        }

        public static void DeleteAllGames(ModelContext modelContext)
        {
            modelContext.RemoveRange(modelContext.Games);
        }
    }
}
