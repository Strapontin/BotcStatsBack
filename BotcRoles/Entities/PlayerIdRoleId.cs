namespace BotcRoles.Entities
{
    public class PlayerIdRoleId
    {
        public PlayerIdRoleId(long playerId, long roleId)
        {
            PlayerId = playerId;
            RoleId = roleId;
        }

        public long PlayerId { get; set; }
        public long RoleId { get; set; }
    }
}
