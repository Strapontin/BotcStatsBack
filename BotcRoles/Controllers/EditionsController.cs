using BotcRoles.Entities;
using BotcRoles.Helper;
using BotcRoles.Misc;
using BotcRoles.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Data;

namespace BotcRoles.Controllers
{
    [Authorize(Policy = "IsStoryteller")]
    [ApiController]
    [Route("[controller]")]
    public class EditionsController : ControllerBase
    {
        private readonly ILogger<EditionsController> _logger;
        private readonly ModelContext _db;
        private readonly IAuthorizationHandler _isStorytellerAuthorizationHandler;

        public EditionsController(ILogger<EditionsController> logger, ModelContext db, IAuthorizationHandler isStorytellerAuthorizationHandler)
        {
            _logger = logger;
            _db = db;
            _isStorytellerAuthorizationHandler = isStorytellerAuthorizationHandler;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("")]
        public ActionResult<IEnumerable<EditionEntities>> GetEditions()
        {
            var allGames = _db.Games
                .Include(g => g.PlayerRoleGames)
                    .ThenInclude(prg => prg.Player)
                .ToList()
                .GroupBy(g => g.EditionId);

            var editions = _db.Editions
                .Where(e => !e.IsHidden)
                .Include(e => e.RolesEdition)
                    .ThenInclude(re => re.Role)
                .ToList()
                .Select(m => new EditionEntities(m, allGames.FirstOrDefault(g => g.Key == m.EditionId)?.ToList()))
                .ToList()
                .OrderBy(m => m.Name.ToLowerRemoveDiacritics())
                .ToList();

            return editions;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("{editionId}")]
        public ActionResult<EditionEntities> GetEditionById(long editionId)
        {
            var gamesWithThisEdition = _db.Games
                .Where(g => g.EditionId == editionId)
                .Include(g => g.PlayerRoleGames)
                    .ThenInclude(prg => prg.Player)
                .ToList();

            var edition = _db.Editions
                .Where(m => m.EditionId == editionId)
                .Include(m => m.RolesEdition)
                    .ThenInclude(rm => rm.Role)
                .ToList()
                .Select(m => new EditionEntities(m, gamesWithThisEdition))
                .FirstOrDefault();

            return edition == null ? NotFound() : edition;
        }

        [HttpPost]
        [Route("")]
        public IActionResult CreateEdition([FromBody] JObject data)
        {
            try
            {
                var edition = GetEditionDataFromBody(data, out string error);
                if (error != null)
                {
                    return BadRequest(error);
                }

                // Save edition with name
                _db.Add(edition);
                _db.SaveChanges();

                Misc.UpdateHistory.AddUpdateHistory(_db,
                    UpdateHistoryAction.Create,
                    UpdateHistoryType.Edition,
                    _isStorytellerAuthorizationHandler,
                    Request.Headers,
                    new ObjectUpdateHistory(edition));

                return Created("", null);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException);
            }
        }

        [HttpPut]
        [Route("")]
        public IActionResult UpdateEdition([FromBody] JObject data)
        {
            try
            {
                if (!long.TryParse(data["editionId"]?.ToString(), out long editionId))
                {
                    return BadRequest($"Aucun id de module trouvé.");
                }
                var edition = _db.Editions
                    .Where(e => e.EditionId == editionId)
                    .Include(e => e.RolesEdition)
                        .ThenInclude(re => re.Role)
                    .FirstOrDefault();

                if (edition == null)
                {
                    return BadRequest($"Le module avec l'id {editionId} n'a pas été trouvé.");
                }

                var editionTemp = GetEditionDataFromBody(data, out string error, edition.Name);

                if (error != null)
                {
                    return BadRequest(error);
                }

                var oldEdition = new Edition()
                {
                    Name = edition.Name,
                    RolesEdition = new(edition.RolesEdition)
                };

                edition.Name = editionTemp.Name;

                _db.RemoveRange(_db.RolesEdition.Where(re => re.EditionId == edition.EditionId));
                _db.SaveChanges();

                edition.RolesEdition = editionTemp.RolesEdition;

                _db.SaveChanges();

                Misc.UpdateHistory.AddUpdateHistory(_db,
                    UpdateHistoryAction.Update,
                    UpdateHistoryType.Edition,
                    _isStorytellerAuthorizationHandler,
                    Request.Headers,
                    new ObjectUpdateHistory(edition, oldEdition));

                return Created("", null);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException);
            }
        }

        [HttpDelete]
        [Route("{editionId}")]
        public IActionResult DeleteEdition(long editionId)
        {
            try
            {
                if (!_db.Editions.Any(p => p.EditionId == editionId))
                {
                    return NotFound();
                }

                var edition = _db.Editions.First(p => p.EditionId == editionId);

                if (_db.Games.Any(g => g.EditionId == editionId))
                {
                    edition.IsHidden = true;
                    _db.SaveChanges();
                }
                else
                {
                    _db.Editions.Remove(edition);
                    _db.SaveChanges();
                }

                Misc.UpdateHistory.AddUpdateHistory(_db,
                    UpdateHistoryAction.Delete,
                    UpdateHistoryType.Edition,
                    _isStorytellerAuthorizationHandler,
                    Request.Headers,
                    new ObjectUpdateHistory(edition));

                return Accepted();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException);
            }
        }

        #region Private methods

        private Edition GetEditionDataFromBody(JObject data, out string error, string editionName = null)
        {
            error = null;

            string? name = data["editionName"]?.ToString().Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                error = $"Le nom du module est vide.";
                return null;
            }
            if ((string.IsNullOrWhiteSpace(editionName) || editionName.ToLowerRemoveDiacritics() != name.ToLowerRemoveDiacritics()) &&
                _db.Editions.ToList().Any(m => m.Name.ToLowerRemoveDiacritics() == name.ToLowerRemoveDiacritics()))
            {
                error = $"Un module avec le nom '{name}' existe déjà.";
                return null;
            }


            // Try to convert to Role object from database to ensure it exists
            List<long>? rolesId = data["rolesId"]?.ToObject<List<long>>();
            List<RoleEdition> rolesEditionDb = new();
            if (rolesId != null)
            {
                foreach (var roleId in rolesId)
                {
                    Role? roleDb = _db.Roles.FirstOrDefault(r => r.RoleId == roleId);
                    if (roleDb == null)
                    {
                        error = $"Le rôle avec l'id '{roleId}' n'a pas été trouvé.";
                        return null;
                    }
                    rolesEditionDb.Add(new(roleDb, null));
                }
            }

            Edition edition = new(name)
            {
                RolesEdition = rolesEditionDb,
            };

            return edition;
        }

        #endregion
    }
}