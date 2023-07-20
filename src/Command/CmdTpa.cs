using BasicCommands.Configuration;
using BasicCommands.Player;
using Vintagestory.API.Common;

namespace BasicCommands.Command {
    public class CmdTpa : AbstractCommand {
        public CmdTpa(Config.Command cmd) : base(cmd) { }

        public override TextCommandResult Execute(BasicPlayer player, TextCommandCallingArgs args) {
            throw new System.NotImplementedException();
        }
    }
}
