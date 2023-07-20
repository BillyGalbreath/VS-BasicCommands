using Essentials.Player;
using System.Text.RegularExpressions;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace Essentials.Command {
    internal class CmdHome : AbstractCommand {
        internal static readonly Regex VALID_NAME = new Regex(@"^(?i)[a-z][a-z0-9]*$");

        internal CmdHome() : base("home", "Teleport to your home", new WordArgParser("name", true)) { }

        internal override TextCommandResult Execute(TextCommandCallingArgs args) {
            EssPlayer player = EssPlayer.Get(args.Caller.Player);
            if (player == null) {
                return TextCommandResult.Error("Player only command.");
            }

            if (args.Parsers[0].IsMissing) {
                return TextCommandResult.Success("Must specify a home name.");
            }

            string name = args[0].ToString().Trim().ToLower();
            if (!VALID_NAME.IsMatch(name)) {
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
