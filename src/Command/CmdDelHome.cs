using BasicCommands.Configuration;
using BasicCommands.Player;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace BasicCommands.Command;

public class CmdDelHome : AbstractCommand {
    public CmdDelHome(ICoreServerAPI api, Config config) : base(api, config, new WordArgParser("name", true)) { }

    protected override TextCommandResult Execute(BasicPlayer sender, TextCommandCallingArgs args) {
        if (args.Parsers[0].IsMissing) {
            return TextCommandResult.Error("must-specify-home");
        }

        string name = args[0].ToString()!.Trim().ToLower();
        if (!CmdHome.VALID_NAME.IsMatch(name)) {
            return TextCommandResult.Error("invalid-home-name");
        }

        // ReSharper disable once ConvertIfStatementToReturnStatement
        if (!sender.RemoveHome(name)) {
            return TextCommandResult.Error("home-doesnt-exist", name);
        }

        return TextCommandResult.Success("delhome-success", name);
    }
}
