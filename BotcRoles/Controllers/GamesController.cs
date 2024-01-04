using BotcRoles.Entities;
using BotcRoles.Enums;
using BotcRoles.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace BotcRoles.Controllers
{
    [Authorize(Policy = "IsStoryteller")]
    [ApiController]
    [Route("[controller]")]
    public class GamesController : ControllerBase
    {
        private readonly ILogger<GamesController> _logger;
        private readonly ModelContext _db;

        public GamesController(ILogger<GamesController> logger, ModelContext db)
        {
            _logger = logger;
            _db = db;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("")]
        public ActionResult<IEnumerable<GameEntities>> GetGames()
        {
            var allGames = _db.Games
                .Include(g => g.Storyteller)
                .Include(g => g.Edition)
                .Include(g => g.PlayerRoleGames)
                //.OrderByDescending(g => g.DatePlayed)
                //.ThenBy(g => g.Storyteller.Name)
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

        [AllowAnonymous]
        [HttpGet]
        [Route("ByPlayerId/{playerId}")]
        public ActionResult<List<GameEntities>> GetGamesByPlayerId(long playerId)
        {
            var games = _db.Games
                .Where(g => g.PlayerRoleGames.Any(prg => prg.PlayerId == playerId))
                .Include(g => g.Storyteller)
                .OrderByDescending(g => g.DatePlayed)
                .ThenBy(g => g.Storyteller.Name)
                .ToList()
                .Select(g => new GameEntities(g))
                .ToList();

            return games;
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
                var game = _db.Games.FirstOrDefault(g => g.GameId == gameId);

                if (game == null)
                {
                    return BadRequest($"La partie avec l'id {gameId} n'a pas été trouvée.");
                }

                var gameTemp = GetGameDataFromBody(data, out string error);

                if (error != null)
                {
                    return BadRequest(error);
                }

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
                _db.Games.Remove(_db.Games.First(g => g.GameId == gameId));
                _db.SaveChanges();
                return Accepted();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException);
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

        #endregion
    }
}