﻿using BasicCommands.Configuration;
using BasicCommands.Player;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;

namespace BasicCommands.Command;

public class CmdTpr : AbstractCommand {
    private static readonly HashSet<string> pending = new();

    public override TextCommandResult Execute(BasicPlayer sender, TextCommandCallingArgs args) {
        if (pending.Contains(sender.UID)) {
            return TextCommandResult.Success(Lang.Get("tpr-already-waiting"));
        }

        ICoreServerAPI api = BasicCommandsMod.Instance().API;
        IWorldManagerAPI worldManager = api.WorldManager;

        Random rand = Random.Shared;
        int randX = rand.Next(worldManager.MapSizeX);
        int randZ = rand.Next(worldManager.MapSizeZ);
        int chunkSize = worldManager.ChunkSize;

        pending.Add(sender.UID);

        worldManager.LoadChunkColumnPriority(randX / chunkSize, randZ / chunkSize, new ChunkLoadOptions() {
            OnLoaded = () => {
                pending.Remove(sender.UID);
                int topY = (worldManager.GetSurfacePosY(randX, randZ) ?? 0) + 1;
                sender.TeleportTo(new(randX, topY, randZ));
                sender.SendMessage(Lang.Get("tpr-success", randX, topY, randZ));
            }
        });

        return TextCommandResult.Success(Lang.Get("tpr-wait"));
    }
}
