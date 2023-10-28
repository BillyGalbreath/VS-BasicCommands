using BasicCommands.Configuration;
using BasicCommands.Player;
using BasicCommands.TeleportRequest;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace BasicCommands.Command;

public class CmdTpToggle : AbstractCommand {
    public CmdTpToggle(ICoreServerAPI api, Config config) : base(api, config, new BoolArgParser("on|off", "enable", false)) { }

    protected override TextCommandResult Execute(BasicPlayer sender, TextCommandCallingArgs args) {
        sender.AllowTeleportRequests = args.Parsers[0].IsMissing ? !sender.AllowTeleportRequests : (bool)args[0];

        if (!sender.AllowTeleportRequests) {
            TpRequest.GetPendingForTarget(sender)?.Message("denied").Remove();
        }

        return TextCommandResult.Success(Lang.Get("tptoggle-success", sender.AllowTeleportRequests ? Lang.Get("on") : Lang.Get("off")));
    }
}
