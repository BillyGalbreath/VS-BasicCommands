using System;
using System.Linq;
using BasicCommands.Configuration;
using BasicCommands.Player;
using Vintagestory.API.Common;

namespace BasicCommands.Command.Parser;

public class BasicPlayerArgParser : ArgumentParserBase {
    private BasicPlayer? player;

    public BasicPlayerArgParser(string argName, bool isMandatoryArg = true) : base(argName, isMandatoryArg) { }

    public override string[] GetValidRange(CmdArgs args) {
        return BasicPlayer.GetAll().Select(basicPlayer => basicPlayer.Name).ToArray();
    }

    public override BasicPlayer? GetValue() {
        return player;
    }

    public override void SetValue(object data) {
        player = (BasicPlayer)data;
    }

    public override EnumParseResult TryProcess(TextCommandCallingArgs args, Action<AsyncParseResults>? onReady = null) {
        string? arg = args.RawArgs.PopWord()?.ToLower();
        if (arg == null) {
            lastErrorMessage = Lang.Get("Argument is missing");
            return EnumParseResult.Bad;
        }

        var online = BasicPlayer.GetAll().Where(basicPlayer => basicPlayer.Name.ToLower().StartsWith(arg)).ToArray();
        if (online.Length > 1) {
            lastErrorMessage = Lang.Get("More than one player matches that name");
            return EnumParseResult.Bad;
        }

        if ((player = online.FirstOrDefault()) != null) {
            return EnumParseResult.Good;
        }

        lastErrorMessage = Lang.Get("No such player online");
        return EnumParseResult.Bad;
    }
}
