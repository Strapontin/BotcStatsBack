using BotcRoles.Controllers;
using BotcRoles.Enums;
using BotcRoles.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotcRoles.Test.HelperMethods
{
    public static class RoleHelper
    {
        public static IActionResult AddRole(ModelContext modelContext, string roleName, Enums.Type? type, Alignment? defaultAlignment)
        {
            RoleController roleController = new(null!, modelContext);

            var res = roleController.AddRole(roleName, type, defaultAlignment);
            return res;
        }

        public static IEnumerable<Role> GetRoles(ModelContext modelContext)
        {
            RoleController roleController = new(null!, modelContext);

            return roleController.GetRoles();
        }
    }
}
