using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace Essentials.Command {
    internal abstract class AbstractCommand {
        internal AbstractCommand(string name, string description) : this(name, description, Privilege.chat, null) { }
        internal AbstractCommand(string name, string description, string[] aliases) : this(name, description, Privilege.chat, aliases) { }
        internal AbstractCommand(string name, string description, params ICommandArgumentParser[] parsers) : this(name, description, Privilege.chat, null, parsers) { }
        internal AbstractCommand(string name, string description, string[] aliases, params ICommandArgumentParser[] parsers) : this(name, description, Privilege.chat, aliases, parsers) { }
        internal AbstractCommand(string name, string description, string privilege, string[] aliases, params ICommandArgumentParser[] parsers) {
            IChatCommand cmd = EssentialsMod.Instance().API.ChatCommands.Create(name);
            if (description != null && description.Length > 0) {
                cmd.WithDescription(description);
            }
            if (parsers != null && parsers.Length > 0) {
                cmd.WithArgs(parsers);
            }
            if (privilege != null && privilege.Length > 0) {
                cmd.RequiresPrivilege(privilege);
            }
            if (aliases != null && aliases.Length > 0) {
                cmd.WithAlias(aliases);
            }
            cmd.HandleWith(Execute);
        }

        internal abstract TextCommandResult Execute(TextCommandCallingArgs args);
    }
}
