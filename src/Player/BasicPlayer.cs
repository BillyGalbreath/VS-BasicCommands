using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using BasicCommands.TeleportRequest;
using ProtoBuf;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;

namespace BasicCommands.Player;

public class BasicPlayer {
    private static readonly ConcurrentDictionary<string, BasicPlayer> PLAYERS = new();

    public static IEnumerable<BasicPlayer> GetAll() {
        return PLAYERS.Values;
    }

    public static BasicPlayer? Get(IPlayer player) {
        return player is IServerPlayer sPlayer ? Get(sPlayer) : null;
    }

    public static BasicPlayer Get(IServerPlayer player) {
        return PLAYERS.GetOrAdd(player.PlayerUID, _ => new BasicPlayer(player));
    }

    public static BasicPlayer Remove(IServerPlayer serverPlayer) {
        BasicPlayer player = Get(serverPlayer);

        TpRequest.GetPendingForSender(player)?.Message("cancelled", true).Remove();
        TpRequest.GetPendingForTarget(player)?.Message("expired", true).Remove();

        player.Save().Remove();

        return player;
    }

    public static void SaveAllPlayers() {
        foreach (BasicPlayer player in PLAYERS.Values) {
            player.Save();
        }
    }

    private readonly Data data;
    private bool dirty;

    private BasicPlayer(IServerPlayer player) {
        Player = player;

        byte[] raw = Player.WorldData.GetModdata(BasicCommandsMod.Id);
        if (raw != null) {
            try {
                data = SerializerUtil.Deserialize<Data>(raw);
            }
            catch (Exception) {
                // ignored
            }
        }

        data ??= new Data();
    }

    public IServerPlayer Player { get; }

    public bool IsOnline { get; set; }

    public string Name => Player.PlayerName;

    public string Uid => Player.PlayerUID;
    
    public Dictionary<string, long> Cooldowns { get; } = new();

    public EntityPos EntityPos => Player.Entity.ServerPos;

    public Vec3d? LastPos {
        get => data.lastPos;
        private set {
            bool changed = !Equals(value, data.lastPos);
            data.lastPos = value;
            dirty |= changed;
        }
    }

    public BlockSelection? TargetBlock {
        get {
            BlockSelection blockSelection = new();
            EntitySelection entitySelection = new();
            Player.Entity.World.RayTraceForSelection(
                Player.Entity.Pos.XYZ.Add(Player.Entity.LocalEyePos),
                Player.Entity.SidedPos.Pitch,
                Player.Entity.SidedPos.Yaw,
                Player.WorldData.LastApprovedViewDistance,
                ref blockSelection,
                ref entitySelection
            );
            return blockSelection;
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
        return Player.HasPrivilege($"{BasicCommandsMod.Id}.{command}.exempt.cooldown");
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

    public void SendMessage(string? message) {
        if (message is { Length: > 0 }) {
            Player.SendMessage(GlobalConstants.GeneralChatGroup, message, EnumChatType.CommandSuccess);
        }
    }

    public void TeleportTo(Vec3d? pos) {
        if (pos == null) {
            throw new ArgumentNullException(nameof(pos), "Cannot teleport to null!");
        }

        Player.Entity.TeleportTo(new Vec3d((int)pos.X + 0.5F, Math.Round(pos.Y, MidpointRounding.ToPositiveInfinity), (int)pos.Z + 0.5F));
    }

    public void UpdateLastPosition() {
        LastPos = EntityPos.XYZ;
    }

    private BasicPlayer Save() {
        if (!dirty) {
            return this;
        }

        dirty = false;
        Player.WorldData.SetModdata(BasicCommandsMod.Id, SerializerUtil.Serialize(data));

        return this;
    }

    private void Remove() {
        PLAYERS.Remove(Player.PlayerUID);
    }

    public override bool Equals(object? obj) {
        if (this == obj) {
            return true;
        }

        return obj is BasicPlayer other && Player.PlayerUID.Equals(other.Player.PlayerUID);
    }

    public override int GetHashCode() {
        return Player.PlayerUID.GetHashCode();
    }

    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
    [SuppressMessage("ReSharper", "FieldCanBeMadeReadOnly.Local")]
    private class Data {
        internal Vec3d? lastPos;
        internal Dictionary<string, Vec3d> homes = new();
        internal bool allowTeleportRequests = true;
    }
}
