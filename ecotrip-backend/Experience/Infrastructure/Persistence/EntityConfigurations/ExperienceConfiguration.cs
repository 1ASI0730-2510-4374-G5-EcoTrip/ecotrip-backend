using Experience.Domain.Entities;
using Experience.Domain.Enums;
using Experience.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Experience.Infrastructure.Persistence.EntityConfigurations
{
    public class ExperienceConfiguration : IEntityTypeConfiguration<Domain.Entities.Experience>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Experience> builder)
        {
            // Table name
            builder.ToTable("Experiences");

            // Key
            builder.HasKey(e => e.Id);

            // Value object ID mapping
            builder.Property(e => e.Id)
                .HasConversion(
                    id => id.Value,
                    value => new ExperienceId(value))
                .ValueGeneratedNever();

            // Basic properties
            builder.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(2000);

            builder.Property(e => e.Date)
                .IsRequired();

            builder.Property(e => e.Location)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.DurationInDays)
                .IsRequired();

            builder.Property(e => e.MainImageUrl)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(e => e.AgentId)
                .IsRequired()
                .HasMaxLength(50);

            // Money value object
            builder.OwnsOne(e => e.Price, priceBuilder =>
            {
                priceBuilder.Property(p => p.Amount)
                    .HasColumnName("Price")
                    .IsRequired()
                    .HasPrecision(18, 2);

                priceBuilder.Property(p => p.Currency)
                    .HasColumnName("Currency")
                    .IsRequired()
                    .HasMaxLength(3);
            });

            // Enum mapping
            builder.Property(e => e.Status)
                .IsRequired()
                .HasConversion<string>();

            // Timestamps
            builder.Property(e => e.CreatedAt)
                .IsRequired();

            builder.Property(e => e.UpdatedAt)
                .IsRequired(false);        // Image URLs collection
        builder.Property(e => e.ImageUrls)
            .HasConversion(
                list => JsonSerializer.Serialize(list, (JsonSerializerOptions?)null),
                json => JsonSerializer.Deserialize<List<string>>(json, (JsonSerializerOptions?)null) ?? new List<string>()
            )
            .HasColumnType("nvarchar(max)");

        // Value comparer for ImageUrls
        var imageUrlsComparer = new ValueComparer<List<string>>(
            (c1, c2) => (c1 == null && c2 == null) || (c1 != null && c2 != null && c1.SequenceEqual(c2)),
            c => c == null ? 0 : c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            c => c == null ? new List<string>() : c.ToList()
            );

            builder.Property(e => e.ImageUrls).Metadata.SetValueComparer(imageUrlsComparer);

            // Navigation properties
            builder.HasMany<Review>(e => e.Reviews)
                .WithOne()
                .HasForeignKey("ExperienceId")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}