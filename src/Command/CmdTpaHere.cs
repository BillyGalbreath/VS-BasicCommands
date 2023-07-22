using BasicCommands.Configuration;
using BasicCommands.Player;
using BasicCommands.TeleportRequest;

namespace BasicCommands.Command {
    public class CmdTpaHere : CmdTpa {
        public CmdTpaHere(Config.Command cmd) : base(cmd) { }

        public override TpRequest Create(BasicPlayer sender, BasicPlayer target) {
            return new TpaHereRequest(sender, target).Message("ask-here");
        }
    }
}
