﻿using BotcRoles.Controllers;
using BotcRoles.Entities;
using BotcRoles.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace BotcRoles.Test.HelperMethods
{
    public static class PlayerHelper
    {
        public static IActionResult PostPlayer(ModelContext modelContext, string playerName, string pseudo = null)
        {
            PlayersController playerController = new(null!, modelContext, DBHelper.GetIsStorytellerAuthorizationHandler())
            {
                ControllerContext = new ControllerContext(DBHelper.GetActionContext())
            };

            JObject playerPost = JObject.FromObject(new { playerName, pseudo });

            var res = playerController.AddPlayer(playerPost);
            return res;
        }

        public static IEnumerable<PlayerEntities> GetPlayers(ModelContext modelContext)
        {
            PlayersController playerController = new(null!, modelContext, null);

            return playerController.GetPlayers().Value;
        }

        public static PlayerEntities GetPlayerById(ModelContext modelContext, long playerId)
        {
            PlayersController playerController = new(null!, modelContext, null);

            var res = playerController.GetPlayerById(playerId);
            return res.Value;
        }

        public static IActionResult UpdatePlayer(ModelContext modelContext, long playerId, string playerName, string pseudo)
        {
            PlayersController playersController = new(null!, modelContext, DBHelper.GetIsStorytellerAuthorizationHandler())
            {
                ControllerContext = new ControllerContext(DBHelper.GetActionContext())
            };

            var data = new
            {
                playerId,
                playerName,
                pseudo,
            };

            var res = playersController.UpdatePlayer(JObject.FromObject(data));
            return res;
        }

        public static IActionResult DeletePlayer(ModelContext modelContext, long playerId)
        {
            PlayersController playersController = new(null!, modelContext, DBHelper.GetIsStorytellerAuthorizationHandler())
            {
                ControllerContext = new ControllerContext(DBHelper.GetActionContext())
            };
            var res = playersController.DeletePlayer(playerId);

            return res;
        }

        public static void DeleteAllPlayers(ModelContext modelContext)
        {
            modelContext.Players.RemoveRange(modelContext.Players);
            modelContext.SaveChanges();
        }
    }
}
