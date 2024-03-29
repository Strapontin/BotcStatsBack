﻿using BotcRoles.Test.HelperMethods;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace BotcRoles.Test
{
    [TestFixture]
    public class PlayerControllerShould
    {
        [Test]
        public void Post_And_Get_Player()
        {
            // Arrange
            string fileName = DBHelper.GetCurrentMethodName();
            var modelContext = DBHelper.GetCleanContext(fileName);
            modelContext.Players.RemoveRange(modelContext.Players);
            string playerName = "PlayerName";
            string pseudo = "";

            try
            {
                // Act
                var res = PlayerHelper.PostPlayer(modelContext, playerName, pseudo);

                // Assert
                Assert.AreEqual(StatusCodes.Status201Created, ((CreatedResult)res).StatusCode);

                // Act
                long playerId = PlayerHelper.GetPlayers(modelContext).Last().Id;

                // Assert
                Assert.AreEqual(playerName, PlayerHelper.GetPlayerById(modelContext, playerId).Name);
            }
            finally
            {
                DBHelper.DeleteCreatedDatabase(modelContext);
            }
        }

        [Test]
        public void Cant_Post_Two_Players_With_Same_Name()
        {
            // Arrange
            string fileName = DBHelper.GetCurrentMethodName();
            var modelContext = DBHelper.GetCleanContext(fileName);
            string playerName = "PlayerName";

            // Act
            PlayerHelper.PostPlayer(modelContext, playerName);
            var res = PlayerHelper.PostPlayer(modelContext, playerName);
            Assert.AreEqual(StatusCodes.Status400BadRequest, ((BadRequestObjectResult)res).StatusCode);

            DBHelper.DeleteCreatedDatabase(modelContext);
        }

        [Test]
        public void Can_Post_Two_Players_With_Same_Name_And_Different_Pseudo()
        {
            // Arrange
            string fileName = DBHelper.GetCurrentMethodName();
            var modelContext = DBHelper.GetCleanContext(fileName);
            string playerName = "PlayerName";
            string pseudo1 = "pseudo1";
            string pseudo2 = "pseudo2";

            // Act
            PlayerHelper.PostPlayer(modelContext, playerName, pseudo1);
            var res = PlayerHelper.PostPlayer(modelContext, playerName, pseudo2);

            DBHelper.DeleteCreatedDatabase(modelContext);
            Assert.AreEqual(StatusCodes.Status201Created, ((ObjectResult)res).StatusCode);
        }

        [Test]
        public void Cant_Post_Player_With_Empty_Name()
        {
            // Arrange
            string fileName = DBHelper.GetCurrentMethodName();
            var modelContext = DBHelper.GetCleanContext(fileName);
            string playerName = string.Empty;

            // Act
            var res = PlayerHelper.PostPlayer(modelContext, playerName);
            Assert.AreEqual(StatusCodes.Status400BadRequest, ((BadRequestObjectResult)res).StatusCode);

            DBHelper.DeleteCreatedDatabase(modelContext);
        }

        [Test]
        public void Can_Update_Player()
        {
            // Arrange
            string fileName = DBHelper.GetCurrentMethodName();
            var modelContext = DBHelper.GetCleanContext(fileName);
            PlayerHelper.DeleteAllPlayers(modelContext);
            string roleName = "playerName";
            var res = PlayerHelper.PostPlayer(modelContext, roleName);

            // Act
            var playerId = PlayerHelper.GetPlayers(modelContext).First().Id;
            Assert.AreEqual(StatusCodes.Status201Created, ((ObjectResult)res).StatusCode);

            string newName = "newName";
            string pseudo = "pseudo";
            res = PlayerHelper.UpdatePlayer(modelContext, playerId, newName, pseudo);
            Assert.AreEqual(StatusCodes.Status201Created, ((ObjectResult)res).StatusCode);

            var role = PlayerHelper.GetPlayerById(modelContext, playerId);
            Assert.AreEqual(playerId, role.Id);
            Assert.AreEqual(newName, role.Name);
            Assert.AreEqual(pseudo, role.Pseudo);
            Assert.AreEqual(1, modelContext.Players.Count());


            DBHelper.DeleteCreatedDatabase(modelContext);
        }

        [Test]
        public void Can_Delete_Player_Not_In_PlayerRoleGame()
        {
            // Arrange
            string fileName = DBHelper.GetCurrentMethodName();
            var modelContext = DBHelper.GetCleanContext(fileName, false);


            string playerName = "playerName";
            var res = PlayerHelper.PostPlayer(modelContext, playerName);
            Assert.AreEqual(StatusCodes.Status201Created, ((ObjectResult)res).StatusCode);

            var playerId = PlayerHelper.GetPlayers(modelContext).First().Id;

            // Act
            res = PlayerHelper.DeletePlayer(modelContext, playerId);
            Assert.AreEqual(StatusCodes.Status202Accepted, ((ObjectResult)res).StatusCode);

            Assert.AreEqual(0, PlayerHelper.GetPlayers(modelContext).Count());
            DBHelper.DeleteCreatedDatabase(modelContext);
        }

        [Test]
        public void Delete_Player_In_PlayerRoleGameOr_Storyteller_Sets_Hidden()
        {
            // Arrange
            string fileName = DBHelper.GetCurrentMethodName();
            var modelContext = DBHelper.GetCleanContext(fileName, false);
            DBHelper.CreateBasicDataInAllTables(modelContext);
            PlayerHelper.PostPlayer(modelContext, "player2");
            modelContext.PlayerRoleGames.Add(new Models.PlayerRoleGame(
                modelContext.Players.First(p => p.Name == "player2"),
                modelContext.Roles.First(),
                modelContext.Games.First()));
            modelContext.SaveChanges();


            foreach (var player in modelContext.Players.ToList())
            {
                var res = PlayerHelper.DeletePlayer(modelContext, player.PlayerId);
                Assert.AreEqual(StatusCodes.Status202Accepted, ((ObjectResult)res).StatusCode);
            }

            Assert.AreEqual(0, PlayerHelper.GetPlayers(modelContext).Count());
            Assert.AreEqual(2, modelContext.Players.Count());
            Assert.IsTrue(modelContext.Players.All(p => p.IsHidden));
            DBHelper.DeleteCreatedDatabase(modelContext);
        }
    }
}
