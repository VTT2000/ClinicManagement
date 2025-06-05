using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace web_api_base.Models.ClinicManagement;

public partial class ClinicContext : DbContext
{
    public ClinicContext()
    {
    }

    public ClinicContext(DbContextOptions<ClinicContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<Bill> Bills { get; set; }

    public virtual DbSet<DiagnosesService> DiagnosesServices { get; set; }

    public virtual DbSet<Diagnosis> Diagnoses { get; set; }

    public virtual DbSet<Doctor> Doctors { get; set; }

    public virtual DbSet<Medicine> Medicines { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    public virtual DbSet<Prescription> Prescriptions { get; set; }

    public virtual DbSet<PrescriptionDetail> PrescriptionDetails { get; set; }

    public virtual DbSet<PrescriptionDetail> PrescriptionDetails { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<WorkSchedule> WorkSchedules { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionString0");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.AppointmentId).HasName("PK__Appointm__8ECDFCA24E5DFB25");

            entity.Property(e => e.AppointmentId).HasColumnName("AppointmentID");
            entity.Property(e => e.DoctorId).HasColumnName("DoctorID");
            entity.Property(e => e.PatientId).HasColumnName("PatientID");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Đã đặt");

            entity.HasOne(d => d.Doctor).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.DoctorId)
                .HasConstraintName("FK__Appointme__Docto__68487DD7");

            entity.HasOne(d => d.Patient).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.PatientId)
                .HasConstraintName("FK__Appointme__Patie__6754599E");
        });

        modelBuilder.Entity<Bill>(entity =>
        {
            entity.HasKey(e => e.BillId).HasName("PK__Bills__11F2FC4AEBB563DD");

            entity.Property(e => e.BillId).HasColumnName("BillID");
            entity.Property(e => e.AppointmentId).HasColumnName("AppointmentID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PaymentStatus)
                .HasMaxLength(50)
                .HasDefaultValue("Chưa thanh toán");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(15, 3)");

            entity.HasOne(d => d.Appointment).WithMany(p => p.Bills)
                .HasForeignKey(d => d.AppointmentId)
                .HasConstraintName("FK__Bills__Appointme__693CA210");
        });

        modelBuilder.Entity<DiagnosesService>(entity =>
        {
            entity.ToTable("Diagnoses_Services");
            entity.ToTable("Diagnoses_Services");

            entity.Property(e => e.DiagnosesServiceId).HasColumnName("DiagnosesServiceID");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.DiagnosesServiceId).HasColumnName("DiagnosesServiceID");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.DiagnosisId).HasColumnName("DiagnosisID");
            entity.Property(e => e.RoomId).HasColumnName("RoomID");
            entity.Property(e => e.ServiceId).HasColumnName("ServiceID");
            entity.Property(e => e.UserIdperformed).HasColumnName("UserIDperformed");

            entity.HasOne(d => d.Diagnosis).WithMany(p => p.DiagnosesServices)
            entity.HasOne(d => d.Diagnosis).WithMany(p => p.DiagnosesServices)
                .HasForeignKey(d => d.DiagnosisId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Diagnoses__Diagn__6B24EA82");
                .HasConstraintName("FK__Diagnoses__Diagn__6B24EA82");

            entity.HasOne(d => d.Room).WithMany(p => p.DiagnosesServices)
            entity.HasOne(d => d.Room).WithMany(p => p.DiagnosesServices)
                .HasForeignKey(d => d.RoomId)
                .HasConstraintName("FK__Diagnoses__RoomI__6E01572D");
                .HasConstraintName("FK__Diagnoses__RoomI__6E01572D");

            entity.HasOne(d => d.Service).WithMany(p => p.DiagnosesServices)
            entity.HasOne(d => d.Service).WithMany(p => p.DiagnosesServices)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Diagnoses__Servi__6C190EBB");
                .HasConstraintName("FK__Diagnoses__Servi__6C190EBB");

            entity.HasOne(d => d.UserIdperformedNavigation).WithMany(p => p.DiagnosesServices)
            entity.HasOne(d => d.UserIdperformedNavigation).WithMany(p => p.DiagnosesServices)
                .HasForeignKey(d => d.UserIdperformed)
                .HasConstraintName("FK__Diagnoses__UserI__6D0D32F4");
                .HasConstraintName("FK__Diagnoses__UserI__6D0D32F4");
        });

        modelBuilder.Entity<Diagnosis>(entity =>
        {
            entity.HasKey(e => e.DiagnosisId).HasName("PK__Diagnose__0C54CB93475B4BBD");

            entity.Property(e => e.DiagnosisId).HasColumnName("DiagnosisID");
            entity.Property(e => e.AppointmentId).HasColumnName("AppointmentID");
            entity.Property(e => e.Diagnosis1)
                .HasMaxLength(255)
                .HasColumnName("Diagnosis");
            entity.Property(e => e.Symptoms).HasMaxLength(255);

            entity.HasOne(d => d.Appointment).WithMany(p => p.Diagnoses)
                .HasForeignKey(d => d.AppointmentId)
                .HasConstraintName("FK__Diagnoses__Appoi__6A30C649");
        });

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(e => e.DoctorId).HasName("PK__Doctors__2DC00EDF32E06797");

            entity.HasIndex(e => e.UserId, "UQ__Doctors__1788CCADF30BC368").IsUnique();

