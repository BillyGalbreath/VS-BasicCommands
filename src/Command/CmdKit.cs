using System;
using BasicCommands.Command.Parser;
using BasicCommands.Configuration;
using BasicCommands.Extensions;
using BasicCommands.Player;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace BasicCommands.Command;

public class CmdKit : AbstractCommand {
    public CmdKit(ICoreServerAPI api, Config config) : base(api, config, new KitArgParser("name", false)) { }

    protected override CommandResult Execute(BasicPlayer sender, TextCommandCallingArgs args) {
        Kit? kit = (Kit)args[0];
        if (kit == null) {
            return Success("listkit-success", string.Join(", ", Kits.All()));
        }

        long now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        long lastUsed = sender.GetKitLastUsed(kit);
        if (kit.Cooldown == 0 && lastUsed > 0) {
            return Error("kit-already-used", kit.Name);
        }

        long remaining = kit.Cooldown - (now - lastUsed);
        if (remaining > 0) {
            return Error("kit-on-cooldown", kit.Name, TimeSpan.FromMilliseconds(remaining).Remaining());
        }

        IPlayerInventoryManager invManager = sender.Player.InventoryManager;
        if (kit.Items.Length > invManager.GetHotbarInventory().EmptySlots()) {
            return Error("kit-need-empty-slots", kit.Items.Length);
        }

        foreach (byte[] data in kit.Items) {
            ItemStack item = new(data);
            if (item.ResolveBlockOrItem(api.World)) {
                invManager.TryGiveItemstack(item, true);
            }
            else {
                string itemInfo = $"{item.StackSize}x{item.Class.Name()[0]}{item.Id}[{item.Collectible?.Code}]";
                sender.Player.Entity.World.Logger.Error(Lang.Error("kit-could-not-give-item", sender, itemInfo, kit.Name));
            }
        }

        sender.SetKitLastUsed(kit, now).Save();

        return Success("kit-success", kit.Name);
    }
}
