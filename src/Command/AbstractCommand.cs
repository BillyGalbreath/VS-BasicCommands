using BasicCommands.Configuration;
using BasicCommands.Player;
using Vintagestory.API.Common;
using Vintagestory.API.Util;

namespace BasicCommands.Command;

public abstract class AbstractCommand {
    public AbstractCommand(params ICommandArgumentParser[] parsers) {
        BasicCommandsMod mod = BasicCommandsMod.Instance();
        Config.Command cmd = mod.Config.commands.Get(GetType().Name.ToLower());
        if (!cmd.enabled) {
            return;
        }
        IChatCommand chatCmd = mod.API.ChatCommands
            .Create(cmd.name)
            .WithDescription(Lang.Get($"{cmd.name}-description"))
            .RequiresPlayer()
            .RequiresPrivilege($"basiccommands.{cmd.name}")
            .HandleWith(args => Execute(BasicPlayer.Get(args.Caller.Player), args));
        if (cmd.aliases != null && cmd.aliases.Length > 0) {
            chatCmd.WithAlias(cmd.aliases);
        }
        if (parsers != null && parsers.Length > 0) {
            chatCmd.WithArgs(parsers);
        }
    }

    public abstract TextCommandResult Execute(BasicPlayer sender, TextCommandCallingArgs args);
}
