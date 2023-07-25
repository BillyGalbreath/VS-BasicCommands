using BasicCommands.Command.Parser;
using BasicCommands.Configuration;
using BasicCommands.Player;
using BasicCommands.TeleportRequest;
using Vintagestory.API.Common;

namespace BasicCommands.Command;

public class CmdTpa : AbstractCommand {
    public CmdTpa() : base(new CaseInsensitiveAndImpartialOnlinePlayerArgParser("target", BasicCommandsMod.Instance().API, true)) { }

    public override TextCommandResult Execute(BasicPlayer sender, TextCommandCallingArgs args) {
        BasicPlayer target = BasicPlayer.Get((IPlayer)args[0]);
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

    public virtual TpRequest Create(BasicPlayer sender, BasicPlayer target) {
        return new TpaRequest(sender, target).Message("ask");
    }
}
