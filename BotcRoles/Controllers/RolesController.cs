using BotcRoles.Entities;
using BotcRoles.Enums;
using BotcRoles.Helper;
using BotcRoles.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace BotcRoles.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly ILogger<RolesController> _logger;
        private readonly ModelContext _db;

        public RolesController(ILogger<RolesController> logger, ModelContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet]
        [Route("")]
        public ActionResult<IEnumerable<RoleEntities>> GetRoles()
        {
            var roles = _db.Roles
                .Select(r => new RoleEntities(_db, r))
                .ToList()
                .OrderBy(r => r.CharacterType)
                .ThenBy(r => r.Name.ToLowerRemoveDiacritics())
                .ToList();

            return roles;
        }

        [HttpGet]
        [Route("{roleId}")]
        public ActionResult<RoleEntities> GetRoleById(long roleId)
        {
            var role = _db.Roles
                .Where(p => p.RoleId == roleId)
                .Select(p => new RoleEntities(_db, p))
                .FirstOrDefault();

            return role == null ? NotFound() : role;
        }

        [HttpPost]
        [Route("")]
        public IActionResult AddRole([FromBody] JObject data)
        {
            try
            {
                string? roleName = data["roleName"]?.ToString();

                if (string.IsNullOrWhiteSpace(roleName))
                {
                    return BadRequest(JObject.FromObject(new { error = $"Le nom du role est vide." }));
                }

                if (_db.Roles.ToList().Any(r => r.Name.ToLowerRemoveDiacritics() == roleName.ToLowerRemoveDiacritics()))
                {
                    return BadRequest(JObject.FromObject(new { error = $"Un rôle avec le nom '{roleName}' existe déjà." }));
                }

                if (!int.TryParse(data["characterType"]?.ToString(), out int ctInt) || !Enum.IsDefined(typeof(CharacterType), ctInt))
                {
                    return BadRequest(JObject.FromObject(new { error = $"Une erreur a été rencontrée avec le paramètre 'characterType'." }));
                }
                CharacterType characterType = (CharacterType)ctInt;

                if (!int.TryParse(data["alignment"]?.ToString(), out int alignmentInt) || !Enum.IsDefined(typeof(Alignment), alignmentInt))
                {
                    return BadRequest(JObject.FromObject(new { error = $"Une erreur a été rencontrée avec le paramètre 'alignment'." }));
                }
                Alignment alignment = (Alignment)alignmentInt;

                _db.Add(new Role(roleName, characterType, alignment));
                _db.SaveChanges();

                return Created("", null);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException);
            }
        }
    }
}