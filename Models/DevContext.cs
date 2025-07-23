using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Assiginment.Models;

public partial class DevContext : DbContext
{
    public DevContext()
    {
    }

    public DevContext(DbContextOptions<DevContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Attendance> Attendances { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Leaf> Leaves { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=DEV;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Attendance>(entity =>
        {
            entity.HasKey(e => e.AttendanceId).HasName("PK__Attendan__8B69261C3BF27B1C");

            entity.ToTable("Attendance");

            entity.HasIndex(e => e.EmployeeId, "IX_Attendance_EmployeeId");

            entity.HasIndex(e => new { e.EmployeeId, e.AttendanceDate }, "UC_Attendance").IsUnique();

            entity.Property(e => e.Remarks).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Present");

            entity.HasOne(d => d.Employee).WithMany(p => p.Attendances)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK__Attendanc__Emplo__44FF419A");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK__Employee__7AD04F118422B7F1");
            entity.HasIndex(e => e.UserId, "UQ__Employee__1788CC4D2BE47381").IsUnique();

            entity.Property(e => e.EmployeeId).HasDefaultValueSql("NEWID()");
            entity.Property(e => e.Department).HasMaxLength(100);
            entity.Property(e => e.Designation).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(20);

            entity.HasOne(d => d.User).WithOne(p => p.Employee)
                .HasForeignKey<Employee>(d => d.UserId)
                .HasConstraintName("FK__Employees__UserI__3F466844");
        });
        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(e => e.Id);
            //how to genrate new id we have to use had defalult value sql
            entity.Property(e => e.Id).HasDefaultValueSql("NEWID()");
            entity.HasOne(i => i.Employee)
            .WithOne(e => e.Image)
            .HasForeignKey<Image>(e => e.EmployeeId)
            .HasConstraintName("FK_Images_EmployeeId")
            .OnDelete(DeleteBehavior.Cascade);

        });


        modelBuilder.Entity<Leaf>(entity =>
        {
            entity.HasKey(e => e.LeaveId).HasName("PK__Leaves__796DB9596B653B53");

            entity.HasIndex(e => e.EmployeeId, "IX_Leaves_EmployeeId");

            entity.Property(e => e.AppliedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.LeaveType).HasMaxLength(50);
            entity.Property(e => e.Reason).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Pending");

            entity.HasOne(d => d.ApprovedByNavigation).WithMany(p => p.Leaves)
                .HasForeignKey(d => d.ApprovedBy)
                .HasConstraintName("FK__Leaves__Approved__4CA06362");

            entity.HasOne(d => d.Employee).WithMany(p => p.Leaves)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK__Leaves__Employee__4BAC3F29");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4CBDBFC54D");

            entity.HasIndex(e => e.UserName, "IX_Users_UserName");

            entity.HasIndex(e => e.UserName, "UQ__Users__C9F28456D5B9B4BC").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.PasswordHash).HasMaxLength(256);
            entity.Property(e => e.Role).HasMaxLength(20);
            entity.Property(e => e.UserName).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
