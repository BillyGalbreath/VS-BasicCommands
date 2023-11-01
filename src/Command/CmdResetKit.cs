using System.Collections.Generic;
using BasicCommands.Configuration;
using BasicCommands.Extensions;
using BasicCommands.Player;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace BasicCommands.Command;

public class CmdResetKit : AbstractCommand {
    private readonly Dictionary<string, Confirmation> confirmations = new();

    public CmdResetKit(ICoreServerAPI api, Config config) : base(api, config, new WordArgParser("name", true), new WordArgParser("players", false)) { }

    protected override CommandResult Execute(BasicPlayer sender, TextCommandCallingArgs args) {
        string? kitName = args[0]?.ToString()?.Trim().ToLower();
        if ("confirm".Equals(kitName)) {
            return Confirmed(sender);
        }

        Confirmation confirm = new();
        if (kitName is not (null or "*")) {
            if (!Kits.VALID_NAME.IsMatch(kitName)) {
                return Error("invalid-kit-name");
            }

            confirm.kit = Kits.Get(kitName)?.Name;
            if (confirm.kit == null) {
                return Error("kit-does-not-exist", kitName);
            }
        }

        string? playerName = args[1]?.ToString()?.Trim().ToLower();
        if (playerName is not (null or "*")) {
            confirm.target = api.World.GetPlayer(playerName)?.PlayerName;

            if (confirm.target == null) {
                return Error("player-not-found");
            }
        }

        confirmations[sender.Uid] = confirm;

        if (confirm.kit == null) {
            // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
            if (confirm.target == null) {
                sender.SendMessage(Lang.Success("resetkit-all-kits-for-all-players-confirm"));
            }
            else {
                sender.SendMessage(Lang.Success("resetkit-all-kits-for-player-confirm", confirm.target));
            }
        }
        else {
            // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
            if (confirm.target == null) {
                sender.SendMessage(Lang.Success("resetkit-kit-for-all-players-confirm", confirm.kit));
            }
            else {
                sender.SendMessage(Lang.Success("resetkit-kit-for-player-confirm", confirm.kit, confirm.target));
            }
        }

        return Success("confirm-action", CmdName);
    }

    private CommandResult Confirmed(BasicPlayer sender) {
        if (!confirmations.Remove(sender.Uid, out Confirmation? confirmation)) {
            return Error("nothing-to-confirm");
        }

        if (confirmation.target == null) {
            api.Event.RegisterCallback(_ => {
                if (confirmation.kit == null) {
                    sender.SendMessage(Lang.Success("resetkit-start-all-kits-all-players"));

                    foreach (IPlayer player in api.World.AllPlayers) {
                        BasicPlayer.Get(player).ResetAllKitsLastUsed().Save();
                    }

                    sender.SendMessage(Lang.Success("resetkit-finish-all-kits-all-players"));
                    return;
                }

                Kit? kit = Kits.Get(confirmation.kit);
                if (kit == null) {
                    sender.SendMessage(Lang.Error("kit-does-not-exist", confirmation.kit));
                    return;
                }

                sender.SendMessage(Lang.Success("resetkit-start-kit-all-players", kit.Name));

                foreach (IPlayer player in api.World.AllPlayers) {
                    BasicPlayer.Get(player).ResetKitLastUsed(kit).Save();
                }

                sender.SendMessage(Lang.Success("resetkit-finish-kit-all-players", kit.Name));
            }, 0);
            return Success();
        }

        IPlayer? target = api.World.GetPlayer(confirmation.target);
        if (target == null) {
            return Error("player-not-found");
        }

        OfflinePlayer player = BasicPlayer.Get(target);
        if (confirmation.kit == null) {
            player.ResetAllKitsLastUsed().Save();
            return Success("resetkit-finish-all-kits-player", target.PlayerName);
        }

        Kit? kit = Kits.Get(confirmation.kit);
        if (kit == null) {
            return Error("kit-does-not-exist", confirmation.kit);
        }

        player.ResetKitLastUsed(kit).Save();
        return Success("resetkit-finish-kit-player", kit.Name, target.PlayerName);
    }
}

public class Confirmation {
    public string? kit;
    public string? target;
}
