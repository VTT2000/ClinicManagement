public class WorkScheduleDoctorVM
{
    public int WorkScheduleId { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? DayOfWeek { get; set; }

    public List<string> ListDoctorName { get; set; } = new List<string>();
}