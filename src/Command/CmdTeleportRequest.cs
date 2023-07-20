using Vintagestory.API.Common;

namespace BasicCommands.Command {
    internal class CmdTeleportRequest : AbstractCommand {
        internal CmdTeleportRequest() : base("teleportrequest", "Request to teleport to the specified player", new[] { "tpa", "tpask" }) {
        }

        internal override TextCommandResult Execute(TextCommandCallingArgs args) {
            throw new System.NotImplementedException();
        }
    }
}
