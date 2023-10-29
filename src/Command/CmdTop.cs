using BasicCommands.Configuration;
using BasicCommands.Player;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace BasicCommands.Command;

public class CmdTop : AbstractCommand {
    public CmdTop(ICoreServerAPI api, Config config) : base(api, config) { }

    protected override TextCommandResult Execute(BasicPlayer sender, TextCommandCallingArgs args) {
        Vec3d pos = sender.EntityPos.XYZ.Clone();

        pos.Y = api.WorldManager.GetSurfacePosY(pos.XInt, pos.ZInt) ?? -1;

        if (pos.Y < 0) {
            return TextCommandResult.Error("top-failed");
        }

        sender.TeleportTo(pos);

        return TextCommandResult.Success("top-success");
    }
}
