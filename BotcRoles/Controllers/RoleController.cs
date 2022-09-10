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
        private ModelContext db;

        public RoleController(ILogger<RoleController> logger)
        {
            _logger = logger;
            db = new ModelContext();
        }

        [HttpGet]
        [Route("")]
        public IEnumerable<Role> Get()
        {
            var roles = db.Roles;
            return roles;
        }

        [HttpPost]
        [Route("{roleName}/{defaultAlignment}/{type}")]
        public IActionResult Post(string roleName, Enums.Type type, Alignment defaultAlignment)
        {
            try
            {
                if (db.Roles.Any(p => p.Name == roleName))
                {
                    return BadRequest($"Le role '{roleName}' existe déjà.");
                }

                db.Add(new Role(roleName, type, defaultAlignment));
                db.SaveChanges();

                return Created("Role", null);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException);
            }
        }
    }
}