            entity.Property(e => e.DoctorId).HasColumnName("DoctorID");
            entity.Property(e => e.Specialization).HasMaxLength(100);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithOne(p => p.Doctor)
                .HasForeignKey<Doctor>(d => d.UserId)
                .HasConstraintName("FK__Doctors__UserID__6EF57B66");
        });

        modelBuilder.Entity<Medicine>(entity =>
        {
            entity.HasKey(e => e.MedicineId).HasName("PK__Medicine__4F2128F033C87AEC");

            entity.Property(e => e.MedicineId).HasColumnName("MedicineID");
            entity.Property(e => e.MedicineName).HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("decimal(15, 3)");
            entity.Property(e => e.Unit).HasMaxLength(50);
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.PatientId).HasName("PK__Patients__970EC34626FC3D5B");

            entity.HasIndex(e => e.UserId, "UQ__Patients__1788CCAD911A3D07").IsUnique();

            entity.Property(e => e.PatientId).HasColumnName("PatientID");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Dob).HasColumnName("DOB");
            entity.Property(e => e.Phone).HasMaxLength(15);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithOne(p => p.Patient)
                .HasForeignKey<Patient>(d => d.UserId)
                .HasConstraintName("FK__Patients__UserID__6FE99F9F");
        });

        modelBuilder.Entity<Prescription>(entity =>
        {
            entity.HasKey(e => e.PrescriptionId).HasName("PK__Prescrip__4013081246B3B494");

            entity.Property(e => e.PrescriptionId).HasColumnName("PrescriptionID");
            entity.Property(e => e.DiagnosisId).HasColumnName("DiagnosisID");
            entity.Property(e => e.Prescription1).HasColumnName("Prescription");
            entity.Property(e => e.Prescription1).HasColumnName("Prescription");

            entity.HasOne(d => d.Diagnosis).WithMany(p => p.Prescriptions)
                .HasForeignKey(d => d.DiagnosisId)
                .HasConstraintName("FK_Prescriptions_Diagnoses");
        });

        modelBuilder.Entity<PrescriptionDetail>(entity =>
        {
            entity.Property(e => e.PrescriptionDetailId).HasColumnName("PrescriptionDetailID");
            entity.Property(e => e.MedicineId).HasColumnName("MedicineID");
            entity.Property(e => e.PrescriptionId).HasColumnName("PrescriptionID");

            entity.HasOne(d => d.Medicine).WithMany(p => p.PrescriptionDetails)
                .HasConstraintName("FK_Prescriptions_Diagnoses");
        });

        modelBuilder.Entity<PrescriptionDetail>(entity =>
        {
            entity.Property(e => e.PrescriptionDetailId).HasColumnName("PrescriptionDetailID");
            entity.Property(e => e.MedicineId).HasColumnName("MedicineID");
            entity.Property(e => e.PrescriptionId).HasColumnName("PrescriptionID");

            entity.HasOne(d => d.Medicine).WithMany(p => p.PrescriptionDetails)
                .HasForeignKey(d => d.MedicineId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PrescriptionDetails_Medicines");

            entity.HasOne(d => d.Prescription).WithMany(p => p.PrescriptionDetails)
                .HasForeignKey(d => d.PrescriptionId)
                .HasConstraintName("FK_PrescriptionDetails_Prescriptions");
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PrescriptionDetails_Medicines");

            entity.HasOne(d => d.Prescription).WithMany(p => p.PrescriptionDetails)
                .HasForeignKey(d => d.PrescriptionId)
                .HasConstraintName("FK_PrescriptionDetails_Prescriptions");
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.RoomId).HasName("PK__Room__328639193B4C2E43");

            entity.ToTable("Room");

            entity.Property(e => e.RoomId).HasColumnName("RoomID");
            entity.Property(e => e.RoomName).HasMaxLength(50);
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PK__Services__C51BB0EAC2B643FF");

            entity.Property(e => e.ServiceId).HasColumnName("ServiceID");
            entity.Property(e => e.Price).HasColumnType("decimal(15, 3)");
            entity.Property(e => e.ServiceName).HasMaxLength(100);
            entity.Property(e => e.ServiceParentId).HasColumnName("ServiceParentID");
            entity.Property(e => e.Type).HasMaxLength(50);
            entity.Property(e => e.Type).HasMaxLength(50);

            entity.HasOne(d => d.ServiceParent).WithMany(p => p.InverseServiceParent)
                .HasForeignKey(d => d.ServiceParentId)
                .HasConstraintName("FK__Services__Servic__72C60C4A");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC8D124F83");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534E3A6E70D").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.ImageUrl).HasMaxLength(255);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.Role).HasMaxLength(50);
        });

        modelBuilder.Entity<WorkSchedule>(entity =>
        {
            entity.HasKey(e => e.WorkScheduleId).HasName("PK__WorkSche__C6AC635E6C7080E1");

            entity.Property(e => e.WorkScheduleId).HasColumnName("WorkScheduleID");
            entity.Property(e => e.DoctorId).HasColumnName("DoctorID");
            entity.Property(e => e.DoctorId).HasColumnName("DoctorID");

            entity.HasOne(d => d.Doctor).WithMany(p => p.WorkSchedules)
                .HasForeignKey(d => d.DoctorId)
                .HasConstraintName("FK_WorkSchedules_Doctors");
            entity.HasOne(d => d.Doctor).WithMany(p => p.WorkSchedules)
                .HasForeignKey(d => d.DoctorId)
                .HasConstraintName("FK_WorkSchedules_Doctors");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
