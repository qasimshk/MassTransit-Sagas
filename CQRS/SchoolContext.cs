using CQRS.Entities;
using Microsoft.EntityFrameworkCore;

namespace CQRS
{
    public class SchoolContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrolled> Enrolleds { get; set; }
        //public DbSet<StudentResult> StudentResults { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Enrolled>()
                .HasKey(sc => new { sc.StudentId, sc.CourseId });

            modelBuilder.Entity<Enrolled>()
                .HasOne(sc => sc.students)
                .WithMany(s => s.Enrolleds)
                .HasForeignKey(sc => sc.StudentId);

            modelBuilder.Entity<Enrolled>()
                .HasOne(c => c.Courses)
                .WithMany(s => s.Enrolleds)
                .HasForeignKey(sc => sc.CourseId);

            base.OnModelCreating(modelBuilder);
        }

        public SchoolContext(DbContextOptions<SchoolContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
        }
    }
}