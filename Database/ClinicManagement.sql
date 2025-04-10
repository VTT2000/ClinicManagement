
--DROp DATABASE ClinicManagement;

use master
go
-- Tạo Database
CREATE DATABASE ClinicManagement;
GO

USE ClinicManagement;
GO

-- Bảng Người Dùng
CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    PasswordHash NVARCHAR(255) NOT NULL,
    Role NVARCHAR(50) CHECK (Role IN (N'Admin', N'Lễ tân', N'Bác sĩ', N'Bệnh nhân', N'Kỹ thuật viên')) NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE()
);

SET IDENTITY_INSERT Users ON; 
INSERT INTO Users (UserID, FullName, Email, PasswordHash, [Role], CreatedAt)
VALUES
	(1, N'Nguyễn Văn Admin', N'admin@gmail.com', '123456', N'Admin', GETDATE()),
	(2, N'Nguyễn Văn Lễ Tân', N'lt@gmail.com', '123456', N'Lễ tân', GETDATE()),
	(3, N'Nguyễn Văn Bác Sĩ A', N'bsa@gmail.com', '123456', N'Bác sĩ', GETDATE()),
	(4, N'Nguyễn Văn Bác Sĩ B', N'bsb@gmail.com', '123456', N'Bác sĩ', GETDATE()),
	(5, N'Nguyễn Văn Bệnh Nhân A', N'bna@gmail.com', '123456', N'Bệnh nhân', GETDATE()),
	(6, N'Nguyễn Văn Bệnh Nhân B', N'bnb@gmail.com', '123456', N'Bệnh nhân', GETDATE()),
	(7, N'Nguyễn Văn Kỹ Thuật Viên A', N'ktva@gmail.com', '123456',  N'Kỹ thuật viên', GETDATE()),
	(8, N'Nguyễn Văn Kỹ Thuật Viên B', N'ktvb@gmail.com', '123456',  N'Kỹ thuật viên', GETDATE()),
	(9, N'Nguyễn Văn Kỹ Thuật Viên C', N'ktvc@gmail.com', '123456',  N'Kỹ thuật viên', GETDATE());
SET IDENTITY_INSERT Users OFF; 

-- Bảng Bệnh Nhân
CREATE TABLE Patients (
    PatientID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT UNIQUE FOREIGN KEY REFERENCES Users(UserID),
    DOB DATE,
    Phone NVARCHAR(15),
    [Address] NVARCHAR(255)
);

SET IDENTITY_INSERT Patients ON; 
INSERT INTO Patients (PatientID, UserID, DOB, Phone, [Address])
VALUES
	(1, 5, '1990-05-20 08:30:00', '84987654321', N'123 Đường A, Xã A, Quận A, Tp.A'),
	(2, 6, '2000-01-01 12:00:00', '84123456789', N'456 Đường B, Xã A, Quận A, Tp.B');
SET IDENTITY_INSERT Patients OFF; 

-- Bảng Bác Sĩ
CREATE TABLE Doctors (
    DoctorID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT UNIQUE FOREIGN KEY REFERENCES Users(UserID),
    Specialization NVARCHAR(100)
);

SET IDENTITY_INSERT Doctors ON; 
INSERT INTO Doctors (DoctorID, UserID, Specialization)
VALUES
	(1, 3, N'Nội khoa'),
	(2, 4, N'Ngoại Khoa');
SET IDENTITY_INSERT Doctors OFF; 

-- Bảng Lịch Khám
CREATE TABLE Appointments (
    AppointmentID INT IDENTITY(1,1) PRIMARY KEY,
    PatientID INT FOREIGN KEY REFERENCES Patients(PatientID),
    DoctorID INT FOREIGN KEY REFERENCES Doctors(DoctorID),
    AppointmentDate DATETIME NOT NULL,
    [Status] NVARCHAR(50) CHECK (Status IN (N'Đã đặt', N'Đang chờ', N'Đã khám')) DEFAULT N'Đã đặt'
);

SET IDENTITY_INSERT Appointments ON; 
INSERT INTO Appointments (AppointmentID, PatientID, DoctorID, AppointmentDate, [Status])
VALUES
	(1, 1, 2, '2010-06-10 07:30:00', N'Đã khám'),
	(2, 2, 1, '2010-05-10 07:15:00', N'Đã khám');
