using BasicCommands.Player;
using Vintagestory.API.Common;

namespace BasicCommands.Command {
    internal class CmdSetHome : AbstractCommand {
        internal CmdSetHome() : base("sethome", "Set home to your current location", new[] { "createhome" }, new WordArgParser("name", true)) { }

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

            player.AddHome(name, player.BlockPos);

            return TextCommandResult.Success($"Home {name} has been set.");
        }
    }
}
