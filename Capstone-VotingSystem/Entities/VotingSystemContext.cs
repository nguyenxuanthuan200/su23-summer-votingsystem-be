﻿using System;
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
        public virtual DbSet<ActionHistory> ActionHistories { get; set; } = null!;
        public virtual DbSet<ActionType> ActionTypes { get; set; } = null!;
        public virtual DbSet<Answer> Answers { get; set; } = null!;
        public virtual DbSet<Campaign> Campaigns { get; set; } = null!;
        public virtual DbSet<CampaignStage> CampaignStages { get; set; } = null!;
        public virtual DbSet<CandidateProfile> CandidateProfiles { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Element> Elements { get; set; } = null!;
        public virtual DbSet<Feedback> Feedbacks { get; set; } = null!;
        public virtual DbSet<Form> Forms { get; set; } = null!;
        public virtual DbSet<FormStage> FormStages { get; set; } = null!;
        public virtual DbSet<Notification> Notifications { get; set; } = null!;
        public virtual DbSet<Question> Questions { get; set; } = null!;
        public virtual DbSet<QuestionType> QuestionTypes { get; set; } = null!;
        public virtual DbSet<RatioCategory> RatioCategories { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Score> Scores { get; set; } = null!;
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
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("userName");

                entity.Property(e => e.Password)
                    .HasMaxLength(100)
                    .HasColumnName("password");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Token).HasColumnName("token");
            });

            modelBuilder.Entity<ActionHistory>(entity =>
            {
                entity.ToTable("ActionHistory");

                entity.Property(e => e.ActionHistoryId)
                    .ValueGeneratedNever()
                    .HasColumnName("actionHistoryId");

                entity.Property(e => e.ActionTypeId).HasColumnName("actionTypeId");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.UserName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("userName");

                entity.HasOne(d => d.ActionType)
                    .WithMany(p => p.ActionHistories)
                    .HasForeignKey(d => d.ActionTypeId)
                    .HasConstraintName("FK_ActionHistory_ActionType");

                entity.HasOne(d => d.UserNameNavigation)
                    .WithMany(p => p.ActionHistories)
                    .HasForeignKey(d => d.UserName)
                    .HasConstraintName("FK_ActionHistory_User");
            });

            modelBuilder.Entity<ActionType>(entity =>
            {
                entity.ToTable("ActionType");

                entity.Property(e => e.ActionTypeId)
                    .ValueGeneratedNever()
                    .HasColumnName("actionTypeId");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Answer>(entity =>
            {
                entity.ToTable("Answer");

                entity.Property(e => e.AnswerId)
                    .ValueGeneratedNever()
                    .HasColumnName("answerId");

                entity.Property(e => e.AnswerSelect).HasColumnName("answerSelect");

                entity.Property(e => e.QuestionId).HasColumnName("questionId");

                entity.Property(e => e.VotingDetailId).HasColumnName("votingDetailId");

                entity.HasOne(d => d.AnswerNavigation)
                    .WithOne(p => p.Answer)
                    .HasForeignKey<Answer>(d => d.AnswerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Answer_Element");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.Answers)
                    .HasForeignKey(d => d.QuestionId)
                    .HasConstraintName("FK_Answer_Question");

                entity.HasOne(d => d.VotingDetail)
                    .WithMany(p => p.Answers)
                    .HasForeignKey(d => d.VotingDetailId)
                    .HasConstraintName("FK_Answer_VotingDetail");
            });

            modelBuilder.Entity<Campaign>(entity =>
            {
                entity.ToTable("Campaign");

                entity.Property(e => e.CampaignId)
                    .ValueGeneratedNever()
                    .HasColumnName("campaignId");

                entity.Property(e => e.EndTime)
                    .HasColumnType("datetime")
                    .HasColumnName("endTime");

                entity.Property(e => e.StartTime)
                    .HasColumnType("datetime")
                    .HasColumnName("startTime");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Title)
                    .HasMaxLength(50)
                    .HasColumnName("title");

                entity.Property(e => e.UserName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("userName");

                entity.Property(e => e.Visibility).HasColumnName("visibility");

                entity.HasOne(d => d.UserNameNavigation)
                    .WithMany(p => p.Campaigns)
                    .HasForeignKey(d => d.UserName)
                    .HasConstraintName("FK_Campaign_User");
            });

            modelBuilder.Entity<CampaignStage>(entity =>
            {
                entity.ToTable("CampaignStage");

                entity.Property(e => e.CampaignStageId)
                    .ValueGeneratedNever()
                    .HasColumnName("campaignStageId");

                entity.Property(e => e.CampaignId).HasColumnName("campaignId");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.EndTime)
                    .HasColumnType("datetime")
                    .HasColumnName("endTime");

                entity.Property(e => e.StartTime)
                    .HasColumnType("datetime")
                    .HasColumnName("startTime");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Text).HasColumnName("text");

                entity.Property(e => e.Title)
                    .HasMaxLength(50)
                    .HasColumnName("title");

                entity.HasOne(d => d.Campaign)
                    .WithMany(p => p.CampaignStages)
                    .HasForeignKey(d => d.CampaignId)
                    .HasConstraintName("FK_CampaignStage_Campaign");
            });

            modelBuilder.Entity<CandidateProfile>(entity =>
            {
                entity.ToTable("CandidateProfile");

                entity.Property(e => e.CandidateProfileId)
                    .ValueGeneratedNever()
                    .HasColumnName("candidateProfileId");

                entity.Property(e => e.CampaignId).HasColumnName("campaignId");

                entity.Property(e => e.Dob)
                    .HasColumnType("date")
                    .HasColumnName("dob");

                entity.Property(e => e.Image)
                    .HasMaxLength(50)
                    .HasColumnName("image");

                entity.Property(e => e.NickName)
                    .HasMaxLength(50)
                    .HasColumnName("nickName");

                entity.Property(e => e.UserName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("userName");

                entity.HasOne(d => d.Campaign)
                    .WithMany(p => p.CandidateProfiles)
                    .HasForeignKey(d => d.CampaignId)
                    .HasConstraintName("FK_CandidateProfile_Campaign");

                entity.HasOne(d => d.CandidateProfileNavigation)
                    .WithOne(p => p.CandidateProfile)
                    .HasForeignKey<CandidateProfile>(d => d.CandidateProfileId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CandidateProfile_Score");

                entity.HasOne(d => d.UserNameNavigation)
                    .WithMany(p => p.CandidateProfiles)
                    .HasForeignKey(d => d.UserName)
                    .HasConstraintName("FK_CandidateProfile_User");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.CategoryId)
                    .ValueGeneratedNever()
                    .HasColumnName("categoryId");

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

                entity.Property(e => e.QuestionId).HasColumnName("questionId");

                entity.Property(e => e.Text).HasColumnName("text");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.Elements)
                    .HasForeignKey(d => d.QuestionId)
                    .HasConstraintName("FK_Element_Question");
            });

            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.ToTable("Feedback");

                entity.Property(e => e.FeedbackId)
                    .ValueGeneratedNever()
                    .HasColumnName("feedbackId");

                entity.Property(e => e.Text).HasColumnName("text");

                entity.Property(e => e.Title)
                    .HasMaxLength(50)
                    .HasColumnName("title");

                entity.Property(e => e.UserName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("userName");

                entity.HasOne(d => d.UserNameNavigation)
                    .WithMany(p => p.Feedbacks)
                    .HasForeignKey(d => d.UserName)
                    .HasConstraintName("FK_Feedback_User");
            });

            modelBuilder.Entity<Form>(entity =>
            {
                entity.ToTable("Form");

                entity.Property(e => e.FormId)
                    .ValueGeneratedNever()
                    .HasColumnName("formId");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.UserName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("userName");

                entity.HasOne(d => d.UserNameNavigation)
                    .WithMany(p => p.Forms)
                    .HasForeignKey(d => d.UserName)
                    .HasConstraintName("FK_Form_User");
            });

            modelBuilder.Entity<FormStage>(entity =>
            {
                entity.ToTable("FormStage");

                entity.Property(e => e.FormStageId)
                    .ValueGeneratedNever()
                    .HasColumnName("formStageId");

                entity.Property(e => e.FormId).HasColumnName("formId");

                entity.HasOne(d => d.Form)
                    .WithMany(p => p.FormStages)
                    .HasForeignKey(d => d.FormId)
                    .HasConstraintName("FK_FormStage_Form");

                entity.HasOne(d => d.FormStageNavigation)
                    .WithOne(p => p.FormStage)
                    .HasForeignKey<FormStage>(d => d.FormStageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FormStage_CampaignStage");
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.ToTable("Notification");

                entity.Property(e => e.NotificationId)
                    .ValueGeneratedNever()
                    .HasColumnName("notificationId");

                entity.Property(e => e.Text).HasColumnName("text");

                entity.Property(e => e.Titile)
                    .HasMaxLength(50)
                    .HasColumnName("titile");

                entity.Property(e => e.UserName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("userName");

                entity.HasOne(d => d.UserNameNavigation)
                    .WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.UserName)
                    .HasConstraintName("FK_Notification_User");
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.ToTable("Question");

                entity.Property(e => e.QuestionId)
                    .ValueGeneratedNever()
                    .HasColumnName("questionId");

                entity.Property(e => e.FormId).HasColumnName("formId");

                entity.Property(e => e.QuestionName)
                    .HasMaxLength(100)
                    .HasColumnName("questionName");

                entity.Property(e => e.QuestionTypeId).HasColumnName("questionTypeId");

                entity.HasOne(d => d.Form)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.FormId)
                    .HasConstraintName("FK_Question_Form");

                entity.HasOne(d => d.QuestionType)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.QuestionTypeId)
                    .HasConstraintName("FK_Question_QuestionType");
            });

            modelBuilder.Entity<QuestionType>(entity =>
            {
                entity.ToTable("QuestionType");

                entity.Property(e => e.QuestionTypeId)
                    .ValueGeneratedNever()
                    .HasColumnName("questionTypeId");

                entity.Property(e => e.TypeName)
                    .HasMaxLength(50)
                    .HasColumnName("typeName");
            });

            modelBuilder.Entity<RatioCategory>(entity =>
            {
                entity.ToTable("RatioCategory");

                entity.Property(e => e.RatioCategoryId)
                    .ValueGeneratedNever()
                    .HasColumnName("ratioCategoryId");

                entity.Property(e => e.CampaignId).HasColumnName("campaignId");

                entity.Property(e => e.CategoryId1).HasColumnName("categoryId1");

                entity.Property(e => e.CategoryId2).HasColumnName("categoryId2");

                entity.Property(e => e.CheckRatio).HasColumnName("checkRatio");

                entity.Property(e => e.Percent)
                    .HasColumnType("decimal(3, 2)")
                    .HasColumnName("percent");

                entity.Property(e => e.Ratio).HasColumnName("ratio");

                entity.HasOne(d => d.Campaign)
                    .WithMany(p => p.RatioCategories)
                    .HasForeignKey(d => d.CampaignId)
                    .HasConstraintName("FK_RatioCategory_Campaign");

                entity.HasOne(d => d.CategoryId1Navigation)
                    .WithMany(p => p.RatioCategoryCategoryId1Navigations)
                    .HasForeignKey(d => d.CategoryId1)
                    .HasConstraintName("FK_RatioCategory_Category");

                entity.HasOne(d => d.CategoryId2Navigation)
                    .WithMany(p => p.RatioCategoryCategoryId2Navigations)
                    .HasForeignKey(d => d.CategoryId2)
                    .HasConstraintName("FK_RatioCategory_Category1");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.RoleId)
                    .ValueGeneratedNever()
                    .HasColumnName("roleId");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Score>(entity =>
            {
                entity.ToTable("Score");

                entity.Property(e => e.ScoreId)
                    .ValueGeneratedNever()
                    .HasColumnName("scoreId");

                entity.Property(e => e.Count).HasColumnName("count");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserName);

                entity.ToTable("User");

                entity.Property(e => e.UserName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("userName");

                entity.Property(e => e.Address)
                    .HasMaxLength(100)
                    .HasColumnName("address");

                entity.Property(e => e.CategoryId).HasColumnName("categoryId");

                entity.Property(e => e.Gender)
                    .HasMaxLength(50)
                    .HasColumnName("gender");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.RoleId).HasColumnName("roleId");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_User_Category");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_User_Role");

                entity.HasOne(d => d.UserNameNavigation)
                    .WithOne(p => p.User)
                    .HasForeignKey<User>(d => d.UserName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_Account");
            });

            modelBuilder.Entity<Voting>(entity =>
            {
                entity.ToTable("Voting");

                entity.Property(e => e.VotingId)
                    .ValueGeneratedNever()
                    .HasColumnName("votingId");

                entity.Property(e => e.CampaignStageId).HasColumnName("campaignStageId");

                entity.Property(e => e.Time)
                    .HasColumnType("datetime")
                    .HasColumnName("time");

                entity.Property(e => e.UserName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("userName");

                entity.HasOne(d => d.CampaignStage)
                    .WithMany(p => p.Votings)
                    .HasForeignKey(d => d.CampaignStageId)
                    .HasConstraintName("FK_Voting_CampaignStage");

                entity.HasOne(d => d.UserNameNavigation)
                    .WithMany(p => p.Votings)
                    .HasForeignKey(d => d.UserName)
                    .HasConstraintName("FK_Voting_User");
            });

            modelBuilder.Entity<VotingDetail>(entity =>
            {
                entity.ToTable("VotingDetail");

                entity.Property(e => e.VotingDetailId)
                    .ValueGeneratedNever()
                    .HasColumnName("votingDetailId");

                entity.Property(e => e.CandidateProfileId).HasColumnName("candidateProfileId");

                entity.Property(e => e.FormStageId).HasColumnName("formStageId");

                entity.Property(e => e.RatioCategoryId).HasColumnName("ratioCategoryId");

                entity.Property(e => e.Time)
                    .HasColumnType("datetime")
                    .HasColumnName("time");

                entity.Property(e => e.VotingId).HasColumnName("votingId");

                entity.HasOne(d => d.CandidateProfile)
                    .WithMany(p => p.VotingDetails)
                    .HasForeignKey(d => d.CandidateProfileId)
                    .HasConstraintName("FK_VotingDetail_CandidateProfile");

                entity.HasOne(d => d.FormStage)
                    .WithMany(p => p.VotingDetails)
                    .HasForeignKey(d => d.FormStageId)
                    .HasConstraintName("FK_VotingDetail_FormStage");

                entity.HasOne(d => d.RatioCategory)
                    .WithMany(p => p.VotingDetails)
                    .HasForeignKey(d => d.RatioCategoryId)
                    .HasConstraintName("FK_VotingDetail_RatioCategory");

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
