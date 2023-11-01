using System.Linq;
using Vintagestory.API.Common;

namespace BasicCommands.Extensions;

public static class InventoryExtensions {
    public static int EmptySlots(this IInventory inventory) {
        return inventory.Count(slot => slot is ItemSlotSurvival && slot.Empty);
    }
}
