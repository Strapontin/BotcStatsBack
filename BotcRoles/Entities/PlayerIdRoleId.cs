using BotcRoles.Enums;

namespace BotcRoles.Entities
{
    public class PlayerIdRoleId
    {
        public PlayerIdRoleId(long playerId, long roleId, Alignment finalAlignment)
        {
            PlayerId = playerId;
            RoleId = roleId;
            FinalAlignment = finalAlignment;
        }

        public long PlayerId { get; set; }
        public long RoleId { get; set; }
        public Alignment FinalAlignment { get; set; }
    }
}
