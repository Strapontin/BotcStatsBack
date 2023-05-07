using BotcRoles.Test.HelperMethods;
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
            string fileName = Helper.GetCurrentMethodName() + ".db";
            var modelContext = Helper.GetContext(fileName);
            string playerName = "PlayerName";

            try
            {
                // Act
                var res = PlayerHelper.PostPlayer(modelContext, playerName);

                // Assert
                Assert.AreEqual(StatusCodes.Status201Created, ((CreatedResult)res).StatusCode);

                // Act
                Assert.AreEqual(playerName, PlayerHelper.GetPlayer(modelContext, playerName).Name);
            }
            finally
            {
                Helper.DeleteCreatedDatabase(modelContext);
            }
        }

        [Test]
        public void Cant_Post_Two_Players_With_Same_Name()
        {
            // Arrange
            string fileName = Helper.GetCurrentMethodName() + ".db";
            var modelContext = Helper.GetContext(fileName);
            string playerName = "PlayerName";

            // Act
            PlayerHelper.PostPlayer(modelContext, playerName);
            var res = PlayerHelper.PostPlayer(modelContext, playerName);
            Assert.AreEqual(StatusCodes.Status400BadRequest, ((BadRequestObjectResult)res).StatusCode);

            Helper.DeleteCreatedDatabase(modelContext);
        }

        [Test]
        public void Cant_Post_Player_With_Empty_Name()
        {
            // Arrange
            string fileName = Helper.GetCurrentMethodName() + ".db";
            var modelContext = Helper.GetContext(fileName);
            string playerName = string.Empty;

            // Act
            var res = PlayerHelper.PostPlayer(modelContext, playerName);
            Assert.AreEqual(StatusCodes.Status400BadRequest, ((BadRequestObjectResult)res).StatusCode);

            Helper.DeleteCreatedDatabase(modelContext);
        }
    }
}
