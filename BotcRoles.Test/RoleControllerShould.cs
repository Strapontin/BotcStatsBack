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
            string fileName = DBHelper.GetCurrentMethodName() + ".db";
            var modelContext = DBHelper.GetContext(fileName);
            string roleName = "RoleName";
            RoleHelper.DeleteAllRoles(modelContext);

            // Act
            var res = RoleHelper.AddRole(modelContext, roleName, CharacterType.Demon, Alignment.Evil);

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
            string fileName = DBHelper.GetCurrentMethodName() + ".db";
            var modelContext = DBHelper.GetContext(fileName);
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
            string fileName = DBHelper.GetCurrentMethodName() + ".db";
            var modelContext = DBHelper.GetContext(fileName);
            string roleName = "RoleName";

            // Act
            RoleHelper.AddRole(modelContext, roleName, CharacterType.Demon, Alignment.Evil);
            var res = RoleHelper.AddRole(modelContext, roleName, CharacterType.Townsfolk, Alignment.Good);
            Assert.AreEqual(StatusCodes.Status400BadRequest, ((ObjectResult)res).StatusCode);

            DBHelper.DeleteCreatedDatabase(modelContext);
        }

        [Test]
        public void Cant_Post_Role_With_Empty_Name()
        {
            // Arrange
            string fileName = DBHelper.GetCurrentMethodName() + ".db";
            var modelContext = DBHelper.GetContext(fileName);
            string roleName = string.Empty;

            // Act
            var res = RoleHelper.AddRole(modelContext, roleName, CharacterType.Demon, Alignment.Evil);
            Assert.AreEqual(StatusCodes.Status400BadRequest, ((BadRequestObjectResult)res).StatusCode);

            DBHelper.DeleteCreatedDatabase(modelContext);
        }

        [Test]
        public void Cant_Post_Role_With_Empty_Type()
        {
            // Arrange
            string fileName = DBHelper.GetCurrentMethodName() + ".db";
            var modelContext = DBHelper.GetContext(fileName);
            string roleName = "RoleName";

            // Act
            var res = RoleHelper.AddRole(modelContext, roleName, null, Alignment.Evil);
            Assert.AreEqual(StatusCodes.Status400BadRequest, ((BadRequestObjectResult)res).StatusCode);

            DBHelper.DeleteCreatedDatabase(modelContext);
        }

        [Test]
        public void Cant_Post_Role_With_Empty_Alignement()
        {
            // Arrange
            string fileName = DBHelper.GetCurrentMethodName() + ".db";
            var modelContext = DBHelper.GetContext(fileName);
            string roleName = "RoleName";

            // Act
            var res = RoleHelper.AddRole(modelContext, roleName, CharacterType.Demon, null);
            Assert.AreEqual(StatusCodes.Status400BadRequest, ((BadRequestObjectResult)res).StatusCode);

            DBHelper.DeleteCreatedDatabase(modelContext);
        }
    }
}
