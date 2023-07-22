using BasicCommands.Command;
using BasicCommands.Configuration;
using BasicCommands.Player;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace BasicCommands;

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

    public override bool ShouldLoad(EnumAppSide side) {
        return side == EnumAppSide.Server;
    }

    public override void StartServerSide(ICoreServerAPI api) {
        API = api;

        Config = api.LoadModConfig<Config>("BasicCommands.json");
        if (Config == null) {
            Config = new Config();
            api.StoreModConfig(Config, "BasicCommands.json");
        }

        api.Event.GameWorldSave += OnGameWorldSave;

        _ = new PlayerListener();

        _ = new CmdBack();
        _ = new CmdDelHome();
        _ = new CmdHome();
        _ = new CmdHomes();
        _ = new CmdSetHome();
        _ = new CmdSpawn();
        _ = new CmdTpa();
        _ = new CmdTpAccept();
        _ = new CmdTpaHere();
        _ = new CmdTpCancel();
        _ = new CmdTpDeny();
        _ = new CmdTpr();
        _ = new CmdTpToggle();
    }

    private void OnGameWorldSave() {
        BasicPlayer.SaveAllPlayers();
    }
}
