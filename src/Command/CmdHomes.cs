using BasicCommands.Configuration;
using BasicCommands.Player;
using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Common;

namespace BasicCommands.Command {
    public class CmdHomes : AbstractCommand {
        public CmdHomes(Config.Command cmd) : base(cmd) { }

        public override TextCommandResult Execute(TextCommandCallingArgs args) {
            BasicPlayer player = BasicPlayer.Get(args.Caller.Player);
            if (player == null) {
                return TextCommandResult.Error(Lang.Get("player-only-command", "0x0001"));
            }

            IEnumerable<string> homes = player.ListHomes();
            if (homes == null || !homes.Any()) {
                return TextCommandResult.Success(Lang.Get("no-homes-set"));
            }

            return TextCommandResult.Success(Lang.Get("homes-success", string.Join(", ", homes)));
        }
    }
}
