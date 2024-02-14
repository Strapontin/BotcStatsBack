using BotcRoles.Entities;
using BotcRoles.Enums;
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
    public class GamesController : ControllerBase
    {
        private readonly ILogger<GamesController> _logger;
        private readonly ModelContext _db;
        private readonly IAuthorizationHandler _isStorytellerAuthorizationHandler;

        public GamesController(ILogger<GamesController> logger, ModelContext db, IAuthorizationHandler isStorytellerAuthorizationHandler)
        {
            _logger = logger;
            _db = db;
            _isStorytellerAuthorizationHandler = isStorytellerAuthorizationHandler;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("")]
        public ActionResult<IEnumerable<GameEntities>> GetGames()
        {
            var allGames = _db.Games
                .Include(g => g.Storyteller)
                .OrderBy(g => g.DatePlayed)
                .ThenBy(g => g.Storyteller.Name)
                .Include(g => g.Edition)
                .Include(g => g.PlayerRoleGames)
                    .ThenInclude(prg => prg.Role)
                .Include(g => g.PlayerRoleGames)
                    .ThenInclude(prg => prg.Player)
                .ToList();

            var games = allGames
                .Select(g => new GameEntities(g, allGames))
                .ToList();

            return games;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("{gameId}")]
        public ActionResult<GameEntities> GetGameById(long gameId)
        {
            var game = _db.Games
                .Where(g => g.GameId == gameId)
                .Include(g => g.Edition)
                    .ThenInclude(e => e.RolesEdition)
                        .ThenInclude(re => re.Role)
                .Include(g => g.Storyteller)
                .Include(g => g.PlayerRoleGames)
                    .ThenInclude(prg => prg.Player)
                .Include(g => g.PlayerRoleGames)
                    .ThenInclude(prg => prg.Role)
                .Include(g => g.DemonBluffs)
                    .ThenInclude(demonBluff => demonBluff.Role)
                .ToList()
                .Select(g => new GameEntities(g))
                .FirstOrDefault();

            return game == null ? NotFound() : game;
        }

        private List<GameEntities> FillDbSetGamesByParamId(IQueryable<Game> games)
        {
            return games
                .Include(g => g.Storyteller)
                .Include(g => g.PlayerRoleGames)
                    .ThenInclude(prg => prg.Role)
                .Include(g => g.PlayerRoleGames)
                    .ThenInclude(prg => prg.Player)
                .OrderByDescending(g => g.DatePlayed)
                .ThenBy(g => g.Storyteller.Name)
                .ToList()
                .Select(g => new GameEntities(g))
                .ToList();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("ByPlayerId/{playerId}")]
        public ActionResult<List<GameEntities>> GetGamesByPlayerId(long playerId)
        {
            var games = _db.Games
                .Where(g => g.PlayerRoleGames.Any(prg => prg.PlayerId == playerId));

            var result = FillDbSetGamesByParamId(games);
            return result;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("ByRoleId/{roleId}")]
        public ActionResult<List<GameEntities>> GetGamesByRoleId(long roleId)
        {
            var games = _db.Games
                .Where(g => g.PlayerRoleGames.Any(prg => prg.RoleId == roleId));

            var result = FillDbSetGamesByParamId(games);
            return result;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("ByEditionId/{editionId}")]
        public ActionResult<List<GameEntities>> GetGamesByEditionId(long editionId)
        {
            var games = _db.Games
                .Where(g => g.EditionId == editionId);

            var result = FillDbSetGamesByParamId(games);
            return result;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("ByStorytellerId/{storytellerId}")]
        public ActionResult<List<GameEntities>> GetGamesByStorytellerId(long storytellerId)
        {
            var games = _db.Games
                .Where(g => g.StorytellerId == storytellerId)
                .Include(g => g.Storyteller)
                .Include(g => g.PlayerRoleGames)
                .Include(g => g.Edition)
                .OrderByDescending(g => g.DatePlayed)
                .ThenBy(g => g.Storyteller.Name)
                .ToList()
                .Select(g => new GameEntities(g))
                .ToList();

            return games;
        }

        [HttpPost]
        [Route("")]
        public IActionResult CreateGame([FromBody] JObject data)
        {
            try
            {
                var game = GetGameDataFromBody(data, out string error);
                if (error != null)
                {
                    return BadRequest(error);
                }

                DateTime dateCreated = DateTime.Now;
                game.DateCreated = dateCreated;

                _db.Add(game);
                _db.SaveChanges();

                Misc.UpdateHistory.AddUpdateHistory(_db,
                    UpdateHistoryAction.Create,
                    UpdateHistoryType.Game,
                    _isStorytellerAuthorizationHandler,
                    Request.Headers,
                    new ObjectUpdateHistory(game));

                // if a gameDraft has the same properties then we delete it
                FindAndDeleteGameDraftFromGameCreated(game);

                return Created("", null);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException);
            }
        }

        [HttpPut]
        [Route("")]
        public IActionResult UpdateGame([FromBody] JObject data)
        {
            try
            {
                if (!long.TryParse(data["gameId"]?.ToString(), out long gameId))
                {
                    return BadRequest($"Aucun id de partie trouvé.");
                }
                var game = _db.Games
                    .Where(g => g.GameId == gameId)
                    .Include(g => g.Edition)
                    .Include(g => g.PlayerRoleGames)
                        .ThenInclude(prg => prg.Player)
                    .Include(g => g.PlayerRoleGames)
                        .ThenInclude(prg => prg.Role)
                    .Include(g => g.DemonBluffs)
                        .ThenInclude(db => db.Role)
                    .FirstOrDefault();

                if (game == null)
                {
                    return BadRequest($"La partie avec l'id {gameId} n'a pas été trouvée.");
                }

                var gameTemp = GetGameDataFromBody(data, out string error);

                if (error != null)
                {
                    return BadRequest(error);
                }

                var oldGame = new Game()
                {
                    Edition = game.Edition,
                    Storyteller = game.Storyteller,
                    DatePlayed = game.DatePlayed,
                    Notes = game.Notes,
                    WinningAlignment = game.WinningAlignment,
                    PlayerRoleGames = new(game.PlayerRoleGames),
                    DemonBluffs = new(game.DemonBluffs),
                };

                game.Edition = gameTemp.Edition;
                game.Storyteller = gameTemp.Storyteller;
                game.DatePlayed = gameTemp.DatePlayed;
                game.Notes = gameTemp.Notes;
                game.WinningAlignment = gameTemp.WinningAlignment;

                _db.RemoveRange(_db.PlayerRoleGames.Where(prg => prg.GameId == game.GameId));
                _db.RemoveRange(_db.DemonBluffs.Where(demonBluff => demonBluff.GameId == game.GameId));
                _db.SaveChanges();

                game.PlayerRoleGames = gameTemp.PlayerRoleGames;
                game.DemonBluffs = gameTemp.DemonBluffs;

                _db.SaveChanges();

                Misc.UpdateHistory.AddUpdateHistory(_db,
                    UpdateHistoryAction.Update,
                    UpdateHistoryType.Game,
                    _isStorytellerAuthorizationHandler,
                    Request.Headers,
                    new ObjectUpdateHistory(game, oldGame));

                return Created("", null);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException);
            }
        }

        [HttpDelete]
        [Route("{gameId}")]
        public IActionResult DeleteGame(long gameId)
        {
            try
            {
                var game = _db.Games
                    .Where(g => g.GameId == gameId)
                    .Include(g => g.Storyteller)
                    .First();

                _db.Games.Remove(game);
                _db.SaveChanges();

                Misc.UpdateHistory.AddUpdateHistory(_db,
                    UpdateHistoryAction.Delete,
                    UpdateHistoryType.Game,
                    _isStorytellerAuthorizationHandler,
                    Request.Headers,
                    new ObjectUpdateHistory(game));

                return Accepted();
            }
            catch (Exception)
            {
                return StatusCode(500, "Une erreur interne est survenue pendant la suppression de la partie.");
            }
        }

        #region Private methods

        private Game GetGameDataFromBody(JObject data, out string error)
        {
            error = null;

            if (!long.TryParse(data["editionId"]?.ToString(), out long editionId))
            {
                error = $"Une partie doit avoir un module.";
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
                error = $"Une partie doit avoir un conteur.";
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

            if (!int.TryParse(data["winningAlignment"]?.ToString(), out int alignmentInt) || !Enum.IsDefined(typeof(Alignment), alignmentInt))
            {
                error = $"L'alignement gagnant n'est pas renseigné.";
                return null;
            }
            Alignment winningAlignment = (Alignment)alignmentInt;


            // Try to convert to Role object from database to ensure it exists
            List<PlayerIdRoleId>? playersIdRolesId = data["playersIdRolesId"]?.ToObject<List<PlayerIdRoleId>>();
            if (playersIdRolesId == null)
            {
                error = $"Aucun joueur-rôle trouvé.";
                return null;
            }

            List<PlayerRoleGame> playersRoles = new();
            foreach (var playerIdRoleId in playersIdRolesId)
            {
                Player? playerDb = _db.Players.FirstOrDefault(p => p.PlayerId == playerIdRoleId.PlayerId);
                if (playerDb == null)
                {
                    error = $"Le joueur avec l'id '{playerIdRoleId.PlayerId}' n'a pas été trouvé.";
                    return null;
                }

                Role? roleDb = _db.Roles.FirstOrDefault(r => r.RoleId == playerIdRoleId.RoleId);
                if (roleDb == null)
                {
                    error = $"Le rôle avec l'id '{playerIdRoleId.RoleId}' n'a pas été trouvé.";
                    return null;
                }

                var pgr = new PlayerRoleGame(playerDb, roleDb, null);
                pgr.FinalAlignment = playerIdRoleId.FinalAlignment;

                playersRoles.Add(pgr);
            }

            // Demon bluffs
            List<long>? demonBluffsId = data["demonBluffsId"]?.ToObject<List<long>>();
            if (demonBluffsId == null)
            {
                error = $"Aucun demon bluff.";
                return null;
            }

            List<DemonBluff> demonBluffs = new();
            foreach (var demonBluffId in demonBluffsId)
            {
                Role? roleDb = _db.Roles.FirstOrDefault(r => r.RoleId == demonBluffId);
                if (roleDb == null)
                {
                    error = $"Le demonBluff avec le rôle Id '{demonBluffId}' n'a pas été trouvé.";
                    return null;
                }
                demonBluffs.Add(new DemonBluff(roleDb, null));
            }


            Game game = new(edition, storyteller, datePlayed, notes, winningAlignment)
            {
                PlayerRoleGames = playersRoles,
                DemonBluffs = demonBluffs,
            };

            return game;
        }

        private void FindAndDeleteGameDraftFromGameCreated(Game game)
        {
            var gameDraft = _db.GamesDraft
                .Include(g => g.Storyteller)
                .FirstOrDefault(g => g.EditionId == game.EditionId &&
            g.StorytellerId == game.StorytellerId &&
            g.DatePlayed == game.DatePlayed);

            if (gameDraft == null) return;

            _db.Remove(gameDraft);
            _db.SaveChanges();

            Misc.UpdateHistory.AddUpdateHistory(_db,
                UpdateHistoryAction.Delete,
                UpdateHistoryType.GameDraft,
                _isStorytellerAuthorizationHandler,
                Request.Headers,
                new ObjectUpdateHistory(gameDraft));
        }

        #endregion
    }
}