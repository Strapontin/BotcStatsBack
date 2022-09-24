using BotcRoles.Test.HelperMethods;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace BotcRoles.Test
{
    [TestFixture]
    public class ModuleControllerShould
    {
        [Test]
        public void PostAndGet_Module()
        {
            // Arrange
            string fileName = Helper.GetCurrentMethodName() + ".db";
            var modelContext = Helper.GetContext(fileName);
            string moduleName = "ModuleName";

            // Act
            var res = (ObjectResult)ModuleHelper.PostModule(modelContext, moduleName);

            // Assert
            Assert.IsNotNull(res);
            Assert.AreEqual(201, res.StatusCode);

            // Act
            long moduleId = ModuleHelper.GetModule(modelContext).First().ModuleId;
            Assert.AreEqual(moduleName, ModuleHelper.GetModule(modelContext, moduleId).Name);

            Helper.DeleteCreatedDatabase(modelContext);
        }

        [Test]
        public void CantPostTwoModulesWithSameName()
        {
            // Arrange
            string fileName = Helper.GetCurrentMethodName() + ".db";
            var modelContext = Helper.GetContext(fileName);
            string moduleName = "ModuleName";

            // Act
            ModuleHelper.PostModule(modelContext, moduleName);
            var res = (ObjectResult)ModuleHelper.PostModule(modelContext, moduleName);
            Assert.IsNotNull(res);
            Assert.AreEqual(400, res.StatusCode);

            Helper.DeleteCreatedDatabase(modelContext);
        }

        [Test]
        public void CantPostModuleWithEmptyName()
        {
            // Arrange
            string fileName = Helper.GetCurrentMethodName() + ".db";
            var modelContext = Helper.GetContext(fileName);
            string moduleName = string.Empty;

            // Act
            var res = (ObjectResult)ModuleHelper.PostModule(modelContext, moduleName);
            Assert.IsNotNull(res);
            Assert.AreEqual(400, res.StatusCode);

            Helper.DeleteCreatedDatabase(modelContext);
        }
    }
}
