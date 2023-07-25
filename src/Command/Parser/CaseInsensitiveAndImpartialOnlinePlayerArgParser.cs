using BasicCommands.Configuration;
using System;
using System.Linq;
using Vintagestory.API.Common;

namespace BasicCommands.Command.Parser;

public class CaseInsensitiveAndImpartialOnlinePlayerArgParser : OnlinePlayerArgParser {
    public CaseInsensitiveAndImpartialOnlinePlayerArgParser(string argName, ICoreAPI api, bool isMandatoryArg) : base(argName, api, isMandatoryArg) { }

    public override EnumParseResult TryProcess(TextCommandCallingArgs args, Action<AsyncParseResults> onReady = null) {
        string playername = args.RawArgs.PopWord()?.ToLower();
        if (playername == null) {
            lastErrorMessage = Lang.Get("Argument is missing");
            return EnumParseResult.Bad;
        }

        System.Collections.Generic.IEnumerable<IPlayer> online = api.World.AllOnlinePlayers.Where(player => player.PlayerName.ToLower().StartsWith(playername));
        if (online.Count() > 1) {
            lastErrorMessage = Lang.Get("More than one player matches that name");
            return EnumParseResult.Bad;
        }

        player = online.FirstOrDefault(player => true);
        if (player == null) {
            lastErrorMessage = Lang.Get("No such player online");
            return EnumParseResult.Bad;
        }

        return EnumParseResult.Good;
    }
}
