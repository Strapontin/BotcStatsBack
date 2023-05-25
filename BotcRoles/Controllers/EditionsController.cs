using BotcRoles.Models;
using BotcRoles.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using BotcRoles.Helper;

namespace BotcRoles.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EditionsController : ControllerBase
    {
        private readonly ILogger<EditionsController> _logger;
        private readonly ModelContext _db;

        public EditionsController(ILogger<EditionsController> logger, ModelContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet]
        [Route("")]
        public ActionResult<IEnumerable<EditionEntities>> GetEditions()
        {
            var editions = _db.Editions
                .Include(m => m.RolesEdition)
                    .ThenInclude(rm => rm.Role)
                .Include(m => m.Games)
                .Select(m => new EditionEntities(_db, m))
                .ToList()
                .OrderBy(m => m.Name.ToLowerRemoveDiacritics())
                .ToList();

            return editions;
        }

        [HttpGet]
        [Route("{editionId}")]
        public ActionResult<EditionEntities> GetEditionById(long editionId)
        {
            var edition = _db.Editions
                .Where(m => m.EditionId == editionId)
                .Include(m => m.RolesEdition)
                    .ThenInclude(rm => rm.Role)
                .Select(m => new EditionEntities(_db, m))
                .FirstOrDefault();

            return edition == null ? NotFound() : edition;
        }

        [HttpPost]
        [Route("")]
        public IActionResult CreateEdition([FromBody] JObject data)
        {
            try
            {
                var edition = GetEditionDataFromBody(data, out string error);
                if (error != null)
                {
                    return BadRequest(error);
                }

                // Save edition with name
                //Edition edition = new(name);
                _db.Add(edition);
                _db.SaveChanges();

                //// Get edition db
                //var editionDb = _db.Editions.First(edition => edition.Name == name);

                //var rolesEditionDb = rolesDb.Select(rdb => new RoleEdition(rdb, editionDb));
                //_db.AddRange(rolesEditionDb);
                //_db.SaveChanges();

                return Created("", null);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException);
            }
        }

        [HttpPut]
        [Route("")]
        public IActionResult UpdateEdition([FromBody] JObject data)
        {
            try
            {
                if (!long.TryParse(data["editionId"].ToString(), out long editionId))
                {
                    return BadRequest($"Aucun id de module trouvé.");
                }
                var edition = _db.Editions.FirstOrDefault(e => e.EditionId == editionId);

                if (edition == null)
                {
                    return BadRequest($"Le module avec l'id {editionId} n'a pas été trouvé.");
                }

                var editionTemp = GetEditionDataFromBody(data, out string error);

                if (error != null)
                {
                    return BadRequest(error);
                }

                edition.Name = editionTemp.Name;

                _db.RemoveRange(_db.RolesEdition.Where(re => re.EditionId == edition.EditionId));
                _db.SaveChanges();

                edition.RolesEdition = editionTemp.RolesEdition;

                _db.SaveChanges();

                return Created("", null);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException);
            }
        }

        #region Private methods

        private Edition GetEditionDataFromBody(JObject data, out string error)
        {
            error = null;

            string? name = data["editionName"]?.ToString();
            if (string.IsNullOrWhiteSpace(name))
            {
                error = $"Le nom du module est vide.";
                return null;
            }
            if (_db.Editions.ToList().Any(m => m.Name.ToLowerRemoveDiacritics() == name.ToLowerRemoveDiacritics()))
            {
                error = $"Un module avec le nom '{name}' existe déjà.";
                return null;
            }


            // Try to convert to Role object from database to ensure it exists
            List<long>? rolesId = data["rolesId"]?.ToObject<List<long>>();
            List<RoleEdition> rolesEditionDb = new();
            if (rolesId != null)
            {
                foreach (var roleId in rolesId)
                {
                    Role? roleDb = _db.Roles.FirstOrDefault(r => r.RoleId == roleId);
                    if (roleDb == null)
                    {
                        error = $"Le rôle avec l'id '{roleId}' n'a pas été trouvé.";
                        return null;
                    }
                    rolesEditionDb.Add(new(roleDb, null));
                }
            }

            Edition edition = new(name)
            {
                RolesEdition = rolesEditionDb,
            };

            return edition;
        }

        #endregion
    }
}