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
        public IEnumerable<Role> Get()
        {
            var roles = _db.Roles;
            return roles;
        }

        [HttpPost]
        [Route("{roleName}/{defaultAlignment}/{type}")]
        public IActionResult Post(string roleName, Enums.Type type, Alignment defaultAlignment)
        {
            try
            {
                if (_db.Roles.Any(p => p.Name == roleName))
                {
                    return BadRequest($"Le role '{roleName}' existe déjà.");
                }

                _db.Add(new Role(roleName, type, defaultAlignment));
                _db.SaveChanges();

                return Created("", null);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException);
            }
        }
    }
}