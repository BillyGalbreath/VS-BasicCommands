using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;

namespace BasicCommands.Player {
    internal class BasicPlayer {
        private static readonly ConcurrentDictionary<string, BasicPlayer> players = new();
        private static readonly string DATA_KEY = "BasicCommands";

        internal static BasicPlayer Get(IPlayer player) {
            return player is IServerPlayer sPlayer ? Get(sPlayer) : null;
        }

        internal static BasicPlayer Get(IServerPlayer player) {
            return players.GetOrAdd(player.PlayerUID, k => new BasicPlayer(player));
        }

        internal static void Remove(IServerPlayer player) {
            Get(player).Save().Remove();
        }

        internal static void SaveAllPlayers() {
            foreach (BasicPlayer player in players.Values) {
                player.Save();
            }
        }

        private readonly IServerPlayer player;
        private readonly Data data;

        private bool dirty;

        internal BasicPlayer(IServerPlayer player) {
            this.player = player;

            byte[] raw = player.WorldData.GetModdata(DATA_KEY);
            data = raw == null ? new Data() : SerializerUtil.Deserialize<Data>(raw);
        }

        internal BlockPos BlockPos {
            get {
                return player.Entity.Pos.AsBlockPos;
            }
            private set { }
        }

        internal BlockPos LastPos {
            get {
                return data.lastPos;
            }
            set {
                data.lastPos = value;
                dirty = true;
            }
        }

        internal IEnumerable<string> ListHomes() {
            return data.homes.Keys;
        }

        internal BlockPos GetHome(string name) {
            return data.homes.Get(name);
        }

        internal void AddHome(string name, BlockPos pos) {
            data.homes.Add(name, pos);
            dirty = true;
        }

        internal bool RemoveHome(string name) {
            bool result = data.homes.Remove(name);
            if (result) {
                dirty = true;
            }
            return result;
        }

        internal void SendMessage(string message) {
            player.SendMessage(GlobalConstants.GeneralChatGroup, message, EnumChatType.CommandSuccess);
        }

        internal void TeleportTo(BlockPos pos) {
            UpdateLastPosition();
            player.Entity.TeleportTo(pos);
        }

        internal void UpdateLastPosition() {
            LastPos = player.Entity.Pos.AsBlockPos;
        }

        internal BasicPlayer Save() {
            if (dirty) {
                dirty = false;
                byte[] raw = SerializerUtil.Serialize(data);
                player.WorldData.SetModdata(DATA_KEY, raw);
            }
            return this;
        }

        internal BasicPlayer Remove() {
            players.Remove(player.PlayerUID);
            return this;
        }

        [Serializable]
        internal class Data {
            internal Dictionary<string, BlockPos> homes = new();
            internal BlockPos lastPos;
        }
    }
}
