﻿using BotcRoles.Controllers;
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
    public static class EditionHelper
    {
        public static IActionResult PostEdition(ModelContext modelContext, string editionName, List<long> rolesId = null)
        {
            EditionsController editionController = new(null!, modelContext);

            var data = new
            {
                editionName,
                rolesId
            };

            var res = editionController.CreateEdition(JObject.FromObject(data));
            return res;
        }

        public static IEnumerable<EditionEntities> GetEditions(ModelContext modelContext)
        {
            EditionsController editionController = new(null!, modelContext);

            return editionController.Get().Value;
        }

        public static EditionEntities GetEdition(ModelContext modelContext, string editionName)
        {
            EditionsController editionController = new(null!, modelContext);

            var res = editionController.Get(editionName);
            return res.Value;
        }

        //public static IActionResult AddRoleInEdition(ModelContext modelContext, long editionId, long roleId)
        //{
        //    EditionsController editionController = new(null!, modelContext);

        //    var res = editionController.AddRoleInEdition(editionId, roleId);
        //    return res;
        //}

        public static IEnumerable<RoleEntities> GetRolesFromEdition(ModelContext modelContext, string editionName)
        {
            EditionsController editionController = new(null!, modelContext);

            var res = editionController.Get(editionName).Value.Roles;
            return res;
        }

        public static void DeleteAllEditions(ModelContext modelContext)
        {
            modelContext.Editions.RemoveRange(modelContext.Editions);
            modelContext.SaveChanges();
        }

        //public static IActionResult RemoveRoleFromEdition(ModelContext modelContext, long editionId, long roleId)
        //{
        //    EditionsController editionController = new(null!, modelContext);

        //    var res = editionController.RemoveRoleFromEdition(editionId, roleId);
        //    return res;
        //}
    }
}
