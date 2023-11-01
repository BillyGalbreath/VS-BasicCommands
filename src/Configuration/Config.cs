using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace BasicCommands.Configuration;

[Serializable]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public class Config {
    public Dictionary<string, Command> Commands = new() {
        { "cmdback", new Command { Name = "back", Aliases = new[] { "return", "previous", "prev" } } },
        { "cmddelhome", new Command { Name = "delhome", Aliases = new[] { "rmhome", "remhome", "removehome", "deletehome" } } },
        { "cmddelkit", new Command { Name = "delkit", Aliases = new[] { "kitdel", "rmkit", "kitrm" } } },
        { "cmdhome", new Command { Name = "home" } },
        { "cmdhomes", new Command { Name = "homes", Aliases = new[] { "listhomes", "homeslist" } } },
        { "cmdjump", new Command { Name = "jump" } },
        { "cmdkit", new Command { Name = "kit", Aliases = new[] { "kits" } } },
        { "cmdresetkit", new Command { Name = "resetkit", Aliases = new[] { "kitreset" } } },
        { "cmdrtp", new Command { Name = "rtp", Aliases = new[] { "randomteleport", "randomtp", "teleportrandom", "tprandom", "tpr" } } },
        { "cmdsethome", new Command { Name = "sethome", Aliases = new[] { "createhome", "makehome" } } },
        { "cmdsetkit", new Command { Name = "setkit", Aliases = new[] { "kitset", "createkit", "makekit" } } },
        { "cmdsetspawn", new Command { Name = "setspawn" } },
        { "cmdspawn", new Command { Name = "spawn" } },
        { "cmdtop", new Command { Name = "top" } },
        { "cmdtpa", new Command { Name = "tpa", Aliases = new[] { "teleportask", "tpask", "teleportrequest", "tprequest" } } },
        { "cmdtpaccept", new Command { Name = "tpaccept", Aliases = new[] { "teleportaccept", "tpyes", "teleportyes" } } },
        { "cmdtpahere", new Command { Name = "tpahere", Aliases = new[] { "teleportaskhere", "tpaskhere", "teleportrequesthere", "tprequesthere" } } },
        { "cmdtpcancel", new Command { Name = "tpcancel", Aliases = new[] { "teleportcancel", "tpacancel", "tpaskcancel", "teleportrequestcancel" } } },
        { "cmdtpdeny", new Command { Name = "tpdeny", Aliases = new[] { "teleportdeny", "tpno", "teleportno" } } },
        { "cmdtptoggle", new Command { Name = "tptoggle", Aliases = new[] { "teleporttoggle", "tpt", "toggleteleport", "toggletp" } } }
    };

    [Serializable]
    public class Command {
        public bool Enabled = true;
        public required string Name;
        public string[]? Aliases;
        public int Cooldown = 0;
    }
}
