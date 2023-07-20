using BasicCommands.Configuration;
using BasicCommands.Player;
using Vintagestory.API.Common;
using Vintagestory.API.Config;

namespace BasicCommands.Command {
    public class CmdDelHome : AbstractCommand {
        public CmdDelHome(Config.Command cmd) : base(cmd, new WordArgParser("name", true)) { }

        public override TextCommandResult Execute(TextCommandCallingArgs args) {
            BasicPlayer player = BasicPlayer.Get(args.Caller.Player);
            if (player == null) {
                return TextCommandResult.Error(Lang.Get("player-only-command", "0x0001"));
            }

            if (args.Parsers[0].IsMissing) {
                return TextCommandResult.Success(Lang.Get("must-specify-home"));
            }

            string name = args[0].ToString().Trim().ToLower();
            if (!CmdHome.ValidName().IsMatch(name)) {
                return TextCommandResult.Success(Lang.Get("invalid-home-name"));
            }

            if (!player.RemoveHome(name)) {
                return TextCommandResult.Success(Lang.Get("home-doesnt-exist", name));
            }

            return TextCommandResult.Success(Lang.Get("delhome-success", name));
        }
    }
}
