using Essentials.Player;
using Vintagestory.API.Common;

namespace Essentials.Command {
    internal class CmdSpawn : AbstractCommand {
        internal CmdSpawn() : base("spawn", "Teleport to the spawnpoint") {
        }

        internal override TextCommandResult Execute(TextCommandCallingArgs args) {
            EssPlayer player = EssPlayer.Get(args.Caller.Player);
            if (player == null) {
                return TextCommandResult.Error("Player only command.");
            }

            player.TeleportTo(EssentialsMod.Instance().API.World.DefaultSpawnPosition.AsBlockPos);

            return TextCommandResult.Success("Teleported to spawn.");
        }
    }
}
