using BasicCommands.Configuration;
using BasicCommands.Player;
using Vintagestory.API.Common;

namespace BasicCommands.Command {
    public class CmdSetHome : AbstractCommand {
        public CmdSetHome(Config.Command cmd) : base(cmd, new WordArgParser("name", true)) { }

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

            player.AddHome(name, player.BlockPos);

            return TextCommandResult.Success(Lang.Get("sethome-success", name));
        }
    }
}
