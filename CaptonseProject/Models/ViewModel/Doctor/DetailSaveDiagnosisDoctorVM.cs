public class DetailSaveDiagnosisDoctorVM
{
    public int? DiagnosisId { get; set; }

    public int? AppointmentId { get; set; }

    public string? Symptoms { get; set; }

    public string? Diagnosis1 { get; set; }

    public int? ClinicalServiceId { get; set; }

    public string ClinicalServiceServiceResultReport { get; set; } = null!;

    public int? ClinicalServiceRoomId { get; set; }

    public List<int> ParaclinicalServiceList { get; set; } = new List<int>();
    public List<PrescriptionDiagnosisDoctorVM> Prescriptions { get; set; } = new List<PrescriptionDiagnosisDoctorVM>();
}

public class PrescriptionDiagnosisDoctorVM
{
    public int? PrescriptionId { get; set; }

    public int? DiagnosisId { get; set; }

    public string? Prescription1 { get; set; }

    public virtual List<PrescriptionDetailDiagnosisDoctorVM> PrescriptionDetails { get; set; } = new List<PrescriptionDetailDiagnosisDoctorVM>();
}

public partial class PrescriptionDetailDiagnosisDoctorVM
{
    public int? PrescriptionDetailId { get; set; }

    public int? PrescriptionId { get; set; }

    public int MedicineId { get; set; }

    public int Quantity { get; set; }
}