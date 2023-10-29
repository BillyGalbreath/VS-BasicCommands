using BasicCommands.Configuration;
using BasicCommands.Player;
using BasicCommands.TeleportRequest;
using Vintagestory.API.Server;

namespace BasicCommands.Command;

public class CmdTpaHere : CmdTpa {
    public CmdTpaHere(ICoreServerAPI api, Config config) : base(api, config) { }

    protected override TpRequest Create(BasicPlayer sender, BasicPlayer target) {
        return new TpaHereRequest(sender, target).Message("ask-here", false);
    }
}
