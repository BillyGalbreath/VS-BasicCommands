using BasicCommands.Command;
using BasicCommands.Configuration;
using BasicCommands.Player;
using System.Collections.Generic;
using System.Text.Json;
using Vintagestory.API.Client;
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
        return true;// side == EnumAppSide.Server;
    }

    public override void StartClientSide(ICoreClientAPI api) {
        api.ChatCommands.Create("exportcolors")
            .HandleWith(args => {
                Dictionary<int, int> colors = new();
                foreach (Block block in BasicCommandsMod.Instance().API.World.Blocks) {
                    int color = api.BlockTextureAtlas.GetAverageColor(block.TextureSubIdForBlockColor) >> 2;
                    colors.Add(block.BlockId, color);
                }
                return TextCommandResult.Success(JsonSerializer.Serialize(colors));
            });
    }

    public override void StartServerSide(ICoreServerAPI api) {
        API = api;

        Config = api.LoadModConfig<Config>("BasicCommands.json");
        if (Config == null) {
            Config = new Config();
            api.StoreModConfig(Config, "BasicCommands.json");
        }

        api.Event.GameWorldSave += OnGameWorldSave;

        _ = new EventListener();

        _ = new CmdBack();
        _ = new CmdDelHome();
        _ = new CmdHome();
        _ = new CmdHomes();
        _ = new CmdJump();
        _ = new CmdSetHome();
        _ = new CmdSetSpawn();
        _ = new CmdSpawn();
        _ = new CmdTop();
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
