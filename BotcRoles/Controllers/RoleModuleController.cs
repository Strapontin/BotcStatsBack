using BotcRoles.Enums;
using BotcRoles.Models;
using Microsoft.AspNetCore.Mvc;

namespace BotcRoles.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoleModuleController : ControllerBase
    {
        private readonly ILogger<RoleModuleController> _logger;
        private readonly ModelContext _db;

        public RoleModuleController(ILogger<RoleModuleController> logger, ModelContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet]
        [Route("{moduleId}")]
        public RoleModule? Get(long moduleId)
        {
            var roleModule = _db.RoleModules.Where(rm => rm.ModuleId == moduleId).FirstOrDefault();
            return roleModule;
        }

        [HttpPost]
        [Route("{moduleId}/{roleId}")]
        public IActionResult Post(long moduleId, long roleId)
        {
            try
            {
                var module = _db.Modules.Find(moduleId);
                var role = _db.Roles.Find(roleId);

                if (module == null)
                {
                    return BadRequest($"Le module avec l'id '{moduleId}' n'a pas été trouvé.");
                }

                if (role == null)
                {
                    return BadRequest($"Le role avec l'id '{roleId}' n'a pas été trouvé.");
                }

                _db.Add(new RoleModule(role, module));
                _db.SaveChanges();

                return Created("", null);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException);
            }
        }

        [HttpDelete]
        [Route("{moduleId}/{roleId}")]
        public IActionResult Put(long moduleId, long roleId)
        {
            try
            {
                var roleModule = _db.RoleModules.Where(rm => rm.ModuleId == moduleId &&
                                                            rm.RoleId == roleId).FirstOrDefault();

                if (roleModule == null)
                {
                    return Ok($"Le role n'existe pas dans ce module.");
                }

                _db.RoleModules.Remove(roleModule);
                _db.SaveChanges();

                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.InnerException);
            }
        }
    }
}