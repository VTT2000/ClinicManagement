public class DiagnosisDoctorVM
{
    public int DiagnosisId { get; set; }

    public string RoomName { get; set; } = null!;
    public DateTime? CreatedAt { get; set; }

    public string ClinicalServiceName { get; set; } = null!;
    public string ClinicalServiceResultReport { get; set; } = null!;
    
    public string? Symptoms { get; set; }
    public string? Diagnosis1 { get; set; }
}