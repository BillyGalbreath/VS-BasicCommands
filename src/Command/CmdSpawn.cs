using BasicCommands.Configuration;
using BasicCommands.Player;
using Vintagestory.API.Common;

namespace BasicCommands.Command {
    public class CmdSpawn : AbstractCommand {
        public override TextCommandResult Execute(BasicPlayer player, TextCommandCallingArgs args) {
            player.TeleportTo(BasicCommandsMod.Instance().API.World.DefaultSpawnPosition.AsBlockPos);

            return TextCommandResult.Success(Lang.Get("spawn-success"));
        }
    }
}
