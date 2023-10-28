using BasicCommands.Configuration;
using BasicCommands.Player;
using BasicCommands.TeleportRequest;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace BasicCommands.Command;

public class CmdTpDeny : AbstractCommand {
    public CmdTpDeny(ICoreServerAPI api, Config config) : base(api, config) { }

    protected override TextCommandResult Execute(BasicPlayer sender, TextCommandCallingArgs args) {
        TpRequest? request = TpRequest.GetPendingForTarget(sender);
        if (request == null) {
            return TextCommandResult.Success(Lang.Get("teleport-request-nothing-pending"));
        }

        request.Message("denied").Remove();

        return TextCommandResult.Success();
    }
}
