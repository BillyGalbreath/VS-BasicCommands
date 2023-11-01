using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using BasicCommands.Configuration;
using BasicCommands.Player;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace BasicCommands.Command;

public class CmdHome : AbstractCommand {
    [SuppressMessage("GeneratedRegex", "SYSLIB1045:Convert to \'GeneratedRegexAttribute\'.")]
    public static readonly Regex VALID_NAME = new("^(?i)[a-z][a-z0-9]*$");

    public CmdHome(ICoreServerAPI api, Config config) : base(api, config, new WordArgParser("name", true)) { }

    protected override CommandResult Execute(BasicPlayer sender, TextCommandCallingArgs args) {
        if (args.Parsers[0].IsMissing) {
            return Error("must-specify-home");
        }

        string name = args[0].ToString()!.Trim().ToLower();
        if (!VALID_NAME.IsMatch(name)) {
            return Error("invalid-home-name");
        }

        Vec3d? pos = sender.GetHome(name);
        if (pos == null) {
            return Error("home-doesnt-exist", name);
        }

        sender.TeleportTo(pos);

        return Success("home-success", name);
    }
}
