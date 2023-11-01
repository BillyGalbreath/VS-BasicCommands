using BasicCommands.Configuration;
using BasicCommands.Player;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace BasicCommands.Command;

public class CmdDelHome : AbstractCommand {
    public CmdDelHome(ICoreServerAPI api, Config config) : base(api, config, new WordArgParser("name", true)) { }

    protected override CommandResult Execute(BasicPlayer sender, TextCommandCallingArgs args) {
        if (args.Parsers[0].IsMissing) {
            return Error("must-specify-home");
        }

        string name = args[0].ToString()!.Trim().ToLower();
        if (!CmdHome.VALID_NAME.IsMatch(name)) {
            return Error("invalid-home-name");
        }

        // ReSharper disable once ConvertIfStatementToReturnStatement
        if (!sender.RemoveHome(name)) {
            return Error("home-doesnt-exist", name);
        }

        return Success("delhome-success", name);
    }
}
