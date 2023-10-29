using BasicCommands.Configuration;
using BasicCommands.Player;
using Vintagestory.API.Common;
using Vintagestory.API.Server;
using Vintagestory.Server;

namespace BasicCommands.Command;

public abstract class AbstractCommand {
    protected readonly ICoreServerAPI api;

    protected AbstractCommand(ICoreServerAPI api, Config config, params ICommandArgumentParser[]? parsers) {
        this.api = api;

        if (!config.Commands.TryGetValue(GetType().Name.ToLower(), out Config.Command? cmd)) {
            return;
        }

        if (!cmd.Enabled) {
            return;
        }

        ((ServerMain)api.World).PlayerDataManager.RegisterPrivilege($"{BasicCommandsMod.Id}.{cmd.Name}", cmd.Name);

        IChatCommand chatCmd = api.ChatCommands
            .Create(cmd.Name)
            .WithDescription(Lang.Get($"{cmd.Name}-description"))
            .RequiresPlayer()
            .RequiresPrivilege($"{BasicCommandsMod.Id}.{cmd.Name}")
            .HandleWith(args => {
                BasicPlayer? sender = BasicPlayer.Get(args.Caller.Player);
                return sender == null ? TextCommandResult.Error(Lang.Get("player-only-command")) : Execute(sender, args);
            });

        if (cmd.Aliases is { Length: > 0 }) {
            chatCmd.WithAlias(cmd.Aliases);
        }

        if (parsers is { Length: > 0 }) {
            chatCmd.WithArgs(parsers);
        }
    }

    protected abstract TextCommandResult Execute(BasicPlayer sender, TextCommandCallingArgs args);
}
