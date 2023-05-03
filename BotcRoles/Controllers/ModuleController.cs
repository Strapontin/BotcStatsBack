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
            var modules = _db.Modules;
            return modules;
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
        public IActionResult CreateModule(string name)
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
                return StatusCode(500, ex.InnerException);
            }
        }

        [HttpGet]
        [Route("{moduleId}/roles/")]
        public IEnumerable<RoleModule> GetRolesFromModule(long moduleId)
        {
            var rolesInModule = _db.RoleModules
                .Where(rm => rm.ModuleId == moduleId);

            return rolesInModule;
        }

        [HttpPost]
        [Route("{moduleId}")]
        public IActionResult AddRoleInModule(long moduleId, [FromQuery] long roleId)
        {
            try
            {
                var module = _db.Modules
                    .Where(m => m.ModuleId == moduleId)
                    .Include(m => m.RoleModules)
                    .FirstOrDefault();

                var role = _db.Roles.Find(roleId);

                if (module == null)
                {
                    return BadRequest($"Le module avec l'id '{moduleId}' n'a pas été trouvé.");
                }

                if (role == null)
                {
                    return BadRequest($"Le role avec l'id '{roleId}' n'a pas été trouvé.");
                }

                if (module.RoleModules.Any(rm => rm.RoleId == roleId))
                {
                    return BadRequest($"Ce rôle existe déjà dans ce module.");
                }

                _db.Add(new RoleModule(role, module));
                _db.SaveChanges();

                return Created("", null);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException);
            }
        }

        [HttpDelete]
        [Route("{moduleId}")]
        public IActionResult RemoveRoleFromModule(long moduleId, [FromQuery] long roleId)
        {
            try
            {
                var roleModule = _db.RoleModules.Where(rm => rm.ModuleId == moduleId &&
                                                            rm.RoleId == roleId).FirstOrDefault();

                if (roleModule == null)
                {
                    return BadRequest($"Le role n'existe pas dans ce module.");
                }

                _db.RoleModules.Remove(roleModule);
                _db.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException);
            }
        }
    }
}