using CleanArchitecture.PracticalTest.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.PracticalTest.Infrastructure.Data.Configurations
{
    public class PackageStatusHistoryConfiguration : IEntityTypeConfiguration<PackageStatusHistory>
    {
        public void Configure(EntityTypeBuilder<PackageStatusHistory> builder)
        {
            builder.ToTable("PackageStatusHistory");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.Property(x => x.PackageId).IsRequired();

            builder.Property(x => x.Status)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(x => x.ChangedAt).IsRequired();

            builder.Property(x => x.Reason)
                .HasMaxLength(500);

            builder.HasIndex(x => x.PackageId);
        }
    }
}