SET IDENTITY_INSERT Appointments OFF; 

-- Bảng Dịch Vụ Xét Nghiệm
CREATE TABLE [Services] (
    ServiceID INT IDENTITY(1,1) PRIMARY KEY,
    ServiceName NVARCHAR(100) NOT NULL,
    Price DECIMAL(15,3) NOT NULL,
	ServiceParentID INT NULL FOREIGN KEY REFERENCES [Services](ServiceID),
);

SET IDENTITY_INSERT [Services] ON; 
INSERT INTO [Services] (ServiceID, ServiceName, Price)
VALUES
	(1, N'Khám bệnh', 100000),
	(2, N'Chụp X-quang toàn thân', 150000),
	(3, N'Xét nghiệm máu', 175000),
	(4, N'Xét nghiệm nước tiểu', 125000);
SET IDENTITY_INSERT [Services] OFF; 

-- Bảng Chẩn Đoán & Kê Toa
CREATE TABLE Diagnoses (
    DiagnosisID INT IDENTITY(1,1) PRIMARY KEY,
    AppointmentID INT FOREIGN KEY REFERENCES Appointments(AppointmentID),
    Symptoms NVARCHAR(255),
    Diagnosis NVARCHAR(255),
    Prescription NVARCHAR(MAX)
);

SET IDENTITY_INSERT Diagnoses ON; 
INSERT INTO Diagnoses (DiagnosisID, AppointmentID, Symptoms, Diagnosis, Prescription)
VALUES
	(1, 1, N'Nóng lạnh toàn thân', N'Sốt xuất huyết', N'Nhập viện theo dõi, truyền dịch y tế(500ml)'),
	(2, 2, N'Đau mỏi chân phải', N'Gãy chân', N'Nhập viện theo dõi, băng bó nẹp y tế(20cmx3cm)');
SET IDENTITY_INSERT Diagnoses OFF; 

-- Bảng Thuốc
CREATE TABLE Medicines (
    MedicineID INT IDENTITY(1,1) PRIMARY KEY,
    MedicineName NVARCHAR(100) NOT NULL,
    Unit NVARCHAR(50),
    Price DECIMAL(15,3) NOT NULL,
    StockQuantity INT NOT NULL
);

SET IDENTITY_INSERT Medicines ON; 
INSERT INTO Medicines (MedicineID, MedicineName, Unit, Price, StockQuantity)
VALUES
	(1, N'Thuốc hạ sốt PARATOL', N'Thuốc', 150000, 879),
	(2, N'Dịch y tế(500ml)', N'Dịch', 115000, 10089),
	(3, N'Nẹp y tế(20cmx3cm)', N'Nẹp', 80000, 879);
SET IDENTITY_INSERT Medicines OFF;

-- Bảng Đơn Thuốc
CREATE TABLE Prescriptions (
    PrescriptionID INT IDENTITY(1,1) PRIMARY KEY,
    DiagnosisID INT FOREIGN KEY REFERENCES Diagnoses(DiagnosisID),
    MedicineID INT FOREIGN KEY REFERENCES Medicines(MedicineID),
    Quantity INT NOT NULL
);

SET IDENTITY_INSERT Prescriptions ON; 
INSERT INTO Prescriptions (PrescriptionID, DiagnosisID, MedicineID, Quantity)
VALUES
	(1, 1, 1, 2),
	(2, 1, 2, 6),
	(3, 2, 3, 4);
SET IDENTITY_INSERT Prescriptions OFF;

CREATE TABLE Room(
	RoomID INT IDENTITY(1,1) PRIMARY KEY,
	RoomName NVARCHAR(50) NOT NULL
)

SET IDENTITY_INSERT Room ON; 
INSERT INTO Room (RoomID, RoomName)
VALUES
	(1, N'Phòng khám 1'),
	(2, N'Phòng khám 2'),
	(3, N'Phòng xét nghiệm máu 1'),
	(4, N'Phòng xét nghiệm nước tiểu 1'),
	(5, N'Phòng chụp X-quang 1');
SET IDENTITY_INSERT Room OFF;

