using BotcRoles.Controllers;
using BotcRoles.Models;
using BotcRoles.Test.HelperMethods;
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
        public void PostAndGetGameIsOk()
        {
            // Arrange
            string fileName = Helper.GetCurrentMethodName() + ".db";
            var modelContext = Helper.GetContext(fileName);
            GameController gameController = new GameController(null!, modelContext);

            // Act
            //gameController.Post()
            Helper.DeleteCreatedDatabase(modelContext);
        }
    }
}
