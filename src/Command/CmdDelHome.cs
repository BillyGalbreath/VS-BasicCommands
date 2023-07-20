using Essentials.Player;
using Vintagestory.API.Common;

namespace Essentials.Command {
    internal class CmdDelHome : AbstractCommand {
        internal CmdDelHome() : base("delhome", "Removes a home", new[] { "remhome", "rmhome", "removehome", "deletehome" }, new WordArgParser("name", true)) { }

        internal override TextCommandResult Execute(TextCommandCallingArgs args) {
            EssPlayer player = EssPlayer.Get(args.Caller.Player);
            if (player == null) {
                return TextCommandResult.Error("Player only command.");
            }

            if (args.Parsers[0].IsMissing) {
                return TextCommandResult.Success("Must specify a home name.");
            }

            string name = args[0].ToString().Trim().ToLower();
            if (!CmdHome.VALID_NAME.IsMatch(name)) {
                return TextCommandResult.Success("Invalid home name.");
            }

            if (!player.RemoveHome(name)) {
                return TextCommandResult.Success($"Home named {name} does not exist.");
            }

            return TextCommandResult.Success($"Deleted home named {name}.");
        }
    }
}
