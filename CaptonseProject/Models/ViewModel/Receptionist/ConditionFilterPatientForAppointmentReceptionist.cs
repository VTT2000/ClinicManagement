public class ConditionFilterPatientForAppointmentReceptionist
{
    public string searchNamePatient { get; set; } = string.Empty;
    public DateOnly dateAppointment { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    public string Status { get; set; } = string.Empty;
}