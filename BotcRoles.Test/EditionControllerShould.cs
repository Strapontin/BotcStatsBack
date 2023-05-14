using BotcRoles.Enums;
using BotcRoles.Test.HelperMethods;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace BotcRoles.Test
{
    [TestFixture]
    public class EditionControllerShould
    {
        [Test]
        public void Get_Editions()
        {
            // Arrange
            string fileName = DBHelper.GetCurrentMethodName() + ".db";
            var modelContext = DBHelper.GetContext(fileName);

            // Act
            var res = EditionHelper.GetEditions(modelContext);

            // Assert 
            Assert.IsTrue(res.Any());

            DBHelper.DeleteCreatedDatabase(modelContext);
        }

        [Test]
        public void Get_Edition()
        {
            // Arrange
            string fileName = DBHelper.GetCurrentMethodName() + ".db";
            var modelContext = DBHelper.GetContext(fileName);

            string editionName = EditionHelper.GetEditions(modelContext).First().Name;

            // Act
            var res = EditionHelper.GetEdition(modelContext, editionName);

            // Assert 
            Assert.IsTrue(res != null);

            DBHelper.DeleteCreatedDatabase(modelContext);
        }

        //[Test]
        //public void Post_And_Get_Edition()
        //{
        //    // Arrange
        //    string fileName = DBHelper.GetCurrentMethodName() + ".db";
        //    var modelContext = DBHelper.GetContext(fileName);
        //    string editionName = "EditionName";

        //    // Act
        //    var res = EditionHelper.PostEdition(modelContext, editionName);

        //    // Assert
        //    Assert.AreEqual(StatusCodes.Status201Created, ((CreatedResult)res).StatusCode);

        //    // Act
        //    Assert.AreEqual(editionName, EditionHelper.GetEdition(modelContext, editionName).Name);

        //    DBHelper.DeleteCreatedDatabase(modelContext);
        //}

        //[Test]
        //public void Cant_Post_Two_Editions_With_Same_Name()
        //{
        //    // Arrange
        //    string fileName = DBHelper.GetCurrentMethodName() + ".db";
        //    var modelContext = DBHelper.GetContext(fileName);
        //    string editionName = "EditionName";

        //    // Act
        //    EditionHelper.PostEdition(modelContext, editionName);
        //    var res = EditionHelper.PostEdition(modelContext, editionName);
        //    Assert.AreEqual(StatusCodes.Status400BadRequest, ((BadRequestObjectResult)res).StatusCode);

        //    DBHelper.DeleteCreatedDatabase(modelContext);
        //}

        //[Test]
        //public void Cant_Post_Edition_With_Empty_Name()
        //{
        //    // Arrange
        //    string fileName = DBHelper.GetCurrentMethodName() + ".db";
        //    var modelContext = DBHelper.GetContext(fileName);
        //    string editionName = string.Empty;

        //    // Act
        //    var res = EditionHelper.PostEdition(modelContext, editionName);
        //    Assert.AreEqual(StatusCodes.Status400BadRequest, ((BadRequestObjectResult)res).StatusCode);

        //    DBHelper.DeleteCreatedDatabase(modelContext);
        //}

        //[Test]
        //public void Add_And_Get_Role_In_Edition()
        //{
        //    // Arrange
        //    string fileName = DBHelper.GetCurrentMethodName() + ".db";
        //    var modelContext = DBHelper.GetContext(fileName);
        //    string editionName = "EditionName";
        //    string roleName = "RoleName";

        //    try
        //    {
        //        EditionHelper.PostEdition(modelContext, editionName);
        //        long editionId = EditionHelper.GetEditions(modelContext).First().Id;

        //        RoleHelper.AddRole(modelContext, roleName, CharacterType.Demon, Alignment.Good);
        //        long roleId = RoleHelper.GetRoles(modelContext).First().Id;

        //        // Act
        //        var res = EditionHelper.AddRoleInEdition(modelContext, editionId, roleId);

        //        // Assert
        //        Assert.AreEqual(StatusCodes.Status201Created, ((CreatedResult)res).StatusCode);

        //        // Act
        //        var rolesInEdition = EditionHelper.GetRolesFromEdition(modelContext, editionId);

        //        // Assert
        //        Assert.AreEqual(roleName, rolesInEdition.First().Role.Name);
        //    }
        //    finally
        //    {
        //        DBHelper.DeleteCreatedDatabase(modelContext);
        //    }
        //}

        //[Test]
        //public void Cant_Add_Twice_Same_Role_In_Edition()
        //{
        //    // Arrange
        //    string fileName = DBHelper.GetCurrentMethodName() + ".db";
        //    var modelContext = DBHelper.GetContext(fileName);
        //    string editionName = "EditionName";
        //    string roleName = "RoleName";

        //    try
        //    {
        //        EditionHelper.PostEdition(modelContext, editionName);
        //        long editionId = EditionHelper.GetEditions(modelContext).First().Id;

        //        RoleHelper.AddRole(modelContext, roleName, CharacterType.Demon, Alignment.Good);
        //        long roleId = RoleHelper.GetRoles(modelContext).First().Id;

        //        // Act
        //        var res1 = EditionHelper.AddRoleInEdition(modelContext, editionId, roleId);
        //        var res2 = EditionHelper.AddRoleInEdition(modelContext, editionId, roleId);

        //        // Assert
        //        Assert.AreEqual(StatusCodes.Status201Created, ((CreatedResult)res1).StatusCode);
        //        Assert.AreEqual(StatusCodes.Status400BadRequest, ((BadRequestObjectResult)res2).StatusCode);
        //    }
        //    finally
        //    {
        //        DBHelper.DeleteCreatedDatabase(modelContext);
        //    }
        //}

        //[Test]
        //public void Cant_Add_Role_In_Edition_With_Wrong_EditionId()
        //{
        //    // Arrange
        //    string fileName = DBHelper.GetCurrentMethodName() + ".db";
        //    var modelContext = DBHelper.GetContext(fileName);
        //    string editionName = "EditionName";
        //    string roleName = "RoleName";

        //    try
        //    {
        //        EditionHelper.PostEdition(modelContext, editionName);
        //        long editionId = EditionHelper.GetEditions(modelContext).First().Id;

        //        RoleHelper.AddRole(modelContext, roleName, CharacterType.Demon, Alignment.Good);
        //        long roleId = RoleHelper.GetRoles(modelContext).First().Id;

        //        // Act
        //        var res = EditionHelper.AddRoleInEdition(modelContext, editionId + 1, roleId);

        //        // Assert
        //        Assert.AreEqual(StatusCodes.Status400BadRequest, ((BadRequestObjectResult)res).StatusCode);
        //    }
        //    finally
        //    {
        //        DBHelper.DeleteCreatedDatabase(modelContext);
        //    }
        //}

        //[Test]
        //public void Cant_Add_Role_In_Edition_With_Wrong_RoleId()
        //{
        //    // Arrange
        //    string fileName = DBHelper.GetCurrentMethodName() + ".db";
        //    var modelContext = DBHelper.GetContext(fileName);
        //    string editionName = "EditionName";
        //    string roleName = "RoleName";

        //    try
        //    {
        //        EditionHelper.PostEdition(modelContext, editionName);
        //        long editionId = EditionHelper.GetEditions(modelContext).First().Id;

        //        RoleHelper.AddRole(modelContext, roleName, CharacterType.Demon, Alignment.Good);
        //        long roleId = RoleHelper.GetRoles(modelContext).First().Id;

        //        // Act
        //        var res = EditionHelper.AddRoleInEdition(modelContext, editionId, roleId + 1);

        //        // Assert
        //        Assert.AreEqual(StatusCodes.Status400BadRequest, ((BadRequestObjectResult)res).StatusCode);
        //    }
        //    finally
        //    {
        //        DBHelper.DeleteCreatedDatabase(modelContext);
        //    }
        //}

        //[Test]
        //public void Remove_Role_From_Edition()
        //{
        //    // Arrange
        //    string fileName = DBHelper.GetCurrentMethodName() + ".db";
        //    var modelContext = DBHelper.GetContext(fileName);
        //    string editionName = "EditionName";
        //    string roleName = "RoleName";

        //    try
        //    {
        //        EditionHelper.PostEdition(modelContext, editionName);
        //        long editionId = EditionHelper.GetEditions(modelContext).First().Id;

        //        RoleHelper.AddRole(modelContext, roleName, CharacterType.Demon, Alignment.Good);
        //        long roleId = RoleHelper.GetRoles(modelContext).First().Id;

        //        EditionHelper.AddRoleInEdition(modelContext, editionId, roleId);

        //        // Act
        //        var res = EditionHelper.RemoveRoleFromEdition(modelContext, editionId, roleId);

        //        // Assert
        //        Assert.AreEqual(StatusCodes.Status200OK, ((OkResult)res).StatusCode);

        //        var roles = EditionHelper.GetRolesFromEdition(modelContext, editionId);
        //        Assert.IsEmpty(roles);
        //    }
        //    finally
        //    {
        //        DBHelper.DeleteCreatedDatabase(modelContext);
        //    }
        //}
    }
}
