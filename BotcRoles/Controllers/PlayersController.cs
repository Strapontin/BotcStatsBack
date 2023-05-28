using BotcRoles.Entities;
using BotcRoles.Helper;
using BotcRoles.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace BotcRoles.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayersController : ControllerBase
    {
        private readonly ILogger<PlayersController> _logger;
        private readonly ModelContext _db;

        public PlayersController(ILogger<PlayersController> logger, ModelContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet]
        [Route("")]
        public ActionResult<IEnumerable<PlayerEntities>> GetPlayers()
        {
            var players = _db.Players
                .Include(p => p.PlayerRoleGames)
                .Select(p => new PlayerEntities(_db, p))
                .ToList()
                .OrderBy(p => p.Name.ToLowerRemoveDiacritics())
                .ThenBy(p => p.Pseudo.ToLowerRemoveDiacritics())
                .ToList();
            return players;
        }

        [HttpGet]
        [Route("{playerId}")]
        public ActionResult<PlayerEntities> GetPlayerById(long playerId)
        {
            var player = _db.Players
                .Where(p => p.PlayerId == playerId)
                .Include(p => p.PlayerRoleGames)
                    .ThenInclude(prg => prg.Game)
                .Select(p => new PlayerEntities(_db, p))
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

                player.Name = playerTemp.Name;
                player.Pseudo = playerTemp.Pseudo;
                _db.SaveChanges();

                return Created("", null);
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
                          (string.IsNullOrWhiteSpace(pseudo) || m.Pseudo.ToLowerRemoveDiacritics() == pseudo.ToLowerRemoveDiacritics())))
            {
                error = $"Un joueur avec le nom '{name}' et le pseudo '{pseudo}' existe déj?.";
                return null;
            }


            Player player = new(name, pseudo);
            return player;
        }

        #endregion   
    }
}
