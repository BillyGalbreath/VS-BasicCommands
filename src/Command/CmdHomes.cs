﻿using System.Linq;
using BasicCommands.Configuration;
using BasicCommands.Player;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace BasicCommands.Command;

public class CmdHomes : AbstractCommand {
    public CmdHomes(ICoreServerAPI api, Config config) : base(api, config) { }

    protected override TextCommandResult Execute(BasicPlayer sender, TextCommandCallingArgs args) {
        string[] homes = sender.ListHomes().ToArray();

        // ReSharper disable once ConvertIfStatementToReturnStatement
        if (homes.Length == 0) {
            return TextCommandResult.Error("no-homes-set");
        }

        return TextCommandResult.Success("homes-success", string.Join(", ", homes));
    }
}
