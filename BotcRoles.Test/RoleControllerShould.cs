using BotcRoles.Enums;
using BotcRoles.Test.HelperMethods;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace BotcRoles.Test
{
    [TestFixture]
    public class RoleControllerShould
    {
        [Test]
        public void Post_And_Get_Role()
        {
            // Arrange
            string fileName = DBHelper.GetCurrentMethodName();
            var modelContext = DBHelper.GetCleanContext(fileName);
            string roleName = "RoleName";
            RoleHelper.DeleteAllRoles(modelContext);

            // Act
            var res = RoleHelper.AddRole(modelContext, roleName, CharacterType.Demon);

            // Assert
            Assert.AreEqual(StatusCodes.Status201Created, ((ObjectResult)res).StatusCode);

            //Act
            long roleId = RoleHelper.GetRoles(modelContext).Last().Id;

            // Assert
            Assert.AreEqual(roleName, RoleHelper.GetRoleById(modelContext, roleId).Name);

            DBHelper.DeleteCreatedDatabase(modelContext);
        }

        [Test]
        public void Get_Role_By_Id()
        {
            // Arrange
            string fileName = DBHelper.GetCurrentMethodName();
            var modelContext = DBHelper.GetCleanContext(fileName);
            var roleId = RoleHelper.GetRoles(modelContext).First().Id;

            // Act
            var res = RoleHelper.GetRoleById(modelContext, roleId);

            // Assert
            Assert.IsNotNull(res);

            DBHelper.DeleteCreatedDatabase(modelContext);
        }

        [Test]
        public void Cant_Post_Two_Roles_With_Same_Name()
        {
            // Arrange
            string fileName = DBHelper.GetCurrentMethodName();
            var modelContext = DBHelper.GetCleanContext(fileName);
            string roleName = "RoleName";

            // Act
            RoleHelper.AddRole(modelContext, roleName, CharacterType.Demon);
            var res = RoleHelper.AddRole(modelContext, roleName, CharacterType.Townsfolk);
            Assert.AreEqual(StatusCodes.Status400BadRequest, ((ObjectResult)res).StatusCode);

            DBHelper.DeleteCreatedDatabase(modelContext);
        }

        [Test]
        public void Cant_Post_Role_With_Empty_Name()
        {
            // Arrange
            string fileName = DBHelper.GetCurrentMethodName();
            var modelContext = DBHelper.GetCleanContext(fileName);
            string roleName = string.Empty;

            // Act
            var res = RoleHelper.AddRole(modelContext, roleName, CharacterType.Demon);
            Assert.AreEqual(StatusCodes.Status400BadRequest, ((BadRequestObjectResult)res).StatusCode);

            DBHelper.DeleteCreatedDatabase(modelContext);
        }

        [Test]
        public void Cant_Post_Role_With_Empty_Type()
        {
            // Arrange
            string fileName = DBHelper.GetCurrentMethodName();
            var modelContext = DBHelper.GetCleanContext(fileName);
            string roleName = "RoleName";

            // Act
            var res = RoleHelper.AddRole(modelContext, roleName, null);
            Assert.AreEqual(StatusCodes.Status400BadRequest, ((BadRequestObjectResult)res).StatusCode);

            DBHelper.DeleteCreatedDatabase(modelContext);
        }

        [Test]
        public void Can_Update_Role()
        {
            // Arrange
            string fileName = DBHelper.GetCurrentMethodName();
            var modelContext = DBHelper.GetCleanContext(fileName);
            RoleHelper.DeleteAllRoles(modelContext);
            string roleName = "roleName";
            var res = RoleHelper.AddRole(modelContext, roleName, CharacterType.Demon);

            // Act
            var roleId = RoleHelper.GetRoles(modelContext).First().Id;
            Assert.AreEqual(StatusCodes.Status201Created, ((ObjectResult)res).StatusCode);

            string newName = "newName";
            CharacterType characterType = CharacterType.Townsfolk;
            res = RoleHelper.UpdateRole(modelContext, roleId, newName, characterType);
            Assert.AreEqual(StatusCodes.Status201Created, ((ObjectResult)res).StatusCode);

            var role = RoleHelper.GetRoleById(modelContext, roleId);
            Assert.AreEqual(roleId, role.Id);
            Assert.AreEqual(newName, role.Name);
            Assert.AreEqual(characterType, role.CharacterType);
            Assert.AreEqual(1, modelContext.Roles.Count());


            DBHelper.DeleteCreatedDatabase(modelContext);
        }

        [Test]
        public void Can_Delete_Role_Not_In_PlayerRoleGame()
        {
            // Arrange
            string fileName = DBHelper.GetCurrentMethodName();
            var modelContext = DBHelper.GetCleanContext(fileName, false);

            string roleName = "playerName";
            CharacterType characterType = CharacterType.Townsfolk;

            var res = RoleHelper.AddRole(modelContext, roleName, characterType);
            Assert.AreEqual(StatusCodes.Status201Created, ((ObjectResult)res).StatusCode);

            var roleId = RoleHelper.GetRoles(modelContext).First().Id;

            // Act
            res = RoleHelper.DeleteRole(modelContext, roleId);
            Assert.AreEqual(StatusCodes.Status202Accepted, ((ObjectResult)res).StatusCode);

            Assert.AreEqual(0, RoleHelper.GetRoles(modelContext).Count());
        }

        [Test]
        public void Delete_Role_In_Edition_Removes_It_From_Edition()
        {
            // Arrange
            string fileName = DBHelper.GetCurrentMethodName();
            var modelContext = DBHelper.GetCleanContext(fileName, false);

            string roleName = "playerName";
            CharacterType characterType = CharacterType.Townsfolk;

            RoleHelper.AddRole(modelContext, roleName, characterType);
            var allRolesId = modelContext.Roles.Select(r => r.RoleId).ToList();
            EditionHelper.PostEdition(modelContext, "editionName", allRolesId);

            Assert.AreEqual(1, EditionHelper.GetEditions(modelContext).First().Roles.Count());

            var roleId = RoleHelper.GetRoles(modelContext).First().Id;

            // Act
            var res = RoleHelper.DeleteRole(modelContext, roleId);
            Assert.AreEqual(StatusCodes.Status202Accepted, ((ObjectResult)res).StatusCode);

            Assert.AreEqual(0, RoleHelper.GetRoles(modelContext).Count());
            Assert.AreEqual(0, EditionHelper.GetEditions(modelContext).First().Roles.Count());
        }

        [Test]
        public void Delete_Role_In_PlayerRoleGame_Sets_Hidden()
        {
            // Arrange
            string fileName = DBHelper.GetCurrentMethodName();
            var modelContext = DBHelper.GetCleanContext(fileName, false);
            DBHelper.CreateBasicDataInAllTables(modelContext);

            modelContext.PlayerRoleGames.Add(new Models.PlayerRoleGame(
                modelContext.Players.First(),
                modelContext.Roles.First(),
                modelContext.Games.First()));
            modelContext.SaveChanges();

            foreach (var role in modelContext.Roles.ToList())
            {
                var res = RoleHelper.DeleteRole(modelContext, role.RoleId);
                Assert.AreEqual(StatusCodes.Status202Accepted, ((ObjectResult)res).StatusCode);
            }

            Assert.AreEqual(0, RoleHelper.GetRoles(modelContext).Count());
            Assert.AreEqual(1, modelContext.Roles.Count());
            Assert.IsTrue(modelContext.Roles.All(p => p.IsHidden));
        }
    }
}
