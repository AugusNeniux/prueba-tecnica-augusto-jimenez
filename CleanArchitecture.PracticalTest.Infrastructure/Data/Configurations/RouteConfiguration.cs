using CleanArchitecture.PracticalTest.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.PracticalTest.Infrastructure.Data.Configurations
{
    public class RouteConfiguration : IEntityTypeConfiguration<Route>
    {
        public void Configure(EntityTypeBuilder<Route> builder)
        {
            builder.ToTable("Routes");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.Property(x => x.Origin)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.Destination)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.DistanceKm)
                .HasColumnType("numeric(10,2)")
                .IsRequired();

            builder.Property(x => x.EstimatedTimeHours)
                .HasColumnType("numeric(10,2)")
                .IsRequired();
        }
    }
}
