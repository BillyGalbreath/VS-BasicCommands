using BasicCommands.Configuration;
using BasicCommands.Player;
using System.Text.RegularExpressions;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace BasicCommands.Command {
    public partial class CmdHome : AbstractCommand {
        [GeneratedRegex("^(?i)[a-z][a-z0-9]*$", RegexOptions.None, "en-US")]
        public static partial Regex ValidName();

        public CmdHome(Config.Command cmd) : base(cmd, new WordArgParser("name", true)) { }

        public override TextCommandResult Execute(TextCommandCallingArgs args) {
            BasicPlayer player = BasicPlayer.Get(args.Caller.Player);
            if (player == null) {
                return TextCommandResult.Error(Lang.Get("player-only-command", "0x0001"));
            }

            if (args.Parsers[0].IsMissing) {
                return TextCommandResult.Success(Lang.Get("must-specify-home"));
            }

            string name = args[0].ToString().Trim().ToLower();
            if (!ValidName().IsMatch(name)) {
                return TextCommandResult.Success(Lang.Get("invalid-home-name"));
            }

            BlockPos pos = player.GetHome(name);
            if (pos == null) {
                return TextCommandResult.Success(Lang.Get("home-doesnt-exist", name));
            }

            player.TeleportTo(pos);

            return TextCommandResult.Success(Lang.Get("home-success", name));
        }
    }
}
