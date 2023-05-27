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

        public virtual DbSet<AccountMod> AccountMods { get; set; } = null!;
        public virtual DbSet<AccountStudent> AccountStudents { get; set; } = null!;
        public virtual DbSet<AnswerVote> AnswerVotes { get; set; } = null!;
        public virtual DbSet<Campaign> Campaigns { get; set; } = null!;
        public virtual DbSet<CampaignDetail> CampaignDetails { get; set; } = null!;
        public virtual DbSet<CampaignType> CampaignTypes { get; set; } = null!;
        public virtual DbSet<Campus> Campuses { get; set; } = null!;
        public virtual DbSet<Department> Departments { get; set; } = null!;
        public virtual DbSet<DepartmentInCampus> DepartmentInCampuses { get; set; } = null!;
        public virtual DbSet<HistoryMod> HistoryMods { get; set; } = null!;
        public virtual DbSet<HistoryStudent> HistoryStudents { get; set; } = null!;
        public virtual DbSet<Major> Majors { get; set; } = null!;
        public virtual DbSet<Qrcode> Qrcodes { get; set; } = null!;
        public virtual DbSet<Question> Questions { get; set; } = null!;
        public virtual DbSet<Rating> Ratings { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Student> Students { get; set; } = null!;
        public virtual DbSet<Teacher> Teachers { get; set; } = null!;
        public virtual DbSet<TypeAction> TypeActions { get; set; } = null!;
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
            modelBuilder.Entity<AccountMod>(entity =>
            {
                entity.HasKey(e => e.Username)
                    .HasName("PK_Account_1");

                entity.ToTable("AccountMod");

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("username");

                entity.Property(e => e.Img)
                    .IsUnicode(false)
                    .HasColumnName("img");

                entity.Property(e => e.Name)
                    .HasMaxLength(200)
                    .HasColumnName("name");

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.RoleId).HasColumnName("role_Id");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Token)
                    .IsUnicode(false)
                    .HasColumnName("token");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AccountMods)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_Account_Role");
            });

            modelBuilder.Entity<AccountStudent>(entity =>
            {
                entity.HasKey(e => e.Mssv);

                entity.ToTable("AccountStudent");

                entity.Property(e => e.Mssv)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("mssv");

                entity.Property(e => e.Token)
                    .IsUnicode(false)
                    .HasColumnName("token");
            });

            modelBuilder.Entity<AnswerVote>(entity =>
            {
                entity.ToTable("AnswerVote");

                entity.Property(e => e.AnswerVoteId)
                    .ValueGeneratedNever()
                    .HasColumnName("answerVoteId");

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

                entity.Property(e => e.CampaignId)
                    .ValueGeneratedNever()
                    .HasColumnName("campaignId");

                entity.Property(e => e.CampaignTypeId).HasColumnName("campaignType_Id");

                entity.Property(e => e.CampusId).HasColumnName("campus_Id");

                entity.Property(e => e.EndTime)
                    .HasColumnType("datetime")
                    .HasColumnName("endTime");

                entity.Property(e => e.StartTime)
                    .HasColumnType("datetime")
                    .HasColumnName("startTime");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.HasOne(d => d.CampaignType)
                    .WithMany(p => p.Campaigns)
                    .HasForeignKey(d => d.CampaignTypeId)
                    .HasConstraintName("FK_Campaign_CampaignType");

                entity.HasOne(d => d.Campus)
                    .WithMany(p => p.Campaigns)
                    .HasForeignKey(d => d.CampusId)
                    .HasConstraintName("FK_Campaign_Campus");
            });

            modelBuilder.Entity<CampaignDetail>(entity =>
            {
                entity.ToTable("CampaignDetail");

                entity.Property(e => e.CampaignDetailId)
                    .ValueGeneratedNever()
                    .HasColumnName("campaignDetailId");

                entity.Property(e => e.AmountVote).HasColumnName("amountVote");

                entity.Property(e => e.CampaignId).HasColumnName("campaign_Id");

                entity.Property(e => e.Day)
                    .HasColumnType("date")
                    .HasColumnName("day");

                entity.HasOne(d => d.Campaign)
                    .WithMany(p => p.CampaignDetails)
                    .HasForeignKey(d => d.CampaignId)
                    .HasConstraintName("FK_CampaignDetail_Campaign");
            });

            modelBuilder.Entity<CampaignType>(entity =>
            {
                entity.ToTable("CampaignType");

                entity.Property(e => e.CampaignTypeId)
                    .ValueGeneratedNever()
                    .HasColumnName("campaignTypeId");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Campus>(entity =>
            {
                entity.ToTable("Campus");

                entity.Property(e => e.CampusId)
                    .ValueGeneratedNever()
                    .HasColumnName("campusId");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.ToTable("Department");

                entity.Property(e => e.DepartmentId)
                    .ValueGeneratedNever()
                    .HasColumnName("departmentId");

                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .HasColumnName("description");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<DepartmentInCampus>(entity =>
            {
                entity.ToTable("departmentInCampus");

                entity.Property(e => e.DepartmentInCampusId)
                    .ValueGeneratedNever()
                    .HasColumnName("departmentInCampusId");

                entity.Property(e => e.CampusId).HasColumnName("campus_Id");

                entity.Property(e => e.DepartmentId).HasColumnName("department_Id");

                entity.HasOne(d => d.Campus)
                    .WithMany(p => p.DepartmentInCampuses)
                    .HasForeignKey(d => d.CampusId)
                    .HasConstraintName("FK_departmentInCampus_Campus");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.DepartmentInCampuses)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("FK_departmentInCampus_Department");
            });

            modelBuilder.Entity<HistoryMod>(entity =>
            {
                entity.ToTable("HistoryMod");

                entity.Property(e => e.HistoryModId)
                    .ValueGeneratedNever()
                    .HasColumnName("historyModId");

                entity.Property(e => e.Description)
                    .HasMaxLength(200)
                    .HasColumnName("description");

                entity.Property(e => e.TypeActionId).HasColumnName("typeAction_Id");

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("username");

                entity.HasOne(d => d.TypeAction)
                    .WithMany(p => p.HistoryMods)
                    .HasForeignKey(d => d.TypeActionId)
                    .HasConstraintName("FK_HistoryMod_TypeAction");

                entity.HasOne(d => d.UsernameNavigation)
                    .WithMany(p => p.HistoryMods)
                    .HasForeignKey(d => d.Username)
                    .HasConstraintName("FK_HistoryMod_AccountMod");
            });

            modelBuilder.Entity<HistoryStudent>(entity =>
            {
                entity.ToTable("HistoryStudent");

                entity.Property(e => e.HistoryStudentId)
                    .ValueGeneratedNever()
                    .HasColumnName("historyStudentId");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Mssv)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("mssv");

                entity.Property(e => e.TypeActionId).HasColumnName("typeAction_Id");

                entity.HasOne(d => d.MssvNavigation)
                    .WithMany(p => p.HistoryStudents)
                    .HasForeignKey(d => d.Mssv)
                    .HasConstraintName("FK_HistoryStudent_Student");

                entity.HasOne(d => d.TypeAction)
                    .WithMany(p => p.HistoryStudents)
                    .HasForeignKey(d => d.TypeActionId)
                    .HasConstraintName("FK_HistoryStudent_TypeAction");
            });

            modelBuilder.Entity<Major>(entity =>
            {
                entity.ToTable("Major");

                entity.Property(e => e.MajorId)
                    .ValueGeneratedNever()
                    .HasColumnName("majorId");

                entity.Property(e => e.CampusId).HasColumnName("campus_Id");

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
                entity.HasKey(e => e.QrId);

                entity.ToTable("QRcode");

                entity.Property(e => e.QrId)
                    .ValueGeneratedNever()
                    .HasColumnName("qrId");

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

                entity.Property(e => e.QuestionId)
                    .ValueGeneratedNever()
                    .HasColumnName("questionId");

                entity.Property(e => e.CampaignId).HasColumnName("campaign_Id");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.QuestionOfCampaign)
                    .HasMaxLength(500)
                    .HasColumnName("questionOfCampaign");

                entity.HasOne(d => d.Campaign)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.CampaignId)
                    .HasConstraintName("FK_Question_Campaign");
            });

            modelBuilder.Entity<Rating>(entity =>
            {
                entity.ToTable("Rating");

                entity.Property(e => e.RatingId)
                    .ValueGeneratedNever()
                    .HasColumnName("ratingId");

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

                entity.Property(e => e.RoleId)
                    .ValueGeneratedNever()
                    .HasColumnName("roleId");

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

                entity.Property(e => e.K)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("k");

                entity.Property(e => e.MajorId).HasColumnName("major_Id");

                entity.HasOne(d => d.Major)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.MajorId)
                    .HasConstraintName("FK_Student_Major");
            });

            modelBuilder.Entity<Teacher>(entity =>
            {
                entity.ToTable("Teacher");

                entity.Property(e => e.TeacherId)
                    .ValueGeneratedNever()
                    .HasColumnName("teacherId");

                entity.Property(e => e.AmountVote).HasColumnName("amountVote");

                entity.Property(e => e.CampaignId).HasColumnName("campaign_Id");

                entity.Property(e => e.DepartmentId).HasColumnName("department_Id");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.Img)
                    .IsUnicode(false)
                    .HasColumnName("img");

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

            modelBuilder.Entity<TypeAction>(entity =>
            {
                entity.ToTable("TypeAction");

                entity.Property(e => e.TypeActionId)
                    .ValueGeneratedNever()
                    .HasColumnName("typeActionId");

                entity.Property(e => e.Name)
                    .HasMaxLength(200)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<VoteDetail>(entity =>
            {
                entity.ToTable("VoteDetail");

                entity.Property(e => e.VoteDetailId)
                    .ValueGeneratedNever()
                    .HasColumnName("voteDetailId");

                entity.Property(e => e.Mssv)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("mssv");

                entity.Property(e => e.TeacherId).HasColumnName("teacher_Id");

                entity.Property(e => e.Time)
                    .HasColumnType("datetime")
                    .HasColumnName("time");

                entity.HasOne(d => d.MssvNavigation)
                    .WithMany(p => p.VoteDetails)
                    .HasForeignKey(d => d.Mssv)
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
