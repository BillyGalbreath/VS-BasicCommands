using BasicCommands.Player;

namespace BasicCommands.TeleportRequest;

public class TpaHereRequest : TpRequest {
    public TpaHereRequest(BasicPlayer sender, BasicPlayer target) : base(sender, target) { }

    protected override void Teleport() {
        target.TeleportTo(sender.CurPos);
    }
}
