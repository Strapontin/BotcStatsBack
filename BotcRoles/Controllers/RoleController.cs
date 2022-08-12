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

        [HttpGet(Name = "GetRoles")]
        public IEnumerable<Role> Get()
        {
            var roles = db.Roles;
            return roles;
        }

        [HttpPost(Name = "PostRole")]
        public void Post(string roleName, Alignment alignment)
        {
            db.Add(new Role { Name = roleName, Alignment = alignment });
            db.SaveChanges();
        }
    }
}