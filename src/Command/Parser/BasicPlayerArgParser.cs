using BasicCommands.Configuration;
using BasicCommands.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Common;

namespace BasicCommands.Command.Parser;

public class BasicPlayerArgParser : ArgumentParserBase {
    protected BasicPlayer player;

    public BasicPlayerArgParser(string argName) : this(argName, true) { }
    public BasicPlayerArgParser(string argName, bool isMandatoryArg) : base(argName, isMandatoryArg) { }

    public override string[] GetValidRange(CmdArgs args) {
        return BasicPlayer.GetAll().Select(player => player.Name).ToArray();
    }

    public override BasicPlayer GetValue() {
        return player;
    }

    public override void SetValue(object data) {
        player = (BasicPlayer)data;
    }

    public override EnumParseResult TryProcess(TextCommandCallingArgs args, Action<AsyncParseResults> onReady = null) {
        args.
        string arg = args.RawArgs.PopWord()?.ToLower();
        if (arg == null) {
            lastErrorMessage = Lang.Get("Argument is missing");
            return EnumParseResult.Bad;
        }

        IEnumerable<BasicPlayer> online = BasicPlayer.GetAll().Where(player => player.Name.ToLower().StartsWith(arg));
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
