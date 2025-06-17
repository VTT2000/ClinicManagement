using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

public interface IChatLocalServiceBE
{
    public Task<dynamic> CreateChatRoom(string authorization, ChatBoxLocalCreateVM chatBoxLocal);
    public Task<dynamic> DeleteChatRoom(Guid chatID);
    public Task<dynamic> GetAllChatRoom(string authorization);
    public Task<dynamic> GetAllMessageChatFromChatRoom(Guid chatID);
    public Task<dynamic> SendMessageChatRoom(string authorization, MemberChatLocalSendVM memberChatLocal);
    public Task<dynamic> GetAllUserForChatBoxLocal(string authorization, PagedResponse<ConditionFilterUserSelectedForChatBoxLocal> pagedResponse);
}

public class ChatLocalServiceBE : IChatLocalServiceBE
{
    private readonly IDatabase _db;
    private readonly JwtAuthService _jwtAuthService;
    private readonly IUnitOfWork _unitOfWork;

    public ChatLocalServiceBE(IUnitOfWork unitOfWork, IConnectionMultiplexer redis, JwtAuthService jwtAuthService)
    {
        _db = redis.GetDatabase();
        _unitOfWork = unitOfWork;
        _jwtAuthService = jwtAuthService;
    }

    // Implement methods for admin functionalities here
    public async Task<dynamic> GetAllUserForChatBoxLocal(string authorization, PagedResponse<ConditionFilterUserSelectedForChatBoxLocal> pagedResponse)
    {
        HTTPResponseClient<PagedResponse<List<UserSelectdVMForChatBoxLocal>>> result = new HTTPResponseClient<PagedResponse<List<UserSelectdVMForChatBoxLocal>>>()
        {
            Data = new PagedResponse<List<UserSelectdVMForChatBoxLocal>>()
            {
                Data = new List<UserSelectdVMForChatBoxLocal>(),
                PageNumber = pagedResponse.PageNumber,
                PageSize = pagedResponse.PageSize
            }
        };
        try
        {
            string token = authorization.Substring("Bearer ".Length);
            string fullName = _jwtAuthService.DecodePayloadToken(token);
            var user = await _unitOfWork._userRepository.SingleOrDefaultAsync(p => p.FullName == fullName
            && (RoleConstant.Doctor == p.Role || RoleConstant.Receptionist == p.Role));

            var kq = await _unitOfWork._userRepository.WhereAsync(p=>p.Role.Equals(RoleConstant.Doctor)||p.Role.Equals(RoleConstant.Receptionist));

            var list = kq.Where(p=>!(pagedResponse.Data!.HasSkipSearcher && p.FullName.Equals(user!.FullName))).Where(p =>
            (string.IsNullOrWhiteSpace(pagedResponse.Data!.Role) || p.Role.Equals(pagedResponse.Data.Role))
            && (string.IsNullOrWhiteSpace(pagedResponse.Data!.Name) || StringHelper.IsMatchSearchKey(pagedResponse.Data.Name, p.FullName))).ToList();
            var data = list.Select(p => new UserSelectdVMForChatBoxLocal()
            {
                UserId = p.UserId,
                Name = p.FullName,
                Role = p.Role
            }).ToList();

            result.Data.TotalRecords = data.Count;
            result.Data.TotalPages = (int)Math.Ceiling((double)data.Count / result.Data.PageSize);
            result.Data.Data = data
            // lấy theo page
            .Skip(result.Data.PageSize * (result.Data.PageNumber - 1))
            .Take(result.Data.PageSize).ToList();
                
            result.Message = "Thành công";
            result.StatusCode = StatusCodes.Status200OK;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            result.Message = "Thất bại";
            result.StatusCode = StatusCodes.Status500InternalServerError;
        }
        result.DateTime = DateTime.Now;
        return result;
    }
    public async Task<dynamic> CreateChatRoom(string authorization, ChatBoxLocalCreateVM chatBoxLocal)
    {
        HTTPResponseClient<bool> result = new HTTPResponseClient<bool>()
        {
            Data = false
        };
        try
        {
            string token = authorization.Substring("Bearer ".Length);
            string fullName = _jwtAuthService.DecodePayloadToken(token);
            var user = await _unitOfWork._userRepository.SingleOrDefaultAsync(p => p.FullName == fullName
            && (RoleConstant.Doctor == p.Role || RoleConstant.Receptionist == p.Role));

            if (!chatBoxLocal.MemberJoined.Contains(user!.FullName))
            {
                chatBoxLocal.MemberJoined.Add(user.FullName);
            }

            var keys = await _db.SetMembersAsync($"user:chats:{user.FullName}");
            List<ChatBoxLocalVM> data = new List<ChatBoxLocalVM>();
            foreach (var item in keys.Select(k => k.ToString()).ToList())
            {
                var temp = await _db.StringGetAsync(item);
                var temp2 = JsonSerializer.Deserialize<ChatBoxLocal>(temp.ToString());
                if (temp2 != null)
                {
                    if (ListHelper.AreEqualExactButUnordered(temp2.MemberJoined, chatBoxLocal.MemberJoined))
                    {
                        result.Data = false;
                        result.Message = "Cuộc trò chuyện này đã được tạo từ trước!";
                        result.StatusCode = StatusCodes.Status400BadRequest;
                        return result;
                    }
                }
            }

            ChatBoxLocal chatBox = new ChatBoxLocal()
            {
                ChatID = Guid.NewGuid(),
                RoomName = chatBoxLocal.MemberJoined.Count > 2 ? chatBoxLocal.RoomName : "",
                NameMemberCreated = user.FullName,
                MemberJoined = chatBoxLocal.MemberJoined
            };
            var roomKey = $"ChatID:{chatBox.ChatID}";
            while (await _db.KeyExistsAsync(roomKey))
            {
                chatBox.ChatID = new Guid();
                roomKey = $"ChatID:{chatBox.ChatID}";
            }
            ;

            await _db.StringSetAsync(roomKey, JsonSerializer.Serialize(chatBox));

            // Điều kiện
            foreach (var item in chatBoxLocal.MemberJoined)
            {
                await _db.SetAddAsync($"user:chats:{item}", roomKey);
            }

            result.Data = true;
            result.Message = "Thành công";
            result.StatusCode = StatusCodes.Status200OK;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            result.Message = "Thất bại";
            result.StatusCode = StatusCodes.Status500InternalServerError;
        }
        result.DateTime = DateTime.Now;
        return result;
    }

