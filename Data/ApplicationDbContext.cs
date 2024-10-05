using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ScoreAnnouncementSoftware.Models.Entities;

namespace ScoreAnnouncementSoftware.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Exam> Exam { get; set; } = default!;
        public DbSet<Student> Student { get; set; } = default!;
        public DbSet<StudentExam> StudentExam { get; set; } = default!;
        public DbSet<ITStudent> ITStudent { get; set; } = default!;
        public DbSet<ScoreFL> ScoreFL { get; set; } = default!;
        public DbSet<ScoreIT> ScoreIT { get; set; } = default!;
        public DbSet<ConvertForm> ConvertForms { get; set; } = default!;
        public DbSet<RequireForm> RequireForms { get; set; } = default!;
        public DbSet<ExamType> ExamType { get; set; } = default!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ExamType>()
                        .HasMany(e => e.Exam)
                        .WithOne(p => p.ExamType)
                        .HasForeignKey(p => p.ExamTypeId)
                        .OnDelete(DeleteBehavior.Restrict);


        }

    }
}
