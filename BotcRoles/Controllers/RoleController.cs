using BotcRoles.Enums;
using BotcRoles.Models;
using Microsoft.AspNetCore.Mvc;

namespace BotcRoles.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly ILogger<RoleController> _logger;
        private readonly ModelContext _db;

        public RoleController(ILogger<RoleController> logger, ModelContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet]
        [Route("")]
        public IEnumerable<Role> GetRoles()
        {
            var roles = _db.Roles;
            return roles;
        }

        [HttpPost]
        [Route("{roleName}")]
        public IActionResult AddRole(string roleName, [FromQuery] Enums.Type? type, [FromQuery] Alignment? defaultAlignment)
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

                if (type == null)
                {
                    return BadRequest($"Le type n'est pas défini.");
                }

                if (defaultAlignment == null)
                {
                    return BadRequest($"L'alignement par défaut n'est pas défini.");
                }

                _db.Add(new Role(roleName, type.Value, defaultAlignment.Value));
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