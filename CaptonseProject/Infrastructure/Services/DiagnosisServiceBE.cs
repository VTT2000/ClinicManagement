using web_api_base.Models.ClinicManagement;

public interface IDiagnosisServiceBE
{
    public Task<dynamic> GetDiagnosisDoctorByIDAsync(int diagnosisID, string authorization);
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

    public async Task<dynamic> GetDiagnosisDoctorByIDAsync(int diagnosisID, string authorization){
        var result = new HTTPResponseClient<DetailSaveDiagnosisDoctorVM>();
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
                result.Data = new DetailSaveDiagnosisDoctorVM();
            }
            else
            {
                var doctor = await _unitOfWork._doctorRepository.SingleOrDefaultAsync(p => p.UserId == user.UserId);
                // cho nay
                var found = await _unitOfWork._diagnosisRepository.GetDiagnosis_Appointment_DiagnosisService_Service_Prescription_PrescriptionDetail(
                    p =>
                    p.DiagnosisId == diagnosisID && doctor != null && p.Appointment != null && p.Appointment.DoctorId == doctor.DoctorId
                );
                if (found == null)
                {
                    result.Message = "Thất bại";
                    result.StatusCode = StatusCodes.Status400BadRequest;
                    result.Data = new DetailSaveDiagnosisDoctorVM();
                }
                else
                {
                    var data2 = found.DiagnosesServices.SingleOrDefault(p => p.Service.Type == TypeServiceConstant.Clinical);
                    var data3 = found.DiagnosesServices.Where(p => p.Service.Type == TypeServiceConstant.Paraclinical).Select(p=>p.ServiceId).ToList();
                    var data4 = found.Prescriptions.Select(p => new PrescriptionDiagnosisDoctorVM()
                    {
                        PrescriptionId = p.PrescriptionId,
                        DiagnosisId = p.DiagnosisId,
                        Prescription1 = p.Prescription1,
                        PrescriptionDetails = p.PrescriptionDetails.Select(q => new PrescriptionDetailDiagnosisDoctorVM()
                        {
                            PrescriptionDetailId = q.PrescriptionDetailId,
                            PrescriptionId = q.PrescriptionId,
                            MedicineId = q.MedicineId,
                            Quantity = q.Quantity
                        }).ToList()
                    }).ToList();

                    var data = new DetailSaveDiagnosisDoctorVM()
                    {
                        DiagnosisId = found.DiagnosisId,
                        AppointmentId = found.AppointmentId??0,
                        Symptoms = found.Symptoms,
                        Diagnosis1 = found.Diagnosis1, 
                        ClinicalServiceId = data2.ServiceId, 
                        ClinicalServiceCreatedAt = data2.CreatedAt,
                        ClinicalServiceServiceResultReport = data2.ServiceResultReport,
                        ClinicalServiceRoomId = data2.RoomId,
                        ParaclinicalServiceList = data3,
                        Prescriptions = data4
                    };
                    
                    result.Message = "Thành công";
                    result.StatusCode = StatusCodes.Status200OK;
                    result.Data = data;
                }
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

                        foreach (var i in item.ParaclinicalServiceList)
                        {
                            await _unitOfWork._diagnosisServiceRepository.AddAsync(new DiagnosesService()
                            {
                                DiagnosisId = temp.DiagnosisId,
                                ServiceId = i
                            });
                        }
                        await _unitOfWork.SaveChangesAsync();

                        foreach (var i in item.Prescriptions)
                        {
                            Prescription save1 = new Prescription()
                            {
                                DiagnosisId = i.DiagnosisId,
                                Prescription1 = i.Prescription1
                            };
                            await _unitOfWork._prescriptionRepository.AddAsync(save1);
                            await _unitOfWork.SaveChangesAsync();
                            foreach (var j in i.PrescriptionDetails)
                            {
                                PrescriptionDetail save2 = new PrescriptionDetail()
                                {
                                    PrescriptionId = save1.PrescriptionId,
                                    MedicineId = j.MedicineId,
                                    Quantity = j.Quantity
                                };
                                await _unitOfWork._prescriptionDetailRepository.AddAsync(save2);
                            }
                            await _unitOfWork.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        var found = await _unitOfWork._diagnosisRepository.GetDiagnosis_Appointment_DiagnosisService_Service_Prescription_PrescriptionDetail(
                            p =>
                            p.DiagnosisId == item.DiagnosisId && doctor != null && p.Appointment != null && p.Appointment.DoctorId == doctor.DoctorId
                        );

                        // update
                        Diagnosis? temp = await _unitOfWork._diagnosisRepository.GetByIdAsync(item.DiagnosisId.Value);
                        temp!.AppointmentId = item.AppointmentId;
                        temp.Symptoms = item.Symptoms;
                        temp.Diagnosis1 = item.Diagnosis1;
                        _unitOfWork._diagnosisRepository.Update(temp);
                        await _unitOfWork.SaveChangesAsync();

                        var diagnosesServices = found.DiagnosesServices.Where(p => p.DiagnosisId == item.DiagnosisId.Value && p.Service.Type == TypeServiceConstant.Clinical);
                        DiagnosesService clinicalService = diagnosesServices.FirstOrDefault()!;
                        clinicalService.DiagnosisId = temp.DiagnosisId;
                        clinicalService.ServiceId = item.ClinicalServiceId;
                        clinicalService.CreatedAt = DateTime.Now;
                        clinicalService.ServiceResultReport = item.ClinicalServiceServiceResultReport;
                        clinicalService.UserIdperformed = user.UserId;
                        clinicalService.RoomId = item.ClinicalServiceRoomId;
                        _unitOfWork._diagnosisServiceRepository.Update(clinicalService);

                        var diagnosesServices2 = found.DiagnosesServices.Where(p => p.DiagnosisId == item.DiagnosisId.Value && p.Service.Type == TypeServiceConstant.Paraclinical);
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
                            }
                        }
                        foreach (var i in item.ParaclinicalServiceList)
                        {
                            await _unitOfWork._diagnosisServiceRepository.AddAsync(new DiagnosesService()
                            {
                                DiagnosisId = temp.DiagnosisId,
                                ServiceId = i
                            });
                        }
                        await _unitOfWork.SaveChangesAsync();

