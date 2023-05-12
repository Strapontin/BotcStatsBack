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
        public static IActionResult AddRole(ModelContext modelContext, string roleName, CharacterType? characterType, Alignment? alignment)
        {
            RolesController roleController = new(null!, modelContext);

            JObject rolePost = JObject.FromObject(new { roleName, characterType, alignment});

            var res = roleController.AddRole(rolePost);
            return res;
        }

        public static IEnumerable<RoleEntities> GetRoles(ModelContext modelContext)
        {
            RolesController roleController = new(null!, modelContext);

            return roleController.GetRoles().Value;
        }

        public static RoleEntities GetRoleById(ModelContext modelContext, long id)
        {
            RolesController roleController = new(null!, modelContext);

            return roleController.GetRoleById(id).Value;
        }
    }
}
