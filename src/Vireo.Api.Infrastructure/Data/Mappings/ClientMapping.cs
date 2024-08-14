using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vireo.Api.Core.Domain.Entities;

namespace Vireo.Api.Infrastructure.Data.Mappings;

public class ClientMapping : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.HasKey(_ => _.Id);

        builder.Property(_ => _.Name)
            .IsRequired()
            .HasColumnType("varchar(100)");

        builder.Property(_ => _.LastName)
            .IsRequired()
            .HasColumnType("varchar(100)");

        builder.Property(_ => _.Phone)
            .IsRequired()
            .HasColumnType("varchar(13)");

        builder.Property(_ => _.Email)
            .IsRequired()
            .HasColumnType("varchar(50)");

        builder.Property(_ => _.Active)
            .IsRequired();

        builder.Property(_ => _.LastServiceDate)
            .IsRequired();
    }
}
