using BasicCommands.Configuration;
using BasicCommands.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace BasicCommands.Command;

public class CmdTpr : AbstractCommand {
    private static readonly Dictionary<string, Vec2i> pending = new();

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

        pending.Add(sender.UID, new Vec2i(randX, randZ));

        return TextCommandResult.Success(Lang.Get("tpr-wait"));
    }

    public static void ProcessWaitingChunk(Vec2i chunk) {
        ICoreServerAPI api = BasicCommandsMod.Instance().API;
        IWorldManagerAPI worldManager = api.WorldManager;
        for (int i = pending.Count - 1; i >= 0; i--) {
            KeyValuePair<string, Vec2i> entry = pending.ElementAt(i);
            Vec2i block = entry.Value;
            if (chunk.X != block.X / worldManager.ChunkSize || chunk.Y != block.Y / worldManager.ChunkSize) {
                continue;
            }
            pending.Remove(entry.Key);
            BasicPlayer player = BasicPlayer.Get(entry.Key);
            if (player == null) {
                continue;
            }
            Vec3d pos = new(block.X, 1, block.Y);
            pos.Y = api.World.BlockAccessor.GetTerrainMapheightAt(pos.AsBlockPos) + 1;
            player.TeleportTo(pos);
            player.SendMessage(Lang.Get("tpr-success", (int)pos.X, (int)pos.Y, (int)pos.Z));
        }
    }
}
