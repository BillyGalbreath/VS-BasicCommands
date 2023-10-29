using System;
using Vintagestory.API.Config;

namespace BasicCommands.Extensions;

public static class TimeSpanExtensions {
    public static string Remaining(this TimeSpan timeSpan) {
        if (timeSpan.Days > 0) {
            return Lang.Get("{p0:# days|# day|# days}", timeSpan.Days);
        }

        if (timeSpan.Hours > 0) {
            return Lang.Get("{p0:# hours|# hour|# hours}", timeSpan.Hours);
        }

        if (timeSpan.Minutes > 0) {
            return Lang.Get("{p0:# minutes|# minute|# minutes}", timeSpan.Minutes);
        }

        if (timeSpan.Seconds > 0) {
            return Lang.Get("{p0:# seconds|# second|# seconds}", timeSpan.Seconds);
        }

        // ReSharper disable once ConvertIfStatementToReturnStatement
        if (timeSpan.Milliseconds > 0) {
            return Lang.Get("{p0:# milliseconds|# millisecond|# milliseconds}", timeSpan.Milliseconds);
        }

        return timeSpan.ToString();
    }
}
