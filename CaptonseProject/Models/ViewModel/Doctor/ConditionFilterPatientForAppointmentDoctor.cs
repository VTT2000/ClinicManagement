public class ConditionFilterPatientForAppointmentDoctor
{
    public string searchNamePatient { get; set; } = string.Empty;
    public DateOnly? dateAppointment { get; set; }
    public string status { get; set; } = string.Empty;
}