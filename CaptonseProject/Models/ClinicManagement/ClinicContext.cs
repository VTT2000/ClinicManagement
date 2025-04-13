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

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<WorkSchedule> WorkSchedules { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionString");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.AppointmentId).HasName("PK__Appointm__8ECDFCA2C8DA5D49");

            entity.Property(e => e.AppointmentId).HasColumnName("AppointmentID");
            entity.Property(e => e.AppointmentDate).HasColumnType("datetime");
            entity.Property(e => e.DoctorId).HasColumnName("DoctorID");
            entity.Property(e => e.PatientId).HasColumnName("PatientID");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Đã đặt");

            entity.HasOne(d => d.Doctor).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.DoctorId)
                .HasConstraintName("FK__Appointme__Docto__44FF419A");

            entity.HasOne(d => d.Patient).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.PatientId)
                .HasConstraintName("FK__Appointme__Patie__440B1D61");
        });

        modelBuilder.Entity<Bill>(entity =>
        {
            entity.HasKey(e => e.BillId).HasName("PK__Bills__11F2FC4AB5A994BD");

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
                .HasConstraintName("FK__Bills__Appointme__5CD6CB2B");
        });

        modelBuilder.Entity<DiagnosesService>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Diagnoses_Services");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DiagnosisId).HasColumnName("DiagnosisID");
            entity.Property(e => e.RoomId).HasColumnName("RoomID");
            entity.Property(e => e.ServiceId).HasColumnName("ServiceID");
            entity.Property(e => e.UserIdperformed).HasColumnName("UserIDperformed");

            entity.HasOne(d => d.Diagnosis).WithMany()
                .HasForeignKey(d => d.DiagnosisId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Diagnoses__Diagn__5629CD9C");

            entity.HasOne(d => d.Room).WithMany()
                .HasForeignKey(d => d.RoomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Diagnoses__RoomI__59FA5E80");

            entity.HasOne(d => d.Service).WithMany()
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Diagnoses__Servi__571DF1D5");

            entity.HasOne(d => d.UserIdperformedNavigation).WithMany()
                .HasForeignKey(d => d.UserIdperformed)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Diagnoses__UserI__59063A47");
        });

        modelBuilder.Entity<Diagnosis>(entity =>
        {
            entity.HasKey(e => e.DiagnosisId).HasName("PK__Diagnose__0C54CB9301359292");

            entity.Property(e => e.DiagnosisId).HasColumnName("DiagnosisID");
            entity.Property(e => e.AppointmentId).HasColumnName("AppointmentID");
            entity.Property(e => e.Diagnosis1)
                .HasMaxLength(255)
                .HasColumnName("Diagnosis");
            entity.Property(e => e.Symptoms).HasMaxLength(255);

            entity.HasOne(d => d.Appointment).WithMany(p => p.Diagnoses)
                .HasForeignKey(d => d.AppointmentId)
                .HasConstraintName("FK__Diagnoses__Appoi__4CA06362");
        });

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(e => e.DoctorId).HasName("PK__Doctors__2DC00EDFB973D3F1");

            entity.HasIndex(e => e.UserId, "UQ__Doctors__1788CCAD8D6ECC4E").IsUnique();

            entity.Property(e => e.DoctorId).HasColumnName("DoctorID");
            entity.Property(e => e.Specialization).HasMaxLength(100);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithOne(p => p.Doctor)
                .HasForeignKey<Doctor>(d => d.UserId)
                .HasConstraintName("FK__Doctors__UserID__412EB0B6");
        });

        modelBuilder.Entity<Medicine>(entity =>
        {
            entity.HasKey(e => e.MedicineId).HasName("PK__Medicine__4F2128F084E675DC");

            entity.Property(e => e.MedicineId).HasColumnName("MedicineID");
            entity.Property(e => e.MedicineName).HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("decimal(15, 3)");
            entity.Property(e => e.Unit).HasMaxLength(50);
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.PatientId).HasName("PK__Patients__970EC346A4E4A0B2");

            entity.HasIndex(e => e.UserId, "UQ__Patients__1788CCAD234A6006").IsUnique();

            entity.Property(e => e.PatientId).HasColumnName("PatientID");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Dob).HasColumnName("DOB");
            entity.Property(e => e.Phone).HasMaxLength(15);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithOne(p => p.Patient)
                .HasForeignKey<Patient>(d => d.UserId)
                .HasConstraintName("FK__Patients__UserID__3D5E1FD2");
        });

        modelBuilder.Entity<Prescription>(entity =>
        {
            entity.HasKey(e => e.PrescriptionId).HasName("PK__Prescrip__4013081265472B03");

            entity.Property(e => e.PrescriptionId).HasColumnName("PrescriptionID");
            entity.Property(e => e.DiagnosisId).HasColumnName("DiagnosisID");
            entity.Property(e => e.MedicineId).HasColumnName("MedicineID");

            entity.HasOne(d => d.Diagnosis).WithMany(p => p.Prescriptions)
                .HasForeignKey(d => d.DiagnosisId)
                .HasConstraintName("FK__Prescript__Diagn__5165187F");

            entity.HasOne(d => d.Medicine).WithMany(p => p.Prescriptions)
                .HasForeignKey(d => d.MedicineId)
                .HasConstraintName("FK__Prescript__Medic__52593CB8");
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.RoomId).HasName("PK__Room__32863919DC8304A7");

            entity.ToTable("Room");

            entity.Property(e => e.RoomId).HasColumnName("RoomID");
            entity.Property(e => e.RoomName).HasMaxLength(50);
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PK__Services__C51BB0EAB95A9644");

            entity.Property(e => e.ServiceId).HasColumnName("ServiceID");
            entity.Property(e => e.Price).HasColumnType("decimal(15, 3)");
            entity.Property(e => e.ServiceName).HasMaxLength(100);
            entity.Property(e => e.ServiceParentId).HasColumnName("ServiceParentID");

            entity.HasOne(d => d.ServiceParent).WithMany(p => p.InverseServiceParent)
                .HasForeignKey(d => d.ServiceParentId)
                .HasConstraintName("FK__Services__Servic__49C3F6B7");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC10E558DD");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D105345C5273C6").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.Role).HasMaxLength(50);
        });

        modelBuilder.Entity<WorkSchedule>(entity =>
        {
            entity.HasKey(e => e.WorkScheduleId).HasName("PK__WorkSche__C6AC635EBF8B08D0");

            entity.Property(e => e.WorkScheduleId).HasColumnName("WorkScheduleID");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.StartDate).HasColumnType("datetime");

            entity.HasMany(d => d.Doctors).WithMany(p => p.WorkSchedules)
                .UsingEntity<Dictionary<string, object>>(
                    "WorkScheduleDoctor",
                    r => r.HasOne<Doctor>().WithMany()
                        .HasForeignKey("DoctorId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__WorkSched__Docto__656C112C"),
                    l => l.HasOne<WorkSchedule>().WithMany()
                        .HasForeignKey("WorkScheduleId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__WorkSched__WorkS__6477ECF3"),
                    j =>
                    {
                        j.HasKey("WorkScheduleId", "DoctorId");
                        j.ToTable("WorkSchedule_Doctor");
                        j.IndexerProperty<int>("WorkScheduleId").HasColumnName("WorkScheduleID");
                        j.IndexerProperty<int>("DoctorId").HasColumnName("DoctorID");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
