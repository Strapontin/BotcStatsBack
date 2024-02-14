using BotcRoles.Entities;
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
    public class GamesDraftController : ControllerBase
    {
        private readonly ILogger<GamesDraftController> _logger;
        private readonly ModelContext _db;
        private readonly IAuthorizationHandler _isStorytellerAuthorizationHandler;

        public GamesDraftController(ILogger<GamesDraftController> logger, ModelContext db, IAuthorizationHandler isStorytellerAuthorizationHandler)
        {
            _logger = logger;
            _db = db;
            _isStorytellerAuthorizationHandler = isStorytellerAuthorizationHandler;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("")]
        public ActionResult<IEnumerable<GameDraftEntities>> GetGamesDraft()
        {
            var allGamesDraft = _db.GamesDraft
                .Include(g => g.Storyteller)
                .OrderBy(g => g.DatePlayed)
                .ThenBy(g => g.Storyteller.Name)
                .Include(g => g.Edition)
                .ToList()
                .Select(g => new GameDraftEntities(g))
                .ToList();

            return allGamesDraft;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("{gameDraftId}")]
        public ActionResult<GameDraftEntities> GetGameDraftById(long gameDraftId)
        {
            var game = _db.GamesDraft
                .Where(g => g.GameDraftId == gameDraftId)
                .Include(g => g.Edition)
                    .ThenInclude(e => e.RolesEdition)
                        .ThenInclude(re => re.Role)
                .Include(g => g.Storyteller)
                .ToList()
                .Select(g => new GameDraftEntities(g))
                .FirstOrDefault();

            return game == null ? NotFound() : game;
        }

        [HttpPost]
        [Route("")]
        public IActionResult CreateGameDraft([FromBody] JObject data)
        {
            try
            {
                var gameDraft = GetGameDraftDataFromBody(data, out string error);
                if (error != null)
                {
                    return BadRequest(error);
                }

                _db.Add(gameDraft);
                _db.SaveChanges();

                Misc.UpdateHistory.AddUpdateHistory(_db,
                    UpdateHistoryAction.Create,
                    UpdateHistoryType.GameDraft,
                    _isStorytellerAuthorizationHandler,
                    Request.Headers,
                    new ObjectUpdateHistory(gameDraft));

                return Created("", null);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException);
            }
        }

        [HttpPut]
        [Route("")]
        public IActionResult UpdateGameDraft([FromBody] JObject data)
        {
            try
            {
                if (!long.TryParse(data["gameDraftId"]?.ToString(), out long gameDraftId))
                {
                    return BadRequest($"Aucun id de partie de rappel trouvé.");
                }
                var gameDraft = _db.GamesDraft
                    .Where(g => g.GameDraftId == gameDraftId)
                    .Include(g => g.Edition)
                    .Include(g => g.Storyteller)
                    .FirstOrDefault();

                if (gameDraft == null)
                {
                    return BadRequest($"La partie de rappel avec l'id {gameDraftId} n'a pas été trouvée.");
                }

                var gameTemp = GetGameDraftDataFromBody(data, out string error);

                if (error != null)
                {
                    return BadRequest(error);
                }

                var oldGameDraft = new GameDraft()
                {
                    Edition = gameDraft.Edition,
                    Storyteller = gameDraft.Storyteller,
                    DatePlayed = gameDraft.DatePlayed,
                    Notes = gameDraft.Notes,
                };

                gameDraft.Edition = gameTemp.Edition;
                gameDraft.Storyteller = gameTemp.Storyteller;
                gameDraft.DatePlayed = gameTemp.DatePlayed;
                gameDraft.Notes = gameTemp.Notes;

                _db.SaveChanges();

                Misc.UpdateHistory.AddUpdateHistory(_db,
                    UpdateHistoryAction.Update,
                    UpdateHistoryType.GameDraft,
                    _isStorytellerAuthorizationHandler,
                    Request.Headers,
                    new ObjectUpdateHistory(gameDraft, oldGameDraft));

                return Created("", null);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException);
            }
        }

        [HttpDelete]
        [Route("{gameDraftId}")]
        public IActionResult DeleteGameDraft(long gameDraftId)
        {
            try
            {
                var gameDraft = _db.GamesDraft
                    .Where(g => g.GameDraftId == gameDraftId)
                    .Include(g => g.Storyteller)
                    .First();

                _db.GamesDraft.Remove(gameDraft);
                _db.SaveChanges();

                Misc.UpdateHistory.AddUpdateHistory(_db,
                    UpdateHistoryAction.Delete,
                    UpdateHistoryType.GameDraft,
                    _isStorytellerAuthorizationHandler,
                    Request.Headers,
                    new ObjectUpdateHistory(gameDraft));

                return Accepted();
            }
            catch (Exception )
            {
                return StatusCode(500, "Une erreur interne est survenue pendant la suppression de la partie de rappel.");
            }
        }

        #region Private methods

        private GameDraft GetGameDraftDataFromBody(JObject data, out string error)
        {
            error = null;

            if (!long.TryParse(data["editionId"]?.ToString(), out long editionId))
            {
                error = $"Une partie de rappel doit avoir un module.";
                return null;
            }
            var edition = _db.Editions.FirstOrDefault(e => e.EditionId == editionId);
            if (edition == null)
            {
                error = $"Le module avec l'id '{editionId}' n'a pas été trouvé";
                return null;
            }

            if (!long.TryParse(data["storytellerId"]?.ToString(), out long storytellerId))
            {
                error = $"Une partie de rappel doit avoir un conteur.";
                return null;
            }
            var storyteller = _db.Players.FirstOrDefault(e => e.PlayerId == storytellerId);
            if (storyteller == null)
            {
                error = $"Le conteur avec l'id '{storytellerId}' n'a pas été trouvé";
                return null;
            }

            if (!DateTime.TryParse(data["datePlayed"]?.ToString(), out DateTime datePlayed))
            {
                error = $"La date n'est pas correcte.";
                return null;
            }

            var notes = data["notes"]?.ToString();


            GameDraft gameDraft = new(edition, storyteller, datePlayed, notes);
            return gameDraft;
        }

        #endregion
    }
}