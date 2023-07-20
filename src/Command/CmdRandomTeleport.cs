using Vintagestory.API.Common;

namespace Essentials.Command {
    internal class CmdRandomTeleport : AbstractCommand {
        internal CmdRandomTeleport() : base("randomteleport", "Teleports you to a random location", new[] { "rtp", "tpr", "tprandom", "randomtp", "teleportrandom" }) {
        }

        internal override TextCommandResult Execute(TextCommandCallingArgs args) {
            throw new System.NotImplementedException();
        }
    }
}
