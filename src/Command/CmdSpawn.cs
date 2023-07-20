using BasicCommands.Player;
using Vintagestory.API.Common;

namespace BasicCommands.Command {
    internal class CmdSpawn : AbstractCommand {
        internal CmdSpawn() : base("spawn", "Teleport to the spawnpoint") {
        }

        internal override TextCommandResult Execute(TextCommandCallingArgs args) {
            BasicPlayer player = BasicPlayer.Get(args.Caller.Player);
            if (player == null) {
                return TextCommandResult.Error("Player only command.");
            }

            player.TeleportTo(BasicCommandsMod.Instance().API.World.DefaultSpawnPosition.AsBlockPos);

            return TextCommandResult.Success("Teleported to spawn.");
        }
    }
}
