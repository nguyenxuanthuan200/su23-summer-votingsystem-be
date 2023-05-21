using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Capstone_VotingSystem.Entities
{
    public partial class VotingSystemContext : DbContext
    {
        public VotingSystemContext()
        {
        }

        public VotingSystemContext(DbContextOptions<VotingSystemContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; } = null!;
        public virtual DbSet<AnswerVote> AnswerVotes { get; set; } = null!;
        public virtual DbSet<Campaign> Campaigns { get; set; } = null!;
        public virtual DbSet<CampaignType> CampaignTypes { get; set; } = null!;
        public virtual DbSet<Campus> Campuses { get; set; } = null!;
        public virtual DbSet<Department> Departments { get; set; } = null!;
        public virtual DbSet<Major> Majors { get; set; } = null!;
        public virtual DbSet<Qrcode> Qrcodes { get; set; } = null!;
        public virtual DbSet<Question> Questions { get; set; } = null!;
        public virtual DbSet<Rating> Ratings { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Student> Students { get; set; } = null!;
        public virtual DbSet<Teacher> Teachers { get; set; } = null!;
        public virtual DbSet<VoteDetail> VoteDetails { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-NNLOED3;Initial Catalog=VotingSystem;User ID=sa;Password=123456");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Account");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.RoleId).HasColumnName("role_Id");

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("username");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_Account_Role");
            });

            modelBuilder.Entity<AnswerVote>(entity =>
            {
                entity.ToTable("AnswerVote");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Answer).HasColumnName("answer");

                entity.Property(e => e.QuestionId).HasColumnName("question_Id");

                entity.Property(e => e.VoteDetailId).HasColumnName("voteDetail_Id");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.AnswerVotes)
                    .HasForeignKey(d => d.QuestionId)
                    .HasConstraintName("FK_AnswerVote_Question");

                entity.HasOne(d => d.VoteDetail)
                    .WithMany(p => p.AnswerVotes)
                    .HasForeignKey(d => d.VoteDetailId)
                    .HasConstraintName("FK_AnswerVote_VoteDetail");
            });

            modelBuilder.Entity<Campaign>(entity =>
            {
                entity.ToTable("Campaign");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.CampaignTypeId).HasColumnName("campaignType_Id");

                entity.Property(e => e.CampusId).HasColumnName("campus_Id");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.TimeEnd)
                    .HasColumnType("datetime")
                    .HasColumnName("timeEnd");

                entity.Property(e => e.TimeStart)
                    .HasColumnType("datetime")
                    .HasColumnName("timeStart");

                entity.HasOne(d => d.CampaignType)
                    .WithMany(p => p.Campaigns)
                    .HasForeignKey(d => d.CampaignTypeId)
                    .HasConstraintName("FK_Campaign_CampaignType");

                entity.HasOne(d => d.Campus)
                    .WithMany(p => p.Campaigns)
                    .HasForeignKey(d => d.CampusId)
                    .HasConstraintName("FK_Campaign_Campus");
            });

            modelBuilder.Entity<CampaignType>(entity =>
            {
                entity.ToTable("CampaignType");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Campus>(entity =>
            {
                entity.ToTable("Campus");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.ToTable("Department");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.CampusId).HasColumnName("campus_Id");

                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .HasColumnName("description");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.HasOne(d => d.Campus)
                    .WithMany(p => p.Departments)
                    .HasForeignKey(d => d.CampusId)
                    .HasConstraintName("FK_Department_Campus");
            });

            modelBuilder.Entity<Major>(entity =>
            {
                entity.ToTable("Major");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.CampusId).HasColumnName("campus_Id");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.HasOne(d => d.Campus)
                    .WithMany(p => p.Majors)
                    .HasForeignKey(d => d.CampusId)
                    .HasConstraintName("FK_Major_Campus");
            });

            modelBuilder.Entity<Qrcode>(entity =>
            {
                entity.ToTable("QRcode");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.CampaignId).HasColumnName("campaign_Id");

                entity.Property(e => e.Img)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("img");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Url)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("url");

                entity.HasOne(d => d.Campaign)
                    .WithMany(p => p.Qrcodes)
                    .HasForeignKey(d => d.CampaignId)
                    .HasConstraintName("FK_QRcode_Campaign");
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.ToTable("Question");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.CampaignId).HasColumnName("campaign_Id");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Question1)
                    .HasMaxLength(500)
                    .HasColumnName("question");

                entity.HasOne(d => d.Campaign)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.CampaignId)
                    .HasConstraintName("FK_Question_Campaign");
            });

            modelBuilder.Entity<Rating>(entity =>
            {
                entity.ToTable("Rating");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.DepartmentId).HasColumnName("department_Id");

                entity.Property(e => e.MajorId).HasColumnName("major_Id");

                entity.Property(e => e.Ratio).HasColumnName("ratio");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.Ratings)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("FK_Rating_Department");

                entity.HasOne(d => d.Major)
                    .WithMany(p => p.Ratings)
                    .HasForeignKey(d => d.MajorId)
                    .HasConstraintName("FK_Rating_Major");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.Mssv);

                entity.ToTable("Student");

                entity.Property(e => e.Mssv)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("mssv");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.Lock)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("lock");

                entity.Property(e => e.MajorId).HasColumnName("major_Id");

                entity.Property(e => e.RoleId).HasColumnName("role_Id");

                entity.HasOne(d => d.Major)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.MajorId)
                    .HasConstraintName("FK_Student_Major");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_Student_Role");
            });

            modelBuilder.Entity<Teacher>(entity =>
            {
                entity.ToTable("Teacher");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.AmountVote).HasColumnName("amountVote");

                entity.Property(e => e.CampaignId).HasColumnName("campaign_Id");

                entity.Property(e => e.DepartmentId).HasColumnName("department_Id");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.Score).HasColumnName("score");

                entity.HasOne(d => d.Campaign)
                    .WithMany(p => p.Teachers)
                    .HasForeignKey(d => d.CampaignId)
                    .HasConstraintName("FK_Teacher_Campaign");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.Teachers)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("FK_Teacher_Department");
            });

            modelBuilder.Entity<VoteDetail>(entity =>
            {
                entity.ToTable("VoteDetail");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.MssvStudent)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("mssv_Student");

                entity.Property(e => e.TeacherId).HasColumnName("teacher_Id");

                entity.Property(e => e.Time)
                    .HasColumnType("datetime")
                    .HasColumnName("time");

                entity.HasOne(d => d.MssvStudentNavigation)
                    .WithMany(p => p.VoteDetails)
                    .HasForeignKey(d => d.MssvStudent)
                    .HasConstraintName("FK_VoteDetail_Student");

                entity.HasOne(d => d.Teacher)
                    .WithMany(p => p.VoteDetails)
                    .HasForeignKey(d => d.TeacherId)
                    .HasConstraintName("FK_VoteDetail_Teacher");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
