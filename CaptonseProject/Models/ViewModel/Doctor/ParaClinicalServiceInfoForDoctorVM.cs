public class ParaClinicalServiceInfoForDoctorVM
{
    public int ServiceId { get; set; }
    public string ServiceName { get; set; } = null!;
    public DateTime? CreatedAt { get; set; }
    public string ServiceResultReport { get; set; } = null!;
    public string FullNameUserperformed { get; set; } = null!;
    public string RoomName { get; set; } = null!;
}