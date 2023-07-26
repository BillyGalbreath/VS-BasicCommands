using BasicCommands.Command;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace BasicCommands.Player;

public class EventListener {
    public EventListener() {
        IServerEventAPI Event = BasicCommandsMod.Instance().API.Event;
        Event.ChunkColumnLoaded += OnChunkColumnLoaded;
        Event.PlayerCreate += OnPlayerCreate;
        Event.PlayerJoin += OnPlayerJoin;
        Event.PlayerDisconnect += OnPlayerDisconnect;
        Event.PlayerDeath += OnPlayerDeath;

        Event.ServerRunPhase(EnumServerRunPhase.Shutdown, () => {
            Event.ChunkColumnLoaded -= OnChunkColumnLoaded;
            Event.PlayerCreate -= OnPlayerCreate;
            Event.PlayerJoin -= OnPlayerJoin;
            Event.PlayerDisconnect -= OnPlayerDisconnect;
            Event.PlayerDeath -= OnPlayerDeath;
        });
    }

    private void OnPlayerCreate(IServerPlayer player) {
        BasicPlayer.Get(player);
    }

    private void OnPlayerJoin(IServerPlayer player) {
        BasicPlayer.Get(player);
    }

    private void OnPlayerDisconnect(IServerPlayer player) {
        BasicPlayer.Remove(player);
    }

    private void OnPlayerDeath(IServerPlayer player, DamageSource damageSource) {
        BasicPlayer.Get(player).UpdateLastPosition();
    }

    private void OnChunkColumnLoaded(Vec2i coords, IWorldChunk[] chunks) {
        CmdTpr.ProcessWaitingChunk(coords);
    }
}
