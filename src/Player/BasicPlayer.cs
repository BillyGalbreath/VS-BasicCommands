using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using BasicCommands.TeleportRequest;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;

namespace BasicCommands.Player;

public class BasicPlayer : OfflinePlayer {
    private static readonly ConcurrentDictionary<string, BasicPlayer> PLAYERS = new();

    public static IEnumerable<BasicPlayer> GetAll() {
        return PLAYERS.Values;
    }

    public static OfflinePlayer Get(IPlayer player) {
        return player is IServerPlayer sPlayer ? Get(sPlayer) : new OfflinePlayer(player);
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

    private BasicPlayer(IServerPlayer player) : base(player) {
        Player = player;
    }

    public IServerPlayer Player { get; }

    public Dictionary<string, long> Cooldowns { get; } = new();

    public EntityPos EntityPos => Player.Entity.ServerPos;

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

    public void SendMessage(string? message) {
        if (IsOnline && message is { Length: > 0 }) {
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

    // ReSharper disable once MemberCanBePrivate.Global
    public new BasicPlayer Save() {
        base.Save();
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
        return base.GetHashCode();
    }
}
