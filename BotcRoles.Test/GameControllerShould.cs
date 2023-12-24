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
        public void Can_Post_Game()
        {
            // Arrange
            string fileName = DBHelper.GetCurrentMethodName();
            var modelContext = DBHelper.GetCleanContext(fileName);

            long editionId = EditionHelper.GetEditions(modelContext).First().Id;
            long storyTellerId = PlayerHelper.GetPlayers(modelContext).First().Id;
            var playersIdRolesId = GameHelper.GetCorrectPlayersIdRolesId(modelContext);
            var rolesId = RoleHelper.GetRoles(modelContext).Take(3).Select(r => r.Id).ToList();

            // Act
            var res = GameHelper.PostGame(modelContext, editionId, storyTellerId, DateTime.Now, Alignment.Good, playersIdRolesId, rolesId);

            // Assert
            Assert.AreEqual(StatusCodes.Status201Created, ((ObjectResult)res).StatusCode);

            DBHelper.DeleteCreatedDatabase(modelContext);
        }

        [Test]
        public void Get_Games()
        {
            // Arrange
            string fileName = DBHelper.GetCurrentMethodName();
            var modelContext = DBHelper.GetCleanContext(fileName);

            long editionId = EditionHelper.GetEditions(modelContext).First().Id;
            long storyTellerId = PlayerHelper.GetPlayers(modelContext).First().Id;
            var playersIdRolesId = GameHelper.GetCorrectPlayersIdRolesId(modelContext);
            var rolesId = RoleHelper.GetRoles(modelContext).Take(3).Select(r => r.Id).ToList();

            GameHelper.PostGame(modelContext, editionId, storyTellerId, DateTime.Now, Alignment.Good, playersIdRolesId, rolesId);

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
            string fileName = DBHelper.GetCurrentMethodName();
            var modelContext = DBHelper.GetCleanContext(fileName);

            long editionId = EditionHelper.GetEditions(modelContext).First().Id;
            long storyTellerId = PlayerHelper.GetPlayers(modelContext).First().Id;
            var playersIdRolesId = GameHelper.GetCorrectPlayersIdRolesId(modelContext);
            var rolesId = RoleHelper.GetRoles(modelContext).Take(3).Select(r => r.Id).ToList();

            GameHelper.PostGame(modelContext, editionId, storyTellerId, DateTime.Now, Alignment.Good, playersIdRolesId, rolesId);

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
            string fileName = DBHelper.GetCurrentMethodName();
            var modelContext = DBHelper.GetCleanContext(fileName);
            string playerName = "PlayerName";

            PlayerHelper.PostPlayer(modelContext, playerName);
            long storyTellerId = PlayerHelper.GetPlayers(modelContext).First().Id;
            var playersIdRolesId = GameHelper.GetCorrectPlayersIdRolesId(modelContext);
            var rolesId = RoleHelper.GetRoles(modelContext).Take(3).Select(r => r.Id).ToList();

            // Act
            var res = GameHelper.PostGame(modelContext, null, storyTellerId, DateTime.Now, Alignment.Good, playersIdRolesId, rolesId);

            // Assert
            Assert.AreEqual(StatusCodes.Status400BadRequest, ((ObjectResult)res).StatusCode);

            DBHelper.DeleteCreatedDatabase(modelContext);
        }

        [Test]
        public void Cant_Post_Game_With_Wrong_EditionId()
        {
            // Arrange
            string fileName = DBHelper.GetCurrentMethodName();
            var modelContext = DBHelper.GetCleanContext(fileName);
            string playerName = "PlayerName";

            PlayerHelper.PostPlayer(modelContext, playerName);
            long storyTellerId = PlayerHelper.GetPlayers(modelContext).First().Id;
            var playersIdRolesId = GameHelper.GetCorrectPlayersIdRolesId(modelContext);
            var rolesId = RoleHelper.GetRoles(modelContext).Take(3).Select(r => r.Id).ToList();

            // Act
            var res = GameHelper.PostGame(modelContext, -1, storyTellerId, DateTime.Now, Alignment.Good, playersIdRolesId, rolesId);

            // Assert
            Assert.AreEqual(StatusCodes.Status400BadRequest, ((ObjectResult)res).StatusCode);

            DBHelper.DeleteCreatedDatabase(modelContext);
        }

        [Test]
        public void Cant_Post_Game_Without_StoryTellerId()
        {
            // Arrange
            string fileName = DBHelper.GetCurrentMethodName();
            var modelContext = DBHelper.GetCleanContext(fileName);

            long editionId = EditionHelper.GetEditions(modelContext).First().Id;
            var playersIdRolesId = GameHelper.GetCorrectPlayersIdRolesId(modelContext);
            var rolesId = RoleHelper.GetRoles(modelContext).Take(3).Select(r => r.Id).ToList();

            // Act
            var res = GameHelper.PostGame(modelContext, editionId, null, DateTime.Now, Alignment.Good, playersIdRolesId, rolesId);

            // Assert
            Assert.AreEqual(StatusCodes.Status400BadRequest, ((ObjectResult)res).StatusCode);

            DBHelper.DeleteCreatedDatabase(modelContext);
        }

        [Test]
        public void Cant_Post_Game_With_Wrong_StoryTellerId()
        {
            // Arrange
            string fileName = DBHelper.GetCurrentMethodName();
            var modelContext = DBHelper.GetCleanContext(fileName);

            long editionId = EditionHelper.GetEditions(modelContext).First().Id;
            var playersIdRolesId = GameHelper.GetCorrectPlayersIdRolesId(modelContext);
            var rolesId = RoleHelper.GetRoles(modelContext).Take(3).Select(r => r.Id).ToList();

            // Act
            var res = GameHelper.PostGame(modelContext, editionId, -1, DateTime.Now, Alignment.Good, playersIdRolesId, rolesId);

            // Assert
            Assert.AreEqual(StatusCodes.Status400BadRequest, ((ObjectResult)res).StatusCode);

            DBHelper.DeleteCreatedDatabase(modelContext);
        }

        [Test]
        public void Cant_Post_Game_Without_DatePlayed()
        {
            // Arrange
            string fileName = DBHelper.GetCurrentMethodName();
            var modelContext = DBHelper.GetCleanContext(fileName);

            long editionId = EditionHelper.GetEditions(modelContext).First().Id;
            long storyTellerId = PlayerHelper.GetPlayers(modelContext).First().Id;
            var playersIdRolesId = GameHelper.GetCorrectPlayersIdRolesId(modelContext);
            var rolesId = RoleHelper.GetRoles(modelContext).Take(3).Select(r => r.Id).ToList();

            // Act
            var res = GameHelper.PostGame(modelContext, editionId, storyTellerId, null, Alignment.Good, playersIdRolesId, rolesId);

            // Assert
            Assert.AreEqual(StatusCodes.Status400BadRequest, ((ObjectResult)res).StatusCode);

            DBHelper.DeleteCreatedDatabase(modelContext);
        }

        [Test]
        public void Cant_Post_Game_Without_WinningAlignment()
        {
            // Arrange
            string fileName = DBHelper.GetCurrentMethodName();
            var modelContext = DBHelper.GetCleanContext(fileName);

            long editionId = EditionHelper.GetEditions(modelContext).First().Id;
            var playersIdRolesId = GameHelper.GetCorrectPlayersIdRolesId(modelContext);
            var rolesId = RoleHelper.GetRoles(modelContext).Take(3).Select(r => r.Id).ToList();

            // Act
            var res = GameHelper.PostGame(modelContext, editionId, null, DateTime.Now, null, playersIdRolesId, rolesId);

            // Assert
            Assert.AreEqual(StatusCodes.Status400BadRequest, ((ObjectResult)res).StatusCode);

            DBHelper.DeleteCreatedDatabase(modelContext);
        }

        [Test]
        public void Cant_Post_Game_With_Wrong_WinningAlignment()
        {
            // Arrange
            string fileName = DBHelper.GetCurrentMethodName();
            var modelContext = DBHelper.GetCleanContext(fileName);

            long editionId = EditionHelper.GetEditions(modelContext).First().Id;
            long storyTellerId = PlayerHelper.GetPlayers(modelContext).First().Id;
            var playersIdRolesId = GameHelper.GetCorrectPlayersIdRolesId(modelContext);
            var rolesId = RoleHelper.GetRoles(modelContext).Take(3).Select(r => r.Id).ToList();

            // Act
            var res = GameHelper.PostGame(modelContext, editionId, storyTellerId, DateTime.Now, (Alignment)3, playersIdRolesId, rolesId);

            // Assert
            Assert.AreEqual(StatusCodes.Status400BadRequest, ((ObjectResult)res).StatusCode);

            DBHelper.DeleteCreatedDatabase(modelContext);
        }

        [Test]
        public void Cant_Post_Game_Without_PlayerRoles()
        {
            // Arrange
            string fileName = DBHelper.GetCurrentMethodName();
            var modelContext = DBHelper.GetCleanContext(fileName);

            long editionId = EditionHelper.GetEditions(modelContext).First().Id;
            long storyTellerId = PlayerHelper.GetPlayers(modelContext).First().Id;
            var playersIdRolesId = GameHelper.GetCorrectPlayersIdRolesId(modelContext);
            var rolesId = RoleHelper.GetRoles(modelContext).Take(3).Select(r => r.Id).ToList();

            // Act
            var res = GameHelper.PostGame(modelContext, editionId, storyTellerId, DateTime.Now, Alignment.Good, null, rolesId);

            // Assert
            Assert.AreEqual(StatusCodes.Status400BadRequest, ((ObjectResult)res).StatusCode);

            DBHelper.DeleteCreatedDatabase(modelContext);
        }

        [Test]
        public void Cant_Post_Game_With_Wrong_PlayerRole_Player_Id()
        {
            // Arrange
            string fileName = DBHelper.GetCurrentMethodName();
            var modelContext = DBHelper.GetCleanContext(fileName);

            long editionId = EditionHelper.GetEditions(modelContext).First().Id;
            long storyTellerId = PlayerHelper.GetPlayers(modelContext).First().Id;
            var playersIdRolesId = GameHelper.GetCorrectPlayersIdRolesId(modelContext);
            playersIdRolesId.Add(new Entities.PlayerIdRoleId(-1, modelContext.Roles.First().RoleId, Alignment.Good));
            var rolesId = RoleHelper.GetRoles(modelContext).Take(3).Select(r => r.Id).ToList();

            // Act
            var res = GameHelper.PostGame(modelContext, editionId, storyTellerId, DateTime.Now, Alignment.Good, playersIdRolesId, rolesId);

            // Assert
            Assert.AreEqual(StatusCodes.Status400BadRequest, ((ObjectResult)res).StatusCode);

            DBHelper.DeleteCreatedDatabase(modelContext);
        }

        [Test]
        public void Cant_Post_Game_With_Wrong_PlayerRole_Role_Id()
        {
            // Arrange
            string fileName = DBHelper.GetCurrentMethodName();
            var modelContext = DBHelper.GetCleanContext(fileName);

            long editionId = EditionHelper.GetEditions(modelContext).First().Id;
            long storyTellerId = PlayerHelper.GetPlayers(modelContext).First().Id;
            var playersIdRolesId = GameHelper.GetCorrectPlayersIdRolesId(modelContext);
            playersIdRolesId.Add(new Entities.PlayerIdRoleId(modelContext.Players.OrderBy(p => p.PlayerId).Last().PlayerId, -1, Alignment.Evil));
            var rolesId = RoleHelper.GetRoles(modelContext).Take(3).Select(r => r.Id).ToList();

            // Act
            var res = GameHelper.PostGame(modelContext, editionId, storyTellerId, DateTime.Now, Alignment.Good, playersIdRolesId, rolesId);

            // Assert
            Assert.AreEqual(StatusCodes.Status400BadRequest, ((ObjectResult)res).StatusCode);

            DBHelper.DeleteCreatedDatabase(modelContext);
        }

        [Test]
        public void Can_Update_Game()
        {
            // Arrange
            string fileName = DBHelper.GetCurrentMethodName();
            var modelContext = DBHelper.GetCleanContext(fileName);
            GameHelper.DeleteAllGames(modelContext);

            #region Create game

            long editionId = EditionHelper.GetEditions(modelContext).First().Id;
            long storyTellerId = PlayerHelper.GetPlayers(modelContext).First().Id;
            var playersIdRolesId = GameHelper.GetCorrectPlayersIdRolesId(modelContext);
            var rolesId = RoleHelper.GetRoles(modelContext).Take(3).Select(r => r.Id).ToList();

            // Act
            var res = GameHelper.PostGame(modelContext, editionId, storyTellerId, DateTime.Now, Alignment.Good, playersIdRolesId, rolesId);

            // Assert
            Assert.AreEqual(StatusCodes.Status201Created, ((ObjectResult)res).StatusCode);

            #endregion

            #region Update game

            var gameId = GameHelper.GetGames(modelContext).First().Id;

            res = GameHelper.UpdateGame(modelContext, gameId, editionId, storyTellerId + 1, DateTime.Now, Alignment.Good, playersIdRolesId, rolesId);
            Assert.AreEqual(StatusCodes.Status201Created, ((ObjectResult)res).StatusCode);
            Assert.AreEqual(storyTellerId + 1, GameHelper.GetGame(modelContext, gameId).StoryTeller.Id);

            #endregion

            DBHelper.DeleteCreatedDatabase(modelContext);
        }

        [Test]
        public void Cant_Update_Game_Without_GameId()
        {
            // Arrange
            string fileName = DBHelper.GetCurrentMethodName();
            var modelContext = DBHelper.GetCleanContext(fileName);
            GameHelper.DeleteAllGames(modelContext);

            #region Create game

            long editionId = EditionHelper.GetEditions(modelContext).First().Id;
            long storyTellerId = PlayerHelper.GetPlayers(modelContext).First().Id;
            var playersIdRolesId = GameHelper.GetCorrectPlayersIdRolesId(modelContext);
            var rolesId = RoleHelper.GetRoles(modelContext).Take(3).Select(r => r.Id).ToList();

            // Act
            var res = GameHelper.PostGame(modelContext, editionId, storyTellerId, DateTime.Now, Alignment.Good, playersIdRolesId, rolesId);

            // Assert
            Assert.AreEqual(StatusCodes.Status201Created, ((ObjectResult)res).StatusCode);

            #endregion

            #region Update game

            res = GameHelper.UpdateGame(modelContext, null, editionId, storyTellerId, DateTime.Now, Alignment.Good, playersIdRolesId, rolesId);
            Assert.AreEqual(StatusCodes.Status400BadRequest, ((ObjectResult)res).StatusCode);

            #endregion

            DBHelper.DeleteCreatedDatabase(modelContext);
        }

        [Test]
        public void Can_Delete_Game()
        {
            // Arrange
            string fileName = DBHelper.GetCurrentMethodName();
            var modelContext = DBHelper.GetCleanContext(fileName);
            GameHelper.DeleteAllGames(modelContext);

            var editionId = EditionHelper.GetEditions(modelContext).First().Id;
            var storyTellerId = PlayerHelper.GetPlayers(modelContext).First().Id;
            var alignment = Alignment.Good;
            var playersIdRolesId = GameHelper.GetCorrectPlayersIdRolesId(modelContext);
            var rolesId = RoleHelper.GetRoles(modelContext).Take(3).Select(r => r.Id).ToList();

            var res = GameHelper.PostGame(modelContext, editionId, storyTellerId, DateTime.Now, alignment, playersIdRolesId, rolesId);
            Assert.AreEqual(StatusCodes.Status201Created, ((ObjectResult)res).StatusCode);

            var gameId = GameHelper.GetGames(modelContext).First().Id;

            // Act
            res = GameHelper.DeleteGame(modelContext, gameId);
            Assert.AreEqual(StatusCodes.Status202Accepted, ((ObjectResult)res).StatusCode);

            Assert.AreEqual(0, GameHelper.GetGames(modelContext).Count());
            DBHelper.DeleteCreatedDatabase(modelContext);
        }

        [Test]
        public void Can_Get_Games_Played_by_Player()
        {
            // Arrange
            string fileName = DBHelper.GetCurrentMethodName();
            var modelContext = DBHelper.GetCleanContext(fileName);

            var editionId = EditionHelper.GetEditions(modelContext).First().Id;
            var storyTellerId = PlayerHelper.GetPlayers(modelContext).First().Id;
            var alignment = Alignment.Good;
            var playersIdRolesId = GameHelper.GetCorrectPlayersIdRolesId(modelContext);
            var rolesId = RoleHelper.GetRoles(modelContext).Take(3).Select(r => r.Id).ToList();

            var res = GameHelper.PostGame(modelContext, editionId, storyTellerId, DateTime.Now, alignment, playersIdRolesId, rolesId);
            Assert.AreEqual(StatusCodes.Status201Created, ((ObjectResult)res).StatusCode);

            // Act
            var games = GameHelper.GetGamesByPlayer(modelContext, modelContext.Players.First().PlayerId);
            Assert.AreEqual(1, games.Count());

            games = GameHelper.GetGamesByPlayer(modelContext, modelContext.Players.ToList().Last().PlayerId);
            Assert.AreEqual(0, games.Count());
            DBHelper.DeleteCreatedDatabase(modelContext);
        }

        [Test]
        public void Can_Get_Games_Storytelled_By_StoryTeller()
        {
            // Arrange
            string fileName = DBHelper.GetCurrentMethodName();
            var modelContext = DBHelper.GetCleanContext(fileName);

            var editionId = EditionHelper.GetEditions(modelContext).First().Id;
            var storyTellerId = PlayerHelper.GetPlayers(modelContext).First().Id;
            var alignment = Alignment.Good;
            var playersIdRolesId = GameHelper.GetCorrectPlayersIdRolesId(modelContext);
            var rolesId = RoleHelper.GetRoles(modelContext).Take(3).Select(r => r.Id).ToList();

            var res = GameHelper.PostGame(modelContext, editionId, storyTellerId, DateTime.Now, alignment, playersIdRolesId, rolesId);
            Assert.AreEqual(StatusCodes.Status201Created, ((ObjectResult)res).StatusCode);

            // Act
            var games = GameHelper.GetGamesByStoryteller(modelContext, storyTellerId);
            Assert.AreEqual(1, games.Count());

            games = GameHelper.GetGamesByStoryteller(modelContext, modelContext.Players.ToList().Last().PlayerId);
            Assert.AreEqual(0, games.Count());
            DBHelper.DeleteCreatedDatabase(modelContext);
        }
    }
}
