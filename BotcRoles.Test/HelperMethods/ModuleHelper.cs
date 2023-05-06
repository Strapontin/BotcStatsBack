using BotcRoles.Controllers;
using BotcRoles.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotcRoles.Test.HelperMethods
{
    public static class ModuleHelper
    {
        public static IActionResult PostModule(ModelContext modelContext, string moduleName)
        {
            ModulesController moduleController = new(null!, modelContext);

            var res = moduleController.CreateModule(moduleName);
            return res;
        }

        public static IEnumerable<Module> GetModules(ModelContext modelContext)
        {
            ModulesController moduleController = new(null!, modelContext);

            return moduleController.Get();
        }

        public static Module GetModule(ModelContext modelContext, long moduleId)
        {
            ModulesController moduleController = new(null!, modelContext);

            var res = moduleController.Get(moduleId);
            return res!;
        }

        public static IActionResult AddRoleInModule(ModelContext modelContext, long moduleId, long roleId)
        {
            ModulesController moduleController = new(null!, modelContext);

            var res = moduleController.AddRoleInModule(moduleId, roleId);
            return res;
        }

        public static IEnumerable<RoleModule> GetRolesFromModule(ModelContext modelContext, long moduleId)
        {
            ModulesController moduleController = new(null!, modelContext);

            var res = moduleController.GetRolesFromModule(moduleId);
            return res;
        }

        public static IActionResult RemoveRoleFromModule(ModelContext modelContext, long moduleId, long roleId)
        {
            ModulesController moduleController = new(null!, modelContext);

            var res = moduleController.RemoveRoleFromModule(moduleId, roleId);
            return res;
        }
    }
}
