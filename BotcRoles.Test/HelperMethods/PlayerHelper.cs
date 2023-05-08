using BotcRoles.Controllers;
using BotcRoles.Entities;
using BotcRoles.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotcRoles.Test.HelperMethods
{
    public static class PlayerHelper
    {
        public static IActionResult PostPlayer(ModelContext modelContext, string playerName)
        {
            PlayersController playerController = new(null!, modelContext);

            var res = playerController.PostPlayer(playerName);
            return res;
        }

        public static IEnumerable<PlayerEntities> GetPlayers(ModelContext modelContext)
        {
            PlayersController playerController = new(null!, modelContext);

            return playerController.GetPlayers().Value;
        }

        public static PlayerEntities GetPlayer(ModelContext modelContext, long playerId)
        {
            PlayersController playerController = new(null!, modelContext);

            var res = playerController.GetPlayerByName(playerId);
            return res.Value;
        }
    }
}
