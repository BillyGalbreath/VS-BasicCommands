using BasicCommands.Configuration;
using BasicCommands.Player;
using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Common;

namespace BasicCommands.Command {
    public class CmdHomes : AbstractCommand {
        public override TextCommandResult Execute(BasicPlayer sender, TextCommandCallingArgs args) {
            IEnumerable<string> homes = sender.ListHomes();
            if (homes == null || !homes.Any()) {
                return TextCommandResult.Success(Lang.Get("no-homes-set"));
            }

            return TextCommandResult.Success(Lang.Get("homes-success", string.Join(", ", homes)));
        }
    }
}
