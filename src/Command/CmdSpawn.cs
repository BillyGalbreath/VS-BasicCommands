using BasicCommands.Configuration;
using BasicCommands.Player;
using Vintagestory.API.Common;

namespace BasicCommands.Command;

public class CmdSpawn : AbstractCommand {
    public override TextCommandResult Execute(BasicPlayer sender, TextCommandCallingArgs args) {
        sender.TeleportTo(BasicCommandsMod.Instance().API.World.DefaultSpawnPosition.XYZ);

        return TextCommandResult.Success(Lang.Get("spawn-success"));
    }
}