                        // toa thuoc nua o day
                        var prescription = found.Prescriptions;
                        foreach (var i in prescription)
                        {
                            var foundInItem = item.Prescriptions.SingleOrDefault(p => p.PrescriptionId == i.PrescriptionId);
                            if (foundInItem != null)
                            {
                                // update
                                i.DiagnosisId = foundInItem.DiagnosisId;
                                i.Prescription1 = foundInItem.Prescription1;
                                _unitOfWork._prescriptionRepository.Update(i);
                                await _unitOfWork.SaveChangesAsync();

                                foreach (var j in i.PrescriptionDetails)
                                {
                                    var foundInItem2 = foundInItem.PrescriptionDetails.SingleOrDefault(p => p.PrescriptionDetailId == j.PrescriptionDetailId);
                                    if (foundInItem2 != null)
                                    {
                                        // update
                                        j.PrescriptionId = foundInItem2.PrescriptionId;
                                        j.MedicineId = foundInItem2.MedicineId;
                                        j.Quantity = foundInItem2.Quantity;
                                        _unitOfWork._prescriptionDetailRepository.Update(j);
                                        foundInItem.PrescriptionDetails.Remove(foundInItem2);
                                    }
                                    else
                                    {
                                        // remove
                                        await _unitOfWork._prescriptionDetailRepository.DeleteAsync(j.PrescriptionDetailId);
                                    }
                                }
                                // add
                                foreach (var z in foundInItem.PrescriptionDetails)
                                {
                                    await _unitOfWork._prescriptionDetailRepository.AddAsync(new PrescriptionDetail()
                                    {
                                        PrescriptionId = foundInItem.PrescriptionId,
                                        MedicineId = z.MedicineId,
                                        Quantity = z.Quantity
                                    });
                                }
                                await _unitOfWork.SaveChangesAsync();

                                item.Prescriptions.Remove(foundInItem);
                            }
                            else
                            {
                                // remove
                                var deletedPrescriptionDetails = i.PrescriptionDetails;
                                foreach (var j in deletedPrescriptionDetails)
                                {
                                    await _unitOfWork._prescriptionDetailRepository.DeleteAsync(j.PrescriptionDetailId);
                                }
                                await _unitOfWork.SaveChangesAsync();

                                await _unitOfWork._prescriptionRepository.DeleteAsync(i.PrescriptionId);
                                await _unitOfWork.SaveChangesAsync();
                            }
                        }
                        // add
                        foreach (var i in item.Prescriptions)
                        {
                            var add1 = new Prescription()
                            {
                                DiagnosisId = i.DiagnosisId,
                                Prescription1 = i.Prescription1
                            };
                            await _unitOfWork._prescriptionRepository.AddAsync(add1);
                            await _unitOfWork.SaveChangesAsync();
                            foreach (var j in i.PrescriptionDetails)
                            {
                                var add2 = new PrescriptionDetail()
                                {
                                    PrescriptionId = add1.PrescriptionId,
                                    MedicineId = j.MedicineId,
                                    Quantity = j.Quantity
                                };
                                await _unitOfWork._prescriptionDetailRepository.AddAsync(add2);
                                await _unitOfWork.SaveChangesAsync();
                            }
                        }
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