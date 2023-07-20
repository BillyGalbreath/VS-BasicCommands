using BasicCommands.Configuration;
using BasicCommands.Player;
using Vintagestory.API.Common;

namespace BasicCommands.Command {
    public class CmdSpawn : AbstractCommand {
        public CmdSpawn(Config.Command cmd) : base(cmd) { }

        public override TextCommandResult Execute(TextCommandCallingArgs args) {
            BasicPlayer player = BasicPlayer.Get(args.Caller.Player);
            if (player == null) {
                return TextCommandResult.Error(Lang.Get("player-only-command", "0x0001"));
            }

            player.TeleportTo(BasicCommandsMod.Instance().API.World.DefaultSpawnPosition.AsBlockPos);

            return TextCommandResult.Success(Lang.Get("spawn-success"));
        }
    }
}
