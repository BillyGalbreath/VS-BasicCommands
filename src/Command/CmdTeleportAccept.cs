using Vintagestory.API.Common;

namespace BasicCommands.Command {
    internal class CmdTeleportAccept : AbstractCommand {
        internal CmdTeleportAccept() : base("teleportaccept", "Accepts teleport requests", new[] { "tpaccept", "tpyes" }) {
        }

        internal override TextCommandResult Execute(TextCommandCallingArgs args) {
            throw new System.NotImplementedException();
        }
    }
}
