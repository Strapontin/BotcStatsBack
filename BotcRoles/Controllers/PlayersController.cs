using BotcRoles.Entities;
using BotcRoles.Models;
using Microsoft.AspNetCore.Authorization;
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
                .OrderByDescending(p => p.PlayerRoleGames.Count)
                .ThenBy(p => p.Name)
                .Select(p => new PlayerEntities(_db, p))
                .ToList();
            return players;
        }

        [HttpGet]
        [Route("{playerId}")]
        public ActionResult<PlayerEntities> GetPlayerByName(long playerId)
        {
            var player = _db.Players
                .Where(p => p.PlayerId == playerId)
                .Include(p => p.PlayerRoleGames)
                    .ThenInclude(prg => prg.Game)
                .Select(p => new PlayerEntities(_db, p))
                .FirstOrDefault();

            return player == null ? NotFound() : player;
        }

        //[Authorize(AuthenticationSchemes = "Discord")]
        [HttpPost]
        [Route("")]
        public IActionResult PostPlayer([FromBody] JObject data)
        {
            try
            {
                string? playerName = data["playerName"]?.ToString();
                string? pseudo = data["pseudo"]?.ToString();

                if (string.IsNullOrWhiteSpace(playerName))
                {
                    return BadRequest(JObject.FromObject(new { error = $"Le nom du joueur est vide." }));
                }

                if (_db.Players.Any(p => p.Name == playerName && p.Pseudo == pseudo))
                {
                    return BadRequest(JObject.FromObject(new { error = $"Un joueur avec le nom '{playerName}' et le pseudo '{pseudo}' existe déjà." }));
                }

                _db.Add(new Player(playerName, pseudo));
                _db.SaveChanges();

                return Created("", null);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException);
            }
        }
    }
}