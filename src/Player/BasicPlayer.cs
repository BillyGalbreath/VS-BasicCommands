using ProtoBuf;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;

namespace BasicCommands.Player {
    public class BasicPlayer {
        private static readonly ConcurrentDictionary<string, BasicPlayer> players = new();
        private static readonly string DATA_KEY = "BasicCommands";

        public static BasicPlayer Get(IPlayer player) {
            return player is IServerPlayer sPlayer ? Get(sPlayer) : null;
        }

        public static BasicPlayer Get(IServerPlayer player) {
            return players.GetOrAdd(player.PlayerUID, k => new BasicPlayer(player));
        }

        public static void Remove(IServerPlayer player) {
            Get(player).Save().Remove();
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

        public Vec3i CurPos {
            get {
                return player.Entity.Pos.XYZInt;
            }
            private set { }
        }

        public Vec3i LastPos {
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

        public Vec3i GetHome(string name) {
            return data.homes.Get(name);
        }

        public void AddHome(string name, Vec3i pos) {
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

        public void TeleportTo(Vec3i pos) {
            if (pos == null) {
                throw new ArgumentNullException(nameof(pos), "Cannot teleport to null!");
            }
            UpdateLastPosition();
            player.Entity.TeleportTo(new Vec3d(pos.X + 0.5F, pos.Y + 0.5F, pos.Z + 0.5F));
        }

        public void UpdateLastPosition() {
            LastPos = CurPos;
        }

        private BasicPlayer Load() {
            byte[] raw = player.WorldData.GetModdata(DATA_KEY);
            data = raw == null ? new Data() : SerializerUtil.Deserialize<Data>(raw);
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
            internal Dictionary<string, Vec3i> homes = new();
            internal Vec3i lastPos;
            internal Dictionary<string, Vec3i> teleportRequests = new();
            internal bool allowTeleportRequests = true;
        }
    }
}
