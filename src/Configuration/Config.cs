using System;
using System.Collections.Generic;

namespace BasicCommands.Configuration {
    [Serializable]
    public class Config {
        public Dictionary<string, Command> commands = new Dictionary<string, Command>() {
            { "cmdback", new() { name = "back", aliases = new[] { "return", "previous", "prev" } } },
            { "cmddelhome", new() { name = "delhome", aliases = new[] { "rmhome", "remhome", "removehome", "deletehome" } } },
            { "cmdhome", new() { name = "home" } },
            { "cmdhomes", new() { name = "homes", aliases = new[] { "listhomes", "homeslist" } } },
            { "cmdsethome", new() { name = "sethome", aliases = new[] { "createhome", "makehome" } } },
            { "cmdspawn", new() { name = "spawn" } },
            { "cmdtpa", new() { name = "tpa", aliases = new[] { "teleportask", "tpask", "teleportrequest", "tprequest" } } },
            { "cmdtpaccept", new() { name = "tpaccept", aliases = new[] { "teleportaccept", "tpyes", "teleportyes" } } },
            { "cmdtpahere", new() { name = "tpahere", aliases = new[] { "teleportaskhere", "tpaskhere", "teleportrequesthere", "tprequesthere" } } },
            { "cmdtpcancel", new() { name = "tpcancel", aliases = new[] { "teleportcancel", "tpacancel", "tpaskcancel", "teleportrequestcancel" } } },
            { "cmdtpdeny", new() { name = "tpdeny", aliases = new[] { "teleportdeny", "tpno", "teleportno" } } },
            { "cmdtpr", new() { name = "tpr", aliases = new[] { "teleportrandom", "tprandom", "rtp", "randomteleport", "randomtp" } } },
            { "cmdtptoggle", new() { name = "tptoggle", aliases = new[] { "teleporttoggle", "tpt", "toggleteleport", "toggletp" } } },
        };

        [Serializable]
        public class Command {
            public bool enabled = true;
            public string name;
            public string[] aliases;
        }
    }
}
