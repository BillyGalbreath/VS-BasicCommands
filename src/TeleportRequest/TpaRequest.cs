using BasicCommands.Player;

namespace BasicCommands.TeleportRequest;

public class TpaRequest : TpRequest {
    public TpaRequest(BasicPlayer sender, BasicPlayer target) : base(sender, target) { }

    protected override void Teleport() {
        sender.TeleportTo(target.EntityPos.XYZ);
    }
}
