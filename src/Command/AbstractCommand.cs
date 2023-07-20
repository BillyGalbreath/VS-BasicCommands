using BasicCommands.Configuration;
using BasicCommands.Player;
using Vintagestory.API.Common;

namespace BasicCommands.Command {
    public abstract class AbstractCommand {
        public AbstractCommand(Config.Command cmd, params ICommandArgumentParser[] parsers) {
            IChatCommand chatCmd = BasicCommandsMod.Instance().API.ChatCommands
                .Create(cmd.name)
                .WithDescription(Lang.Get($"{cmd.name}-description"))
                .RequiresPrivilege($"basiccommands.{cmd.name}")
                .HandleWith(args => {
                    BasicPlayer player = BasicPlayer.Get(args.Caller.Player);
                    if (player == null) {
                        return TextCommandResult.Error(Lang.Get("player-only-command"), "0x0001");
                    }
                    return Execute(player, args);
                });
            if (cmd.aliases != null && cmd.aliases.Length > 0) {
                chatCmd.WithAlias(cmd.aliases);
            }
            if (parsers != null && parsers.Length > 0) {
                chatCmd.WithArgs(parsers);
            }
        }

        public abstract TextCommandResult Execute(BasicPlayer player, TextCommandCallingArgs args);
    }
}
