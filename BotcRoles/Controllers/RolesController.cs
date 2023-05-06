using BotcRoles.Enums;
using BotcRoles.Models;
using Microsoft.AspNetCore.Mvc;

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
        public ActionResult<IEnumerable<Role>> GetRoles()
        {
            var roles = _db.Roles.ToList();
            return roles;
        }

        [HttpPost]
        [Route("{roleName}")]
        public IActionResult AddRole(string roleName, [FromQuery] CharacterType characterType, [FromQuery] Alignment defaultAlignment)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(roleName))
                {
                    return BadRequest($"Le nom du role est vide.");
                }

                if (_db.Roles.Any(p => p.Name == roleName))
                {
                    return BadRequest($"Le role '{roleName}' existe déjà.");
                }

                _db.Add(new Role(roleName, characterType, defaultAlignment));
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