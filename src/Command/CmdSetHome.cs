using Essentials.Player;
using Vintagestory.API.Common;

namespace Essentials.Command {
    internal class CmdSetHome : AbstractCommand {
        internal CmdSetHome() : base("sethome", "Set home to your current location", new[] { "createhome" }, new WordArgParser("name", true)) { }

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

            player.AddHome(name, player.BlockPos);

            return TextCommandResult.Success($"Home {name} has been set.");
        }
    }
}
