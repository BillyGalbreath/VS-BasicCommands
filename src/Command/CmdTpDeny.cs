using BasicCommands.Configuration;
using BasicCommands.Player;
using BasicCommands.TeleportRequest;
using Vintagestory.API.Common;

namespace BasicCommands.Command {
    public class CmdTpDeny : AbstractCommand {
        public override TextCommandResult Execute(BasicPlayer player, TextCommandCallingArgs args) {
            TpRequest request = TpRequest.GetPendingForTarget(player);
            if (request == null) {
                return TextCommandResult.Success(Lang.Get("teleport-request-nothing-pending"));
            }
            request.Remove();
            request.Message("denied");
            return TextCommandResult.Success();
        }
    }
}
