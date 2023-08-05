using BasicCommands.Configuration;
using BasicCommands.Player;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace BasicCommands.Command;

public class CmdJump : AbstractCommand {
    public override TextCommandResult Execute(BasicPlayer sender, TextCommandCallingArgs args) {
        BlockSelection target = sender.TargetBlock;

        if (target == null || target.Position == null) {
            return TextCommandResult.Error(Lang.Get("jump-failed"));
        }

        sender.TeleportTo(target.Position.ToVec3d().Add(0.5, 0, 0.5));

        return TextCommandResult.Success(Lang.Get("jump-success"));
    }
}
