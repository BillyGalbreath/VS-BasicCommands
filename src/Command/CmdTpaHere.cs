using BasicCommands.Player;
using BasicCommands.TeleportRequest;

namespace BasicCommands.Command;

public class CmdTpaHere : CmdTpa {
    public override TpRequest Create(BasicPlayer sender, BasicPlayer target) {
        return new TpaHereRequest(sender, target).Message("ask-here");
    }
}
