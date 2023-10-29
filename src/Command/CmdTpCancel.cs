using BasicCommands.Configuration;
using BasicCommands.Player;
using BasicCommands.TeleportRequest;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace BasicCommands.Command;

public class CmdTpCancel : AbstractCommand {
    public CmdTpCancel(ICoreServerAPI api, Config config) : base(api, config) { }

    protected override TextCommandResult Execute(BasicPlayer sender, TextCommandCallingArgs args) {
        TpRequest? request = TpRequest.GetPendingForSender(sender);
        if (request == null) {
            return TextCommandResult.Error("teleport-request-nothing-pending");
        }

        request.Message("cancelled", true).Remove();

        return TextCommandResult.Success();
    }
}