CREATE TABLE Diagnoses_Services(
	DiagnosisID int NOT NULL FOREIGN KEY REFERENCES Diagnoses(DiagnosisID),
	ServiceID int NOT NULL FOREIGN KEY REFERENCES Services(ServiceID),
	CreatedAt DATETIME DEFAULT GETDATE(),
	ServiceResultReport NVARCHAR(max) NOT NULL,
	UserIDperformed INT NOT NULL FOREIGN KEY REFERENCES Users(UserID),
	RoomID INT NOT NULL FOREIGN KEY REFERENCES Room(RoomID),
)

INSERT INTO Diagnoses_Services (DiagnosisID, ServiceID, CreatedAt, ServiceResultReport, UserIDperformed, RoomID)
VALUES
	(1, 1, '2010-06-10 07:35:00', N'Nhịp tim: 200 lần / phút, Huyết áp: 85', 6, 1),
	(1, 3, '2010-06-10 07:45:00', N'Hông cầu 80%, Bạch cầu 0,05%, ... -> Lượng Bạch cầu trong máu thấp', 7, 3),
	(1, 4, '2010-06-10 08:45:00', N'Chỉ số SR trong nước tiểu thấp', 8, 4),
	(2, 1, '2010-05-10 07:20:00', N'Chân sưng có vết đỏ thẩm', 5, 2),
	(2, 2, '2010-05-10 07:30:00', N'Xương chân gãy', 9, 5);

-- Bảng Hóa Đơn
CREATE TABLE Bills (
    BillID INT IDENTITY(1,1) PRIMARY KEY,
    AppointmentID INT FOREIGN KEY REFERENCES Appointments(AppointmentID),
    TotalAmount DECIMAL(15,3) NOT NULL,
    PaymentStatus NVARCHAR(50) CHECK (PaymentStatus IN (N'Chưa thanh toán', N'Đã thanh toán')) DEFAULT N'Chưa thanh toán',
    CreatedAt DATETIME DEFAULT GETDATE()
);

SET IDENTITY_INSERT Bills ON; 
INSERT INTO Bills (BillID, AppointmentID, TotalAmount, PaymentStatus, CreatedAt)
VALUES
	(1, 1, 1390000, N'Đã thanh toán', '2010-06-10 10:00:00'),
	(2, 2, 570000, N'Đã thanh toán', '2010-05-10 08:00:00');
SET IDENTITY_INSERT Bills OFF;

-- Bảng Lịch làm việc
CREATE TABLE WorkSchedules (
	WorkScheduleID INT IDENTITY(1,1) PRIMARY KEY,
	StartDate datetime,
	EndDate datetime,
	[DayOfWeek] nvarchar(max)
);

SET IDENTITY_INSERT WorkSchedules ON; 
INSERT INTO WorkSchedules (WorkScheduleID, StartDate, EndDate, [DayOfWeek])
VALUES
	(1, '2010-06-10 10:00:00', '2030-06-10 10:00:00', N'Thứ Hai, Thứ Tư, Thứ Sáu, Chủ Nhật'),
	(2, '2010-06-10 10:00:00', '2030-06-10 10:00:00', N'Thứ Ba, Thứ Năm, Thứ Bảy, Chủ Nhật');
SET IDENTITY_INSERT WorkSchedules OFF;

-- Bảng Lịch làm việc bác sĩ
CREATE TABLE WorkSchedule_Doctor (
	WorkScheduleID INT FOREIGN KEY REFERENCES WorkSchedules(WorkScheduleID),
	DoctorID INT FOREIGN KEY REFERENCES Doctors(DoctorID),
	CONSTRAINT PK_WorkSchedule_Doctor PRIMARY KEY(WorkScheduleID, DoctorID)
);

INSERT INTO WorkSchedule_Doctor (WorkScheduleID, DoctorID)
VALUES
	(1, 1),
	(2, 2);


--SELECT * FROM Users
--SELECT * FROM Patients
--SELECT * FROM Doctors
--SELECT * FROM Appointments
--SELECT * FROM [Services]
--SELECT * FROM Diagnoses
--SELECT * FROM Medicines
--SELECT * FROM Prescriptions
--SELECT * FROM Diagnoses_Services
--SELECT * FROM Bills
--SELECT * FROM Room
--SELECT * FROM WorkSchedules
--SELECT * FROM WorkSchedule_Doctor