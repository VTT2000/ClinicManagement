public class WorkScheduleDoctorVM
{
    public int WorkScheduleId { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public TimeOnly? StartTime { get; set; }

    public TimeOnly? EndTime { get; set; }
    public string DoctorName { get; set; }
}