using BasicCommands.Command.Parser;
using BasicCommands.Configuration;
using BasicCommands.Player;
using BasicCommands.TeleportRequest;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace BasicCommands.Command;

public class CmdTpa : AbstractCommand {
    public CmdTpa(ICoreServerAPI api, Config config) : base(api, config, new BasicPlayerArgParser("target")) { }

    protected override TextCommandResult Execute(BasicPlayer sender, TextCommandCallingArgs args) {
        BasicPlayer target = (BasicPlayer)args[0];
        if (target == null) {
            return TextCommandResult.Success(Lang.Get("player-not-found"));
        }

        if (sender.Equals(target)) {
            return TextCommandResult.Success(Lang.Get("teleport-request-self-not-allowed"));
        }

        if (!target.AllowTeleportRequests) {
            return TextCommandResult.Success(Lang.Get("teleport-request-disabled", target.Name));
        }

        if (TpRequest.HasPendingForTarget(target)) {
            return TextCommandResult.Success(Lang.Get("teleport-request-pending-target", target.Name));
        }

        if (TpRequest.HasPendingForSender(sender)) {
            return TextCommandResult.Success(Lang.Get("teleport-request-pending-sender"));
        }

        TpRequest.Add(Create(sender, target));

        return TextCommandResult.Success();
    }

    protected virtual TpRequest Create(BasicPlayer sender, BasicPlayer target) {
        return new TpaRequest(sender, target).Message("ask");
    }
}
