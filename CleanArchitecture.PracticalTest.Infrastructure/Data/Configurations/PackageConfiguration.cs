using CleanArchitecture.PracticalTest.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.PracticalTest.Infrastructure.Data.Configurations
{
    public class PackageConfiguration : IEntityTypeConfiguration<Package>
    {
        public void Configure(EntityTypeBuilder<Package> builder)
        {
            builder.ToTable("Packages");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.Property(x => x.WeightKg)
                .HasColumnType("numeric(10,2)")
                .IsRequired();

            builder.Property(x => x.LengthCm)
                .HasColumnType("numeric(10,2)")
                .IsRequired();

            builder.Property(x => x.WidthCm)
                .HasColumnType("numeric(10,2)")
                .IsRequired();

            builder.Property(x => x.HeightCm)
                .HasColumnType("numeric(10,2)")
                .IsRequired();

            builder.Property(x => x.Status)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(x => x.ShippingCost)
                .HasColumnType("numeric(12,2)");

            builder.Ignore(x => x.VolumeCm3);
            builder.Ignore(x => x.HasHighVolumeSurcharge);

            builder.HasOne(x => x.Route)
                .WithMany()
                .HasForeignKey(x => x.RouteId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(x => x.StatusHistory)
                .WithOne()
                .HasForeignKey(x => x.PackageId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(x => x.StatusHistory)
                .HasField("_statusHistory")
                .UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
