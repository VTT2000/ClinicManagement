public class DetailSaveDiagnosisDoctorVM
{
    public int? DiagnosisId { get; set; }

    public int AppointmentId { get; set; }

    public string Symptoms { get; set; }

    public string Diagnosis1 { get; set; }

    public string? Prescription { get; set; }

    public int ClinicalServiceId { get; set; }
    public DateTime? ClinicalServiceCreatedAt { get; set; }

    public string ClinicalServiceServiceResultReport { get; set; } = null!;

    public int ClinicalServiceUserIdperformed { get; set; }
    public int ClinicalServiceRoomId { get; set; }

    public List<int> ParaclinicalServiceList = new List<int>();
}