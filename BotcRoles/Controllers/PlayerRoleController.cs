using BotcRoles.Enums;
using BotcRoles.Models;
using Microsoft.AspNetCore.Mvc;

namespace BotcRoles.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayerRoleController : ControllerBase
    {
        private readonly ILogger<PlayerRoleController> _logger;
        private ModelContext db;

        public PlayerRoleController(ILogger<PlayerRoleController> logger)
        {
            _logger = logger;
            db = new ModelContext();
        }

        //[HttpGet]
        //[Route("")]
        //public IEnumerable<Role> Get()
        //{
        //    var roles = db.Roles;
        //    return roles;
        //}

        [HttpPost]
        [Route("{playerId}/{gameId}")]
        public IActionResult Post(long playerId, long gameId)
        {
            try
            {
                var player = db.Players.Find(playerId);
                var game = db.Games.Find(gameId);

                if (player == null)
                {
                    return BadRequest($"Le joueur avec l'id '{playerId}' n'a pas été trouvé.");
                }

                if (game == null)
                {
                    return BadRequest($"La partie avec l'id '{gameId}' n'a pas été trouvé.");
                }

                db.Add(new PlayerRoleGame(player, game));
                db.SaveChanges();

                return Created("PlayerRoles", null);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException);
            }
        }
    }
}