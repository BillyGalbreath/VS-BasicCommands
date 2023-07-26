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
        if (pending.ContainsKey(sender.UID)) {
            return TextCommandResult.Success(Lang.Get("tpr-already-waiting"));
        }

        ICoreServerAPI api = BasicCommandsMod.Instance().API;
        IWorldManagerAPI worldManager = api.WorldManager;

        int apothemX = worldManager.MapSizeX / 2;
        int apothemZ = worldManager.MapSizeZ / 2;
        int chunkSize = worldManager.ChunkSize;

        Random rand = Random.Shared;
        int randX = rand.Next(-apothemX, apothemX);
        int randZ = rand.Next(-apothemZ, apothemZ);

        api.Logger.StoryEvent($"aX: {apothemX} aZ: {apothemZ} cS: {chunkSize} rX: {randX} rZ: {randZ}");

        Vec2i chunk = new(randX / chunkSize, randZ / chunkSize);

        worldManager.LoadChunkColumnPriority(chunk.X, chunk.Y, new ChunkLoadOptions() { OnLoaded = () => { ProcessWaitingChunk(chunk); } });

        pending.TryAdd(sender.UID, new(randX, randZ));

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
            Vec3d pos = new(block.X, (worldManager.GetSurfacePosY(block.X, block.Y) ?? 0) + 1, block.Y);
            api.Logger.Event($"Pos: {pos}");
            player.TeleportTo(pos);
            player.SendMessage(Lang.Get("tpr-success", (int)pos.X, (int)pos.Y, (int)pos.Z));
        }
    }
}
