using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

[Authorize]
public class ChatLocalHub : Hub
{
    private readonly IChatLocalServiceBE _chatLocalServiceBE;
    public ChatLocalHub(IChatLocalServiceBE chatLocalServiceBE)
    {
        _chatLocalServiceBE = chatLocalServiceBE;
    }
    
    public async Task ReceivedChangeMessageBox()
    {
        await Clients.All.SendAsync("ReceivedChangeMessageBox");
        await Task.CompletedTask;
    }

    public async Task ReceivedChangeListChat()
    {
        await Clients.All.SendAsync("ReceivedChangeListChat");
        await Task.CompletedTask;
    }

    public override async Task OnConnectedAsync()
    {
        Console.WriteLine($"[SignalR] Connected: {Context.ConnectionId}");
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        Console.WriteLine($"[SignalR] Disconnected: {Context.ConnectionId}");
        await base.OnDisconnectedAsync(exception);
    }
}