using BotcRoles.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BotcRoles.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly ILogger<PlayerController> _logger;
        private readonly ModelContext _db;

        public PlayerController(ILogger<PlayerController> logger, ModelContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet]
        [Route("")]
        public IEnumerable<Player> GetPlayers()
        {
            var players = _db.Players;
            return players;
        }

        [HttpGet]
        [Route("{playerId}")]
        public Player? GetPlayer(long playerId)
        {
            var player = _db.Players
                .Where(p => p.PlayerId == playerId)
                .Include(p => p.PlayerRoleGames)
                .FirstOrDefault();

            return player;
        }

        [Authorize(AuthenticationSchemes = "Discord")]
        [HttpPost]
        [Route("{playerName}")]
        public IActionResult PostPlayer(string playerName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(playerName))
                {
                    return BadRequest($"Le nom du joueur est vide.");
                }

                if (_db.Players.Any(p => p.Name == playerName))
                {
                    return BadRequest($"Un joueur avec le nom '{playerName}' existe déjà.");
                }

                _db.Add(new Player(playerName));
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