    public async Task<dynamic> DeleteChatRoom(Guid chatID)
    {
        HTTPResponseClient<bool> result = new HTTPResponseClient<bool>()
        {
            Data = false
        };
        try
        {
            var roomKey = $"ChatID:{chatID}";
            var roomKey2 = $"MessageChatLocal:{chatID}";

            // lay value info room chat
            var temp = await _db.StringGetAsync(roomKey);
            if (temp.HasValue)
            {
                var temp0 = JsonSerializer.Deserialize<ChatBoxLocal>(temp!);
                if (temp0 != null)
                {
                    foreach (var item in temp0.MemberJoined)
                    {
                        // xoa cac user chat co phong room chat la room key
                        await _db.SetRemoveAsync($"user:chats:{item}", roomKey);
                    }
                }
            }
            // xoa info room chat
            await _db.KeyDeleteAsync(roomKey);
            // xoa doan chat
            await _db.KeyDeleteAsync(roomKey2);

            result.Data = true;
            result.Message = "Thành công";
            result.StatusCode = StatusCodes.Status200OK;
        }
        catch (Exception ex) {
            Console.WriteLine(ex.Message);
            result.Message = "Thất bại";
            result.StatusCode = StatusCodes.Status500InternalServerError;
        }
        result.DateTime = DateTime.Now;
        return result;
    }

    public async Task<dynamic> GetAllChatRoom(string authorization)
    {
        HTTPResponseClient<List<ChatBoxLocalVM>> result = new HTTPResponseClient<List<ChatBoxLocalVM>>()
        {
            Data = new List<ChatBoxLocalVM>()
        };
        try
        {
            string token = authorization.Substring("Bearer ".Length);
            string fullName = _jwtAuthService.DecodePayloadToken(token);
            var user = await _unitOfWork._userRepository.SingleOrDefaultAsync(p => p.FullName == fullName
            && (RoleConstant.Doctor == p.Role || RoleConstant.Receptionist == p.Role));

            var keys = await _db.SetMembersAsync($"user:chats:{fullName}");
            List<ChatBoxLocalVM> data = new List<ChatBoxLocalVM>();
            foreach (var item in keys.Select(k => k.ToString()).ToList())
            {
                var temp = await _db.StringGetAsync(item);
                var temp2 = JsonSerializer.Deserialize<ChatBoxLocal>(temp.ToString());
                if (temp2 != null)
                {
                    string roomName = string.Empty;
                    if (temp2.MemberJoined.Count > 2)
                    {
                        roomName = temp2.RoomName;
                    }
                    else
                    {
                        roomName = temp2.MemberJoined.Where(p => !p.Equals(user!.FullName)).First();
                    }
                    data.Add(new ChatBoxLocalVM()
                    {
                        ChatID = temp2.ChatID,
                        RoomName = roomName
                    });
                }
            }
            result.Data = data;
            result.Message = "Thành công";
            result.StatusCode = StatusCodes.Status200OK;
        }
        catch (Exception ex) {
            Console.WriteLine(ex.Message);
            result.Message = "Thất bại";
            result.StatusCode = StatusCodes.Status500InternalServerError;
        }
        result.DateTime = DateTime.Now;
        return result;
    }

