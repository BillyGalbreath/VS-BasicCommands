﻿using System;
using System.Collections.Generic;

namespace BasicCommands.Configuration {
    [Serializable]
    public class Config {
        public Command cmdBack = new() { name = "back", aliases = new[] { "return", "previous", "prev" } };
        public Command cmdDelHome = new() { name = "delhome", aliases = new[] { "rmhome", "remhome", "removehome", "deletehome" } };
        public Command cmdHome = new() { name = "home", aliases = null };
        public Command cmdHomes = new() { name = "homes", aliases = new[] { "listhomes", "homeslist" } };
        public Command cmdSetHome = new() { name = "sethome", aliases = new[] { "createhome", "makehome" } };
        public Command cmdSpawn = new() { name = "spawn", aliases = null };
        public Command cmdTpa = new() { name = "tpa", aliases = new[] { "teleportask", "tpask", "teleportrequest", "tprequest" } };
        public Command cmdTpAccept = new() { name = "tpaccept", aliases = new[] { "teleportaccept", "tpyes", "teleportyes" } };
        public Command cmdTpDeny = new() { name = "tpdeny", aliases = new[] { "teleportdeny", "tpno", "teleportno" } };
        public Command cmdTpr = new() { name = "tpr", aliases = new[] { "teleportrandom", "tprandom", "rtp", "randomteleport", "randomtp" } };

        [Serializable]
        public class Command {
            public string name;
            public string[] aliases;
        }
    }
}