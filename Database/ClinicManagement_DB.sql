USE [ClinicManagement]
GO
/****** Object:  Table [dbo].[Appointments]    Script Date: 14-Jul-25 01:25:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Appointments](
	[AppointmentID] [int] IDENTITY(1,1) NOT NULL,
	[PatientID] [int] NULL,
	[DoctorID] [int] NULL,
	[Status] [nvarchar](50) NULL,
	[AppointmentDate] [date] NULL,
	[AppointmentTime] [time](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[AppointmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Bills]    Script Date: 14-Jul-25 01:25:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Bills](
	[BillID] [int] IDENTITY(1,1) NOT NULL,
	[AppointmentID] [int] NULL,
	[TotalAmount] [decimal](15, 3) NOT NULL,
	[PaymentStatus] [nvarchar](50) NULL,
	[CreatedAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[BillID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Diagnoses]    Script Date: 14-Jul-25 01:25:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Diagnoses](
	[DiagnosisID] [int] IDENTITY(1,1) NOT NULL,
	[AppointmentID] [int] NULL,
	[Symptoms] [nvarchar](255) NULL,
	[Diagnosis] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[DiagnosisID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Diagnoses_Services]    Script Date: 14-Jul-25 01:25:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Diagnoses_Services](
	[DiagnosesServiceID] [int] IDENTITY(1,1) NOT NULL,
	[DiagnosisID] [int] NOT NULL,
	[ServiceID] [int] NOT NULL,
	[CreatedAt] [datetime] NULL,
	[ServiceResultReport] [nvarchar](max) NULL,
	[UserIDperformed] [int] NULL,
	[RoomID] [int] NULL,
 CONSTRAINT [PK_Diagnoses_Services] PRIMARY KEY CLUSTERED 
(
	[DiagnosesServiceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Doctors]    Script Date: 14-Jul-25 01:25:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Doctors](
	[DoctorID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NULL,
	[Specialization] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[DoctorID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Medicines]    Script Date: 14-Jul-25 01:25:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Medicines](
	[MedicineID] [int] IDENTITY(1,1) NOT NULL,
	[MedicineName] [nvarchar](100) NOT NULL,
	[Unit] [nvarchar](50) NULL,
	[Price] [decimal](15, 3) NOT NULL,
	[StockQuantity] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[MedicineID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Patients]    Script Date: 14-Jul-25 01:25:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Patients](
	[PatientID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NULL,
	[DOB] [date] NULL,
	[Phone] [nvarchar](15) NULL,
	[Address] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[PatientID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PrescriptionDetails]    Script Date: 14-Jul-25 01:25:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PrescriptionDetails](
	[PrescriptionDetailID] [int] IDENTITY(1,1) NOT NULL,
	[PrescriptionID] [int] NULL,
	[MedicineID] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
 CONSTRAINT [PK_PrescriptionDetails] PRIMARY KEY CLUSTERED 
(
	[PrescriptionDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Prescriptions]    Script Date: 14-Jul-25 01:25:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Prescriptions](
	[PrescriptionID] [int] IDENTITY(1,1) NOT NULL,
	[DiagnosisID] [int] NULL,
	[Prescription] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[PrescriptionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Room]    Script Date: 14-Jul-25 01:25:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Room](
	[RoomID] [int] IDENTITY(1,1) NOT NULL,
	[RoomName] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RoomID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Services]    Script Date: 14-Jul-25 01:25:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Services](
	[ServiceID] [int] IDENTITY(1,1) NOT NULL,
	[ServiceName] [nvarchar](100) NOT NULL,
	[Price] [decimal](15, 3) NOT NULL,
	[ServiceParentID] [int] NULL,
	[Type] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[ServiceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 14-Jul-25 01:25:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	[PasswordHash] [nvarchar](255) NOT NULL,
	[Role] [nvarchar](50) NOT NULL,
	[CreatedAt] [datetime] NULL,
	[ImageUrl] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WorkSchedules]    Script Date: 14-Jul-25 01:25:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WorkSchedules](
	[WorkScheduleID] [int] IDENTITY(1,1) NOT NULL,
	[StartTime] [time](7) NULL,
	[EndTime] [time](7) NULL,
	[StartDate] [date] NULL,
	[EndDate] [date] NULL,
	[DoctorID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[WorkScheduleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Appointments] ON 

INSERT [dbo].[Appointments] ([AppointmentID], [PatientID], [DoctorID], [Status], [AppointmentDate], [AppointmentTime]) VALUES (1, 1, 2, N'Đã khám', CAST(N'2010-06-10' AS Date), CAST(N'07:30:00' AS Time))
INSERT [dbo].[Appointments] ([AppointmentID], [PatientID], [DoctorID], [Status], [AppointmentDate], [AppointmentTime]) VALUES (2, 2, 1, N'Đã khám', CAST(N'2010-05-10' AS Date), CAST(N'07:15:00' AS Time))
INSERT [dbo].[Appointments] ([AppointmentID], [PatientID], [DoctorID], [Status], [AppointmentDate], [AppointmentTime]) VALUES (5, 9, 1, N'Đã khám', CAST(N'2025-05-01' AS Date), CAST(N'07:00:00' AS Time))
INSERT [dbo].[Appointments] ([AppointmentID], [PatientID], [DoctorID], [Status], [AppointmentDate], [AppointmentTime]) VALUES (6, 2, 1, N'Đã đặt', CAST(N'2025-05-01' AS Date), CAST(N'07:10:00' AS Time))
INSERT [dbo].[Appointments] ([AppointmentID], [PatientID], [DoctorID], [Status], [AppointmentDate], [AppointmentTime]) VALUES (7, 1, 1, N'Đang khám', CAST(N'2025-06-01' AS Date), CAST(N'07:00:00' AS Time))
INSERT [dbo].[Appointments] ([AppointmentID], [PatientID], [DoctorID], [Status], [AppointmentDate], [AppointmentTime]) VALUES (8, 2, 1, N'Qua lượt', CAST(N'2025-06-01' AS Date), CAST(N'07:10:00' AS Time))
INSERT [dbo].[Appointments] ([AppointmentID], [PatientID], [DoctorID], [Status], [AppointmentDate], [AppointmentTime]) VALUES (9, 9, 1, N'Đang chờ', CAST(N'2025-06-01' AS Date), CAST(N'07:30:00' AS Time))
INSERT [dbo].[Appointments] ([AppointmentID], [PatientID], [DoctorID], [Status], [AppointmentDate], [AppointmentTime]) VALUES (10, 9, 1, N'Đã khám', CAST(N'2025-06-07' AS Date), CAST(N'07:00:00' AS Time))
INSERT [dbo].[Appointments] ([AppointmentID], [PatientID], [DoctorID], [Status], [AppointmentDate], [AppointmentTime]) VALUES (11, 11, 1, N'Đã khám', CAST(N'2025-06-07' AS Date), CAST(N'13:00:00' AS Time))
INSERT [dbo].[Appointments] ([AppointmentID], [PatientID], [DoctorID], [Status], [AppointmentDate], [AppointmentTime]) VALUES (12, 1, 1, N'Đang khám', CAST(N'2025-06-07' AS Date), CAST(N'13:10:00' AS Time))
INSERT [dbo].[Appointments] ([AppointmentID], [PatientID], [DoctorID], [Status], [AppointmentDate], [AppointmentTime]) VALUES (13, 19, 1, N'Đang chờ', CAST(N'2025-07-10' AS Date), CAST(N'07:00:00' AS Time))
INSERT [dbo].[Appointments] ([AppointmentID], [PatientID], [DoctorID], [Status], [AppointmentDate], [AppointmentTime]) VALUES (14, 1, 1, N'Đang chờ', CAST(N'2025-07-10' AS Date), CAST(N'07:10:00' AS Time))
INSERT [dbo].[Appointments] ([AppointmentID], [PatientID], [DoctorID], [Status], [AppointmentDate], [AppointmentTime]) VALUES (15, 2, 1, N'Đang chờ', CAST(N'2025-07-10' AS Date), CAST(N'07:30:00' AS Time))
INSERT [dbo].[Appointments] ([AppointmentID], [PatientID], [DoctorID], [Status], [AppointmentDate], [AppointmentTime]) VALUES (16, 11, 1, N'Đang chờ', CAST(N'2025-07-10' AS Date), CAST(N'07:20:00' AS Time))
INSERT [dbo].[Appointments] ([AppointmentID], [PatientID], [DoctorID], [Status], [AppointmentDate], [AppointmentTime]) VALUES (17, 12, 1, N'Đang chờ', CAST(N'2025-07-10' AS Date), CAST(N'07:40:00' AS Time))
INSERT [dbo].[Appointments] ([AppointmentID], [PatientID], [DoctorID], [Status], [AppointmentDate], [AppointmentTime]) VALUES (18, 13, 1, N'Đang chờ', CAST(N'2025-07-12' AS Date), CAST(N'13:00:00' AS Time))
INSERT [dbo].[Appointments] ([AppointmentID], [PatientID], [DoctorID], [Status], [AppointmentDate], [AppointmentTime]) VALUES (19, 14, 1, N'Đang chờ', CAST(N'2025-07-12' AS Date), CAST(N'13:10:00' AS Time))
INSERT [dbo].[Appointments] ([AppointmentID], [PatientID], [DoctorID], [Status], [AppointmentDate], [AppointmentTime]) VALUES (20, 15, 1, N'Đang chờ', CAST(N'2025-07-12' AS Date), CAST(N'13:20:00' AS Time))
INSERT [dbo].[Appointments] ([AppointmentID], [PatientID], [DoctorID], [Status], [AppointmentDate], [AppointmentTime]) VALUES (21, 16, 1, N'Đang chờ', CAST(N'2025-07-12' AS Date), CAST(N'13:30:00' AS Time))
INSERT [dbo].[Appointments] ([AppointmentID], [PatientID], [DoctorID], [Status], [AppointmentDate], [AppointmentTime]) VALUES (22, 17, 1, N'Qua lượt', CAST(N'2025-07-12' AS Date), CAST(N'14:00:00' AS Time))
INSERT [dbo].[Appointments] ([AppointmentID], [PatientID], [DoctorID], [Status], [AppointmentDate], [AppointmentTime]) VALUES (23, 18, 1, N'Đang chờ', CAST(N'2025-07-10' AS Date), CAST(N'07:50:00' AS Time))
INSERT [dbo].[Appointments] ([AppointmentID], [PatientID], [DoctorID], [Status], [AppointmentDate], [AppointmentTime]) VALUES (24, 1, 1, N'Đang khám', CAST(N'2025-07-12' AS Date), CAST(N'13:40:00' AS Time))
SET IDENTITY_INSERT [dbo].[Appointments] OFF
GO
SET IDENTITY_INSERT [dbo].[Bills] ON 

INSERT [dbo].[Bills] ([BillID], [AppointmentID], [TotalAmount], [PaymentStatus], [CreatedAt]) VALUES (1, 1, CAST(1390000.000 AS Decimal(15, 3)), N'Đã thanh toán', CAST(N'2010-06-10T10:00:00.000' AS DateTime))
INSERT [dbo].[Bills] ([BillID], [AppointmentID], [TotalAmount], [PaymentStatus], [CreatedAt]) VALUES (2, 2, CAST(570000.000 AS Decimal(15, 3)), N'Đã thanh toán', CAST(N'2010-05-10T08:00:00.000' AS DateTime))
SET IDENTITY_INSERT [dbo].[Bills] OFF
GO
SET IDENTITY_INSERT [dbo].[Diagnoses] ON 

INSERT [dbo].[Diagnoses] ([DiagnosisID], [AppointmentID], [Symptoms], [Diagnosis]) VALUES (1, 1, N'Nóng lạnh toàn thân', N'Sốt xuất huyết')
INSERT [dbo].[Diagnoses] ([DiagnosisID], [AppointmentID], [Symptoms], [Diagnosis]) VALUES (2, 2, N'Đau mỏi chân phải', N'Gãy chân')
INSERT [dbo].[Diagnoses] ([DiagnosisID], [AppointmentID], [Symptoms], [Diagnosis]) VALUES (8, 7, N'ho, cảm', N'Cúm N1T7')
INSERT [dbo].[Diagnoses] ([DiagnosisID], [AppointmentID], [Symptoms], [Diagnosis]) VALUES (16, 5, N'Không có triệu chứng bất thường', N'Bình thường')
INSERT [dbo].[Diagnoses] ([DiagnosisID], [AppointmentID], [Symptoms], [Diagnosis]) VALUES (17, 10, N'ho, dau dau', N'Benh T7N1')
INSERT [dbo].[Diagnoses] ([DiagnosisID], [AppointmentID], [Symptoms], [Diagnosis]) VALUES (18, 11, N'ho, cảm', N'Bệnh T1A6')
INSERT [dbo].[Diagnoses] ([DiagnosisID], [AppointmentID], [Symptoms], [Diagnosis]) VALUES (21, 24, N'hơi thở yếu', N'Bệnh cảm N1')
SET IDENTITY_INSERT [dbo].[Diagnoses] OFF
GO
SET IDENTITY_INSERT [dbo].[Diagnoses_Services] ON 

INSERT [dbo].[Diagnoses_Services] ([DiagnosesServiceID], [DiagnosisID], [ServiceID], [CreatedAt], [ServiceResultReport], [UserIDperformed], [RoomID]) VALUES (1, 1, 1, CAST(N'2010-06-10T07:35:00.000' AS DateTime), N'Nhịp tim: 200 lần / phút, Huyết áp: 85', 6, 1)
INSERT [dbo].[Diagnoses_Services] ([DiagnosesServiceID], [DiagnosisID], [ServiceID], [CreatedAt], [ServiceResultReport], [UserIDperformed], [RoomID]) VALUES (2, 1, 3, CAST(N'2010-06-10T07:45:00.000' AS DateTime), N'Hông cầu 80%, Bạch cầu 0,05%, ... -> Lượng Bạch cầu trong máu thấp', 7, 3)
INSERT [dbo].[Diagnoses_Services] ([DiagnosesServiceID], [DiagnosisID], [ServiceID], [CreatedAt], [ServiceResultReport], [UserIDperformed], [RoomID]) VALUES (3, 1, 4, CAST(N'2010-06-10T08:45:00.000' AS DateTime), N'Chỉ số SR trong nước tiểu thấp', 8, 4)
INSERT [dbo].[Diagnoses_Services] ([DiagnosesServiceID], [DiagnosisID], [ServiceID], [CreatedAt], [ServiceResultReport], [UserIDperformed], [RoomID]) VALUES (4, 2, 1, CAST(N'2010-05-10T07:20:00.000' AS DateTime), N'Chân sưng có vết đỏ thẩm', 5, 2)
INSERT [dbo].[Diagnoses_Services] ([DiagnosesServiceID], [DiagnosisID], [ServiceID], [CreatedAt], [ServiceResultReport], [UserIDperformed], [RoomID]) VALUES (5, 2, 2, CAST(N'2010-05-10T07:30:00.000' AS DateTime), N'Xương chân gãy', 9, 5)
INSERT [dbo].[Diagnoses_Services] ([DiagnosesServiceID], [DiagnosisID], [ServiceID], [CreatedAt], [ServiceResultReport], [UserIDperformed], [RoomID]) VALUES (7, 8, 1, CAST(N'2025-06-01T15:49:14.430' AS DateTime), N'thân nhiệt và huyết áp thấp', 3, 1)
INSERT [dbo].[Diagnoses_Services] ([DiagnosesServiceID], [DiagnosisID], [ServiceID], [CreatedAt], [ServiceResultReport], [UserIDperformed], [RoomID]) VALUES (23, 16, 1, CAST(N'2025-06-02T12:06:33.233' AS DateTime), N'huyết áp và nhịp tim bình thường', 3, 1)
INSERT [dbo].[Diagnoses_Services] ([DiagnosesServiceID], [DiagnosisID], [ServiceID], [CreatedAt], [ServiceResultReport], [UserIDperformed], [RoomID]) VALUES (26, 17, 1, CAST(N'2025-06-06T23:20:12.957' AS DateTime), N'than nhiet bat thuong, nhip tim cao', 3, 1)
INSERT [dbo].[Diagnoses_Services] ([DiagnosesServiceID], [DiagnosisID], [ServiceID], [CreatedAt], [ServiceResultReport], [UserIDperformed], [RoomID]) VALUES (27, 17, 3, CAST(N'2025-06-06T23:30:19.177' AS DateTime), N'Huyet ap thap', 7, 3)
INSERT [dbo].[Diagnoses_Services] ([DiagnosesServiceID], [DiagnosisID], [ServiceID], [CreatedAt], [ServiceResultReport], [UserIDperformed], [RoomID]) VALUES (29, 18, 1, CAST(N'2025-06-07T11:15:55.380' AS DateTime), N'huyết áp thấp, thân nhiệt thấp', 3, 1)
INSERT [dbo].[Diagnoses_Services] ([DiagnosesServiceID], [DiagnosisID], [ServiceID], [CreatedAt], [ServiceResultReport], [UserIDperformed], [RoomID]) VALUES (30, 18, 3, CAST(N'2025-06-07T11:18:28.050' AS DateTime), N'Lượng bạch cầu cao', 7, 3)
INSERT [dbo].[Diagnoses_Services] ([DiagnosesServiceID], [DiagnosisID], [ServiceID], [CreatedAt], [ServiceResultReport], [UserIDperformed], [RoomID]) VALUES (31, 18, 4, CAST(N'2025-06-07T11:19:05.227' AS DateTime), N'lượng ion trong nước tiểu thấp', 7, 4)
INSERT [dbo].[Diagnoses_Services] ([DiagnosesServiceID], [DiagnosisID], [ServiceID], [CreatedAt], [ServiceResultReport], [UserIDperformed], [RoomID]) VALUES (36, 21, 1, CAST(N'2025-07-12T09:48:13.100' AS DateTime), N'nhịp tim thấp, huyét áp thấp', 3, 1)
INSERT [dbo].[Diagnoses_Services] ([DiagnosesServiceID], [DiagnosisID], [ServiceID], [CreatedAt], [ServiceResultReport], [UserIDperformed], [RoomID]) VALUES (37, 21, 3, CAST(N'2025-07-12T09:50:35.397' AS DateTime), N'nong độ N trong hoi thỏ 3%', 7, 3)
INSERT [dbo].[Diagnoses_Services] ([DiagnosesServiceID], [DiagnosisID], [ServiceID], [CreatedAt], [ServiceResultReport], [UserIDperformed], [RoomID]) VALUES (38, 21, 4, NULL, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Diagnoses_Services] OFF
GO
SET IDENTITY_INSERT [dbo].[Doctors] ON 

INSERT [dbo].[Doctors] ([DoctorID], [UserID], [Specialization]) VALUES (1, 3, N'Nội khoa')
INSERT [dbo].[Doctors] ([DoctorID], [UserID], [Specialization]) VALUES (2, 4, N'Ngoại Khoa')
INSERT [dbo].[Doctors] ([DoctorID], [UserID], [Specialization]) VALUES (8, 32, NULL)
SET IDENTITY_INSERT [dbo].[Doctors] OFF
GO
SET IDENTITY_INSERT [dbo].[Medicines] ON 

INSERT [dbo].[Medicines] ([MedicineID], [MedicineName], [Unit], [Price], [StockQuantity]) VALUES (1, N'Thuốc hạ sốt PARATOL', N'Thuốc', CAST(150000.000 AS Decimal(15, 3)), 879)
INSERT [dbo].[Medicines] ([MedicineID], [MedicineName], [Unit], [Price], [StockQuantity]) VALUES (2, N'Dịch y tế(500ml)', N'Dịch', CAST(115000.000 AS Decimal(15, 3)), 10089)
INSERT [dbo].[Medicines] ([MedicineID], [MedicineName], [Unit], [Price], [StockQuantity]) VALUES (3, N'Nẹp y tế(20cmx3cm)', N'Nẹp', CAST(80000.000 AS Decimal(15, 3)), 879)
SET IDENTITY_INSERT [dbo].[Medicines] OFF
GO
SET IDENTITY_INSERT [dbo].[Patients] ON 

INSERT [dbo].[Patients] ([PatientID], [UserID], [DOB], [Phone], [Address]) VALUES (1, 5, CAST(N'1990-05-20' AS Date), N'84987654321', N'123 Đường A, Xã A, Quận A, Tp.A')
INSERT [dbo].[Patients] ([PatientID], [UserID], [DOB], [Phone], [Address]) VALUES (2, 6, CAST(N'2000-01-01' AS Date), N'84123456789', N'456 Đường B, Xã A, Quận A, Tp.B')
INSERT [dbo].[Patients] ([PatientID], [UserID], [DOB], [Phone], [Address]) VALUES (9, 22, CAST(N'1990-05-20' AS Date), N'84987654325', N'789 Đường C, Xã M, Quận E, Tp.K')
INSERT [dbo].[Patients] ([PatientID], [UserID], [DOB], [Phone], [Address]) VALUES (11, 34, CAST(N'2000-06-20' AS Date), N'1234567898', N'123 dien a, in city a')
INSERT [dbo].[Patients] ([PatientID], [UserID], [DOB], [Phone], [Address]) VALUES (12, 38, CAST(N'2000-07-20' AS Date), N'84258954763', N'123 Diem D, in City X')
INSERT [dbo].[Patients] ([PatientID], [UserID], [DOB], [Phone], [Address]) VALUES (13, 40, CAST(N'1999-09-03' AS Date), N'123582695', N'112 Điểm A, Khu X')
INSERT [dbo].[Patients] ([PatientID], [UserID], [DOB], [Phone], [Address]) VALUES (14, 42, CAST(N'2001-02-25' AS Date), N'123987698', N'112 Điểm D, Khu Z')
INSERT [dbo].[Patients] ([PatientID], [UserID], [DOB], [Phone], [Address]) VALUES (15, 43, CAST(N'1999-06-28' AS Date), N'369845753', N'963 Điểm C, Khu X')
INSERT [dbo].[Patients] ([PatientID], [UserID], [DOB], [Phone], [Address]) VALUES (16, 44, CAST(N'1999-05-30' AS Date), N'235654258', N'101 Điểm A, Khu X')
INSERT [dbo].[Patients] ([PatientID], [UserID], [DOB], [Phone], [Address]) VALUES (17, 45, CAST(N'1999-10-05' AS Date), N'159654852', N'98 Điểm A, Khu X')
INSERT [dbo].[Patients] ([PatientID], [UserID], [DOB], [Phone], [Address]) VALUES (18, 46, CAST(N'1998-09-03' AS Date), N'976963584', N'111 Điểm A, Khu XZ')
INSERT [dbo].[Patients] ([PatientID], [UserID], [DOB], [Phone], [Address]) VALUES (19, 47, CAST(N'2001-07-25' AS Date), N'0235523963', N'123 Khóm 1, City AMX')
SET IDENTITY_INSERT [dbo].[Patients] OFF
GO
SET IDENTITY_INSERT [dbo].[PrescriptionDetails] ON 

INSERT [dbo].[PrescriptionDetails] ([PrescriptionDetailID], [PrescriptionID], [MedicineID], [Quantity]) VALUES (2, 1, 1, 2)
INSERT [dbo].[PrescriptionDetails] ([PrescriptionDetailID], [PrescriptionID], [MedicineID], [Quantity]) VALUES (3, 1, 2, 6)
INSERT [dbo].[PrescriptionDetails] ([PrescriptionDetailID], [PrescriptionID], [MedicineID], [Quantity]) VALUES (4, 3, 3, 4)
INSERT [dbo].[PrescriptionDetails] ([PrescriptionDetailID], [PrescriptionID], [MedicineID], [Quantity]) VALUES (38, 22, 1, 6)
INSERT [dbo].[PrescriptionDetails] ([PrescriptionDetailID], [PrescriptionID], [MedicineID], [Quantity]) VALUES (39, 22, 2, 9)
INSERT [dbo].[PrescriptionDetails] ([PrescriptionDetailID], [PrescriptionID], [MedicineID], [Quantity]) VALUES (40, 23, 1, 10)
INSERT [dbo].[PrescriptionDetails] ([PrescriptionDetailID], [PrescriptionID], [MedicineID], [Quantity]) VALUES (41, 23, 2, 15)
INSERT [dbo].[PrescriptionDetails] ([PrescriptionDetailID], [PrescriptionID], [MedicineID], [Quantity]) VALUES (42, 23, 3, 20)
INSERT [dbo].[PrescriptionDetails] ([PrescriptionDetailID], [PrescriptionID], [MedicineID], [Quantity]) VALUES (43, 24, 1, 9)
INSERT [dbo].[PrescriptionDetails] ([PrescriptionDetailID], [PrescriptionID], [MedicineID], [Quantity]) VALUES (45, 26, 1, 6)
SET IDENTITY_INSERT [dbo].[PrescriptionDetails] OFF
GO
SET IDENTITY_INSERT [dbo].[Prescriptions] ON 

INSERT [dbo].[Prescriptions] ([PrescriptionID], [DiagnosisID], [Prescription]) VALUES (1, 1, N'Nhập viện theo dõi, truyền dịch y tế(500ml)')
INSERT [dbo].[Prescriptions] ([PrescriptionID], [DiagnosisID], [Prescription]) VALUES (3, 2, N'Nhập viện theo dõi, băng bó nẹp y tế(20cmx3cm)')
INSERT [dbo].[Prescriptions] ([PrescriptionID], [DiagnosisID], [Prescription]) VALUES (22, 17, N'uong 1 ngay 3 lan')
INSERT [dbo].[Prescriptions] ([PrescriptionID], [DiagnosisID], [Prescription]) VALUES (23, 18, N'Toa thuốc 1 , chỉ định uống 3 lần 1 ngày')
INSERT [dbo].[Prescriptions] ([PrescriptionID], [DiagnosisID], [Prescription]) VALUES (24, 18, N'Toa thuốc phụ')
INSERT [dbo].[Prescriptions] ([PrescriptionID], [DiagnosisID], [Prescription]) VALUES (26, 21, N'Ngày uống3 lần')
SET IDENTITY_INSERT [dbo].[Prescriptions] OFF
GO
SET IDENTITY_INSERT [dbo].[Room] ON 

INSERT [dbo].[Room] ([RoomID], [RoomName]) VALUES (1, N'Phòng khám 1')
INSERT [dbo].[Room] ([RoomID], [RoomName]) VALUES (2, N'Phòng khám 2')
INSERT [dbo].[Room] ([RoomID], [RoomName]) VALUES (3, N'Phòng xét nghiệm máu 1')
INSERT [dbo].[Room] ([RoomID], [RoomName]) VALUES (4, N'Phòng xét nghiệm nước tiểu 1')
INSERT [dbo].[Room] ([RoomID], [RoomName]) VALUES (5, N'Phòng chụp X-quang 1')
INSERT [dbo].[Room] ([RoomID], [RoomName]) VALUES (6, N'Phòng xét nghiệm hơi thở 1')
SET IDENTITY_INSERT [dbo].[Room] OFF
GO
SET IDENTITY_INSERT [dbo].[Services] ON 

INSERT [dbo].[Services] ([ServiceID], [ServiceName], [Price], [ServiceParentID], [Type]) VALUES (1, N'Khám bệnh', CAST(100000.000 AS Decimal(15, 3)), NULL, N'Lâm sàng')
INSERT [dbo].[Services] ([ServiceID], [ServiceName], [Price], [ServiceParentID], [Type]) VALUES (2, N'Chụp X-quang toàn thân', CAST(150000.000 AS Decimal(15, 3)), NULL, N'Cận lâm sàng')
INSERT [dbo].[Services] ([ServiceID], [ServiceName], [Price], [ServiceParentID], [Type]) VALUES (3, N'Xét nghiệm máu', CAST(175000.000 AS Decimal(15, 3)), 7, N'Cận lâm sàng')
INSERT [dbo].[Services] ([ServiceID], [ServiceName], [Price], [ServiceParentID], [Type]) VALUES (4, N'Xét nghiệm nước tiểu', CAST(125000.000 AS Decimal(15, 3)), 7, N'Cận lâm sàng')
INSERT [dbo].[Services] ([ServiceID], [ServiceName], [Price], [ServiceParentID], [Type]) VALUES (6, N'Khám bệnh 2', CAST(200000.000 AS Decimal(15, 3)), NULL, N'Lâm sàng')
INSERT [dbo].[Services] ([ServiceID], [ServiceName], [Price], [ServiceParentID], [Type]) VALUES (7, N'Gói xét nghiệm máu và nước tiểu', CAST(300000.000 AS Decimal(15, 3)), NULL, N'Cận lâm sàng')
INSERT [dbo].[Services] ([ServiceID], [ServiceName], [Price], [ServiceParentID], [Type]) VALUES (8, N'Xét nghiệm hơi thở', CAST(10000.000 AS Decimal(15, 3)), NULL, N'Cận lâm sàng')
SET IDENTITY_INSERT [dbo].[Services] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([UserID], [FullName], [Email], [PasswordHash], [Role], [CreatedAt], [ImageUrl]) VALUES (2, N'Nguyễn Văn Lễ Tân', N'lt@gmail.com', N'$2a$12$p0PEQZqR/RBMfgAP9JKoi.OKs2bu9r1qCNtB0Q969J9n4uzADdvE2', N'Lễ tân', CAST(N'2025-04-13T17:35:25.333' AS DateTime), NULL)
INSERT [dbo].[Users] ([UserID], [FullName], [Email], [PasswordHash], [Role], [CreatedAt], [ImageUrl]) VALUES (3, N'Nguyễn Văn Bác Sĩ A', N'bsa@gmail.com', N'$2a$12$p0PEQZqR/RBMfgAP9JKoi.OKs2bu9r1qCNtB0Q969J9n4uzADdvE2', N'Bác sĩ', CAST(N'2025-04-13T17:35:25.333' AS DateTime), NULL)
INSERT [dbo].[Users] ([UserID], [FullName], [Email], [PasswordHash], [Role], [CreatedAt], [ImageUrl]) VALUES (4, N'Nguyễn Văn Bác Sĩ B', N'bsb@gmail.com', N'$2a$12$p0PEQZqR/RBMfgAP9JKoi.OKs2bu9r1qCNtB0Q969J9n4uzADdvE2', N'Bác sĩ', CAST(N'2025-04-13T17:35:25.333' AS DateTime), NULL)
INSERT [dbo].[Users] ([UserID], [FullName], [Email], [PasswordHash], [Role], [CreatedAt], [ImageUrl]) VALUES (5, N'Nguyễn Văn Bệnh Nhân A', N'bna@gmail.com', N'$2a$12$p0PEQZqR/RBMfgAP9JKoi.OKs2bu9r1qCNtB0Q969J9n4uzADdvE2', N'Bệnh nhân', CAST(N'2025-04-13T17:35:25.333' AS DateTime), NULL)
INSERT [dbo].[Users] ([UserID], [FullName], [Email], [PasswordHash], [Role], [CreatedAt], [ImageUrl]) VALUES (6, N'Nguyễn Văn Bệnh Nhân B', N'bnb@gmail.com', N'$2a$12$p0PEQZqR/RBMfgAP9JKoi.OKs2bu9r1qCNtB0Q969J9n4uzADdvE2', N'Bệnh nhân', CAST(N'2025-04-13T17:35:25.333' AS DateTime), NULL)
INSERT [dbo].[Users] ([UserID], [FullName], [Email], [PasswordHash], [Role], [CreatedAt], [ImageUrl]) VALUES (7, N'Nguyễn Văn Kỹ Thuật Viên A', N'ktva@gmail.com', N'$2a$12$p0PEQZqR/RBMfgAP9JKoi.OKs2bu9r1qCNtB0Q969J9n4uzADdvE2', N'Kỹ thuật viên', CAST(N'2025-04-13T17:35:25.333' AS DateTime), NULL)
INSERT [dbo].[Users] ([UserID], [FullName], [Email], [PasswordHash], [Role], [CreatedAt], [ImageUrl]) VALUES (8, N'Nguyễn Văn Kỹ Thuật Viên B', N'ktvb@gmail.com', N'$2a$12$p0PEQZqR/RBMfgAP9JKoi.OKs2bu9r1qCNtB0Q969J9n4uzADdvE2', N'Kỹ thuật viên', CAST(N'2025-04-13T17:35:25.333' AS DateTime), NULL)
INSERT [dbo].[Users] ([UserID], [FullName], [Email], [PasswordHash], [Role], [CreatedAt], [ImageUrl]) VALUES (9, N'Nguyễn Văn Kỹ Thuật Viên C', N'ktvc@gmail.com', N'$2a$12$p0PEQZqR/RBMfgAP9JKoi.OKs2bu9r1qCNtB0Q969J9n4uzADdvE2', N'Kỹ thuật viên', CAST(N'2025-04-13T17:35:25.333' AS DateTime), NULL)
INSERT [dbo].[Users] ([UserID], [FullName], [Email], [PasswordHash], [Role], [CreatedAt], [ImageUrl]) VALUES (12, N'Bill Gate', N'admin@gmail.com', N'$2a$12$p0PEQZqR/RBMfgAP9JKoi.OKs2bu9r1qCNtB0Q969J9n4uzADdvE2', N'Admin', CAST(N'2025-04-20T13:20:50.297' AS DateTime), NULL)
INSERT [dbo].[Users] ([UserID], [FullName], [Email], [PasswordHash], [Role], [CreatedAt], [ImageUrl]) VALUES (13, N'Bill Gate 2', N'admin2@gmail.com', N'$2a$12$2UOAnFotnzNHkYkeAskZwuKxwu72Ef8335ZtDmfqz80aGFM1KGXee', N'Admin', CAST(N'2025-04-20T13:23:56.897' AS DateTime), NULL)
INSERT [dbo].[Users] ([UserID], [FullName], [Email], [PasswordHash], [Role], [CreatedAt], [ImageUrl]) VALUES (15, N'Jack Zuckerberg', N'zackzukerberg@gmail.com', N'$2a$12$nukmHuYQ0J7rGg8GhugSvOKx.BZovK.i8zGVQAWfP/m/3I4IkWxfG', N'Lễ tân', CAST(N'2025-04-21T10:16:16.070' AS DateTime), NULL)
INSERT [dbo].[Users] ([UserID], [FullName], [Email], [PasswordHash], [Role], [CreatedAt], [ImageUrl]) VALUES (22, N'Lương Văn Phong - update', N'luongvanphongupdate@gmail.com', N'$2a$12$QhUHw8LMvO/js5nuPIdvTeUwe8AX2m5uITCIrKIs2QYBhszDcNP.O', N'Bệnh nhân', CAST(N'2025-04-25T18:25:52.490' AS DateTime), N'/images/avatars/ba9a40a8-a91c-4ada-a528-be59de0a5068.png')
INSERT [dbo].[Users] ([UserID], [FullName], [Email], [PasswordHash], [Role], [CreatedAt], [ImageUrl]) VALUES (31, N'Trần Thị Việt Hương', N'tranthiviethuong@gmail.com', N'$2a$12$TJVQz9Uap67DXa57aEkiz.gW/ZR6ieIsn24Uc9BWsdJ6Oz5NKKH56', N'Kỹ thuật viên', CAST(N'2025-04-23T14:56:01.063' AS DateTime), N'/images/avatars/5a0ad5d8-4c4c-4ec8-99a6-a7996fc34992.jpg')
INSERT [dbo].[Users] ([UserID], [FullName], [Email], [PasswordHash], [Role], [CreatedAt], [ImageUrl]) VALUES (32, N'Trần Dịch Hinh', N'trandichhinh@gmail.com', N'$2a$12$USXHD9RICtplKbxNJbbWeuYevJxnU6zNnoMAmIb6d70nEOUaEiumC', N'Bác sĩ', CAST(N'2025-04-23T14:58:24.720' AS DateTime), N'/images/avatars/bd2dbf5a-d5d0-4cf7-adf4-a78ef0c417f3.jpg')
INSERT [dbo].[Users] ([UserID], [FullName], [Email], [PasswordHash], [Role], [CreatedAt], [ImageUrl]) VALUES (34, N'Nguyễn Văn Bệnh Nhân C', N'bnc@gmail.com', N'$2a$12$OSCTXuoK6WVwIbum42x4..Yzx3Fk8s72.IvmNb61tdXjcPwpjwzem', N'Bệnh nhân', CAST(N'2025-06-07T11:10:52.847' AS DateTime), NULL)
INSERT [dbo].[Users] ([UserID], [FullName], [Email], [PasswordHash], [Role], [CreatedAt], [ImageUrl]) VALUES (38, N'Nguyễn Văn Bệnh Nhân D', N'bnd@gmail.com', N'$2a$12$OSCTXuoK6WVwIbum42x4..Yzx3Fk8s72.IvmNb61tdXjcPwpjwzem', N'Bệnh nhân', CAST(N'2025-07-09T15:11:43.493' AS DateTime), NULL)
INSERT [dbo].[Users] ([UserID], [FullName], [Email], [PasswordHash], [Role], [CreatedAt], [ImageUrl]) VALUES (40, N'Nguyễn Văn Bệnh Nhân X', N'bnx@gmail.com', N'$2a$12$OSCTXuoK6WVwIbum42x4..Yzx3Fk8s72.IvmNb61tdXjcPwpjwzem', N'Bệnh nhân', CAST(N'2025-07-09T15:16:14.790' AS DateTime), NULL)
INSERT [dbo].[Users] ([UserID], [FullName], [Email], [PasswordHash], [Role], [CreatedAt], [ImageUrl]) VALUES (42, N'Nguyễn Văn Bệnh Nhân Y', N'bny@gmail.com', N'$2a$12$OSCTXuoK6WVwIbum42x4..Yzx3Fk8s72.IvmNb61tdXjcPwpjwzem', N'Bệnh nhân', CAST(N'2025-07-09T15:16:50.743' AS DateTime), NULL)
INSERT [dbo].[Users] ([UserID], [FullName], [Email], [PasswordHash], [Role], [CreatedAt], [ImageUrl]) VALUES (43, N'Nguyễn Văn Bênh Nhân Z', N'bnz@gmail.com', N'$2a$12$OSCTXuoK6WVwIbum42x4..Yzx3Fk8s72.IvmNb61tdXjcPwpjwzem', N'Bệnh nhân', CAST(N'2025-07-09T15:17:34.790' AS DateTime), NULL)
INSERT [dbo].[Users] ([UserID], [FullName], [Email], [PasswordHash], [Role], [CreatedAt], [ImageUrl]) VALUES (44, N'Nguyễn Văn Bé', N'bnbe@gmal.com', N'$2a$12$OSCTXuoK6WVwIbum42x4..Yzx3Fk8s72.IvmNb61tdXjcPwpjwzem', N'Bệnh nhân', CAST(N'2025-07-09T15:18:04.160' AS DateTime), NULL)
INSERT [dbo].[Users] ([UserID], [FullName], [Email], [PasswordHash], [Role], [CreatedAt], [ImageUrl]) VALUES (45, N'Nguyễn Văn Lớn', N'bnlon@gmail.com', N'$2a$12$OSCTXuoK6WVwIbum42x4..Yzx3Fk8s72.IvmNb61tdXjcPwpjwzem', N'Bệnh nhân', CAST(N'2025-07-09T15:18:34.947' AS DateTime), NULL)
INSERT [dbo].[Users] ([UserID], [FullName], [Email], [PasswordHash], [Role], [CreatedAt], [ImageUrl]) VALUES (46, N'Nguyễn Văn Lam', N'bnlam@gmail.com', N'$2a$12$OSCTXuoK6WVwIbum42x4..Yzx3Fk8s72.IvmNb61tdXjcPwpjwzem', N'Bệnh nhân', CAST(N'2025-07-09T15:19:04.310' AS DateTime), NULL)
INSERT [dbo].[Users] ([UserID], [FullName], [Email], [PasswordHash], [Role], [CreatedAt], [ImageUrl]) VALUES (47, N'Nguyễn Văn Bệnh Nhân Thu', N'bnthu@gmail.com', N'$2a$12$TkxSeqwGklKgpEkWhTZH8uayq1NBoBMKy57fh2ewR/xqemw5rbkce', N'Bệnh nhân', CAST(N'2025-07-09T15:28:05.587' AS DateTime), NULL)
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
SET IDENTITY_INSERT [dbo].[WorkSchedules] ON 

INSERT [dbo].[WorkSchedules] ([WorkScheduleID], [StartTime], [EndTime], [StartDate], [EndDate], [DoctorID]) VALUES (1, CAST(N'07:00:00' AS Time), CAST(N'11:00:00' AS Time), CAST(N'2010-06-10' AS Date), CAST(N'2030-06-10' AS Date), 1)
INSERT [dbo].[WorkSchedules] ([WorkScheduleID], [StartTime], [EndTime], [StartDate], [EndDate], [DoctorID]) VALUES (2, CAST(N'07:00:00' AS Time), CAST(N'11:00:00' AS Time), CAST(N'2010-06-10' AS Date), CAST(N'2030-06-10' AS Date), 2)
INSERT [dbo].[WorkSchedules] ([WorkScheduleID], [StartTime], [EndTime], [StartDate], [EndDate], [DoctorID]) VALUES (4, CAST(N'13:00:00' AS Time), CAST(N'17:00:00' AS Time), CAST(N'2010-06-10' AS Date), CAST(N'2030-06-10' AS Date), 1)
SET IDENTITY_INSERT [dbo].[WorkSchedules] OFF
GO
/****** Object:  Index [UQ__Doctors__1788CCADF30BC368]    Script Date: 14-Jul-25 01:25:52 ******/
ALTER TABLE [dbo].[Doctors] ADD UNIQUE NONCLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [UQ__Patients__1788CCAD911A3D07]    Script Date: 14-Jul-25 01:25:52 ******/
ALTER TABLE [dbo].[Patients] ADD UNIQUE NONCLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Users__A9D10534E3A6E70D]    Script Date: 14-Jul-25 01:25:52 ******/
ALTER TABLE [dbo].[Users] ADD UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Appointments] ADD  DEFAULT (N'Đã đặt') FOR [Status]
GO
ALTER TABLE [dbo].[Bills] ADD  DEFAULT (N'Chưa thanh toán') FOR [PaymentStatus]
GO
ALTER TABLE [dbo].[Bills] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Appointments]  WITH CHECK ADD FOREIGN KEY([DoctorID])
REFERENCES [dbo].[Doctors] ([DoctorID])
GO
ALTER TABLE [dbo].[Appointments]  WITH CHECK ADD FOREIGN KEY([PatientID])
REFERENCES [dbo].[Patients] ([PatientID])
GO
ALTER TABLE [dbo].[Bills]  WITH CHECK ADD FOREIGN KEY([AppointmentID])
REFERENCES [dbo].[Appointments] ([AppointmentID])
GO
ALTER TABLE [dbo].[Diagnoses]  WITH CHECK ADD FOREIGN KEY([AppointmentID])
REFERENCES [dbo].[Appointments] ([AppointmentID])
GO
ALTER TABLE [dbo].[Diagnoses_Services]  WITH CHECK ADD  CONSTRAINT [FK__Diagnoses__Diagn__6B24EA82] FOREIGN KEY([DiagnosisID])
REFERENCES [dbo].[Diagnoses] ([DiagnosisID])
GO
ALTER TABLE [dbo].[Diagnoses_Services] CHECK CONSTRAINT [FK__Diagnoses__Diagn__6B24EA82]
GO
ALTER TABLE [dbo].[Diagnoses_Services]  WITH CHECK ADD  CONSTRAINT [FK__Diagnoses__RoomI__6E01572D] FOREIGN KEY([RoomID])
REFERENCES [dbo].[Room] ([RoomID])
GO
ALTER TABLE [dbo].[Diagnoses_Services] CHECK CONSTRAINT [FK__Diagnoses__RoomI__6E01572D]
GO
ALTER TABLE [dbo].[Diagnoses_Services]  WITH CHECK ADD  CONSTRAINT [FK__Diagnoses__Servi__6C190EBB] FOREIGN KEY([ServiceID])
REFERENCES [dbo].[Services] ([ServiceID])
GO
ALTER TABLE [dbo].[Diagnoses_Services] CHECK CONSTRAINT [FK__Diagnoses__Servi__6C190EBB]
GO
ALTER TABLE [dbo].[Diagnoses_Services]  WITH CHECK ADD  CONSTRAINT [FK__Diagnoses__UserI__6D0D32F4] FOREIGN KEY([UserIDperformed])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Diagnoses_Services] CHECK CONSTRAINT [FK__Diagnoses__UserI__6D0D32F4]
GO
ALTER TABLE [dbo].[Doctors]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Patients]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[PrescriptionDetails]  WITH CHECK ADD  CONSTRAINT [FK_PrescriptionDetails_Medicines] FOREIGN KEY([MedicineID])
REFERENCES [dbo].[Medicines] ([MedicineID])
GO
ALTER TABLE [dbo].[PrescriptionDetails] CHECK CONSTRAINT [FK_PrescriptionDetails_Medicines]
GO
ALTER TABLE [dbo].[PrescriptionDetails]  WITH CHECK ADD  CONSTRAINT [FK_PrescriptionDetails_Prescriptions] FOREIGN KEY([PrescriptionID])
REFERENCES [dbo].[Prescriptions] ([PrescriptionID])
GO
ALTER TABLE [dbo].[PrescriptionDetails] CHECK CONSTRAINT [FK_PrescriptionDetails_Prescriptions]
GO
ALTER TABLE [dbo].[Prescriptions]  WITH CHECK ADD  CONSTRAINT [FK_Prescriptions_Diagnoses] FOREIGN KEY([DiagnosisID])
REFERENCES [dbo].[Diagnoses] ([DiagnosisID])
GO
ALTER TABLE [dbo].[Prescriptions] CHECK CONSTRAINT [FK_Prescriptions_Diagnoses]
GO
ALTER TABLE [dbo].[Services]  WITH CHECK ADD FOREIGN KEY([ServiceParentID])
REFERENCES [dbo].[Services] ([ServiceID])
GO
ALTER TABLE [dbo].[WorkSchedules]  WITH CHECK ADD  CONSTRAINT [FK_WorkSchedules_Doctors] FOREIGN KEY([DoctorID])
REFERENCES [dbo].[Doctors] ([DoctorID])
GO
ALTER TABLE [dbo].[WorkSchedules] CHECK CONSTRAINT [FK_WorkSchedules_Doctors]
GO
ALTER TABLE [dbo].[Bills]  WITH CHECK ADD CHECK  (([PaymentStatus]=N'Đã thanh toán' OR [PaymentStatus]=N'Chưa thanh toán'))
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD CHECK  (([Role]=N'Kỹ thuật viên' OR [Role]=N'Bệnh nhân' OR [Role]=N'Bác sĩ' OR [Role]=N'Lễ tân' OR [Role]=N'Admin'))
GO
