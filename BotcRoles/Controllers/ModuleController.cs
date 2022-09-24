using BotcRoles.Enums;
using BotcRoles.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BotcRoles.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ModuleController : ControllerBase
    {
        private readonly ILogger<ModuleController> _logger;
        private readonly ModelContext _db;

        public ModuleController(ILogger<ModuleController> logger, ModelContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet]
        [Route("")]
        public IEnumerable<Module> Get()
        {
            var roles = _db.Modules
                .ToList();

            return roles;
        }

        [HttpGet]
        [Route("{moduleId}")]
        public Module? Get(long moduleId)
        {
            var module = _db.Modules
                .Where(m => m.ModuleId == moduleId)
                .Include(m => m.RoleModules)
                .FirstOrDefault();

            return module;
        }

        [HttpPost]
        [Route("{name}")]
        public IActionResult Post(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return BadRequest($"Le nom du module est vide.");
                }

                if (_db.Modules.Any(m => m.Name == name))
                {
                    return BadRequest($"Un module avec le nom '{name}' existe déjà.");
                }

                _db.Add(new Module(name));
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