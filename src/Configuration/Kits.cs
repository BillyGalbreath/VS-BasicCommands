using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;
using ProtoBuf;
using Vintagestory.API.Server;
using Vintagestory.API.Util;

namespace BasicCommands.Configuration;

public class Kits {
    private static readonly Kits INSTANCE = new();

    [SuppressMessage("GeneratedRegex", "SYSLIB1045:Convert to \'GeneratedRegexAttribute\'.")]
    public static readonly Regex VALID_NAME = new("^(?i)[a-z][a-z0-9_-]*$");

    public static void Add(Kit kit) {
        INSTANCE.kits.Add(kit.Name, kit);
    }

    public static Kit? Get(string name) {
        return INSTANCE.kits!.Get(name);
    }

    public static bool Remove(string name) {
        return INSTANCE.kits.Remove(name);
    }

    public static string[] All() {
        return INSTANCE.kits.Keys.ToArray();
    }

    public static void Load(ICoreServerAPI sapi) {
        string key = $"{BasicCommandsMod.Id}:kits";
        byte[]? data = sapi.WorldManager.SaveGame.GetData(key);
        try {
            INSTANCE.kits = data == null
                ? new Dictionary<string, Kit>()
                : SerializerUtil.Deserialize<Dictionary<string, Kit>>(data);
        }
        catch (Exception) {
            INSTANCE.kits = new Dictionary<string, Kit>();
        }
    }

    public static void Save(ICoreServerAPI sapi) {
        string key = $"{BasicCommandsMod.Id}:kits";
        byte[] data = SerializerUtil.Serialize(INSTANCE.kits);
        sapi.WorldManager.SaveGame.StoreData(key, data);
    }

    private Dictionary<string, Kit> kits = new();
}

[ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
[SuppressMessage("ReSharper", "InconsistentNaming")]
public class Kit {
    public required string Name;
    public required byte[][] Items;
    public required long Cooldown;
}
