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
            ModuleController moduleController = new(null!, modelContext);

            var res = moduleController.Post(moduleName);
            return res;
        }

        public static IEnumerable<Module> GetModule(ModelContext modelContext)
        {
            ModuleController moduleController = new(null!, modelContext);

            return moduleController.Get();
        }

        public static Module GetModule(ModelContext modelContext, long moduleId)
        {
            ModuleController moduleController = new(null!, modelContext);

            var res = moduleController.Get(moduleId);
            return res!;
        }
    }
}
