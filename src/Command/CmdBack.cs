using BasicCommands.Configuration;
using BasicCommands.Player;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace BasicCommands.Command {
    public class CmdBack : AbstractCommand {
        public override TextCommandResult Execute(BasicPlayer player, TextCommandCallingArgs args) {
            BlockPos lastPos = player.LastPos;
            if (lastPos == null) {
                return TextCommandResult.Success(Lang.Get("back-empty"));
            }

            player.TeleportTo(lastPos);

            return TextCommandResult.Success(Lang.Get("back-success"));
        }
    }
}
