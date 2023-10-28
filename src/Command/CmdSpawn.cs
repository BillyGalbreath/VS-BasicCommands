﻿using BasicCommands.Configuration;
using BasicCommands.Player;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace BasicCommands.Command;

public class CmdSpawn : AbstractCommand {
    public CmdSpawn(ICoreServerAPI api, Config config) : base(api, config) { }

    protected override TextCommandResult Execute(BasicPlayer sender, TextCommandCallingArgs args) {
        sender.TeleportTo(api.World.DefaultSpawnPosition.XYZ);

        return TextCommandResult.Success(Lang.Get("spawn-success"));
    }
}
