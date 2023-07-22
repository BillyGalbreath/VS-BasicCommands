using BasicCommands.Configuration;
using BasicCommands.Player;
using BasicCommands.TeleportRequest;
using Vintagestory.API.Common;

namespace BasicCommands.Command {
    public class CmdTpToggle : AbstractCommand {
        public CmdTpToggle() : base(new BoolArgParser("on|off", "enable", false)) { }

        public override TextCommandResult Execute(BasicPlayer player, TextCommandCallingArgs args) {
            bool enabled = args.Parsers[0].IsMissing ? !player.AllowTeleportRequests : (bool)args[0];

            if (!enabled) {
                TpRequest.RemovePendingForTarget(player)?.Message("denied");
            }

            return TextCommandResult.Success(Lang.Get("tptoggle-success", enabled ? Lang.Get("on") : Lang.Get("off")));
        }
    }
}
