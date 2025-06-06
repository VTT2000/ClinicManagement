public class ConnectionMapping
{
    private readonly Dictionary<string, string> _connections = new Dictionary<string, string>();

    public void Add(string username, string connectionId)
        => _connections.Add(username, connectionId);

    public void Remove(string username)
        => _connections.Remove(username);

    public string? GetConnectionId(string username)
        => _connections.TryGetValue(username, out var connId) ? connId : null;
}