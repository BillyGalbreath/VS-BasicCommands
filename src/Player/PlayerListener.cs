using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace BasicCommands.Player {
    public class PlayerListener {
        public PlayerListener() {
            IServerEventAPI Event = BasicCommandsMod.Instance().API.Event;
            Event.PlayerCreate += OnPlayerCreate;
            Event.PlayerJoin += OnPlayerJoin;
            Event.PlayerDisconnect += OnPlayerDisconnect;
            Event.PlayerDeath += OnPlayerDeath;

            Event.ServerRunPhase(EnumServerRunPhase.Shutdown, () => {
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
    }
}
