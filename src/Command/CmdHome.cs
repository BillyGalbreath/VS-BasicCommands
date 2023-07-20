using BasicCommands.Player;
using System.Text.RegularExpressions;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace BasicCommands.Command {
    internal partial class CmdHome : AbstractCommand {
        [GeneratedRegex("^(?i)[a-z][a-z0-9]*$", RegexOptions.None, "en-US")]
        internal static partial Regex ValidName();

        internal CmdHome() : base("home", "Teleport to your home", new WordArgParser("name", true)) { }

        internal override TextCommandResult Execute(TextCommandCallingArgs args) {
            BasicPlayer player = BasicPlayer.Get(args.Caller.Player);
            if (player == null) {
                return TextCommandResult.Error("Player only command.");
            }

            if (args.Parsers[0].IsMissing) {
                return TextCommandResult.Success("Must specify a home name.");
            }

            string name = args[0].ToString().Trim().ToLower();
            if (!ValidName().IsMatch(name)) {
                return TextCommandResult.Success("Invalid home name.");
            }

            BlockPos pos = player.GetHome(name);
            if (pos == null) {
                return TextCommandResult.Success($"Home named {name} does not exist.");
            }

            player.TeleportTo(pos);

            return TextCommandResult.Success($"Teleported to {name}.");
        }
    }
}
