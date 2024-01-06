using BotcRoles.Enums;

namespace BotcRoles.Entities
{
    public class PlayerRoleEntities
    {
        public PlayerRoleEntities(Models.PlayerRoleGame playerRoleGame)
        {
            this.Player = new PlayerEntities(playerRoleGame.Player);
            this.Role = new RoleEntities(playerRoleGame.Role);
            this.FinalAlignment = playerRoleGame.FinalAlignment;
        }

        public PlayerEntities Player { get; set; }
        public RoleEntities Role { get; set; }
        public Alignment FinalAlignment { get; set; }
    }
}
