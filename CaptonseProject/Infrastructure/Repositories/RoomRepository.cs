using web_api_base.Models.ClinicManagement;

public interface IRoomRepository : IRepository<Room>
{
    // Add custom methods for Room here if needed
}

public class RoomRepository : Repository<Room>, IRoomRepository
{
    public RoomRepository(ClinicContext context) : base(context)
    {
    }
}