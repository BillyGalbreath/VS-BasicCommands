using BasicCommands.Player;
using Vintagestory.API.Common;

namespace BasicCommands.Command {
    internal class CmdDelHome : AbstractCommand {
        internal CmdDelHome() : base("delhome", "Removes a home", new[] { "remhome", "rmhome", "removehome", "deletehome" }, new WordArgParser("name", true)) { }

        internal override TextCommandResult Execute(TextCommandCallingArgs args) {
            BasicPlayer player = BasicPlayer.Get(args.Caller.Player);
            if (player == null) {
                return TextCommandResult.Error("Player only command.");
            }

            if (args.Parsers[0].IsMissing) {
                return TextCommandResult.Success("Must specify a home name.");
            }

            string name = args[0].ToString().Trim().ToLower();
            if (!CmdHome.ValidName().IsMatch(name)) {
                return TextCommandResult.Success("Invalid home name.");
            }

            if (!player.RemoveHome(name)) {
                return TextCommandResult.Success($"Home named {name} does not exist.");
            }

            return TextCommandResult.Success($"Deleted home named {name}.");
        }
    }
}
