using BasicCommands.Configuration;
using BasicCommands.Player;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;

namespace BasicCommands.Command;

public class CmdSetSpawn : AbstractCommand {
    public override TextCommandResult Execute(BasicPlayer sender, TextCommandCallingArgs args) {
        BlockPos pos = sender.EntityPos.AsBlockPos;
        BasicCommandsMod.Instance().API.WorldManager.SetDefaultSpawnPosition(pos.X, pos.Y, pos.Z);

        return TextCommandResult.Success(Lang.Get("setspawn-success"));
    }
}
