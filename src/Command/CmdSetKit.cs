using System;
using System.Collections.Generic;
using BasicCommands.Configuration;
using BasicCommands.Extensions;
using BasicCommands.Player;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace BasicCommands.Command;

public class CmdSetKit : AbstractCommand {
    public CmdSetKit(ICoreServerAPI api, Config config) : base(api, config, new WordArgParser("name", true), new IntArgParser("cooldown", 0, false)) { }

    protected override CommandResult Execute(BasicPlayer sender, TextCommandCallingArgs args) {
        string name = ((string)args[0]).Trim().ToLower();
        if (!Kits.VALID_NAME.IsMatch(name)) {
            return Error("invalid-kit-name");
        }

        if (Kits.Get(name) != null) {
            return Error("kit-already-exists", name);
        }

        List<byte[]> items = new();
        foreach (ItemSlot? slot in sender.Player.InventoryManager.GetHotbarInventory()) {
            if (slot.GetType() != typeof(ItemSlotSurvival) || slot.Itemstack == null) {
                continue;
            }

            ItemStack item = slot.Itemstack.Clone();
            item.Attributes.RemoveAttribute("transitionstate");
            items.Add(item.ToBytes());
        }

        // ReSharper disable once ConvertIfStatementToSwitchStatement
        if (items.Count == 0) {
            return Error("kit-empty-hotbar");
        }

        if (items.Count > 10) {
            return Error("kit-hotbar-too-big");
        }

        long cooldown = (int)args[1] * 1000;

        Kits.Add(new Kit {
            Name = name,
            Items = items.ToArray(),
            Cooldown = cooldown
        });

        Kits.Save(api);

        // ReSharper disable once ConvertIfStatementToReturnStatement
        if (cooldown == 0) {
            return Success("setkit-single-use-success", name);
        }

        return Success("setkit-success", name, TimeSpan.FromMilliseconds(cooldown).Remaining());
    }
}
