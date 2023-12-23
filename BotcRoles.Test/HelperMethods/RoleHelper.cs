﻿using BotcRoles.Controllers;
using BotcRoles.Entities;
using BotcRoles.Enums;
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
    public static class RoleHelper
    {
        public static IActionResult AddRole(ModelContext modelContext, string roleName, CharacterType? characterType)
        {
            RolesController roleController = new(null!, modelContext);

            JObject rolePost = JObject.FromObject(new { roleName, characterType });

            var res = roleController.AddRole(rolePost);
            return res;
        }

        public static IEnumerable<RoleEntities> GetRoles(ModelContext modelContext)
        {
            RolesController roleController = new(null!, modelContext);

            return roleController.GetRoles(null).Value;
        }

        public static RoleEntities GetRoleById(ModelContext modelContext, long id)
        {
            RolesController roleController = new(null!, modelContext);

            return roleController.GetRoleById(id).Value;
        }

        internal static IActionResult UpdateRole(ModelContext modelContext, long roleId, string roleName, CharacterType characterType)
        {
            RolesController rolesController = new(null!, modelContext);

            var data = new
            {
                roleId,
                roleName,
                characterType,
            };

            var res = rolesController.UpdateRole(JObject.FromObject(data));
            return res;
        }

        public static IActionResult DeleteRole(ModelContext modelContext, long roleId)
        {
            RolesController rolesController = new(null!, modelContext);
            var res = rolesController.DeleteRole(roleId);

            return res;
        }

        public static void DeleteAllRoles(ModelContext modelContext)
        {
            modelContext.Roles.RemoveRange(modelContext.Roles);
            modelContext.SaveChanges();
        }
    }
}
