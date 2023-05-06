using BotcRoles.Models;
using BotcRoles.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace BotcRoles.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ModulesController : ControllerBase
    {
        private readonly ILogger<ModulesController> _logger;
        private readonly ModelContext _db;

        public ModulesController(ILogger<ModulesController> logger, ModelContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet]
        [Route("")]
        public ActionResult<IEnumerable<ModuleEntities>> Get()
        {
            var modules = _db.Modules
                .Include(m => m.RolesModule)
                .ThenInclude(rm => rm.Role)
                .Select(m => new ModuleEntities(m))
                .ToList();

            return modules;
        }

        //[HttpGet]
        //[Route("{moduleName}")]
        //public ActionResult<Entities.Module> Get(string moduleName)
        //{
        //    var module = _db.Modules
        //        .Where(m => m.Name == moduleName)
        //        .Include(m => m.RolesModule)
        //        .Select(m => new Entities.Module(m))
        //        .FirstOrDefault();

        //    return module == null ? NotFound() : module;
        //}

        //[HttpPost]
        //[Route("")]
        //public IActionResult CreateModule([FromBody] JObject data)
        //{
        //    try
        //    {
        //        // Get Module name and test errors
        //        string? name = data["name"]?.ToString();
        //        if (string.IsNullOrWhiteSpace(name))
        //        {
        //            return BadRequest($"Le nom du module est vide.");
        //        }
        //        if (_db.Modules.Any(m => m.Name == name))
        //        {
        //            return BadRequest($"Un module avec le nom '{name}' existe déjà.");
        //        }

        //        // Get roles names and test errors
        //        List<Entities.Role>? roles = data["roles"]?.ToObject<List<Entities.Role>>();
        //        if (roles == null || !roles.Any())
        //        {
        //            return BadRequest($"Il n'y a aucun rôle d'ajouté pour ce module.");
        //        }
        //        // Try to convert to Role object from database to ensure it exists
        //        List<Models.Role> rolesDb = new List<Models.Role>();
        //        foreach (var role in roles)
        //        {
        //            Models.Role? roleDb = _db.Roles.FirstOrDefault(r => r.Name == role.Name);
        //            if (roleDb == null)
        //            {
        //                return BadRequest($"Le rôle avec le nom '{role.Name}' n'a pas été trouvé.");
        //            }
        //            rolesDb.Add(roleDb);
        //        }

        //        // Save module with name
        //        Models.Module module = new Models.Module(name);
        //        _db.Add(module);
        //        _db.SaveChanges();

        //        // Get module db
        //        var moduleDb = _db.Modules.First(module => module.Name == name);

        //        var rolesModuleDb = rolesDb.Select(rdb => new RoleModule(rdb, moduleDb));
        //        _db.AddRange(rolesModuleDb);
        //        _db.SaveChanges();

        //        return Created("", null);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex.InnerException);
        //    }
        //}

        //[HttpGet]
        //[Route("{moduleId}/roles")]
        //public ActionResult<IEnumerable<Entities.Role>> GetRolesFromModule(long moduleId)
        //{
        //    var roles = _db.RolesModule
        //        .Where(rm => rm.ModuleId == moduleId)
        //        .Select(rm => new Entities.Role(rm))
        //        .ToList();

        //    return roles;
        //}

        ////[HttpPost]
        ////[Route("{moduleId}")]
        ////public IActionResult AddRoleInModule(long moduleId, [FromQuery] long roleId)
        ////{
        ////    try
        ////    {
        ////        var module = _db.Modules
        ////            .Where(m => m.ModuleId == moduleId)
        ////            .Include(m => m.RolesModule)
        ////            .FirstOrDefault();

        ////        var role = _db.Roles.Find(roleId);

        ////        if (module == null)
        ////        {
        ////            return BadRequest($"Le module avec l'id '{moduleId}' n'a pas été trouvé.");
        ////        }

        ////        if (role == null)
        ////        {
        ////            return BadRequest($"Le role avec l'id '{roleId}' n'a pas été trouvé.");
        ////        }

        ////        if (module.RolesModule.Any(rm => rm.RoleId == roleId))
        ////        {
        ////            return BadRequest($"Ce rôle existe déjà dans ce module.");
        ////        }

        ////        _db.Add(new RoleModule(role, module));
        ////        _db.SaveChanges();

        ////        return Created("", null);
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        return StatusCode(500, ex.InnerException);
        ////    }
        ////}

        ////[HttpDelete]
        ////[Route("{moduleId}")]
        ////public IActionResult RemoveRoleFromModule(long moduleId, [FromQuery] long roleId)
        ////{
        ////    try
        ////    {
        ////        var roleModule = _db.RolesModule.Where(rm => rm.ModuleId == moduleId &&
        ////                                                    rm.RoleId == roleId).FirstOrDefault();

        ////        if (roleModule == null)
        ////        {
        ////            return BadRequest($"Le role n'existe pas dans ce module.");
        ////        }

        ////        _db.RolesModule.Remove(roleModule);
        ////        _db.SaveChanges();

        ////        return Ok();
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        return StatusCode(500, ex.InnerException);
        ////    }
        ////}
    }
}