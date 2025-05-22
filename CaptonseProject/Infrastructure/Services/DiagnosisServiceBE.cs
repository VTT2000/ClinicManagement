using web_api_base.Models.ClinicManagement;

public interface IDiagnosisServiceBE
{
    public Task<dynamic> GetAllDiagnosisByAppointmentIDAsync(int appointmentID, string authorization);
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

    public async Task<dynamic> GetAllDiagnosisByAppointmentIDAsync(int appointmentID, string authorization)
    {
        var result = new HTTPResponseClient<List<DiagnosisDoctorVM>>();
        try
        {
            string token = authorization.Substring("Bearer ".Length);
            string fullName = _jwtAuthService.DecodePayloadToken(token);
            var user = await _unitOfWork._userRepository.SingleOrDefaultAsync(p => p.FullName == fullName
            && RoleConstant.Doctor == p.Role);

            if (user == null)
            {
                result.Message = "Thất bại";
                result.StatusCode = StatusCodes.Status401Unauthorized;
                result.Data = new List<DiagnosisDoctorVM>();
            }
            else
            {
                var doctor = await _unitOfWork._doctorRepository.SingleOrDefaultAsync(p => p.UserId == user.UserId);
                var list = await _unitOfWork._diagnosisRepository.GetAllDiagnosis_Appointment_DiagnosisService_Service_Room(
                    p =>
                    p.AppointmentId == appointmentID && doctor != null && p.Appointment != null && p.Appointment.DoctorId == doctor.DoctorId
                );
                var data = list.Select(p =>
                {
                    var clinicalService = p.DiagnosesServices.SingleOrDefault(p => p.Service.Type == TypeServiceConstant.Clinical);
                    return new DiagnosisDoctorVM()
                    {
                        DiagnosisId = p.DiagnosisId,
                        RoomName = clinicalService != null ? clinicalService.Room.RoomName : "",
                        CreatedAt = clinicalService != null ? clinicalService.CreatedAt : null,
                        ClinicalServiceName = clinicalService != null ? clinicalService.Service.ServiceName : string.Empty,
                        ClinicalServiceResultReport = clinicalService != null ? clinicalService.ServiceResultReport : string.Empty,
                        Symptoms = p.Symptoms,
                        Diagnosis1 = p.Diagnosis1
                    };
                }).ToList();

                result.Message = "Thành công";
                result.StatusCode = StatusCodes.Status200OK;
                result.Data = data;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            result.Message = "Thất bại";
            result.StatusCode = StatusCodes.Status500InternalServerError;
        }
        result.DateTime = DateTime.Now;
        return result;
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
            && RoleConstant.Doctor == p.Role);
            
            if (user == null)
            {
                result.Message = "Thất bại";
                result.StatusCode = StatusCodes.Status401Unauthorized;
                result.Data = false;
            }
            else
            {
                var doctor = await _unitOfWork._doctorRepository.SingleOrDefaultAsync(p => p.UserId == user.UserId);
                var appointment = await _unitOfWork._appointmentRepository.GetByIdAsync(item.AppointmentId);
                if (doctor == null || appointment == null || doctor.DoctorId != appointment.DoctorId)
                {
                    result.Message = "Thất bại";
                    result.StatusCode = StatusCodes.Status400BadRequest;
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
                        };
                        await _unitOfWork._diagnosisRepository.AddAsync(temp);
                        await _unitOfWork.SaveChangesAsync();

                        DiagnosesService clinicalService = new DiagnosesService()
                        {
                            DiagnosisId = temp.DiagnosisId,
                            ServiceId = item.ClinicalServiceId,
                            CreatedAt = DateTime.Now,
                            ServiceResultReport = item.ClinicalServiceServiceResultReport,
                            UserIdperformed = user.UserId,
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

                        // toa thuoc nua o day


                    }
                    else
                    {
                        // update
                        Diagnosis? temp = await _unitOfWork._diagnosisRepository.GetByIdAsync(item.DiagnosisId.Value);
                        temp!.AppointmentId = item.AppointmentId;
                        temp.Symptoms = item.Symptoms;
                        temp.Diagnosis1 = item.Diagnosis1;
                        _unitOfWork._diagnosisRepository.Update(temp);
                        await _unitOfWork.SaveChangesAsync();

                        var diagnosesServices = await _unitOfWork._diagnosisServiceRepository.GetAllDiagnosisServiceAndService(p => p.DiagnosisId == item.DiagnosisId.Value && p.Service.Type == TypeServiceConstant.Clinical);
                        DiagnosesService clinicalService = diagnosesServices.FirstOrDefault()!;

                        clinicalService.DiagnosisId = temp.DiagnosisId;
                        clinicalService.ServiceId = item.ClinicalServiceId;
                        clinicalService.CreatedAt = DateTime.Now;
                        clinicalService.ServiceResultReport = item.ClinicalServiceServiceResultReport;
                        clinicalService.UserIdperformed = user.UserId;
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
                        
                        // toa thuoc nua o day
                    }
                    result.Message = "Thành công";
                    result.StatusCode = StatusCodes.Status200OK;
                    result.Data = true;

                    await _unitOfWork.CommitTransaction();
                }
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