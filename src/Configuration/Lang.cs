namespace BasicCommands.Configuration;

public abstract class Lang {
    public static string Get(string key, params object[] args) {
        return Vintagestory.API.Config.Lang.Get($"basiccommands:{key}", args);
    }
}
