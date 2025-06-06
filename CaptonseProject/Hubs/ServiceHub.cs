using Microsoft.AspNetCore.SignalR;

public class ServiceHub : Hub
{
    ConnectionMapping connectionMapping = new ConnectionMapping();
    public async Task SendChangeListWaitingForDoctor(int doctorID)
    {
        await Task.CompletedTask;
    }
    public async Task SendToAll(string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", message);
    }

    public async Task SendToClient(string connectionId, string message)
    {
        await Clients.Client(connectionId).SendAsync("ReceiveMessage", message);
    }

    public async Task SendToGroup(string groupName, string message)
    {
        await Clients.Group(groupName).SendAsync("ReceiveMessage", message);
    }

    public async Task JoinGroup(string groupName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
    }

    public async Task LeaveGroup(string groupName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
    }

    public override async Task OnConnectedAsync()
    {
        Console.WriteLine($"[SignalR] Connected: {Context.ConnectionId}");
        string connectionId = Context.ConnectionId;
        string? userName = Context.User?.Identity?.Name; // Nếu dùng Auth
        if (!string.IsNullOrWhiteSpace(userName))
        {
            connectionMapping.Add(userName, connectionId);
        }
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        Console.WriteLine($"[SignalR] Disconnected: {Context.ConnectionId}");
        string? userName = Context.User?.Identity?.Name; // Nếu dùng Auth
        if (!string.IsNullOrWhiteSpace(userName))
        {
            connectionMapping.Remove(userName);
        }
        await base.OnDisconnectedAsync(exception);
    }
}

