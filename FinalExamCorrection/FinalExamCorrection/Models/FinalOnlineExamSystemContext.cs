﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FinalExamCorrection.Models;

public partial class FinalOnlineExamSystemContext : DbContext
{
    public FinalOnlineExamSystemContext(DbContextOptions<FinalOnlineExamSystemContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Answer> Answers { get; set; }

    public virtual DbSet<Choice> Choices { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Exam> Exams { get; set; }

    public virtual DbSet<Instructor> Instructors { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Topic> Topics { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Answer>(entity =>
        {
            entity.HasKey(e => new { e.StudId, e.QuestionId, e.ExamId });

            entity.ToTable("Answer");

            entity.Property(e => e.StudId).HasColumnName("stud_id");
            entity.Property(e => e.QuestionId).HasColumnName("question_id");
            entity.Property(e => e.ExamId).HasColumnName("exam_id");
            entity.Property(e => e.ChoiceId).HasColumnName("choice_id");

            entity.HasOne(d => d.Exam).WithMany(p => p.Answers)
                .HasForeignKey(d => d.ExamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Answer_Exam");

            entity.HasOne(d => d.Question).WithMany(p => p.Answers)
                .HasForeignKey(d => d.QuestionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Answer_Question");

            entity.HasOne(d => d.Stud).WithMany(p => p.Answers)
                .HasForeignKey(d => d.StudId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Answer_Student");

            entity.HasOne(d => d.Choice).WithMany(p => p.Answers)
                .HasForeignKey(d => new { d.ChoiceId, d.QuestionId })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Answer_Choice");
        });

        modelBuilder.Entity<Choice>(entity =>
        {
            entity.HasKey(e => new { e.ChoiceId, e.QuestionId });

            entity.ToTable("Choice");

            entity.Property(e => e.ChoiceId)
                .ValueGeneratedOnAdd()
                .HasColumnName("choice_id");
            entity.Property(e => e.QuestionId).HasColumnName("question_id");
            entity.Property(e => e.IsCorrect).HasColumnName("is_correct");
            entity.Property(e => e.Text)
                .IsRequired()
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("text");

            entity.HasOne(d => d.Question).WithMany(p => p.Choices)
                .HasForeignKey(d => d.QuestionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Choice_question");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("PK__Course__37E309C3460B3936");

            entity.ToTable("Course");

            entity.Property(e => e.CourseId).HasColumnName("Course_id");
            entity.Property(e => e.CourseName)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("course_name");
            entity.Property(e => e.DeptId).HasColumnName("dept_id");
            entity.Property(e => e.InsId)
                .IsRequired()
                .HasMaxLength(450)
                .HasColumnName("ins_id");

            entity.HasOne(d => d.Dept).WithMany(p => p.Courses)
                .HasForeignKey(d => d.DeptId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Course_department");

            entity.HasOne(d => d.Ins).WithMany(p => p.Courses)
                .HasForeignKey(d => d.InsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Course_Instructor");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.DeptId).HasName("PK__Departme__DCA65974734F66F9");

            entity.ToTable("Department");

            entity.Property(e => e.DeptId).HasColumnName("dept_id");
            entity.Property(e => e.DeptName)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("dept_name");
        });

        modelBuilder.Entity<Exam>(entity =>
        {
            entity.HasKey(e => e.ExamId).HasName("PK__Exam__C79BD6715DE384AF");

            entity.ToTable("Exam");

            entity.Property(e => e.ExamId).HasColumnName("Exam_id");
            entity.Property(e => e.CrsId).HasColumnName("crs_id");
            entity.Property(e => e.Date).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Duration)
                .HasDefaultValue(2)
                .HasColumnName("duration");
            entity.Property(e => e.Score).HasColumnName("score");
            entity.Property(e => e.StudId)
                .HasMaxLength(450)
                .HasColumnName("stud_id");
            entity.Property(e => e.TotalMarks).HasColumnName("total_marks");

            entity.HasOne(d => d.Crs).WithMany(p => p.Exams)
                .HasForeignKey(d => d.CrsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Exam_Course");

            entity.HasOne(d => d.Stud).WithMany(p => p.Exams)
                .HasForeignKey(d => d.StudId)
                .HasConstraintName("FK_Exam_Student");

            entity.HasMany(d => d.Questions).WithMany(p => p.Exams)
                .UsingEntity<Dictionary<string, object>>(
                    "ExamQuestion",
                    r => r.HasOne<Question>().WithMany()
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_ExamQuestions_Question"),
                    l => l.HasOne<Exam>().WithMany()
                        .HasForeignKey("ExamId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_ExamQuestions_Exam"),
                    j =>
                    {
                        j.HasKey("ExamId", "QuestionId");
                        j.ToTable("ExamQuestions");
                        j.IndexerProperty<int>("ExamId").HasColumnName("exam_id");
                        j.IndexerProperty<int>("QuestionId").HasColumnName("question_id");
                    });
        });

        modelBuilder.Entity<Instructor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Instruct__3214EC078FC34180");

            entity.ToTable("Instructor");

            entity.Property(e => e.DeptId).HasColumnName("dept_id");
            entity.Property(e => e.InsName)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ins_name");

            entity.HasOne(d => d.Dept).WithMany(p => p.Instructors)
                .HasForeignKey(d => d.DeptId)
                .HasConstraintName("FK_Instructor_department");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.QestId).HasName("PK__Question__0E446F55B9919C35");

            entity.ToTable("Question");

            entity.Property(e => e.QestId).HasColumnName("qest_id");
            entity.Property(e => e.CrsId).HasColumnName("crsID");
            entity.Property(e => e.Level)
                .IsRequired()
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("level");
            entity.Property(e => e.Text)
                .IsRequired()
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("text");
            entity.Property(e => e.Type)
                .IsRequired()
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("type");

            entity.HasOne(d => d.Crs).WithMany(p => p.Questions)
                .HasForeignKey(d => d.CrsId)
                .HasConstraintName("FK_Question_Course");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Student__3214EC07DE1166A7");

            entity.ToTable("Student");

            entity.Property(e => e.DeptId).HasColumnName("dept_id");
            entity.Property(e => e.Fname)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Lname)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(300)
                .IsUnicode(false);

            entity.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Dept).WithMany(p => p.Students)
                .HasForeignKey(d => d.DeptId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Student_department");

            entity.HasMany(d => d.Courses).WithMany(p => p.Stds)
                .UsingEntity<Dictionary<string, object>>(
                    "StudentCourse",
                    r => r.HasOne<Course>().WithMany()
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_StudentCourses_Course"),
                    l => l.HasOne<Student>().WithMany()
                        .HasForeignKey("StdId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_StudentCourses_Student"),
                    j =>
                    {
                        j.HasKey("StdId", "CourseId");
                        j.ToTable("StudentCourses");
                        j.IndexerProperty<string>("StdId").HasColumnName("Std_id");
                        j.IndexerProperty<int>("CourseId").HasColumnName("Course_id");
                    });
        });

        modelBuilder.Entity<Topic>(entity =>
        {
            entity.HasKey(e => e.TopicId).HasName("PK__Topic__8DEBA02D834831CC");

            entity.ToTable("Topic");

            entity.Property(e => e.TopicId).HasColumnName("Topic_id");
            entity.Property(e => e.CrsId).HasColumnName("crs_id");
            entity.Property(e => e.TopicName)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Topic_name");

            entity.HasOne(d => d.Crs).WithMany(p => p.Topics)
                .HasForeignKey(d => d.CrsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Topic_Course");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}