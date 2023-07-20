using BasicCommands.Command;
using BasicCommands.Player;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace BasicCommands {
    public class BasicCommandsMod : ModSystem {
        private static BasicCommandsMod instance;

        internal static BasicCommandsMod Instance() {
            return instance;
        }

        internal ICoreServerAPI API { get; private set; }

        public BasicCommandsMod() {
            instance = this;
        }

        public override void StartServerSide(ICoreServerAPI api) {
            API = api;

            api.Event.GameWorldSave += OnGameWorldSave;

            _ = new PlayerListener();

            _ = new CmdBack();
            _ = new CmdDelHome();
            _ = new CmdHome();
            _ = new CmdHomes();
            _ = new CmdRandomTeleport();
            _ = new CmdSetHome();
            _ = new CmdSpawn();
            _ = new CmdTeleportAccept();
            _ = new CmdTeleportDeny();
            _ = new CmdTeleportRequest();
        }

        private void OnGameWorldSave() {
            BasicPlayer.SaveAllPlayers();
        }
    }
}
