using System;
using System.Linq;
using BasicCommands.Configuration;
using BasicCommands.Player;
using Vintagestory.API.Common;

namespace BasicCommands.Command.Parser;

public class KitArgParser : ArgumentParserBase {
    private Kit? kit;

    public KitArgParser(string argName, bool isMandatoryArg = true) : base(argName, isMandatoryArg) { }

    public override string[] GetValidRange(CmdArgs args) {
        return BasicPlayer.GetAll().Select(basicPlayer => basicPlayer.Name).ToArray();
    }

    public override Kit? GetValue() => !isMandatoryArg && IsMissing ? null : kit;

    public override void SetValue(object data) => kit = (Kit)data;

    public override EnumParseResult TryProcess(TextCommandCallingArgs args, Action<AsyncParseResults>? onReady = null) {
        string? arg = args.RawArgs.PopWord()?.ToLower();
        if (arg == null) {
            lastErrorMessage = Lang.Error("must-specify-kit");
            return EnumParseResult.Bad;
        }

        kit = Kits.Get(arg);
        if (kit != null) {
            return EnumParseResult.Good;
        }

        lastErrorMessage = Lang.Error("kit-does-not-exist", arg);
        return EnumParseResult.Bad;
    }
}
