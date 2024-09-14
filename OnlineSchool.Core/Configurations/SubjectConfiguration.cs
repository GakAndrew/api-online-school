using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OnlineSchool.Core.Entities;

namespace OnlineSchool.Core.Configurations
{
    public class SubjectConfiguration : IEntityTypeConfiguration<Subject>
    {
        public void Configure(EntityTypeBuilder<Subject> builder)
        {
            builder.HasKey(s => s.Id);

            builder.HasOne(s => s.Teacher)
                .WithMany(s => s.Subjects)
                .HasForeignKey(s => s.TeacherId);
        }
    }
}
