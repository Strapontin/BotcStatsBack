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
        private readonly ModelContext _db;

        public PlayerRoleController(ILogger<PlayerRoleController> logger, ModelContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet]
        [Route("")]
        public IEnumerable<PlayerRoleGame> Get()
        {
            var roles = _db.PlayerRoles.ToList();
            return roles;
        }

        [HttpPost]
        [Route("{playerId}/{gameId}")]
        public IActionResult Post(long playerId, long gameId)
        {
            try
            {
                var player = _db.Players.Find(playerId);
                var game = _db.Games.Find(gameId);

                if (player == null)
                {
                    return BadRequest($"Le joueur avec l'id '{playerId}' n'a pas été trouvé.");
                }

                if (game == null)
                {
                    return BadRequest($"La partie avec l'id '{gameId}' n'a pas été trouvé.");
                }

                _db.Add(new PlayerRoleGame(player, game));
                _db.SaveChanges();

                return Created("", null);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException);
            }
        }

        [HttpPut]
        [Route("{playerId}/{gameId}/{roleId}/{finalAlignment}")]
        public IActionResult Put(long playerId, long gameId, long roleId, Alignment finalAlignment)
        {
            try
            {
                var playerRole = _db.PlayerRoles.Where(pr => pr.PlayerId == playerId &&
                                                            pr.GameId == gameId).FirstOrDefault();

                if (playerRole == null)
                {
                    return BadRequest($"Le PlayerRole n'a pas été trouvé. L'utilisateur a-t-il bien été ajouté à la partie ?");
                }

                if (!playerRole.Game.Module.RoleModules.Any(rm => rm.RoleId == roleId))
                {
                    return BadRequest($"Le rôle que vous essayez d'assigner n'appartient pas aux roles assignés au module de cette game.");
                }

                playerRole.RoleId = roleId;
                playerRole.FinalAlignment = finalAlignment;
                _db.SaveChanges();

                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.InnerException);
            }
        }
    }
}