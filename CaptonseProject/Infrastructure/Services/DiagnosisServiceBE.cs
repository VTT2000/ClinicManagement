using web_api_base.Models.ClinicManagement;

public interface IDiagnosisServiceBE
{
    public Task<dynamic> SaveDiagnosisDoctorAsync(DetailSaveDiagnosisDoctorVM item, string authorization);
}

public class DiagnosisServiceBE : IDiagnosisServiceBE
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly JwtAuthService _jwtAuthService;
    public DiagnosisServiceBE(IUnitOfWork unitOfWork, JwtAuthService jwtAuthService)
    {
        _unitOfWork = unitOfWork;
        _jwtAuthService = jwtAuthService;
    }

    public async Task<dynamic> SaveDiagnosisDoctorAsync(DetailSaveDiagnosisDoctorVM item, string authorization)
    {
        var result = new HTTPResponseClient<bool>();
        result.Data = false;
        
        try
        {
            string token = authorization.Substring("Bearer ".Length);
            string fullName = _jwtAuthService.DecodePayloadToken(token);
            var user = await _unitOfWork._userRepository.SingleOrDefaultAsync(p => p.FullName == fullName
            && (RoleConstant.Doctor == p.Role || RoleConstant.Admin == p.Role));

            if (user == null)
            {
                result.Message = "Thất bại";
                result.StatusCode = StatusCodes.Status401Unauthorized;
                result.Data = false;
            }
            else
            {
                await _unitOfWork.BeginTransaction();
                if (item.DiagnosisId == null)
                {
                    // create
                    Diagnosis temp = new Diagnosis()
                    {
                        AppointmentId = item.AppointmentId,
                        Symptoms = item.Symptoms,
                        Diagnosis1 = item.Diagnosis1,
                        Prescription = item.Prescription
                    };
                    await _unitOfWork._diagnosisRepository.AddAsync(temp);
                    await _unitOfWork.SaveChangesAsync();

                    int userIdperformed = user.UserId;
                    if (user.Role.Equals(RoleConstant.Admin))
                    {
                        userIdperformed = item.ClinicalServiceUserIdperformed;
                    }

                    DiagnosesService clinicalService = new DiagnosesService()
                    {
                        DiagnosisId = temp.DiagnosisId,
                        ServiceId = item.ClinicalServiceId,
                        CreatedAt = DateTime.Now,
                        ServiceResultReport = item.ClinicalServiceServiceResultReport,
                        UserIdperformed = userIdperformed,
                        RoomId = item.ClinicalServiceRoomId
                    };

                    await _unitOfWork._diagnosisServiceRepository.AddAsync(clinicalService);
                    await _unitOfWork.SaveChangesAsync();

                    foreach (var i in item.ParaclinicalServiceList)
                    {
                        await _unitOfWork._diagnosisServiceRepository.AddAsync(new DiagnosesService()
                        {
                            DiagnosisId = temp.DiagnosisId,
                            ServiceId = i
                        });
                        await _unitOfWork.SaveChangesAsync();
                    }

                    result.Message = "Thành công";
                    result.StatusCode = StatusCodes.Status200OK;
                    result.Data = true;
                }
                else
                {
                    // update
                    Diagnosis temp = await _unitOfWork._diagnosisRepository.GetByIdAsync(item.DiagnosisId.Value);
                    temp.AppointmentId = item.AppointmentId;
                    temp.Symptoms = item.Symptoms;
                    temp.Diagnosis1 = item.Diagnosis1;
                    temp.Prescription = item.Prescription;
                    _unitOfWork._diagnosisRepository.Update(temp);
                    await _unitOfWork.SaveChangesAsync();

                    var diagnosesServices = await _unitOfWork._diagnosisServiceRepository.GetAllDiagnosisServiceAndService(p => p.DiagnosisId == item.DiagnosisId.Value && p.Service.Type == TypeServiceConstant.Clinical);

                    int userIdperformed = user.UserId;
                    if (user.Role.Equals(RoleConstant.Admin))
                    {
                        userIdperformed = item.ClinicalServiceUserIdperformed;
                    }

                    DiagnosesService clinicalService = diagnosesServices.FirstOrDefault();

                    clinicalService.DiagnosisId = temp.DiagnosisId;
                    clinicalService.ServiceId = item.ClinicalServiceId;
                    clinicalService.CreatedAt = DateTime.Now;
                    clinicalService.ServiceResultReport = item.ClinicalServiceServiceResultReport;
                    clinicalService.UserIdperformed = userIdperformed;
                    clinicalService.RoomId = item.ClinicalServiceRoomId;
        
                    _unitOfWork._diagnosisServiceRepository.Update(clinicalService);
                    await _unitOfWork.SaveChangesAsync();

                    var diagnosesServices2 = await _unitOfWork._diagnosisServiceRepository.GetAllDiagnosisServiceAndService(p => p.DiagnosisId == item.DiagnosisId.Value && p.Service.Type == TypeServiceConstant.Paraclinical);

                    foreach (var i in diagnosesServices2)
                    {
                        if (item.ParaclinicalServiceList.Contains(i.ServiceId))
                        {
                            item.ParaclinicalServiceList.Remove(i.ServiceId);
                        }
                        else
                        {
                            // remove
                            await _unitOfWork._diagnosisServiceRepository.DeleteAsync(i.DiagnosesServiceId);
                            await _unitOfWork.SaveChangesAsync();
                        }
                    }

                    foreach (var i in item.ParaclinicalServiceList)
                    {
                        await _unitOfWork._diagnosisServiceRepository.AddAsync(new DiagnosesService()
                        {
                            DiagnosisId = temp.DiagnosisId,
                            ServiceId = i
                        });
                        await _unitOfWork.SaveChangesAsync();
                    }
                    
                    result.Message = "Thành công";
                    result.StatusCode = StatusCodes.Status200OK;
                    result.Data = true;
                }
                await _unitOfWork.CommitTransaction();
            }
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollBack();
            Console.WriteLine(ex.Message);
            result.Message = "Thất bại";
            result.StatusCode = StatusCodes.Status500InternalServerError;
        }
        result.DateTime = DateTime.Now;
        return result;
    }
}