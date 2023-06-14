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
        public virtual DbSet<Campaign> Campaigns { get; set; } = null!;
        public virtual DbSet<Candidate> Candidates { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Element> Elements { get; set; } = null!;
        public virtual DbSet<FeedBack> FeedBacks { get; set; } = null!;
        public virtual DbSet<Form> Forms { get; set; } = null!;
        public virtual DbSet<Group> Groups { get; set; } = null!;
        public virtual DbSet<HistoryAction> HistoryActions { get; set; } = null!;
        public virtual DbSet<Notification> Notifications { get; set; } = null!;
        public virtual DbSet<Question> Questions { get; set; } = null!;
        public virtual DbSet<Ratio> Ratios { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Score> Scores { get; set; } = null!;
        public virtual DbSet<Stage> Stages { get; set; } = null!;
        public virtual DbSet<Type> Types { get; set; } = null!;
        public virtual DbSet<TypeAction> TypeActions { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Voting> Votings { get; set; } = null!;
        public virtual DbSet<VotingDetail> VotingDetails { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=sql8003.site4now.net;Initial Catalog=db_a9a782_votingsystem;Persist Security Info=True;User ID=db_a9a782_votingsystem_admin;Password=votingsystem123");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.UserName);

                entity.ToTable("Account");

                entity.Property(e => e.UserName)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("userName");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("datetime")
                    .HasColumnName("createAt");

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.RoleId).HasColumnName("roleId");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Token).HasColumnName("token");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_Account_Role");
            });

            modelBuilder.Entity<Campaign>(entity =>
            {
                entity.ToTable("Campaign");

                entity.Property(e => e.CampaignId)
                    .ValueGeneratedNever()
                    .HasColumnName("campaignId");

                entity.Property(e => e.CategoryId).HasColumnName("categoryId");

                entity.Property(e => e.EndTime)
                    .HasColumnType("datetime")
                    .HasColumnName("endTime");

                entity.Property(e => e.ImgUrl)
                    .IsUnicode(false)
                    .HasColumnName("imgURL");

                entity.Property(e => e.StartTime)
                    .HasColumnType("datetime")
                    .HasColumnName("startTime");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Title)
                    .HasMaxLength(200)
                    .HasColumnName("title");

                entity.Property(e => e.UserId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("userId");

                entity.Property(e => e.Visibility)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("visibility");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Campaigns)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Campaign_Category");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Campaigns)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Campaign_User");
            });

            modelBuilder.Entity<Candidate>(entity =>
            {
                entity.ToTable("Candidate");

                entity.Property(e => e.CandidateId)
                    .ValueGeneratedNever()
                    .HasColumnName("candidateId");

                entity.Property(e => e.CampaignId).HasColumnName("campaignId");

                entity.Property(e => e.Description)
                    .HasMaxLength(200)
                    .HasColumnName("description");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.UserId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("userId");

                entity.HasOne(d => d.Campaign)
                    .WithMany(p => p.Candidates)
                    .HasForeignKey(d => d.CampaignId)
                    .HasConstraintName("FK_CandidateProfile_Campaign");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Candidates)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_CandidateProfile_User");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.CategoryId)
                    .ValueGeneratedNever()
                    .HasColumnName("categoryId");

                entity.Property(e => e.Description)
                    .HasMaxLength(200)
                    .HasColumnName("description");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Element>(entity =>
            {
                entity.ToTable("Element");

                entity.Property(e => e.ElementId)
                    .ValueGeneratedNever()
                    .HasColumnName("elementId");

                entity.Property(e => e.Content)
                    .HasMaxLength(50)
                    .HasColumnName("content");

                entity.Property(e => e.QuestionId).HasColumnName("questionId");

                entity.Property(e => e.Rate)
                    .HasColumnType("decimal(10, 0)")
                    .HasColumnName("rate");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.Elements)
                    .HasForeignKey(d => d.QuestionId)
                    .HasConstraintName("FK_Element_Question");
            });

            modelBuilder.Entity<FeedBack>(entity =>
            {
                entity.ToTable("FeedBack");

                entity.Property(e => e.FeedBackId)
                    .ValueGeneratedNever()
                    .HasColumnName("feedBackId");

                entity.Property(e => e.CampaignId).HasColumnName("campaignId");

                entity.Property(e => e.Content)
                    .HasMaxLength(100)
                    .HasColumnName("content");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("date")
                    .HasColumnName("createDate");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.UserId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("userId");

                entity.HasOne(d => d.Campaign)
                    .WithMany(p => p.FeedBacks)
                    .HasForeignKey(d => d.CampaignId)
                    .HasConstraintName("FK_FeedBack_Campaign");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.FeedBacks)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_FeedBack_User");
            });

            modelBuilder.Entity<Form>(entity =>
            {
                entity.ToTable("Form");

                entity.Property(e => e.FormId)
                    .ValueGeneratedNever()
                    .HasColumnName("formId");

                entity.Property(e => e.CategoryId).HasColumnName("categoryId");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.UserId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("userId");

                entity.Property(e => e.Visibility)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("visibility");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Forms)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Form_Category");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Forms)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Form_User");
            });

            modelBuilder.Entity<Group>(entity =>
            {
                entity.ToTable("Group");

                entity.Property(e => e.GroupId)
                    .ValueGeneratedNever()
                    .HasColumnName("groupId");

                entity.Property(e => e.Description)
                    .HasMaxLength(200)
                    .HasColumnName("description");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<HistoryAction>(entity =>
            {
                entity.ToTable("HistoryAction");

                entity.Property(e => e.HistoryActionId)
                    .ValueGeneratedNever()
                    .HasColumnName("historyActionId");

                entity.Property(e => e.Description)
                    .HasMaxLength(100)
                    .HasColumnName("description");

                entity.Property(e => e.TypeActionId).HasColumnName("typeActionId");

                entity.Property(e => e.UserId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("userId");

                entity.HasOne(d => d.TypeAction)
                    .WithMany(p => p.HistoryActions)
                    .HasForeignKey(d => d.TypeActionId)
                    .HasConstraintName("FK_HistoryAction_TypeAction");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.HistoryActions)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_HistoryAction_User");
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.ToTable("Notification");

                entity.Property(e => e.NotificationId)
                    .ValueGeneratedNever()
                    .HasColumnName("notificationId");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("date")
                    .HasColumnName("createDate");

                entity.Property(e => e.Message)
                    .HasMaxLength(200)
                    .HasColumnName("message");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Title)
                    .HasMaxLength(50)
                    .HasColumnName("title");

                entity.Property(e => e.Username)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("username");

                entity.HasOne(d => d.UsernameNavigation)
                    .WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.Username)
                    .HasConstraintName("FK_Notification_Account");
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.ToTable("Question");

                entity.Property(e => e.QuestionId)
                    .ValueGeneratedNever()
                    .HasColumnName("questionId");

                entity.Property(e => e.Content)
                    .HasMaxLength(100)
                    .HasColumnName("content");

                entity.Property(e => e.FormId).HasColumnName("formId");

                entity.Property(e => e.Title)
                    .HasMaxLength(200)
                    .HasColumnName("title");

                entity.Property(e => e.TypeId).HasColumnName("typeId");

                entity.HasOne(d => d.Form)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.FormId)
                    .HasConstraintName("FK_Question_Form");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.TypeId)
                    .HasConstraintName("FK_Question_Type");
            });

            modelBuilder.Entity<Ratio>(entity =>
            {
                entity.HasKey(e => e.RatioGroupId);

                entity.ToTable("Ratio");

                entity.Property(e => e.RatioGroupId)
                    .ValueGeneratedNever()
                    .HasColumnName("ratioGroupId");

                entity.Property(e => e.CampaignId).HasColumnName("campaignId");

                entity.Property(e => e.GroupId).HasColumnName("groupId");

                entity.Property(e => e.Percent)
                    .HasColumnType("decimal(3, 2)")
                    .HasColumnName("percent");

                entity.HasOne(d => d.Campaign)
                    .WithMany(p => p.Ratios)
                    .HasForeignKey(d => d.CampaignId)
                    .HasConstraintName("FK_Ratio_Campaign");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.Ratios)
                    .HasForeignKey(d => d.GroupId)
                    .HasConstraintName("FK_Ratio_Group");

                entity.HasOne(d => d.RatioGroup)
                    .WithOne(p => p.Ratio)
                    .HasForeignKey<Ratio>(d => d.RatioGroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ratio_Candidate");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.RoleId)
                    .ValueGeneratedNever()
                    .HasColumnName("roleId");

                entity.Property(e => e.Name)
                    .HasMaxLength(20)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Score>(entity =>
            {
                entity.ToTable("Score");

                entity.Property(e => e.ScoreId)
                    .ValueGeneratedNever()
                    .HasColumnName("scoreId");

                entity.Property(e => e.CandidateId).HasColumnName("candidateId");

                entity.Property(e => e.Score1).HasColumnName("score");

                entity.Property(e => e.StageId).HasColumnName("stageId");

                entity.HasOne(d => d.Candidate)
                    .WithMany(p => p.Scores)
                    .HasForeignKey(d => d.CandidateId)
                    .HasConstraintName("FK_Score_Candidate");

                entity.HasOne(d => d.Stage)
                    .WithMany(p => p.Scores)
                    .HasForeignKey(d => d.StageId)
                    .HasConstraintName("FK_Score_Stage");
            });

            modelBuilder.Entity<Stage>(entity =>
            {
                entity.ToTable("Stage");

                entity.Property(e => e.StageId)
                    .ValueGeneratedNever()
                    .HasColumnName("stageId");

                entity.Property(e => e.CampaignId).HasColumnName("campaignId");

                entity.Property(e => e.Content)
                    .HasMaxLength(50)
                    .HasColumnName("content");

                entity.Property(e => e.Description)
                    .HasMaxLength(200)
                    .HasColumnName("description");

                entity.Property(e => e.EndTime)
                    .HasColumnType("datetime")
                    .HasColumnName("endTime");

                entity.Property(e => e.FormId).HasColumnName("formId");

                entity.Property(e => e.StartTime)
                    .HasColumnType("datetime")
                    .HasColumnName("startTime");

                entity.Property(e => e.Title)
                    .HasMaxLength(50)
                    .HasColumnName("title");

                entity.HasOne(d => d.Campaign)
                    .WithMany(p => p.Stages)
                    .HasForeignKey(d => d.CampaignId)
                    .HasConstraintName("FK_Stage_Campaign");

                entity.HasOne(d => d.Form)
                    .WithMany(p => p.Stages)
                    .HasForeignKey(d => d.FormId)
                    .HasConstraintName("FK_Stage_Form");
            });

            modelBuilder.Entity<Type>(entity =>
            {
                entity.ToTable("Type");

                entity.Property(e => e.TypeId)
                    .ValueGeneratedNever()
                    .HasColumnName("typeId");

                entity.Property(e => e.Description)
                    .HasMaxLength(200)
                    .HasColumnName("description");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<TypeAction>(entity =>
            {
                entity.ToTable("TypeAction");

                entity.Property(e => e.TypeActionId)
                    .ValueGeneratedNever()
                    .HasColumnName("typeActionId");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.UserId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("userId");

                entity.Property(e => e.Address)
                    .HasMaxLength(50)
                    .HasColumnName("address");

                entity.Property(e => e.AvatarUrl)
                    .IsUnicode(false)
                    .HasColumnName("avatarURL");

                entity.Property(e => e.Dob)
                    .HasColumnType("date")
                    .HasColumnName("dob");

                entity.Property(e => e.Email)
                    .HasMaxLength(55)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.FullName)
                    .HasMaxLength(100)
                    .HasColumnName("fullName");

                entity.Property(e => e.Gender)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("gender");

                entity.Property(e => e.GroupId).HasColumnName("groupId");

                entity.Property(e => e.Phone)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("phone");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.GroupId)
                    .HasConstraintName("FK_User_Group");

                entity.HasOne(d => d.UserNavigation)
                    .WithOne(p => p.User)
                    .HasForeignKey<User>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_Account");
            });

            modelBuilder.Entity<Voting>(entity =>
            {
                entity.HasKey(e => e.VoringId);

                entity.ToTable("Voting");

                entity.Property(e => e.VoringId)
                    .ValueGeneratedNever()
                    .HasColumnName("voringId");

                entity.Property(e => e.CandidateId).HasColumnName("candidateId");

                entity.Property(e => e.RatioGroupId).HasColumnName("ratioGroupId");

                entity.Property(e => e.SendingTime)
                    .HasColumnType("datetime")
                    .HasColumnName("sendingTime");

                entity.Property(e => e.StageId).HasColumnName("stageId");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.UserId)
                    .HasMaxLength(36)
                    .IsUnicode(false)
                    .HasColumnName("userId");

                entity.HasOne(d => d.Candidate)
                    .WithMany(p => p.Votings)
                    .HasForeignKey(d => d.CandidateId)
                    .HasConstraintName("FK_Voting_CandidateProfile");

                entity.HasOne(d => d.RatioGroup)
                    .WithMany(p => p.Votings)
                    .HasForeignKey(d => d.RatioGroupId)
                    .HasConstraintName("FK_Voting_Ratio");

                entity.HasOne(d => d.Stage)
                    .WithMany(p => p.Votings)
                    .HasForeignKey(d => d.StageId)
                    .HasConstraintName("FK_Voting_Stage");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Votings)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Voting_User");
            });

            modelBuilder.Entity<VotingDetail>(entity =>
            {
                entity.ToTable("VotingDetail");

                entity.Property(e => e.VotingDetailId)
                    .ValueGeneratedNever()
                    .HasColumnName("votingDetailId");

                entity.Property(e => e.CreateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("createTime");

                entity.Property(e => e.ElementId).HasColumnName("elementId");

                entity.Property(e => e.VotingId).HasColumnName("votingId");

                entity.HasOne(d => d.Element)
                    .WithMany(p => p.VotingDetails)
                    .HasForeignKey(d => d.ElementId)
                    .HasConstraintName("FK_VotingDetail_Element");

                entity.HasOne(d => d.Voting)
                    .WithMany(p => p.VotingDetails)
                    .HasForeignKey(d => d.VotingId)
                    .HasConstraintName("FK_VotingDetail_Voting");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
