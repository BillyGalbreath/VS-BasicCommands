using Essentials.Player;
using Vintagestory.API.Common;

namespace Essentials.Command {
    internal class CmdBack : AbstractCommand {
        internal CmdBack() : base("back", "Teleports you to your previous location", new[] { "return" }) { }

        internal override TextCommandResult Execute(TextCommandCallingArgs args) {
            EssPlayer player = EssPlayer.Get(args.Caller.Player);
            if (player == null) {
                return TextCommandResult.Error("Player only command.");
            }

            if (player.LastPos == null) {
                return TextCommandResult.Success("No back location to go to.");
            }

            player.TeleportTo(player.LastPos);

            return TextCommandResult.Success("Teleported to your previous location.");
        }
    }
}
