using BasicCommands.Configuration;
using BasicCommands.Player;
using Vintagestory.API.Common;

namespace BasicCommands.Command {
    public class CmdBack : AbstractCommand {
        public CmdBack(Config.Command cmd) : base(cmd) { }

        public override TextCommandResult Execute(BasicPlayer player, TextCommandCallingArgs args) {
            player.TeleportTo(player.LastPos);

            return TextCommandResult.Success(Lang.Get("back-success"));
        }
    }
}
