using BotcRoles.Controllers;
using BotcRoles.Entities;
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
    public static class PlayerHelper
    {
        public static IActionResult PostPlayer(ModelContext modelContext, string playerName, string pseudo = null)
        {
            PlayersController playerController = new(null!, modelContext);

            JObject playerPost = JObject.FromObject(new { playerName, pseudo });

            var res = playerController.AddPlayer(playerPost);
            return res;
        }

        public static IEnumerable<PlayerEntities> GetPlayers(ModelContext modelContext)
        {
            PlayersController playerController = new(null!, modelContext);

            return playerController.GetPlayers().Value;
        }

        public static PlayerEntities GetPlayerById(ModelContext modelContext, long playerId)
        {
            PlayersController playerController = new(null!, modelContext);

            var res = playerController.GetPlayerById(playerId);
            return res.Value;
        }

        public static void DeleteAllPlayers(ModelContext modelContext)
        {
            modelContext.Players.RemoveRange(modelContext.Players);
            modelContext.SaveChanges();
        }

        public static IActionResult UpdatePlayer(ModelContext modelContext, long playerId, string playerName, string pseudo)
        {
            PlayersController playersController = new(null!, modelContext);

            var data = new
            {
                playerId,
                playerName,
                pseudo,
            };

            var res = playersController.UpdatePlayer(JObject.FromObject(data));
            return res;
        }
    }
}
