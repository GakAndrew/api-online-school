
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineSchool.Core.Entities;

namespace OnlineSchool.Core.Configurations
{
    public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
    {
        public void Configure(EntityTypeBuilder<Teacher> builder)
        {
            builder.HasKey(t => t.Id);

            builder.HasMany(t => t.Subjects)
                .WithOne(s => s.Teacher)
                .HasForeignKey(t => t.TeacherId);
        }
    }
}
