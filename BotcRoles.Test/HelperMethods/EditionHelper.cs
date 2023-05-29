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

            return editionController.GetEditions().Value;
        }

        public static EditionEntities GetEdition(ModelContext modelContext, long editionId)
        {
            EditionsController editionController = new(null!, modelContext);

            var res = editionController.GetEditionById(editionId);
            return res.Value;
        }

        public static void DeleteAllEditions(ModelContext modelContext)
        {
            modelContext.Editions.RemoveRange(modelContext.Editions);
            modelContext.SaveChanges();
        }

        internal static IActionResult UpdateEdition(ModelContext modelContext, long editionId, string editionName, List<long> rolesId)
        {
            EditionsController editionsController = new(null!, modelContext);

            var data = new
            {
                editionId,
                editionName,
                rolesId,
            };

            var res = editionsController.UpdateEdition(JObject.FromObject(data));
            return res;
        }

        public static IActionResult DeleteEdition(ModelContext modelContext, long editionId)
        {
            EditionsController editionsController = new(null!, modelContext);
            var res = editionsController.DeleteEdition(editionId);

            return res;
        }
    }
}
