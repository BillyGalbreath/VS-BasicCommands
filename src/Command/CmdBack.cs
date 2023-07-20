using BasicCommands.Configuration;
using BasicCommands.Player;
using Vintagestory.API.Common;
using Vintagestory.API.Config;

namespace BasicCommands.Command {
    public class CmdBack : AbstractCommand {
        public CmdBack(Config.Command cmd) : base(cmd) { }

        public override TextCommandResult Execute(TextCommandCallingArgs args) {
            BasicPlayer player = BasicPlayer.Get(args.Caller.Player);
            if (player == null) {
                return TextCommandResult.Error(Lang.Get("player-only-command", "0x0001"));
            }

            if (player.LastPos == null) {
                return TextCommandResult.Success(Lang.Get("back-empty"));
            }

            player.TeleportTo(player.LastPos);

            return TextCommandResult.Success(Lang.Get("back-success"));
        }
    }
}
