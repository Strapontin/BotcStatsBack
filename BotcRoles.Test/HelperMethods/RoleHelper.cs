using BotcRoles.Controllers;
using BotcRoles.Entities;
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
        public static IActionResult AddRole(ModelContext modelContext, string roleName, CharacterType type, Alignment defaultAlignment)
        {
            RolesController roleController = new(null!, modelContext);

            var res = roleController.AddRole(roleName, type, defaultAlignment);
            return res;
        }

        public static IEnumerable<RoleEntities> GetRoles(ModelContext modelContext)
        {
            RolesController roleController = new(null!, modelContext);

            return roleController.GetRoles().Value;
        }
    }
}
