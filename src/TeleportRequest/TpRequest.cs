using System.Collections.Concurrent;
using System.Linq;
using BasicCommands.Configuration;
using BasicCommands.Player;

namespace BasicCommands.TeleportRequest;

public abstract class TpRequest {
    private static readonly ConcurrentDictionary<TpRequest, bool> REQUESTS = new();

    public static void Add(TpRequest request) {
        REQUESTS.TryAdd(request, true);
    }

    public static TpRequest? GetPendingForSender(BasicPlayer sender) {
        return REQUESTS.Keys.FirstOrDefault(request => request.sender.Equals(sender));
    }

    public static bool HasPendingForSender(BasicPlayer sender) {
        return GetPendingForSender(sender) != null;
    }

    public static TpRequest? GetPendingForTarget(BasicPlayer target) {
        return REQUESTS.Keys.FirstOrDefault(request => request.target.Equals(target));
    }

    public static bool HasPendingForTarget(BasicPlayer target) {
        return GetPendingForSender(target) != null;
    }

    protected readonly BasicPlayer sender;
    protected readonly BasicPlayer target;

    private readonly long task;

    protected TpRequest(BasicPlayer sender, BasicPlayer target) {
        this.sender = sender;
        this.target = target;

        task = sender.Player.Entity.World.Api.Event.RegisterCallback(_ => {
            Message("expired", true);
            Remove();
        }, 30000);
    }

    public TpRequest Message(string type, bool error) {
        if (error) {
            sender.SendMessage(Lang.Error($"teleport-request-{type}-sender", target.Name));
            target.SendMessage(Lang.Error($"teleport-request-{type}-target", sender.Name));
        }
        else {
            sender.SendMessage(Lang.Success($"teleport-request-{type}-sender", target.Name));
            target.SendMessage(Lang.Success($"teleport-request-{type}-target", sender.Name));
        }

        return this;
    }

    public void Remove() {
        REQUESTS.TryRemove(this, out bool _);
        sender.Player.Entity.World.Api.Event.UnregisterCallback(task);
    }

    public void Accept() {
        if (target.IsOnline) {
            Teleport();
        }
        else {
            sender.SendMessage(Lang.Error("teleport-request-target-offline", target.Name));
        }

        Remove();
    }

    protected abstract void Teleport();
}
