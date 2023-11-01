using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using BasicCommands.Player;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace BasicCommands.Patches;

public class EntityPlayerPatches {
    protected internal EntityPlayerPatches(Harmony harmony) {
        _ = new TeleportToDoublePatch(harmony);
    }

    private class TeleportToDoublePatch {
        public TeleportToDoublePatch(Harmony harmony) {
            harmony.Patch(typeof(EntityPlayer).GetMethod("TeleportToDouble", BindingFlags.Instance | BindingFlags.Public),
                prefix: GetType().GetMethod("Prefix"));
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        public static void Prefix(EntityPlayer __instance) {
            if (__instance.Player is IServerPlayer player) {
                BasicPlayer.Get(player).UpdateLastPosition();
            }
        }
    }
}
