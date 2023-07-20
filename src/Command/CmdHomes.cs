using Essentials.Player;
using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Common;

namespace Essentials.Command {
    internal class CmdHomes : AbstractCommand {
        internal CmdHomes() : base("homes", "List your homes", new[] { "listhomes" }) { }

        internal override TextCommandResult Execute(TextCommandCallingArgs args) {
            EssPlayer player = EssPlayer.Get(args.Caller.Player);
            if (player == null) {
                return TextCommandResult.Error("Player only command.");
            }

            IEnumerable<string> homes = player.ListHomes();
            if (homes == null || homes.Count() == 0) {
                return TextCommandResult.Success("You don't have any homes set.");
            }

            return TextCommandResult.Success($"Homes: {string.Join(", ", homes)}");
        }
    }
}
