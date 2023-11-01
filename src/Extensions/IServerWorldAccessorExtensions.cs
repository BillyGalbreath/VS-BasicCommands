using System.Linq;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace BasicCommands.Extensions;

public static class ServerWorldAccessorExtensions {
    public static IPlayer? GetPlayer(this IServerWorldAccessor world, string name) {
        return world.AllPlayers.FirstOrDefault(player => player.PlayerName.ToLower().Equals(name));
    }
}
