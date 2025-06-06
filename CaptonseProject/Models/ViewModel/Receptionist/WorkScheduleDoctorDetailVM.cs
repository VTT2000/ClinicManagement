using System.ComponentModel.DataAnnotations;

public class WorkScheduleDoctorDetailVM
{
    public int? WorkScheduleId { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public TimeOnly? StartTime { get; set; }

    public TimeOnly? EndTime { get; set; }
    public int? DoctorId { get; set; }
}