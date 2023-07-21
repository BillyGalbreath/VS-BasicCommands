using BasicCommands.Player;
using System.Threading.Tasks;

namespace BasicCommands.TeleportRequest {
    public class TpaRequest : TpRequest {
        public TpaRequest(BasicPlayer sender, BasicPlayer target) : base(sender, target) { }

        public override void Accept() {
            throw new System.NotImplementedException();
        }
    }
}
