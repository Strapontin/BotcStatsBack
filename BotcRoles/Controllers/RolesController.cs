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
                var role = GetRoleDataFromBody(data, out string error);
                if (error != null)
                {
                    return BadRequest(error);
                }

                _db.Add(role);
                _db.SaveChanges();

                return Created("", null);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException);
            }
        }

        [HttpPut]
        [Route("")]
        public IActionResult UpdateRole([FromBody] JObject data)
        {
            try
            {
                if (!long.TryParse(data["roleId"].ToString(), out long roleId))
                {
                    return BadRequest($"Aucun id de role trouvé.");
                }
                var role = _db.Roles.FirstOrDefault(e => e.RoleId == roleId);

                if (role == null)
                {
                    return BadRequest($"Le role avec l'id {roleId} n'a pas été trouvé.");
                }

                var tempRole = GetRoleDataFromBody(data, out string error, role.Name);

                if (error != null)
                {
                    return BadRequest(error);
                }

                role.Name = tempRole.Name;
                role.CharacterType = tempRole.CharacterType;
                role.DefaultAlignment = tempRole.DefaultAlignment;
                _db.SaveChanges();

                return Created("", null);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException);
            }
        }

        #region Private methods

        private Role GetRoleDataFromBody(JObject data, out string error, string oldRoleName = null)
        {
            error = null;

            string? roleName = data["roleName"]?.ToString().Trim();
            if (string.IsNullOrWhiteSpace(roleName))
            {
                error = $"Le nom du role est vide.";
                return null;
            }

            if (_db.Roles.ToList().Any(r => r.Name.ToLowerRemoveDiacritics() == roleName.ToLowerRemoveDiacritics()))
            {
                error = $"Un rôle avec le nom '{roleName}' existe déjà.";
                return null;
            }

            if (!int.TryParse(data["characterType"]?.ToString(), out int ctInt) || !Enum.IsDefined(typeof(CharacterType), ctInt))
            {
                error = $"Une erreur a été rencontrée avec le paramètre 'characterType'.";
                return null;
            }
            CharacterType characterType = (CharacterType)ctInt;

            if (!int.TryParse(data["alignment"]?.ToString(), out int alignmentInt) || !Enum.IsDefined(typeof(Alignment), alignmentInt))
            {
                error = $"Une erreur a été rencontrée avec le paramètre 'alignment'.";
                return null;
            }
            Alignment alignment = (Alignment)alignmentInt;


            Role role = new(roleName, characterType, alignment);

            return role;
        }

        #endregion
    }
}