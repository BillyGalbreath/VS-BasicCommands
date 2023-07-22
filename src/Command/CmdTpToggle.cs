using BasicCommands.Configuration;
using BasicCommands.Player;
using BasicCommands.TeleportRequest;
using Vintagestory.API.Common;

namespace BasicCommands.Command {
    public class CmdTpToggle : AbstractCommand {
        public CmdTpToggle() : base(new BoolArgParser("on|off", "enable", false)) { }

        public override TextCommandResult Execute(BasicPlayer sender, TextCommandCallingArgs args) {
            bool enabled = args.Parsers[0].IsMissing ? !sender.AllowTeleportRequests : (bool)args[0];

            if (!enabled) {
                TpRequest.RemovePendingForTarget(sender)?.Message("denied");
            }

            return TextCommandResult.Success(Lang.Get("tptoggle-success", enabled ? Lang.Get("on") : Lang.Get("off")));
        }
    }
}
