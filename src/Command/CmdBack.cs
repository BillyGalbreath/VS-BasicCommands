using BasicCommands.Configuration;
using BasicCommands.Player;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace BasicCommands.Command {
    public class CmdBack : AbstractCommand {
        public override TextCommandResult Execute(BasicPlayer sender, TextCommandCallingArgs args) {
            BlockPos lastPos = sender.LastPos;
            if (lastPos == null) {
                return TextCommandResult.Success(Lang.Get("back-empty"));
            }

            sender.TeleportTo(lastPos);

            return TextCommandResult.Success(Lang.Get("back-success"));
        }
    }
}
