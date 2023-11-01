using BasicCommands.Configuration;
using BasicCommands.Player;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace BasicCommands.Command;

public class CmdSetSpawn : AbstractCommand {
    public CmdSetSpawn(ICoreServerAPI api, Config config) : base(api, config) { }

    protected override CommandResult Execute(BasicPlayer sender, TextCommandCallingArgs args) {
        BlockPos pos = sender.EntityPos.AsBlockPos;
        api.WorldManager.SetDefaultSpawnPosition(pos.X, pos.Y, pos.Z);

        return Success("setspawn-success");
    }
}
