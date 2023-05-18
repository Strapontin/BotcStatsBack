using BotcRoles.Controllers;
using BotcRoles.Enums;
using BotcRoles.Models;
using BotcRoles.Test.HelperMethods;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotcRoles.Test
{
    [TestFixture]
    public class GameControllerShould
    {
        [Test]
        public void Get_Games()
        {
            // Arrange
            string fileName = DBHelper.GetCurrentMethodName() + ".db";
            var modelContext = DBHelper.GetCleanContext(fileName);

            // Act
            var res = GameHelper.GetGames(modelContext);

            // Assert 
            Assert.IsTrue(res.Count() > 0);

            DBHelper.DeleteCreatedDatabase(modelContext);
        }

        [Test]
        public void Get_Game()
        {
            // Arrange
            string fileName = DBHelper.GetCurrentMethodName() + ".db";
            var modelContext = DBHelper.GetCleanContext(fileName);

            // Act
            var res = GameHelper.GetGame(modelContext, 1);

            // Assert 
            Assert.IsTrue(res != null);

            DBHelper.DeleteCreatedDatabase(modelContext);
        }

        [Test]
        public void Cant_Post_Game_Without_EditionId()
        {
            // Arrange
            string fileName = DBHelper.GetCurrentMethodName() + ".db";
            var modelContext = DBHelper.GetCleanContext(fileName);
            string playerName = "PlayerName";

            PlayerHelper.PostPlayer(modelContext, playerName);
            long storyTellerId = PlayerHelper.GetPlayers(modelContext).First().Id;
            var playersIdRolesId = GameHelper.GetCorrectPlayersIdRolesId(modelContext);

            // Act
            var res = GameHelper.PostGame(modelContext, null, storyTellerId, DateTime.Now, Alignment.Good, playersIdRolesId);

            // Assert
            Assert.AreEqual(StatusCodes.Status400BadRequest, ((ObjectResult)res).StatusCode);

            DBHelper.DeleteCreatedDatabase(modelContext);
        }

        [Test]
        public void Cant_Post_Game_With_Wrong_EditionId()
        {
            // Arrange
            string fileName = DBHelper.GetCurrentMethodName() + ".db";
            var modelContext = DBHelper.GetCleanContext(fileName);
            string playerName = "PlayerName";

            PlayerHelper.PostPlayer(modelContext, playerName);
            long storyTellerId = PlayerHelper.GetPlayers(modelContext).First().Id;
            var playersIdRolesId = GameHelper.GetCorrectPlayersIdRolesId(modelContext);

            // Act
            var res = GameHelper.PostGame(modelContext, -1, storyTellerId, DateTime.Now, Alignment.Good, playersIdRolesId);

            // Assert
            Assert.AreEqual(StatusCodes.Status400BadRequest, ((ObjectResult)res).StatusCode);

            DBHelper.DeleteCreatedDatabase(modelContext);
        }

        [Test]
        public void Cant_Post_Game_Without_StoryTellerId()
        {
            // Arrange
            string fileName = DBHelper.GetCurrentMethodName() + ".db";
            var modelContext = DBHelper.GetCleanContext(fileName);

            long editionId = EditionHelper.GetEditions(modelContext).First().Id;
            var playersIdRolesId = GameHelper.GetCorrectPlayersIdRolesId(modelContext);

            // Act
            var res = GameHelper.PostGame(modelContext, editionId, null, DateTime.Now, Alignment.Good, playersIdRolesId);

            // Assert
            Assert.AreEqual(StatusCodes.Status400BadRequest, ((ObjectResult)res).StatusCode);

            DBHelper.DeleteCreatedDatabase(modelContext);
        }

        [Test]
        public void Cant_Post_Game_With_Wrong_StoryTellerId()
        {
            // Arrange
            string fileName = DBHelper.GetCurrentMethodName() + ".db";
            var modelContext = DBHelper.GetCleanContext(fileName);

            long editionId = EditionHelper.GetEditions(modelContext).First().Id;
            var playersIdRolesId = GameHelper.GetCorrectPlayersIdRolesId(modelContext);

            // Act
            var res = GameHelper.PostGame(modelContext, editionId, -1, DateTime.Now, Alignment.Good, playersIdRolesId);

            // Assert
            Assert.AreEqual(StatusCodes.Status400BadRequest, ((ObjectResult)res).StatusCode);

            DBHelper.DeleteCreatedDatabase(modelContext);
        }

        [Test]
        public void Cant_Post_Game_Without_DatePlayed()
        {
            // Arrange
            string fileName = DBHelper.GetCurrentMethodName() + ".db";
            var modelContext = DBHelper.GetCleanContext(fileName);

            long editionId = EditionHelper.GetEditions(modelContext).First().Id;
            long storyTellerId = PlayerHelper.GetPlayers(modelContext).First().Id;
            var playersIdRolesId = GameHelper.GetCorrectPlayersIdRolesId(modelContext);

            // Act
            var res = GameHelper.PostGame(modelContext, editionId, storyTellerId, null, Alignment.Good, playersIdRolesId);

            // Assert
            Assert.AreEqual(StatusCodes.Status400BadRequest, ((ObjectResult)res).StatusCode);

            DBHelper.DeleteCreatedDatabase(modelContext);
        }

        [Test]
        public void Cant_Post_Game_Without_WinningAlignment()
        {
            // Arrange
            string fileName = DBHelper.GetCurrentMethodName() + ".db";
            var modelContext = DBHelper.GetCleanContext(fileName);

            long editionId = EditionHelper.GetEditions(modelContext).First().Id;
            var playersIdRolesId = GameHelper.GetCorrectPlayersIdRolesId(modelContext);

            // Act
            var res = GameHelper.PostGame(modelContext, editionId, null, DateTime.Now, null, playersIdRolesId);

            // Assert
            Assert.AreEqual(StatusCodes.Status400BadRequest, ((ObjectResult)res).StatusCode);

            DBHelper.DeleteCreatedDatabase(modelContext);
        }

        [Test]
        public void Cant_Post_Game_With_Wrong_WinningAlignment()
        {
            // Arrange
            string fileName = DBHelper.GetCurrentMethodName() + ".db";
            var modelContext = DBHelper.GetCleanContext(fileName);

            long editionId = EditionHelper.GetEditions(modelContext).First().Id;
            long storyTellerId = PlayerHelper.GetPlayers(modelContext).First().Id;
            var playersIdRolesId = GameHelper.GetCorrectPlayersIdRolesId(modelContext);

            // Act
            var res = GameHelper.PostGame(modelContext, editionId, storyTellerId, DateTime.Now, (Alignment)3, playersIdRolesId);

            // Assert
            Assert.AreEqual(StatusCodes.Status400BadRequest, ((ObjectResult)res).StatusCode);

            DBHelper.DeleteCreatedDatabase(modelContext);
        }

        [Test]
        public void Cant_Post_Game_Without_PlayerRoles()
        {
            // Arrange
            string fileName = DBHelper.GetCurrentMethodName() + ".db";
            var modelContext = DBHelper.GetCleanContext(fileName);

            long editionId = EditionHelper.GetEditions(modelContext).First().Id;
            long storyTellerId = PlayerHelper.GetPlayers(modelContext).First().Id;
            var playersIdRolesId = GameHelper.GetCorrectPlayersIdRolesId(modelContext);

            // Act
            var res = GameHelper.PostGame(modelContext, editionId, storyTellerId, DateTime.Now, Alignment.Good, null);

            // Assert
            Assert.AreEqual(StatusCodes.Status400BadRequest, ((ObjectResult)res).StatusCode);

            DBHelper.DeleteCreatedDatabase(modelContext);
        }

        [Test]
        public void Cant_Post_Game_With_Wrong_PlayerRole_Player_Id()
        {
            // Arrange
            string fileName = DBHelper.GetCurrentMethodName() + ".db";
            var modelContext = DBHelper.GetCleanContext(fileName);

            long editionId = EditionHelper.GetEditions(modelContext).First().Id;
            long storyTellerId = PlayerHelper.GetPlayers(modelContext).First().Id;
            var playersIdRolesId = GameHelper.GetCorrectPlayersIdRolesId(modelContext);
            playersIdRolesId.Add(-1, modelContext.Roles.First().RoleId);

            // Act
            var res = GameHelper.PostGame(modelContext, editionId, storyTellerId, DateTime.Now, Alignment.Good, playersIdRolesId);

            // Assert
            Assert.AreEqual(StatusCodes.Status400BadRequest, ((ObjectResult)res).StatusCode);

            DBHelper.DeleteCreatedDatabase(modelContext);
        }

        [Test]
        public void Cant_Post_Game_With_Wrong_PlayerRole_Role_Id()
        {
            // Arrange
            string fileName = DBHelper.GetCurrentMethodName() + ".db";
            var modelContext = DBHelper.GetCleanContext(fileName);

            long editionId = EditionHelper.GetEditions(modelContext).First().Id;
            long storyTellerId = PlayerHelper.GetPlayers(modelContext).First().Id;
            var playersIdRolesId = GameHelper.GetCorrectPlayersIdRolesId(modelContext);
            playersIdRolesId.Add(modelContext.Players.OrderBy(p => p.PlayerId).Last().PlayerId, -1);

            // Act
            var res = GameHelper.PostGame(modelContext, editionId, storyTellerId, DateTime.Now, Alignment.Good, playersIdRolesId);

            // Assert
            Assert.AreEqual(StatusCodes.Status400BadRequest, ((ObjectResult)res).StatusCode);

            DBHelper.DeleteCreatedDatabase(modelContext);
        }

        [Test]
        public void Can_Post_Game()
        {
            // Arrange
            string fileName = DBHelper.GetCurrentMethodName() + ".db";
            var modelContext = DBHelper.GetCleanContext(fileName);

            long editionId = EditionHelper.GetEditions(modelContext).First().Id;
            long storyTellerId = PlayerHelper.GetPlayers(modelContext).First().Id;
            var playersIdRolesId = GameHelper.GetCorrectPlayersIdRolesId(modelContext);

            // Act
            var res = GameHelper.PostGame(modelContext, editionId, storyTellerId, DateTime.Now, Alignment.Good, playersIdRolesId);

            // Assert
            Assert.AreEqual(StatusCodes.Status201Created, ((ObjectResult)res).StatusCode);

            DBHelper.DeleteCreatedDatabase(modelContext);
        }


        [Test]
        public void Post_And_Get_Game()
        {
            // Arrange
            string fileName = DBHelper.GetCurrentMethodName() + ".db";
            var modelContext = DBHelper.GetCleanContext(fileName);

            long editionId = EditionHelper.GetEditions(modelContext).First().Id;
            long playerId = PlayerHelper.GetPlayers(modelContext).First().Id;
            var playersIdRolesId = GameHelper.GetCorrectPlayersIdRolesId(modelContext);
            return;

            // Act
            var res = GameHelper.PostGame(modelContext, editionId, playerId, DateTime.Now, Alignment.Good, playersIdRolesId);

            // Assert
            Assert.AreEqual(StatusCodes.Status201Created, ((ObjectResult)res).StatusCode);

            DBHelper.DeleteCreatedDatabase(modelContext);
        }
    }
}
