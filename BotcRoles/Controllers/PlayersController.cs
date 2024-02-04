using BotcRoles.Entities;
using BotcRoles.Helper;
using BotcRoles.Misc;
using BotcRoles.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace BotcRoles.Controllers
{
    [Authorize(Policy = "IsStoryteller")]
    [ApiController]
    [Route("[controller]")]
    public class PlayersController : ControllerBase
    {
        private readonly ILogger<PlayersController> _logger;
        private readonly ModelContext _db;
        private readonly IAuthorizationHandler _isStorytellerAuthorizationHandler;

        public PlayersController(ILogger<PlayersController> logger, ModelContext db, IAuthorizationHandler isStorytellerAuthorizationHandler)
        {
            _logger = logger;
            _db = db;
            _isStorytellerAuthorizationHandler = isStorytellerAuthorizationHandler;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("")]
        public ActionResult<IEnumerable<PlayerEntities>> GetPlayers()
        {
            var players = _db.Players
                .Where(p => !p.IsHidden)
                .Include(p => p.PlayerRoleGames)
                    .ThenInclude(prg => prg.Game)
                .Include(p => p.PlayerRoleGames)
                    .ThenInclude(prg => prg.Role)
                .ToList()
                .Select(p => new PlayerEntities(p))
                .OrderByDescending(p => p.NbGamesPlayed)
                .ThenByDescending(p => p.NbGamesWon)
                .ToList();
            return players;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("{playerId}")]
        public ActionResult<PlayerEntities> GetPlayerById(long playerId)
        {
            var player = _db.Players
                .Where(p => p.PlayerId == playerId)
                .Include(p => p.PlayerRoleGames)
                    .ThenInclude(prg => prg.Game)
                .Include(p => p.PlayerRoleGames)
                    .ThenInclude(prg => prg.Role)
                .ToList()
                .Select(p => new PlayerEntities(p))
                .FirstOrDefault();

            return player == null ? NotFound() : player;
        }

        [HttpPost]
        [Route("")]
        public IActionResult AddPlayer([FromBody] JObject data)
        {
            try
            {
                var player = GetPlayerDataFromBody(data, out string error);
                if (error != null)
                {
                    return BadRequest(error);
                }

                _db.Add(player);
                _db.SaveChanges();

                Misc.UpdateHistory.AddUpdateHistory(_db,
                    UpdateHistoryAction.Create,
                    UpdateHistoryType.Player,
                    _isStorytellerAuthorizationHandler,
                    Request.Headers,
                    new ObjectUpdateHistory(player));


                return Created("", null);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException);
            }
        }

        [HttpPut]
        [Route("")]
        public IActionResult UpdatePlayer([FromBody] JObject data)
        {
            try
            {
                if (!long.TryParse(data["playerId"]?.ToString(), out long playerId))
                {
                    return BadRequest($"Aucun id de module trouvé.");
                }
                var player = _db.Players.FirstOrDefault(e => e.PlayerId == playerId);

                if (player == null)
                {
                    return BadRequest($"Le joueur avec l'id {playerId} n'a pas été trouvé.");
                }

                var playerTemp = GetPlayerDataFromBody(data, out string error, player.Name);

                if (error != null)
                {
                    return BadRequest(error);
                }

                var oldPlayer = new Player()
                {
                    Name = player.Name,
                    Pseudo = player.Pseudo
                };

                player.Name = playerTemp.Name;
                player.Pseudo = playerTemp.Pseudo;
                _db.SaveChanges();

                Misc.UpdateHistory.AddUpdateHistory(_db,
                    UpdateHistoryAction.Update,
                    UpdateHistoryType.Player,
                    _isStorytellerAuthorizationHandler,
                    Request.Headers,
                    new ObjectUpdateHistory(player, oldPlayer));

                return Created("", null);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException);
            }
        }

        [HttpDelete]
        [Route("{playerId}")]
        public IActionResult DeletePlayer(long playerId)
        {
            try
            {
                if (!_db.Players.Any(p => p.PlayerId == playerId))
                {
                    return NotFound();
                }

                var player = _db.Players.First(p => p.PlayerId == playerId);

                if (_db.PlayerRoleGames.Any(prg => prg.PlayerId == playerId) ||
                    _db.Games.Any(g => g.StorytellerId == playerId))
                {
                    player.IsHidden = true;
                    _db.SaveChanges();
                }
                else
                {
                    _db.Players.Remove(player);
                    _db.SaveChanges();
                }

                Misc.UpdateHistory.AddUpdateHistory(_db,
                    UpdateHistoryAction.Delete,
                    UpdateHistoryType.Player,
                    _isStorytellerAuthorizationHandler,
                    Request.Headers,
                    new ObjectUpdateHistory(player));

                return Accepted();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException);
            }
        }

        #region Private methods

        private Player GetPlayerDataFromBody(JObject data, out string error, string playerName = null)
        {
            error = null;

            string? name = data["playerName"]?.ToString()?.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                error = $"Le nom du joueur est vide.";
                return null;
            }

            string? pseudo = data["pseudo"]?.ToString()?.Trim();

            if ((string.IsNullOrWhiteSpace(playerName) || playerName.ToLowerRemoveDiacritics() != name.ToLowerRemoveDiacritics()) &&
                _db.Players.ToList()
                .Any(m => m.Name.ToLowerRemoveDiacritics() == name.ToLowerRemoveDiacritics() &&
                          (m.Pseudo.ToLowerRemoveDiacritics() == pseudo.ToLowerRemoveDiacritics())))
            {
                error = $"Un joueur avec le nom '{name}' et le pseudo '{pseudo}' existe déjà.";
                return null;
            }


            Player player = new(name, pseudo);
            return player;
        }

        #endregion   
    }
}
