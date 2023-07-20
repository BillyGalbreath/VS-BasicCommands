using Essentials.Command;
using Essentials.Player;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace Essentials {
    public class EssentialsMod : ModSystem {
        private static EssentialsMod instance;

        internal static EssentialsMod Instance() {
            return instance;
        }

        internal ICoreServerAPI API { get; private set; }

        public EssentialsMod() {
            instance = this;
        }

        public override void StartServerSide(ICoreServerAPI api) {
            API = api;

            api.Event.GameWorldSave += OnGameWorldSave;

            new PlayerListener();

            new CmdBack();
            new CmdDelHome();
            new CmdHome();
            new CmdHomes();
            new CmdRandomTeleport();
            new CmdSetHome();
            new CmdSpawn();
            new CmdTeleportAccept();
            new CmdTeleportDeny();
            new CmdTeleportRequest();
        }

        private void OnGameWorldSave() {
            EssPlayer.SaveAllPlayers();
        }
    }
}
