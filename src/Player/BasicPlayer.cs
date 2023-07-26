using BasicCommands.TeleportRequest;
using ProtoBuf;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;

namespace BasicCommands.Player;

public class BasicPlayer {
    private static readonly ConcurrentDictionary<string, BasicPlayer> players = new();
    private static readonly string DATA_KEY = "BasicCommands";

    public static IEnumerable<BasicPlayer> GetAll() {
        return players.Values;
    }

    public static BasicPlayer Get(string uid) {
        return Get(BasicCommandsMod.Instance().API.World.PlayerByUid(uid));
    }

    public static BasicPlayer Get(IPlayer player) {
        return player is IServerPlayer sPlayer ? Get(sPlayer) : null;
    }

    public static BasicPlayer Get(IServerPlayer player) {
        return players.GetOrAdd(player.PlayerUID, k => new BasicPlayer(player));
    }

    public static void Remove(IServerPlayer serverPlayer) {
        BasicPlayer player = Get(serverPlayer);
        TpRequest pending = TpRequest.GetPendingForSender(player);
        if (pending != null) {
            pending.Remove();
            pending.Message("cancelled");
        }
        pending = TpRequest.GetPendingForTarget(player);
        if (pending != null) {
            pending.Remove();
            pending.Message("expired");
        }
        player.Save().Remove();
    }

    public static void SaveAllPlayers() {
        foreach (BasicPlayer player in players.Values) {
            player.Save();
        }
    }

    private readonly IServerPlayer player;

    private Data data;
    private bool dirty;

    private BasicPlayer(IServerPlayer player) {
        this.player = player;
        Load();
    }

    public string Name {
        get {
            return player.PlayerName;
        }
        private set { }
    }

    public string UID {
        get {
            return player.PlayerUID;
        }
        private set { }
    }

    public Vec3d CurPos {
        get {
            return player.Entity.Pos.XYZ;
        }
        private set { }
    }

    public Vec3d LastPos {
        get {
            return data.lastPos;
        }
        set {
            bool changed = !value.Equals(data.lastPos);
            data.lastPos = value;
            dirty |= changed;
        }
    }

    public bool AllowTeleportRequests {
        get {
            return data.allowTeleportRequests;
        }
        set {
            bool changed = data.allowTeleportRequests != value;
            data.allowTeleportRequests = value;
            dirty |= changed;
        }
    }

    public IEnumerable<string> ListHomes() {
        return data.homes.Keys;
    }

    public Vec3d GetHome(string name) {
        return data.homes.Get(name);
    }

    public void AddHome(string name, Vec3d pos) {
        bool changed = !pos.Equals(data.homes.Get(name));
        data.homes.Add(name, pos);
        dirty |= changed;
    }

    public bool RemoveHome(string name) {
        bool changed = data.homes.Remove(name);
        dirty |= changed;
        return changed;
    }

    public void SendMessage(string message) {
        if (message != null && message.Length > 0) {
            player.SendMessage(GlobalConstants.GeneralChatGroup, message, EnumChatType.CommandSuccess);
        }
    }

    public void TeleportTo(Vec3d pos) {
        if (pos == null) {
            throw new ArgumentNullException(nameof(pos), "Cannot teleport to null!");
        }
        UpdateLastPosition();
        player.Entity.TeleportTo(new Vec3d((int)pos.X + 0.5F, Math.Round(pos.Y, MidpointRounding.ToPositiveInfinity), (int)pos.Z + 0.5F));
    }

    public void UpdateLastPosition() {
        LastPos = CurPos;
    }

    private BasicPlayer Load() {
        byte[] raw = player.WorldData.GetModdata(DATA_KEY);
        if (raw != null) {
            try {
                data = SerializerUtil.Deserialize<Data>(raw);
            } catch (Exception) {
                DataOldFormat old = raw == null ? new DataOldFormat() : SerializerUtil.Deserialize<DataOldFormat>(raw);
                data = new Data {
                    lastPos = new Vec3d(old.lastPos.X, old.lastPos.Y, old.lastPos.Z)
                };
                foreach (var (name, pos) in old.homes) {
                    data.homes.Add(name, new Vec3d(pos.X, pos.Y, pos.Z));
                }
            }
        } else {
            data = new Data();
        }
        return this;
    }

    public BasicPlayer Save() {
        if (dirty) {
            dirty = false;
            byte[] raw = SerializerUtil.Serialize(data);
            player.WorldData.SetModdata(DATA_KEY, raw);
        }
        return this;
    }

    public BasicPlayer Remove() {
        players.Remove(player.PlayerUID);
        return this;
    }

    public override bool Equals(object obj) {
        if (this == obj) {
            return true;
        }
        if (obj == null) {
            return false;
        }
        if (obj is not BasicPlayer other) {
            return false;
        }
        return player.PlayerUID.Equals(other.player.PlayerUID);
    }

    public override int GetHashCode() {
        return player.PlayerUID.GetHashCode();
    }

    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
    private class Data {
        internal Vec3d lastPos;
        internal Dictionary<string, Vec3d> homes = new();
        internal bool allowTeleportRequests = true;
    }

    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
    private class DataOldFormat {
        internal Vec3i lastPos;
        internal Dictionary<string, Vec3i> homes = new();
        internal bool allowTeleportRequests = true;
    }
}
