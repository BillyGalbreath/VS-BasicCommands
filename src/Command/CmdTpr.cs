using System;
using System.Collections.Generic;
using BasicCommands.Configuration;
using BasicCommands.Player;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace BasicCommands.Command;

public class CmdTpr : AbstractCommand {
    private static readonly HashSet<string> PENDING = new();

    public CmdTpr(ICoreServerAPI api, Config config) : base(api, config) { }

    protected override TextCommandResult Execute(BasicPlayer sender, TextCommandCallingArgs args) {
        if (PENDING.Contains(sender.Uid)) {
            return TextCommandResult.Error("tpr-already-waiting");
        }

        Random rand = Random.Shared;
        int randX = rand.Next(api.WorldManager.MapSizeX);
        int randZ = rand.Next(api.WorldManager.MapSizeZ);
        int chunkSize = api.WorldManager.ChunkSize;

        PENDING.Add(sender.Uid);

        api.WorldManager.LoadChunkColumnPriority(randX / chunkSize, randZ / chunkSize, new ChunkLoadOptions {
            OnLoaded = () => {
                PENDING.Remove(sender.Uid);
                if (!sender.IsOnline) {
                    return;
                }

                int topY = (api.WorldManager.GetSurfacePosY(randX, randZ) ?? 0) + 1;
                sender.TeleportTo(new Vec3d(randX, topY, randZ));
                sender.SendMessage(Lang.Success("tpr-success", randX, topY, randZ));
            }
        });

        return TextCommandResult.Success("tpr-wait");
    }
}
