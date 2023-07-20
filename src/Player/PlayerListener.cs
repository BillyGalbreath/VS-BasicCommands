using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace BasicCommands.Player {
    internal class PlayerListener {
        internal PlayerListener() {
            BasicCommandsMod mod = BasicCommandsMod.Instance();

            mod.API.Event.PlayerCreate += OnPlayerCreate;
            mod.API.Event.PlayerJoin += OnPlayerJoin;
            mod.API.Event.PlayerDisconnect += OnPlayerDisconnect;
            mod.API.Event.PlayerDeath += OnPlayerDeath;

            mod.API.Event.ServerRunPhase(EnumServerRunPhase.Shutdown, () => {
                mod.API.Event.PlayerCreate -= OnPlayerCreate;
                mod.API.Event.PlayerJoin -= OnPlayerJoin;
                mod.API.Event.PlayerDisconnect -= OnPlayerDisconnect;
                mod.API.Event.PlayerDeath -= OnPlayerDeath;
            });
        }

        private void OnPlayerCreate(IServerPlayer player) {
            BasicPlayer.Get(player);
        }

        private void OnPlayerJoin(IServerPlayer player) {
            BasicPlayer.Get(player);
        }

        private void OnPlayerDisconnect(IServerPlayer player) {
        }

        private void OnPlayerDeath(IServerPlayer player, DamageSource damageSource) {
            BasicPlayer.Get(player).UpdateLastPosition();
        }
    }
}
