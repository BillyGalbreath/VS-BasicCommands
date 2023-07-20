using Vintagestory.API.Common;

namespace BasicCommands.Command {
    internal class CmdTeleportDeny : AbstractCommand {
        internal CmdTeleportDeny() : base("teleportdeny", "Rejects teleport requests", new[] { "tpdeny", "tpno" }) {
        }

        internal override TextCommandResult Execute(TextCommandCallingArgs args) {
            throw new System.NotImplementedException();
        }
    }
}
