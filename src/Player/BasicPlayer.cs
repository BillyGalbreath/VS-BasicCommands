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
        private readonly Data data;

        private bool dirty;

        public BasicPlayer(IServerPlayer player) {
            this.player = player;

            byte[] raw = player.WorldData.GetModdata(DATA_KEY);
            data = raw == null ? new Data() : SerializerUtil.Deserialize<Data>(raw);
        }

        public BlockPos BlockPos {
            get {
                return player.Entity.Pos.AsBlockPos;
            }
            private set { }
        }

        public BlockPos LastPos {
            get {
                return data.lastPos;
            }
            set {
                data.lastPos = value;
                dirty = true;
            }
        }

        public IEnumerable<string> ListHomes() {
            return data.homes.Keys;
        }

        public BlockPos GetHome(string name) {
            return data.homes.Get(name);
        }

        public void AddHome(string name, BlockPos pos) {
            data.homes.Add(name, pos);
            dirty = true;
        }

        public bool RemoveHome(string name) {
            bool result = data.homes.Remove(name);
            if (result) {
                dirty = true;
            }
            return result;
        }

        public void SendMessage(string message) {
            player.SendMessage(GlobalConstants.GeneralChatGroup, message, EnumChatType.CommandSuccess);
        }

        public void TeleportTo(BlockPos pos) {
            UpdateLastPosition();
            player.Entity.TeleportTo(pos);
        }

        public void UpdateLastPosition() {
            LastPos = player.Entity.Pos.AsBlockPos;
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

        [Serializable]
        public class Data {
            internal Dictionary<string, BlockPos> homes = new();
            internal BlockPos lastPos;
        }
    }
}
