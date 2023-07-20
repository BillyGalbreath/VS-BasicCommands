using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;

namespace Essentials.Player {
    internal class EssPlayer {
        private static readonly ConcurrentDictionary<string, EssPlayer> players = new ConcurrentDictionary<string, EssPlayer>();
        private static readonly string DATA_KEY = "EssentialsData";

        internal static EssPlayer Get(IPlayer player) {
            return player is IServerPlayer sPlayer ? Get(sPlayer) : null;
        }

        internal static EssPlayer Get(IServerPlayer player) {
            return players.GetOrAdd(player.PlayerUID, k => new EssPlayer(player));
        }

        internal static void SaveAllPlayers() {
            foreach (KeyValuePair<string, EssPlayer> player in players) {
                if (player.Value.dirty) {
                    player.Value.dirty = false;
                    byte[] raw = SerializerUtil.Serialize(player.Value.data);
                    player.Value.player.WorldData.SetModdata(DATA_KEY, raw);
                }
            }
        }

        private readonly IServerPlayer player;
        private readonly Data data;

        private bool dirty;

        internal EssPlayer(IServerPlayer player) {
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

        [Serializable]
        internal class Data {
            internal Dictionary<string, BlockPos> homes = new Dictionary<string, BlockPos>();
            internal BlockPos lastPos;
        }
    }
}