    public async Task<dynamic> GetAllMessageChatFromChatRoom(Guid chatID)
    {
        HTTPResponseClient<List<MemberChatLocalVM>> result = new HTTPResponseClient<List<MemberChatLocalVM>>()
        {
            Data = new List<MemberChatLocalVM>()
        };
        try
        {
            List<MemberChatLocalVM> data = new List<MemberChatLocalVM>();
            var roomKey = $"MessageChatLocal:{chatID}";

            var kq = await _db.ListRangeAsync(roomKey);
            foreach (var item in kq) {
                var temp = JsonSerializer.Deserialize<MemberChatLocal>(item.ToString());
                if (temp != null)
                {
                    data.Add(new MemberChatLocalVM()
                    {
                        NameMember = temp.NameMember,
                        Message = temp.Message,
                        CreatedAt = temp.CreatedAt
                    });
                }
            }
            result.Data = data;
            result.Message = "Thành công";
            result.StatusCode = StatusCodes.Status200OK;
        }
        catch (Exception ex) {
            Console.WriteLine(ex.Message);
            result.Message = "Thất bại";
            result.StatusCode = StatusCodes.Status500InternalServerError;
        }
        result.DateTime = DateTime.Now;
        return result;
    }

    public async Task<dynamic> SendMessageChatRoom(string authorization, MemberChatLocalSendVM memberChatLocal)
    {
        HTTPResponseClient<bool> result = new HTTPResponseClient<bool>()
        {
            Data = false
        };
        try
        {
            string token = authorization.Substring("Bearer ".Length);
            string fullName = _jwtAuthService.DecodePayloadToken(token);
            var user = await _unitOfWork._userRepository.SingleOrDefaultAsync(p => p.FullName == fullName
            && (RoleConstant.Doctor == p.Role || RoleConstant.Receptionist == p.Role));

            MemberChatLocalVM item = new MemberChatLocalVM()
            {
                NameMember = user!.FullName,
                Message = memberChatLocal.Message,
                CreatedAt = memberChatLocal.CreatedAt
            };

            if (memberChatLocal.ChatID.HasValue)
            {
                var roomKey = $"MessageChatLocal:{memberChatLocal.ChatID.Value}";
                await _db.ListRightPushAsync(roomKey, JsonSerializer.Serialize(item));
                await AutoDeleteMessageChatRoom(memberChatLocal.ChatID.Value);
            }
            
            result.Data = true;
            result.Message = "Thành công";
            result.StatusCode = StatusCodes.Status200OK;
        }
        catch (Exception ex) {
            Console.WriteLine(ex.Message);
            result.Message = "Thất bại";
            result.StatusCode = StatusCodes.Status500InternalServerError;
        }
        result.DateTime = DateTime.Now;
        return result;
    }

    private async Task AutoDeleteMessageChatRoom(Guid chatID)
    {
        var roomKey = $"MessageChatLocal:{chatID}";
        var entries = await _db.ListRangeAsync(roomKey);
        foreach (var item in entries
            .Select(x => JsonSerializer.Deserialize<MemberChatLocal>(x.ToString()))
            .Where(x => x != null)
            .ToList())
        {
            if (item!.CreatedAt.HasValue && item.CreatedAt.Value < DateTime.Now.AddDays(-3))
            {
                // Đã quá 3 ngày
                await _db.ListRemoveAsync(roomKey, JsonSerializer.Serialize(item));
            }
        }
    }
}

public class ChatBoxLocal
{
    public Guid ChatID { get; set; } = new Guid();
    public string RoomName { get; set; } = string.Empty;
    public string NameMemberCreated { get; set; } = string.Empty;
    public List<string> MemberJoined { get; set; } = new List<string>();
}
public class MemberChatLocal {
    public string NameMember { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime? CreatedAt { get; set; } = null;
}