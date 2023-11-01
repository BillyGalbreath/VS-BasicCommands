using BasicCommands.Configuration;
using BasicCommands.Player;
using BasicCommands.TeleportRequest;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace BasicCommands.Command;

public class CmdTpAccept : AbstractCommand {
    public CmdTpAccept(ICoreServerAPI api, Config config) : base(api, config) { }

    protected override CommandResult Execute(BasicPlayer sender, TextCommandCallingArgs args) {
        TpRequest? request = TpRequest.GetPendingForTarget(sender);
        if (request == null) {
            return Error("teleport-request-nothing-pending");
        }

        request.Message("accepted", false).Accept();

        return Success();
    }
}
