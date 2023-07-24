using BasicCommands.Configuration;
using BasicCommands.Player;
using System.Collections.Concurrent;
using System.Linq;

namespace BasicCommands.TeleportRequest;

public abstract class TpRequest {
    public static readonly ConcurrentDictionary<TpRequest, bool> requests = new();

    public static void Add(TpRequest request) {
        requests.TryAdd(request, true);
    }

    public static TpRequest GetPendingForSender(BasicPlayer sender) {
        return requests.Keys.Where(request => request.sender == sender).FirstOrDefault();
    }

    public static bool HasPendingForSender(BasicPlayer sender) {
        return GetPendingForSender(sender) != null;
    }

    public static TpRequest GetPendingForTarget(BasicPlayer target) {
        return requests.Keys.Where(request => request.target == target).FirstOrDefault();
    }

    public static bool HasPendingForTarget(BasicPlayer target) {
        return GetPendingForSender(target) != null;
    }

    public BasicPlayer sender;
    public BasicPlayer target;

    private readonly long task;

    public TpRequest(BasicPlayer sender, BasicPlayer target) {
        this.sender = sender;
        this.target = target;

        task = BasicCommandsMod.Instance().API.Event.RegisterCallback(delta => {
            Message("expired");
            Remove();
        }, 30000);
    }

    public TpRequest Message(string type) {
        sender.SendMessage(Lang.Get($"teleport-request-{type}-sender", target.Name));
        target.SendMessage(Lang.Get($"teleport-request-{type}-target", sender.Name));
        return this;
    }

    public void Remove() {
        requests.TryRemove(this, out var _);
        BasicCommandsMod.Instance().API.Event.UnregisterCallback(task);
    }

    public void Accept() {
        Teleport();
        Remove();
    }

    protected abstract void Teleport();
}
