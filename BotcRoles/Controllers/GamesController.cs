using BotcRoles.Entities;
using BotcRoles.Enums;
using BotcRoles.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BotcRoles.Controllers
{
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

        [HttpGet]
        [Route("")]
        public ActionResult<IEnumerable<GameEntities>> Get()
        {
            var games = _db.Games
                .Include(g => g.Edition)
                .Include(g => g.StoryTeller)
                .Include(g => g.PlayerRoleGames)
                .Select(g => new GameEntities(_db, g))
                .ToList();

            return games;
        }

        [HttpGet]
        [Route("{gameId}")]
        public ActionResult<GameEntities> Get(long gameId)
        {
            var game = _db.Games
                .Where(g => g.GameId == gameId)
                .Include(g => g.Edition)
                    .ThenInclude(g => g.Games)
                .Include(g => g.StoryTeller)
                .Include(g => g.PlayerRoleGames)
                    .ThenInclude(prg => prg.Player)
                .Include(g => g.PlayerRoleGames)
                    .ThenInclude(prg => prg.Role)
                .Select(g => new GameEntities(_db, g))
                .FirstOrDefault();

            return game == null ? NotFound() : game;
        }

        [HttpPost]
        [Route("createGame")]
        public IActionResult Post(long EditionId, long storyTellerId)
        {
            try
            {
                var Edition = _db.Editions.Find(EditionId);
                var storyTeller = _db.Players.Find(storyTellerId);

                if (Edition == null)
                {
                    return BadRequest($"Le Edition avec l'id '{Edition}' n'a pas été trouvé.");
                }

                if (storyTeller == null)
                {
                    return BadRequest($"Le joueur avec l'id '{storyTeller}' n'a pas été trouvé.");
                }

                _db.Add(new Game(Edition, storyTeller));
                _db.SaveChanges();

                return Created("", null);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException);
            }
        }

        //[HttpDelete]
        //[Route("{gameId}")]
        //public IActionResult Delete(long gameId)
        //{
        //    try
        //    {
        //        _db.Games.Remove(_db.Games.First(g => g.GameId == gameId));
        //        _db.SaveChanges();
        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex.InnerException);
        //    }
        //}

        //[HttpPost]
        //[Route("{gameId}")]
        //public IActionResult AddPlayerInGame(long gameId, [FromQuery] long? playerId)
        //{
        //    try
        //    {
        //        if (playerId == null)
        //        {
        //            return BadRequest($"Le paramètre playerId n'est pas spécifié");
        //        }

        //        var game = _db.Games
        //            .Where(g => g.GameId == gameId)
        //            .Include(g => g.PlayerRoleGames)
        //            .FirstOrDefault();

        //        var player = _db.Players.Find(playerId);

        //        if (game == null)
        //        {
        //            return BadRequest($"La partie avec l'id '{gameId}' n'a pas été trouvé.");
        //        }

        //        if (player == null)
        //        {
        //            return BadRequest($"Le joueur avec l'id '{playerId}' n'a pas été trouvé.");
        //        }

        //        if (game.PlayerRoleGames.Any(prg => prg.PlayerId == playerId))
        //        {
        //            return BadRequest($"Le joueur est déjà dans la partie.");
        //        }

        //        _db.Add(new PlayerRoleGame(player, game));
        //        _db.SaveChanges();

        //        return Created("", null);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex.InnerException);
        //    }
        //}

        //[HttpPut]
        //[Route("{gameId}/players/{playerId}")]
        //public IActionResult ChangePlayerRoleAndAlignmentInGame(long gameId, long playerId, [FromQuery] long roleId, [FromQuery] Alignment finalAlignment)
        //{
        //    try
        //    {
        //        var playerRole = _db.PlayerRoleGames.Where(pr => pr.PlayerId == playerId &&
        //                                                    pr.GameId == gameId)
        //            .Include(pr => pr.Game.Edition.RolesEdition)
        //            .FirstOrDefault();

        //        if (playerRole == null)
        //        {
        //            return BadRequest($"Le PlayerRole n'a pas été trouvé. L'utilisateur a-t-il bien été ajouté à la partie ?");
        //        }

        //        if (!playerRole.Game.Edition.RolesEdition.Any(rm => rm.RoleId == roleId))
        //        {
        //            return BadRequest($"Le rôle que vous essayez d'assigner n'appartient pas aux roles assign�sEditiondule de cette game.");
        //        }

        //        playerRole.RoleId = roleId;
        //        playerRole.FinalAlignment = finalAlignment;
        //        _db.SaveChanges();

        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex.InnerException);
        //    }
        //}

        //[HttpGet]
        //[Route("{gameId}/players")]
        //public IEnumerable<Player>? GetPlayers(long gameId)
        //{
        //    try
        //    {
        //        var game = _db.Games.Find(gameId);

        //        var result = game?.PlayerRoleGames.Select(prg => prg.Player);
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, ex.Message);
        //        return null;
        //    }
        //}

        //[HttpGet]
        //[Route("{gameId}/players/{playerId}")]
        //public IEnumerable<PlayerRoleGame>? GetPlayerRoleFromGame(long gameId, long playerId)
        //{
        //    try
        //    {
        //        var game = _db.Games.Find(gameId);
        //        var player = _db.Players.Find(playerId);


        //        var result = player?.PlayerRoleGames.Where(prg => prg.GameId == gameId);
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, ex.Message);
        //        return null;
        //    }
        //}
    }
}