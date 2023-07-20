using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace Essentials.Player {
    internal class PlayerListener {
        internal PlayerListener() {
            EssentialsMod mod = EssentialsMod.Instance();

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
            EssPlayer.Get(player);
        }

        private void OnPlayerJoin(IServerPlayer player) {
            EssPlayer.Get(player);
        }

        private void OnPlayerDisconnect(IServerPlayer player) {
        }

        private void OnPlayerDeath(IServerPlayer player, DamageSource damageSource) {
            EssPlayer.Get(player).UpdateLastPosition();
        }
    }
}
