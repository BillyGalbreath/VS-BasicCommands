using BasicCommands.Command.Parser;
using BasicCommands.Configuration;
using BasicCommands.Player;
using BasicCommands.TeleportRequest;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace BasicCommands.Command;

public class CmdTpa : AbstractCommand {
    public CmdTpa(ICoreServerAPI api, Config config) : base(api, config, new BasicPlayerArgParser("target")) { }

    protected override CommandResult Execute(BasicPlayer sender, TextCommandCallingArgs args) {
        BasicPlayer target = (BasicPlayer)args[0];
        if (target == null) {
            return Error("player-not-found");
        }

        if (sender.Equals(target)) {
            return Error("teleport-request-self-not-allowed");
        }

        if (!target.AllowTeleportRequests) {
            return Error("teleport-request-disabled", target.Name);
        }

        if (TpRequest.HasPendingForTarget(target)) {
            return Error("teleport-request-pending-target", target.Name);
        }

        if (TpRequest.HasPendingForSender(sender)) {
            return Error("teleport-request-pending-sender");
        }

        TpRequest.Add(Create(sender, target));

        return Success();
    }

    protected virtual TpRequest Create(BasicPlayer sender, BasicPlayer target) {
        return new TpaRequest(sender, target).Message("ask", false);
    }
}
