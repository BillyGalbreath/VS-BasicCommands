using BasicCommands.Configuration;
using BasicCommands.Player;
using BasicCommands.TeleportRequest;
using Vintagestory.API.Common;

namespace BasicCommands.Command {
    public class CmdTpa : AbstractCommand {
        public CmdTpa(Config.Command cmd) : base(cmd, new OnlinePlayerArgParser("target", BasicCommandsMod.Instance().API, true)) { }

        public override TextCommandResult Execute(BasicPlayer player, TextCommandCallingArgs args) {
            BasicPlayer target = BasicPlayer.Get((IPlayer)args[0]);
            if (target == null) {
                return TextCommandResult.Success(Lang.Get("player-not-found"));
            }

            if (!target.AllowTeleportRequests) {
                return TextCommandResult.Success(Lang.Get("teleport-request-disabled", target.Name));
            }

            TpRequest.Add(Create(player, target));

            return TextCommandResult.Success();
        }

        public virtual TpRequest Create(BasicPlayer sender, BasicPlayer target) {
            return new TpaRequest(sender, target).Message("ask");
        }
    }
}
