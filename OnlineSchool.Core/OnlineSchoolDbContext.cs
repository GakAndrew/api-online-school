using Microsoft.EntityFrameworkCore;
using OnlineSchool.Core.Configurations;
using OnlineSchool.Core.Entities;

namespace OnlineSchool.Core
{
    public class OnlineSchoolDbContext(DbContextOptions<OnlineSchoolDbContext> options) 
        : DbContext(options)
    {
        public DbSet<Admin> Admins { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Subject> Subjects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new SubjectConfiguration());
            modelBuilder.ApplyConfiguration(new TeacherConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
