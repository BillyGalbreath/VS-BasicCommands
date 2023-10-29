using BasicCommands.Configuration;
using BasicCommands.Player;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace BasicCommands.Command;

public class CmdBack : AbstractCommand {
    public CmdBack(ICoreServerAPI api, Config config) : base(api, config) { }

    protected override TextCommandResult Execute(BasicPlayer sender, TextCommandCallingArgs args) {
        if (sender.LastPos == null) {
            return TextCommandResult.Error("back-empty");
        }

        sender.TeleportTo(sender.LastPos);

        return TextCommandResult.Success("back-success");
    }
}
