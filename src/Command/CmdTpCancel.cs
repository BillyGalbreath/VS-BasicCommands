﻿using BasicCommands.Configuration;
using BasicCommands.Player;
using BasicCommands.TeleportRequest;
using Vintagestory.API.Common;

namespace BasicCommands.Command {
    public class CmdTpCancel : AbstractCommand {
        public override TextCommandResult Execute(BasicPlayer sender, TextCommandCallingArgs args) {
            TpRequest request = TpRequest.GetPendingForSender(sender);
            if (request == null) {
                return TextCommandResult.Success(Lang.Get("teleport-request-nothing-pending"));
            }
            request.Remove();
            request.Message("cancelled");
            return TextCommandResult.Success();
        }
    }
}
