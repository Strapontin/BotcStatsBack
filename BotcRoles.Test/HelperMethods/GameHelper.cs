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
        public static IActionResult PostGame(ModelContext modelContext, long? editionId, long? storyTellerId, DateTime? datePlayed,
            Alignment? winningAlignment, List<PlayerIdRoleId> playersIdRolesId, string notes = null)
        {
            GamesController gameController = new(null!, modelContext);

            var data = new
            {
                editionId,
                storyTellerId,
                datePlayed,
                notes,
                winningAlignment,
                playersIdRolesId
            };

            var res = gameController.CreateGame(JObject.FromObject(data));
            return res;
        }

        public static IEnumerable<GameEntities> GetGames(ModelContext modelContext)
        {
            GamesController gameController = new(null!, modelContext);

            return gameController.GetGames().Value;
        }

        public static GameEntities GetGame(ModelContext modelContext, long gameId)
        {
            GamesController gameController = new(null!, modelContext);

            var res = gameController.GetGameById(gameId);
            return res.Value;
        }


        public static List<PlayerIdRoleId> GetCorrectPlayersIdRolesId(ModelContext modelContext)
        {
            var result = new List<PlayerIdRoleId>()
            {
                new (modelContext.Players.First().PlayerId, modelContext.Roles.First().RoleId),
                new (modelContext.Players.Skip(1).First().PlayerId, modelContext.Roles.Skip(1).First().RoleId),
                new (modelContext.Players.Skip(2).First().PlayerId, modelContext.Roles.Skip(2).First().RoleId),
                new (modelContext.Players.Skip(3).First().PlayerId, modelContext.Roles.Skip(3).First().RoleId),
                new (modelContext.Players.Skip(4).First().PlayerId, modelContext.Roles.Skip(4).First().RoleId),
            };

            return result;
        }
    }
}
