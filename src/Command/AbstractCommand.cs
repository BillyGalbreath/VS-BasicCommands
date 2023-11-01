using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using BasicCommands.Configuration;
using BasicCommands.Extensions;
using BasicCommands.Player;
using Vintagestory.API.Common;
using Vintagestory.API.Server;
using Vintagestory.Server;

namespace BasicCommands.Command;

public abstract class AbstractCommand {
    protected readonly ICoreServerAPI api;
    private readonly string? name;

    protected string CmdName => name ?? "";

    protected AbstractCommand(ICoreServerAPI api, Config config, params ICommandArgumentParser[]? parsers) {
        this.api = api;

        if (!config.Commands.TryGetValue(GetType().Name.ToLower(), out Config.Command? cmdConfig)) {
            return;
        }

        if (!cmdConfig.Enabled) {
            return;
        }

        name = cmdConfig.Name;

        string basePerm = $"{BasicCommandsMod.Id}.{name}";
        PlayerDataManager pdm = ((ServerMain)api.World).PlayerDataManager;
        pdm.RegisterPrivilege(basePerm, name);
        pdm.RegisterPrivilege($"{basePerm}.exempt.cooldown", name);

        IChatCommand chatCmd = api.ChatCommands
            .Create(name)
            .WithDescription(Lang.Get($"{name}-description"))
            .RequiresPlayer()
            .RequiresPrivilege($"{BasicCommandsMod.Id}.{name}")
            .HandleWith(args => HandleCommand(cmdConfig, args));

        if (cmdConfig.Aliases is { Length: > 0 }) {
            chatCmd.WithAlias(cmdConfig.Aliases);
        }

        if (parsers is { Length: > 0 }) {
            chatCmd.WithArgs(parsers);
        }
    }

    private TextCommandResult HandleCommand(Config.Command cmdConfig, TextCommandCallingArgs args) {
        BasicPlayer? sender = args.Caller.Player is IServerPlayer player ? BasicPlayer.Get(player) : null;
        if (sender == null) {
            return TextCommandResult.Success(Lang.Error("player-only-command"));
        }

        CommandResult result;
        if (cmdConfig.Cooldown <= 0 || sender.Exempt(name)) {
            result = Execute(sender, args);
        }
        else {
            long cooldown = sender.Cooldowns.GetValueOrDefault(name);
            long now = DateTimeOffset.Now.ToUnixTimeSeconds();

            result = cooldown > now ? Error("command-on-cooldown", TimeSpan.FromSeconds(cooldown - now).Remaining()) : Execute(sender, args);

            if (result.Status == EnumCommandStatus.Success) {
                sender.Cooldowns[name] = now + cmdConfig.Cooldown;
            }
        }

        string message;
        if (result.Status == EnumCommandStatus.Error) {
            message = Lang.Error(result.Message, result.Args);
        }
        else if (result.Message.Length > 0) {
            message = Lang.Success(result.Message, result.Args);
        }
        else {
            message = "";
        }

        return TextCommandResult.Success(message);
    }

    protected abstract CommandResult Execute(BasicPlayer sender, TextCommandCallingArgs args);

    protected static CommandResult Success(string message = "", params object[]? args) => new() {
        Status = EnumCommandStatus.Success,
        Message = message,
        Args = args
    };

    protected static CommandResult Error(string message, params object[]? args) => new() {
        Status = EnumCommandStatus.Error,
        Message = message,
        Args = args
    };
}

[SuppressMessage("ReSharper", "InconsistentNaming")]
public class CommandResult {
    public required EnumCommandStatus Status;
    public required string Message;
    public object[]? Args;
}
