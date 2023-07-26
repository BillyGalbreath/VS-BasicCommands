using BasicCommands.Configuration;
using BasicCommands.Player;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;

namespace BasicCommands.Command;

public class CmdTpr : AbstractCommand {
    private static readonly ConcurrentDictionary<string, Vec2i> pending = new();

    public override TextCommandResult Execute(BasicPlayer sender, TextCommandCallingArgs args) {
        if (pending.ContainsKey(sender.Name)) {
            return TextCommandResult.Success(Lang.Get("tpr-already-waiting"));
        }

        IWorldManagerAPI worldManager = BasicCommandsMod.Instance().API.WorldManager;

        int apothemX = worldManager.MapSizeX / 2;
        int apothemZ = worldManager.MapSizeZ / 2;

        Random rand = Random.Shared;
        int randX = rand.Next(-apothemX, apothemX);
        int randZ = rand.Next(-apothemZ, apothemZ);

        pending.TryAdd(sender.UID, new Vec2i(randX, randZ));

        return TextCommandResult.Success(Lang.Get("tpr-wait"));
    }

    public static void ProcessWaitingChunk(Vec2i chunk) {
        ICoreServerAPI api = BasicCommandsMod.Instance().API;
        api.Logger.Event($"Chunk loaded: {chunk}");
        IWorldManagerAPI worldManager = api.WorldManager;
        IBlockAccessor blockAccessor = api.World.BlockAccessor;
        for (int i = pending.Count - 1; i >= 0; i--) {
            KeyValuePair<string, Vec2i> entry = pending.ElementAt(i);
            Vec2i block = entry.Value;
            if (chunk.X != block.X / worldManager.ChunkSize || chunk.Y != block.Y / worldManager.ChunkSize) {
                api.Logger.Event($"Chunk mismatch: {block}");
                continue;
            }
            pending.Remove(entry.Key);
            BasicPlayer player = BasicPlayer.Get(entry.Key);
            if (player == null) {
                api.Logger.Event("Null player");
                continue;
            }
            Vec3d pos = new(block.X, 1, block.Y);
            api.Logger.Event($"Pos1: {pos}");
            pos.Y = blockAccessor.GetTerrainMapheightAt(pos.AsBlockPos) + 1;
            api.Logger.Event($"Pos2: {pos}");
            player.TeleportTo(pos);
            player.SendMessage(Lang.Get("tpr-success", (int)pos.X, (int)pos.Y, (int)pos.Z));
        }
    }
}
