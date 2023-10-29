namespace BasicCommands.Configuration;

public abstract class Lang {
    public static string Get(string key, params object[] args) {
        return Vintagestory.API.Config.Lang.Get($"{BasicCommandsMod.Id}:{key}", args);
    }

    public static string Error(string key, params object[] args) {
        return Get("error", Get(key, args));
    }

    public static string Success(string key, params object[] args) {
        return Get("success", Get(key, args));
    }
}
