using BasicCommands.Configuration;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Server;

namespace BasicCommands.Command {
    public abstract class AbstractCommand {
        public AbstractCommand(Config.Command cmd, params ICommandArgumentParser[] parsers) {
            IChatCommand chatCmd = BasicCommandsMod.Instance().API.ChatCommands
                .Create(cmd.name)
                .WithDescription(Lang.Get($"{cmd.name}-description"))
                .RequiresPrivilege($"basiccommands.{cmd.name}")
                .HandleWith(Execute);
            if (cmd.aliases != null && cmd.aliases.Length > 0) {
                chatCmd.WithAlias(cmd.aliases);
            }
            if (parsers != null && parsers.Length > 0) {
                chatCmd.WithArgs(parsers);
            }
        }

        public abstract TextCommandResult Execute(TextCommandCallingArgs args);
    }
}
