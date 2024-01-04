using BotcRoles.Entities;
using BotcRoles.Enums;
using BotcRoles.Helper;
using Microsoft.AspNetCore.Authorization;
using BotcRoles.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore;

namespace BotcRoles.Controllers
{
    [Authorize(Policy = "IsStoryteller")]
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

        [AllowAnonymous]
        [HttpGet]
        [Route("")]
        public ActionResult<IEnumerable<RoleEntities>> GetRoles([FromQuery(Name = "characterTypes")] List<int>? characterTypes)
        {
            characterTypes = characterTypes != null && characterTypes.Count != 0 ? characterTypes :
                new List<int>()
                {
                    (int)CharacterType.Townsfolk,
                    (int)CharacterType.Outsider,
                    (int)CharacterType.Minion,
                    (int)CharacterType.Demon,
                    (int)CharacterType.Traveller,
                    (int)CharacterType.Fabled,
                };

            var allPlayerRoleGames = _db.PlayerRoleGames
                .Include(prg => prg.Game)
                .ToList()
                .GroupBy(prg => prg.RoleId);

            var roles = _db.Roles
                .Where(p => !p.IsHidden && characterTypes.Contains((int)p.CharacterType))
                .ToList()
                .Select(r => new RoleEntities(r, allPlayerRoleGames.FirstOrDefault(prg => prg.Key== r.RoleId)?.ToList()))
                .ToList()
                .OrderBy(r => r.CharacterType)
                .ThenBy(r => r.Name.ToLowerRemoveDiacritics())
                .ToList();

            return roles;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("{roleId}")]
        public ActionResult<RoleEntities> GetRoleById(long roleId)
        {
            var prgRole = _db.PlayerRoleGames.Where(prg => prg.RoleId == roleId).ToList();

            var role = _db.Roles
                .Where(r => r.RoleId == roleId)
                .ToList()
                .Select(r => new RoleEntities(r, prgRole))
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
                if (!long.TryParse(data["roleId"]?.ToString(), out long roleId))
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
                _db.SaveChanges();

                return Created("", null);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException);
            }
        }

        [HttpDelete]
        [Route("{roleId}")]
        public IActionResult DeleteRole(long roleId)
        {
            try
            {
                if (!_db.Roles.Any(p => p.RoleId == roleId))
                {
                    return NotFound();
                }

                if (_db.PlayerRoleGames.Any(prg => prg.RoleId == roleId))
                {
                    _db.Roles.First(p => p.RoleId == roleId).IsHidden = true;
                    _db.SaveChanges();
                }
                else
                {
                    _db.Roles.Remove(_db.Roles.First(g => g.RoleId == roleId));
                    _db.SaveChanges();
                }

                return Accepted();
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
                error = $"Le nom du rôle est vide.";
                return null;
            }
            if ((string.IsNullOrWhiteSpace(oldRoleName) || oldRoleName.ToLowerRemoveDiacritics() != roleName.ToLowerRemoveDiacritics()) &&
                _db.Roles.ToList().Any(r => r.Name.ToLowerRemoveDiacritics() == roleName.ToLowerRemoveDiacritics()))
            {
                error = $"Un rôle avec le nom '{roleName}' existe déjà.";
                return null;
            }

            if (!int.TryParse(data["characterType"]?.ToString(), out int ctInt) || !Enum.IsDefined(typeof(CharacterType), ctInt))
            {
                error = $"Le type de personnage n'est pas renseigné.";
                return null;
            }
            CharacterType characterType = (CharacterType)ctInt;

            Role role = new(roleName, characterType);

            return role;
        }

        #endregion
    }
}