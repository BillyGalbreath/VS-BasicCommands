using BasicCommands.Player;

namespace BasicCommands.TeleportRequest;

public class TpaRequest : TpRequest {
    public TpaRequest(BasicPlayer sender, BasicPlayer target) : base(sender, target) { }

    public override void Accept() {
        sender.TeleportTo(target.CurPos);
    }
}
