using BasicCommands.Command.Parser;
using BasicCommands.Configuration;
using BasicCommands.Player;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace BasicCommands.Command;

public class CmdDelKit : AbstractCommand {
    public CmdDelKit(ICoreServerAPI api, Config config) : base(api, config, new KitArgParser("name")) { }

    protected override CommandResult Execute(BasicPlayer sender, TextCommandCallingArgs args) {
        Kit kit = (Kit)args[0];
        if (!Kits.Remove(kit.Name)) {
            return Error("kit-cannot-delete", kit.Name);
        }

        Kits.Save(api);

        return Success("delkit-success", kit.Name);
    }
}
