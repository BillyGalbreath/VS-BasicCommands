using System.Diagnostics.CodeAnalysis;
using BasicCommands.Command;
using BasicCommands.Configuration;
using BasicCommands.Player;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace BasicCommands;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
public class BasicCommandsMod : ModSystem {
    //private ICoreClientAPI? capi;
    private ICoreServerAPI? sapi;

    public override void StartClientSide(ICoreClientAPI api) {
        //capi = api;
    }

    public override void StartServerSide(ICoreServerAPI api) {
        sapi = api;

        sapi.Event.GameWorldSave += OnGameWorldSave;
        sapi.Event.PlayerCreate += OnPlayerCreate;
        sapi.Event.PlayerJoin += OnPlayerJoin;
        sapi.Event.PlayerDisconnect += OnPlayerDisconnect;
        sapi.Event.PlayerDeath += OnPlayerDeath;

        Config config = api.LoadModConfig<Config>($"{Mod.Info.ModID}.json");
        if (config == null) {
            api.StoreModConfig(config = new Config(), $"{Mod.Info.ModID}.json");
        }

        _ = new CmdBack(api, config);
        _ = new CmdDelHome(api, config);
        _ = new CmdHome(api, config);
        _ = new CmdHomes(api, config);
        _ = new CmdJump(api, config);
        _ = new CmdSetHome(api, config);
        _ = new CmdSetSpawn(api, config);
        _ = new CmdSpawn(api, config);
        _ = new CmdTop(api, config);
        _ = new CmdTpa(api, config);
        _ = new CmdTpAccept(api, config);
        _ = new CmdTpaHere(api, config);
        _ = new CmdTpCancel(api, config);
        _ = new CmdTpDeny(api, config);
        _ = new CmdTpr(api, config);
        _ = new CmdTpToggle(api, config);
    }

    private static void OnGameWorldSave() {
        BasicPlayer.SaveAllPlayers();
    }

    private static void OnPlayerCreate(IServerPlayer player) {
        BasicPlayer.Get(player).IsOnline = true;
    }

    private static void OnPlayerJoin(IServerPlayer player) {
        BasicPlayer.Get(player).IsOnline = true;
    }

    private static void OnPlayerDisconnect(IServerPlayer player) {
        BasicPlayer.Remove(player).IsOnline = false;
    }

    private static void OnPlayerDeath(IServerPlayer player, DamageSource damageSource) {
        BasicPlayer.Get(player).UpdateLastPosition();
    }

    public override void Dispose() {
        if (sapi == null) {
            return;
        }

        sapi.Event.GameWorldSave -= OnGameWorldSave;
        sapi.Event.PlayerCreate -= OnPlayerCreate;
        sapi.Event.PlayerJoin -= OnPlayerJoin;
        sapi.Event.PlayerDisconnect -= OnPlayerDisconnect;
        sapi.Event.PlayerDeath -= OnPlayerDeath;
    }
}
