using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using BasicCommands.Configuration;
using ProtoBuf;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace BasicCommands.Player;

public class OfflinePlayer {
    private readonly IPlayer player;

    protected readonly Data data;
    private bool dirty;

    // ReSharper disable once MemberCanBeProtected.Global
    public OfflinePlayer(IPlayer player) {
        this.player = player;

        byte[] raw = player.WorldData.GetModdata(BasicCommandsMod.Id);
        if (raw != null) {
            try {
                data = SerializerUtil.Deserialize<Data>(raw);
            }
            catch (Exception e) {
                player.Entity.World.Logger.Event(e.ToString());
            }
        }

        data ??= new Data();
    }

    public bool IsOnline { get; set; }

    public string Name => player.PlayerName;

    public string Uid => player.PlayerUID;

    public Vec3d? LastPos {
        get => data.lastPos;
        protected set {
            bool changed = !Equals(value, data.lastPos);
            data.lastPos = value;
            dirty |= changed;
        }
    }

    public bool AllowTeleportRequests {
        get => data.allowTeleportRequests;
        set {
            bool changed = data.allowTeleportRequests != value;
            data.allowTeleportRequests = value;
            dirty |= changed;
        }
    }

    public bool Exempt(string command) {
        return player.HasPrivilege($"{BasicCommandsMod.Id}.{command}.exempt.cooldown");
    }

    public IEnumerable<string> ListHomes() {
        return data.homes.Keys;
    }

    public Vec3d? GetHome(string name) {
        return data.homes.GetValueOrDefault(name);
    }

    public void AddHome(string name, Vec3d pos) {
        bool changed = !pos.Equals(GetHome(name));
        data.homes.Add(name, pos);
        dirty |= changed;
    }

    public bool RemoveHome(string name) {
        bool changed = data.homes.Remove(name);
        dirty |= changed;
        return changed;
    }

    public long GetKitLastUsed(Kit kit) {
        return data.kitLastUsed.GetValueOrDefault(kit.Name);
    }

    public OfflinePlayer SetKitLastUsed(Kit kit, long millis) {
        bool changed = !millis.Equals(GetKitLastUsed(kit));
        data.kitLastUsed[kit.Name] = millis;
        dirty |= changed;
        return this;
    }

    public OfflinePlayer ResetKitLastUsed(Kit kit) {
        dirty |= data.kitLastUsed.Remove(kit.Name);
        return this;
    }

    public OfflinePlayer ResetAllKitsLastUsed() {
        bool changed = data.kitLastUsed.Count > 0;
        data.kitLastUsed.Clear();
        dirty |= changed;
        return this;
    }

    public void Save() {
        if (!dirty) {
            return;
        }

        dirty = false;

        player.WorldData.SetModdata(BasicCommandsMod.Id, SerializerUtil.Serialize(data));
    }

    public override bool Equals(object? obj) {
        if (this == obj) {
            return true;
        }

        return obj is OfflinePlayer other && player.PlayerUID.Equals(other.player.PlayerUID);
    }

    public override int GetHashCode() {
        return player.PlayerUID.GetHashCode();
    }

    [SuppressMessage("ReSharper", "FieldCanBeMadeReadOnly.Global")]
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
    protected class Data {
        internal Vec3d? lastPos;
        internal Dictionary<string, Vec3d> homes = new();
        internal Dictionary<string, long> kitLastUsed = new();
        internal bool allowTeleportRequests = true;
    }
}
