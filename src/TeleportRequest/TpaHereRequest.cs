using BasicCommands.Player;

namespace BasicCommands.TeleportRequest;

public class TpaHereRequest : TpRequest {
    public TpaHereRequest(BasicPlayer sender, BasicPlayer target) : base(sender, target) { }

    public override void Accept() {
        target.TeleportTo(sender.CurPos);
    }
}
