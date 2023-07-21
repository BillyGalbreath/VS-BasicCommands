using BasicCommands.Configuration;
using BasicCommands.Player;
using BasicCommands.TeleportRequest;
using Vintagestory.API.Common;

namespace BasicCommands.Command {
    public class CmdTpAccept : AbstractCommand {
        public CmdTpAccept(Config.Command cmd) : base(cmd) { }

        public override TextCommandResult Execute(BasicPlayer player, TextCommandCallingArgs args) {
            TpRequest request = TpRequest.GetPendingForTarget(player);
            if (request == null) {
                return TextCommandResult.Success(Lang.Get("teleport-request-nothing-pending"));
            }
            request.Accept();
            request.Message("accepted");
            return TextCommandResult.Success();
        }
    }
}
