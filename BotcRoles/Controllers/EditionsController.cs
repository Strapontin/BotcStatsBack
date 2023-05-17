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
                // Get Edition name and test errors
                string? name = data["editionName"]?.ToString();
                if (string.IsNullOrWhiteSpace(name))
                {
                    return BadRequest($"Le nom du edition est vide.");
                }
                if (_db.Editions.ToList().Any(m => m.Name.ToLowerRemoveDiacritics() == name.ToLowerRemoveDiacritics()))
                {
                    return BadRequest($"Un edition avec le nom '{name}' existe déjà.");
                }


                // Try to convert to Role object from database to ensure it exists
                List<long>? rolesId = data["rolesId"]?.ToObject<List<long>>();
                List<Role> rolesDb = new();
                if (rolesId != null)
                {
                    foreach (var roleId in rolesId)
                    {
                        Role? roleDb = _db.Roles.FirstOrDefault(r => r.RoleId == roleId);
                        if (roleDb == null)
                        {
                            return BadRequest($"Le rôle avec l'id '{roleId}' n'a pas été trouvé.");
                        }
                        rolesDb.Add(roleDb);
                    }
                }

                // Save edition with name
                Edition edition = new(name);
                var test = _db.Add(edition);
                _db.SaveChanges();

                // Get edition db
                var editionDb = _db.Editions.First(edition => edition.Name == name);

                var rolesEditionDb = rolesDb.Select(rdb => new RoleEdition(rdb, editionDb));
                _db.AddRange(rolesEditionDb);
                _db.SaveChanges();

                return Created("", null);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException);
            }
        }

        //[HttpGet]
        //[Route("{editionId}/roles")]
        //public ActionResult<IEnumerable<Entities.Role>> GetRolesFromEdition(long editionId)
        //{
        //    var roles = _db.RolesEdition
        //        .Where(rm => rm.EditionId == editionId)
        //        .Select(rm => new Entities.Role(rm))
        //        .ToList();

        //    return roles;
        //}

        ////[HttpPost]
        ////[Route("{editionId}")]
        ////public IActionResult AddRoleInEdition(long editionId, [FromQuery] long roleId)
        ////{
        ////    try
        ////    {
        ////        var edition = _db.Editions
        ////            .Where(m => m.EditionId == editionId)
        ////            .Include(m => m.RolesEdition)
        ////            .FirstOrDefault();

        ////        var role = _db.Roles.Find(roleId);

        ////        if (edition == null)
        ////        {
        ////            return BadRequest($"Le edition avec l'id '{editionId}' n'a pas été trouvé.");
        ////        }

        ////        if (role == null)
        ////        {
        ////            return BadRequest($"Le role avec l'id '{roleId}' n'a pas été trouvé.");
        ////        }

        ////        if (edition.RolesEdition.Any(rm => rm.RoleId == roleId))
        ////        {
        ////            return BadRequest($"Ce rôle existe déjà dans ce edition.");
        ////        }

        ////        _db.Add(new RoleEdition(role, edition));
        ////        _db.SaveChanges();

        ////        return Created("", null);
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        return StatusCode(500, ex.InnerException);
        ////    }
        ////}

        ////[HttpDelete]
        ////[Route("{editionId}")]
        ////public IActionResult RemoveRoleFromEdition(long editionId, [FromQuery] long roleId)
        ////{
        ////    try
        ////    {
        ////        var roleEdition = _db.RolesEdition.Where(rm => rm.EditionId == editionId &&
        ////                                                    rm.RoleId == roleId).FirstOrDefault();

        ////        if (roleEdition == null)
        ////        {
        ////            return BadRequest($"Le role n'existe pas dans ce edition.");
        ////        }

        ////        _db.RolesEdition.Remove(roleEdition);
        ////        _db.SaveChanges();

        ////        return Ok();
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        return StatusCode(500, ex.InnerException);
        ////    }
        ////}
    }
}