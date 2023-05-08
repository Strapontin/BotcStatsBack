using BotcRoles.Controllers;
using BotcRoles.Entities;
using BotcRoles.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotcRoles.Test.HelperMethods
{
    public static class ModuleHelper
    {
        //public static IActionResult PostModule(ModelContext modelContext, string moduleName, List<RoleEntities> roleEntities = null)
        //{
        //    ModulesController moduleController = new(null!, modelContext);

        //    var data = new
        //    {
        //        name = moduleName,
        //        roles = roleEntities
        //    };

        //    var res = moduleController.CreateModule(JObject.FromObject(data));
        //    return res;
        //}

        public static IEnumerable<ModuleEntities> GetModules(ModelContext modelContext)
        {
            ModulesController moduleController = new(null!, modelContext);

            return moduleController.Get().Value;
        }

        public static ModuleEntities GetModule(ModelContext modelContext, string moduleName)
        {
            ModulesController moduleController = new(null!, modelContext);

            var res = moduleController.Get(moduleName);
            return res.Value;
        }

        //public static IActionResult AddRoleInModule(ModelContext modelContext, long moduleId, long roleId)
        //{
        //    ModulesController moduleController = new(null!, modelContext);

        //    var res = moduleController.AddRoleInModule(moduleId, roleId);
        //    return res;
        //}

        public static IEnumerable<RoleEntities> GetRolesFromModule(ModelContext modelContext, string moduleName)
        {
            ModulesController moduleController = new(null!, modelContext);

            var res = moduleController.Get(moduleName).Value.Roles;
            return res;
        }

        //public static IActionResult RemoveRoleFromModule(ModelContext modelContext, long moduleId, long roleId)
        //{
        //    ModulesController moduleController = new(null!, modelContext);

        //    var res = moduleController.RemoveRoleFromModule(moduleId, roleId);
        //    return res;
        //}
    }
}
