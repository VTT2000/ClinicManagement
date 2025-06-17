public class ChatBoxLocalVM
{
    public Guid ChatID { get; set; } = new Guid();
    public string RoomName { get; set; } = string.Empty;
}
public class MemberChatLocalVM
{
    public string NameMember { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime? CreatedAt { get; set; } = null;
}

public class ChatBoxLocalCreateVM
{
    public string RoomName { get; set; } = string.Empty;
    public List<string> MemberJoined { get; set; } = new List<string>();
}
public class MemberChatLocalSendVM
{
    public Guid? ChatID { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime? CreatedAt { get; set; } = null;
}