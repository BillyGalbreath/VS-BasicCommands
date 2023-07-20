using BasicCommands.Configuration;
using Vintagestory.API.Common;

namespace BasicCommands.Command {
    public class CmdTpDeny : AbstractCommand {
        public CmdTpDeny(Config.Command cmd) : base(cmd) { }

        public override TextCommandResult Execute(TextCommandCallingArgs args) {
            throw new System.NotImplementedException();
        }
    }
}
