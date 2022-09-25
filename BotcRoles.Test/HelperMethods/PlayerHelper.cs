using BotcRoles.Controllers;
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
            PlayerController playerController = new(null!, modelContext);

            var res = playerController.PostPlayer(playerName);
            return res;
        }

        public static IEnumerable<Player> GetPlayers(ModelContext modelContext)
        {
            PlayerController playerController = new(null!, modelContext);

            return playerController.GetPlayers();
        }

        public static Player GetPlayer(ModelContext modelContext, long playerId)
        {
            PlayerController playerController = new(null!, modelContext);

            var res = playerController.GetPlayer(playerId);
            return res!;
        }
    }
}
