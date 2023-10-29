using System;
using System.Collections.Generic;
using BasicCommands.Configuration;
using BasicCommands.Extensions;
using BasicCommands.Player;
using Vintagestory.API.Common;
using Vintagestory.API.Server;
using Vintagestory.Server;

namespace BasicCommands.Command;

public abstract class AbstractCommand {
    protected readonly ICoreServerAPI api;

    protected AbstractCommand(ICoreServerAPI api, Config config, params ICommandArgumentParser[]? parsers) {
        this.api = api;

        if (!config.Commands.TryGetValue(GetType().Name.ToLower(), out Config.Command? cmdConfig)) {
            return;
        }

        if (!cmdConfig.Enabled) {
            return;
        }

        string basePerm = $"{BasicCommandsMod.Id}.{cmdConfig.Name}";
        PlayerDataManager pdm = ((ServerMain)api.World).PlayerDataManager;
        pdm.RegisterPrivilege(basePerm, cmdConfig.Name);
        pdm.RegisterPrivilege($"{basePerm}.exempt.cooldown", cmdConfig.Name);

        IChatCommand chatCmd = api.ChatCommands
            .Create(cmdConfig.Name)
            .WithDescription(Lang.Get($"{cmdConfig.Name}-description"))
            .RequiresPlayer()
            .RequiresPrivilege($"{BasicCommandsMod.Id}.{cmdConfig.Name}")
            .HandleWith(args => HandleCommand(cmdConfig, args));

        if (cmdConfig.Aliases is { Length: > 0 }) {
            chatCmd.WithAlias(cmdConfig.Aliases);
        }

        if (parsers is { Length: > 0 }) {
            chatCmd.WithArgs(parsers);
        }
    }

    private TextCommandResult HandleCommand(Config.Command cmdConfig, TextCommandCallingArgs args) {
        BasicPlayer? sender = BasicPlayer.Get(args.Caller.Player);
        if (sender == null) {
            return TextCommandResult.Success(Lang.Error("player-only-command"));
        }

        if (cmdConfig.Cooldown <= 0 || sender.Exempt(cmdConfig.Name)) {
            return Execute(sender, args);
        }

        long cooldown = sender.Cooldowns.GetValueOrDefault(cmdConfig.Name);
        long now = DateTimeOffset.Now.ToUnixTimeSeconds();

        if (cooldown > now) {
            return TextCommandResult.Success(Lang.Error("command-on-cooldown", TimeSpan.FromSeconds(cooldown - now).Remaining()));
        }

        TextCommandResult result = Execute(sender, args);
        if (result.Status == EnumCommandStatus.Error) {
            result.Status = EnumCommandStatus.Success;
            result.StatusMessage = Lang.Error(result.StatusMessage);
        }
        else {
            sender.Cooldowns[cmdConfig.Name] = now + cmdConfig.Cooldown;
            if (result.StatusMessage.Length > 0) {
                result.StatusMessage = Lang.Success(result.StatusMessage);
            }
        }

        return result;
    }

    protected abstract TextCommandResult Execute(BasicPlayer sender, TextCommandCallingArgs args);
}
