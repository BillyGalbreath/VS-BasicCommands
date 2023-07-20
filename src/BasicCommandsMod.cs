﻿using BasicCommands.Command;
using BasicCommands.Configuration;
using BasicCommands.Player;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace BasicCommands {
    public class BasicCommandsMod : ModSystem {
        private static BasicCommandsMod instance;

        public static BasicCommandsMod Instance() {
            return instance;
        }

        public ICoreServerAPI API { get; private set; }

        public Config Config { get; private set; }

        public BasicCommandsMod() {
            instance = this;
        }

        public override void StartServerSide(ICoreServerAPI api) {
            API = api;

            Config = api.LoadModConfig<Config>("BasicCommands.json");

            api.Event.GameWorldSave += OnGameWorldSave;

            _ = new PlayerListener();

            _ = new CmdBack(Config.cmdBack);
            _ = new CmdDelHome(Config.cmdDelHome);
            _ = new CmdHome(Config.cmdHome);
            _ = new CmdHomes(Config.cmdHomes);
            _ = new CmdSetHome(Config.cmdSetHome);
            _ = new CmdSpawn(Config.cmdSpawn);
            _ = new CmdTpa(Config.cmdTpa);
            _ = new CmdTpAccept(Config.cmdTpAccept);
            _ = new CmdTpDeny(Config.cmdTpDeny);
            _ = new CmdTpr(Config.cmdTpr);
        }

        private void OnGameWorldSave() {
            BasicPlayer.SaveAllPlayers();
        }
    }
}
