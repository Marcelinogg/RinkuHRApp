using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace RinkuHRApp.Data
{
    public partial class HumanResourcesContext : DbContext
    {
        public HumanResourcesContext()
        {
        }

        public HumanResourcesContext(DbContextOptions<HumanResourcesContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Concept> Concepts { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<EmployeeStatus> EmployeeStatuses { get; set; }
        public virtual DbSet<Payroll> Payrolls { get; set; }
        public virtual DbSet<Period> Periods { get; set; }
        public virtual DbSet<Position> Positions { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<VwPayrollConcept> VwPayrollConcepts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=LAPTOP-VAA09C66\\SQLEXPRESS;Database=dbHumanResources;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Concept>(entity =>
            {
                entity.ToTable("Concepts", "Payroll");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.SalaryPerHour).HasColumnType("money");

                entity.HasOne(d => d.Payroll)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.PayrollId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Employees_Payrolls");

                entity.HasOne(d => d.Position)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.PositionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Employees_Positions");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Employees_EmployeeStatus");
            });

            modelBuilder.Entity<EmployeeStatus>(entity =>
            {
                entity.ToTable("EmployeeStatus");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Payroll>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Period>(entity =>
            {
                entity.HasKey(e => new { e.PayrollId, e.Id })
                    .HasName("PK_PayrollPeriods");

                entity.ToTable("Periods", "Payroll");

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.PaymentDate).HasColumnType("date");

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.HasOne(d => d.Payroll)
                    .WithMany(p => p.Periods)
                    .HasForeignKey(d => d.PayrollId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PayrollPeriods_Payrolls");
            });

            modelBuilder.Entity<Position>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(e => new { e.PayrollId, e.PeriodId, e.ConceptId, e.EmployeeId, e.Sequence })
                    .HasName("PK_PayrollTransactions");

                entity.ToTable("Transactions", "Payroll");

                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("amount");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Times).HasColumnName("times");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Concept)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.ConceptId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PayrollTransactions_PayrollConcepts");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PayrollTransactions_Employees");

                entity.HasOne(d => d.Payroll)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.PayrollId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PayrollTransactions_Payrolls");

                entity.HasOne(d => d.P)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => new { d.PayrollId, d.PeriodId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PayrollTransactions_PayrollPeriods");
            });

            modelBuilder.Entity<VwPayrollConcept>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vwPayrollConcepts", "Payroll");

                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("amount");

                entity.Property(e => e.ConceptName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.EmployeeFullName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.PositionName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.TypeConcept)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
