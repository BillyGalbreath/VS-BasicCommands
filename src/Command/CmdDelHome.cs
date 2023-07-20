using BasicCommands.Configuration;
using BasicCommands.Player;
using Vintagestory.API.Common;

namespace BasicCommands.Command {
    public class CmdDelHome : AbstractCommand {
        public CmdDelHome(Config.Command cmd) : base(cmd, new WordArgParser("name", true)) { }

        public override TextCommandResult Execute(BasicPlayer player, TextCommandCallingArgs args) {
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
