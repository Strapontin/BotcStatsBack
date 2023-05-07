using BotcRoles.Enums;
using BotcRoles.Test.HelperMethods;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace BotcRoles.Test
{
    [TestFixture]
    public class ModuleControllerShould
    {
        //[Test]
        //public void Post_And_Get_Module()
        //{
        //    // Arrange
        //    string fileName = Helper.GetCurrentMethodName() + ".db";
        //    var modelContext = Helper.GetContext(fileName);
        //    string moduleName = "ModuleName";

        //    // Act
        //    var res = ModuleHelper.PostModule(modelContext, moduleName);

        //    // Assert
        //    Assert.AreEqual(StatusCodes.Status201Created, ((CreatedResult)res).StatusCode);

        //    // Act
        //    Assert.AreEqual(moduleName, ModuleHelper.GetModule(modelContext, moduleName).Name);

        //    Helper.DeleteCreatedDatabase(modelContext);
        //}

        //[Test]
        //public void Cant_Post_Two_Modules_With_Same_Name()
        //{
        //    // Arrange
        //    string fileName = Helper.GetCurrentMethodName() + ".db";
        //    var modelContext = Helper.GetContext(fileName);
        //    string moduleName = "ModuleName";

        //    // Act
        //    ModuleHelper.PostModule(modelContext, moduleName);
        //    var res = ModuleHelper.PostModule(modelContext, moduleName);
        //    Assert.AreEqual(StatusCodes.Status400BadRequest, ((BadRequestObjectResult)res).StatusCode);

        //    Helper.DeleteCreatedDatabase(modelContext);
        //}

        //[Test]
        //public void Cant_Post_Module_With_Empty_Name()
        //{
        //    // Arrange
        //    string fileName = Helper.GetCurrentMethodName() + ".db";
        //    var modelContext = Helper.GetContext(fileName);
        //    string moduleName = string.Empty;

        //    // Act
        //    var res = ModuleHelper.PostModule(modelContext, moduleName);
        //    Assert.AreEqual(StatusCodes.Status400BadRequest, ((BadRequestObjectResult)res).StatusCode);

        //    Helper.DeleteCreatedDatabase(modelContext);
        //}

        //[Test]
        //public void Add_And_Get_Role_In_Module()
        //{
        //    // Arrange
        //    string fileName = Helper.GetCurrentMethodName() + ".db";
        //    var modelContext = Helper.GetContext(fileName);
        //    string moduleName = "ModuleName";
        //    string roleName = "RoleName";

        //    try
        //    {
        //        ModuleHelper.PostModule(modelContext, moduleName);
        //        long moduleId = ModuleHelper.GetModules(modelContext).First().Id;

        //        RoleHelper.AddRole(modelContext, roleName, CharacterType.Demon, Alignment.Good);
        //        long roleId = RoleHelper.GetRoles(modelContext).First().Id;

        //        // Act
        //        var res = ModuleHelper.AddRoleInModule(modelContext, moduleId, roleId);

        //        // Assert
        //        Assert.AreEqual(StatusCodes.Status201Created, ((CreatedResult)res).StatusCode);

        //        // Act
        //        var rolesInModule = ModuleHelper.GetRolesFromModule(modelContext, moduleId);

        //        // Assert
        //        Assert.AreEqual(roleName, rolesInModule.First().Role.Name);
        //    }
        //    finally
        //    {
        //        Helper.DeleteCreatedDatabase(modelContext);
        //    }
        //}

        //[Test]
        //public void Cant_Add_Twice_Same_Role_In_Module()
        //{
        //    // Arrange
        //    string fileName = Helper.GetCurrentMethodName() + ".db";
        //    var modelContext = Helper.GetContext(fileName);
        //    string moduleName = "ModuleName";
        //    string roleName = "RoleName";

        //    try
        //    {
        //        ModuleHelper.PostModule(modelContext, moduleName);
        //        long moduleId = ModuleHelper.GetModules(modelContext).First().Id;

        //        RoleHelper.AddRole(modelContext, roleName, CharacterType.Demon, Alignment.Good);
        //        long roleId = RoleHelper.GetRoles(modelContext).First().Id;

        //        // Act
        //        var res1 = ModuleHelper.AddRoleInModule(modelContext, moduleId, roleId);
        //        var res2 = ModuleHelper.AddRoleInModule(modelContext, moduleId, roleId);

        //        // Assert
        //        Assert.AreEqual(StatusCodes.Status201Created, ((CreatedResult)res1).StatusCode);
        //        Assert.AreEqual(StatusCodes.Status400BadRequest, ((BadRequestObjectResult)res2).StatusCode);
        //    }
        //    finally
        //    {
        //        Helper.DeleteCreatedDatabase(modelContext);
        //    }
        //}

        //[Test]
        //public void Cant_Add_Role_In_Module_With_Wrong_ModuleId()
        //{
        //    // Arrange
        //    string fileName = Helper.GetCurrentMethodName() + ".db";
        //    var modelContext = Helper.GetContext(fileName);
        //    string moduleName = "ModuleName";
        //    string roleName = "RoleName";

        //    try
        //    {
        //        ModuleHelper.PostModule(modelContext, moduleName);
        //        long moduleId = ModuleHelper.GetModules(modelContext).First().Id;

        //        RoleHelper.AddRole(modelContext, roleName, CharacterType.Demon, Alignment.Good);
        //        long roleId = RoleHelper.GetRoles(modelContext).First().Id;

        //        // Act
        //        var res = ModuleHelper.AddRoleInModule(modelContext, moduleId + 1, roleId);

        //        // Assert
        //        Assert.AreEqual(StatusCodes.Status400BadRequest, ((BadRequestObjectResult)res).StatusCode);
        //    }
        //    finally
        //    {
        //        Helper.DeleteCreatedDatabase(modelContext);
        //    }
        //}

        //[Test]
        //public void Cant_Add_Role_In_Module_With_Wrong_RoleId()
        //{
        //    // Arrange
        //    string fileName = Helper.GetCurrentMethodName() + ".db";
        //    var modelContext = Helper.GetContext(fileName);
        //    string moduleName = "ModuleName";
        //    string roleName = "RoleName";

        //    try
        //    {
        //        ModuleHelper.PostModule(modelContext, moduleName);
        //        long moduleId = ModuleHelper.GetModules(modelContext).First().Id;

        //        RoleHelper.AddRole(modelContext, roleName, CharacterType.Demon, Alignment.Good);
        //        long roleId = RoleHelper.GetRoles(modelContext).First().Id;

        //        // Act
        //        var res = ModuleHelper.AddRoleInModule(modelContext, moduleId, roleId + 1);

        //        // Assert
        //        Assert.AreEqual(StatusCodes.Status400BadRequest, ((BadRequestObjectResult)res).StatusCode);
        //    }
        //    finally
        //    {
        //        Helper.DeleteCreatedDatabase(modelContext);
        //    }
        //}

        //[Test]
        //public void Remove_Role_From_Module()
        //{
        //    // Arrange
        //    string fileName = Helper.GetCurrentMethodName() + ".db";
        //    var modelContext = Helper.GetContext(fileName);
        //    string moduleName = "ModuleName";
        //    string roleName = "RoleName";

        //    try
        //    {
        //        ModuleHelper.PostModule(modelContext, moduleName);
        //        long moduleId = ModuleHelper.GetModules(modelContext).First().Id;

        //        RoleHelper.AddRole(modelContext, roleName, CharacterType.Demon, Alignment.Good);
        //        long roleId = RoleHelper.GetRoles(modelContext).First().Id;

        //        ModuleHelper.AddRoleInModule(modelContext, moduleId, roleId);

        //        // Act
        //        var res = ModuleHelper.RemoveRoleFromModule(modelContext, moduleId, roleId);

        //        // Assert
        //        Assert.AreEqual(StatusCodes.Status200OK, ((OkResult)res).StatusCode);

        //        var roles = ModuleHelper.GetRolesFromModule(modelContext, moduleId);
        //        Assert.IsEmpty(roles);
        //    }
        //    finally
        //    {
        //        Helper.DeleteCreatedDatabase(modelContext);
        //    }
        //}
    }
}
