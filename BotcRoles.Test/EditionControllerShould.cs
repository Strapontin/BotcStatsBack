using BotcRoles.Entities;
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
            var modelContext = DBHelper.GetCleanContext(fileName);

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
            var modelContext = DBHelper.GetCleanContext(fileName);

            var editionId = EditionHelper.GetEditions(modelContext).First().Id;

            // Act
            var res = EditionHelper.GetEdition(modelContext, editionId);

            // Assert 
            Assert.IsTrue(res != null);

            DBHelper.DeleteCreatedDatabase(modelContext);
        }

        [Test]
        public void Post_And_Get_Edition()
        {
            // Arrange
            string fileName = DBHelper.GetCurrentMethodName() + ".db";
            var modelContext = DBHelper.GetCleanContext(fileName, false);
            string editionName = "EditionName";

            // Act
            var res = EditionHelper.PostEdition(modelContext, editionName);

            // Assert
            Assert.AreEqual(StatusCodes.Status201Created, ((ObjectResult)res).StatusCode);

            var editionId = EditionHelper.GetEditions(modelContext).First().Id;

            // Act
            Assert.AreEqual(editionName, EditionHelper.GetEdition(modelContext, editionId).Name);

            DBHelper.DeleteCreatedDatabase(modelContext);
        }

        [Test]
        public void Cant_Post_Two_Editions_With_Same_Name()
        {
            // Arrange
            string fileName = DBHelper.GetCurrentMethodName() + ".db";
            var modelContext = DBHelper.GetCleanContext(fileName);
            string editionName = "EditionName";

            // Act
            EditionHelper.PostEdition(modelContext, editionName);
            var res = EditionHelper.PostEdition(modelContext, editionName + " ");
            Assert.AreEqual(StatusCodes.Status400BadRequest, ((ObjectResult)res).StatusCode);

            DBHelper.DeleteCreatedDatabase(modelContext);
        }

        [Test]
        public void Cant_Post_Edition_With_Empty_Name()
        {
            // Arrange
            string fileName = DBHelper.GetCurrentMethodName() + ".db";
            var modelContext = DBHelper.GetCleanContext(fileName);
            string editionName = string.Empty;

            // Act
            var res = EditionHelper.PostEdition(modelContext, editionName);
            Assert.AreEqual(StatusCodes.Status400BadRequest, ((ObjectResult)res).StatusCode);

            DBHelper.DeleteCreatedDatabase(modelContext);
        }

        [Test]
        public void Post_Edition_And_Assign_Roles()
        {
            // Arrange
            string fileName = DBHelper.GetCurrentMethodName() + ".db";
            var modelContext = DBHelper.GetCleanContext(fileName);
            string editionName = "EditionName";
            EditionHelper.DeleteAllEditions(modelContext);

            List<long> roles = new()
            {
                modelContext.Roles.First().RoleId,
                modelContext.Roles.Skip(1).First().RoleId,
                modelContext.Roles.Skip(2).First().RoleId,
            };

            // Act
            var res = EditionHelper.PostEdition(modelContext, editionName, roles);

            var editionId = EditionHelper.GetEditions(modelContext).First().Id;

            // Assert
            Assert.AreEqual(StatusCodes.Status201Created, ((ObjectResult)res).StatusCode);
            Assert.AreEqual(editionName, EditionHelper.GetEdition(modelContext, editionId).Name);

            Assert.AreEqual(3, modelContext.RolesEdition.Count(re => re.EditionId == editionId));

            DBHelper.DeleteCreatedDatabase(modelContext);
        }

        [Test]
        public void Post_Edition_And_Assign_Non_Existing_Roles_Should_Fail()
        {
            // Arrange
            string fileName = DBHelper.GetCurrentMethodName() + ".db";
            var modelContext = DBHelper.GetCleanContext(fileName);
            string editionName = "EditionName";
            EditionHelper.DeleteAllEditions(modelContext);

            List<long> roles = new()
            {
                -1,
                modelContext.Roles.Skip(1).First().RoleId,
                modelContext.Roles.Skip(2).First().RoleId,
            };

            // Act
            var res = EditionHelper.PostEdition(modelContext, editionName, roles);

            // Assert
            Assert.AreEqual(StatusCodes.Status400BadRequest, ((ObjectResult)res).StatusCode);
            Assert.IsEmpty(EditionHelper.GetEditions(modelContext));

            DBHelper.DeleteCreatedDatabase(modelContext);
        }

        [Test]
        public void Can_Update_Edition()
        {
            // Arrange
            string fileName = DBHelper.GetCurrentMethodName() + ".db";
            var modelContext = DBHelper.GetCleanContext(fileName);
            EditionHelper.DeleteAllEditions(modelContext);
            string editionName = "editionName";
            List<long> rolesId = modelContext.Roles.Take(5).Select(r => r.RoleId).ToList();
            var res = EditionHelper.PostEdition(modelContext, editionName);

            // Act
            var editionId = EditionHelper.GetEditions(modelContext).First().Id;

            string newName = "newName";
            res = EditionHelper.UpdateEdition(modelContext, editionId, newName, rolesId);
            Assert.AreEqual(StatusCodes.Status201Created, ((ObjectResult)res).StatusCode);

            var edition = EditionHelper.GetEdition(modelContext, editionId);
            Assert.AreEqual(editionId, edition.Id);
            Assert.AreEqual(newName, edition.Name);
            Assert.AreEqual(5, edition.Roles.Count);
            Assert.True(edition.Roles.All(r => !string.IsNullOrWhiteSpace(r.Name)));


            DBHelper.DeleteCreatedDatabase(modelContext);
        }

        [Test]
        public void Can_Update_Edition_With_Same_Name()
        {
            // Arrange
            string fileName = DBHelper.GetCurrentMethodName() + ".db";
            var modelContext = DBHelper.GetCleanContext(fileName);
            EditionHelper.DeleteAllEditions(modelContext);
            string editionName = "editionName";
            List<long> rolesId = modelContext.Roles.Take(5).Select(r => r.RoleId).ToList();
            var res = EditionHelper.PostEdition(modelContext, editionName);

            // Act
            var editionId = EditionHelper.GetEditions(modelContext).First().Id;

            res = EditionHelper.UpdateEdition(modelContext, editionId, editionName, rolesId);
            Assert.AreEqual(StatusCodes.Status201Created, ((ObjectResult)res).StatusCode);

            var edition = EditionHelper.GetEdition(modelContext, editionId);
            Assert.AreEqual(editionId, edition.Id);
            Assert.AreEqual(editionName, edition.Name);
            Assert.AreEqual(5, edition.Roles.Count);
            Assert.True(edition.Roles.All(r => !string.IsNullOrWhiteSpace(r.Name)));


            DBHelper.DeleteCreatedDatabase(modelContext);
        }

        [Test]
        public void Can_Delete_Edition_Not_In_Game()
        {
            // Arrange
            string fileName = DBHelper.GetCurrentMethodName() + ".db";
            var modelContext = DBHelper.GetCleanContext(fileName, false);


            string editionName = "editionName";
            var res = EditionHelper.PostEdition(modelContext, editionName);
            Assert.AreEqual(StatusCodes.Status201Created, ((ObjectResult)res).StatusCode);

            var playerId = EditionHelper.GetEditions(modelContext).First().Id;

            // Act
            res = EditionHelper.DeleteEdition(modelContext, playerId);
            Assert.AreEqual(StatusCodes.Status202Accepted, ((ObjectResult)res).StatusCode);

            Assert.AreEqual(0, EditionHelper.GetEditions(modelContext).Count());
        }

        [Test]
        public void Delete_Edition_In_Game_Sets_Hidden()
        {
            // Arrange
            string fileName = DBHelper.GetCurrentMethodName() + ".db";
            var modelContext = DBHelper.GetCleanContext(fileName, false);
            DBHelper.CreateBasicDataInAllTables(modelContext);
            int countEditions = modelContext.Editions.Count();

            foreach (var player in modelContext.Editions)
            {
                var res = EditionHelper.DeleteEdition(modelContext, player.EditionId);
                Assert.AreEqual(StatusCodes.Status202Accepted, ((ObjectResult)res).StatusCode);
            }

            Assert.AreEqual(0, EditionHelper.GetEditions(modelContext).Count());
            Assert.AreEqual(countEditions, modelContext.Editions.Count());
            Assert.IsTrue(modelContext.Editions.All(p => p.IsHidden));
        }
    }
}
