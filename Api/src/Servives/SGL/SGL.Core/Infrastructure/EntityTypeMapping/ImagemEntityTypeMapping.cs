using Core.Abstractions.Attribute;
using Core.Abstractions.Infrastructure.Data.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGL.Core.Domain.Entities;

namespace SGL.Infrastructure.EntityTypeMapping
{
    [Map]
    public sealed class ImagemEntityTypeMapping : IEntityTypeMapping<Imagem>
    {
        public void Map(EntityTypeBuilder<Imagem> builder)
        {
            // table
            builder.ToTable("Imagem");
            // keys
            builder.HasKey(t => t.Id);
            ////Events
            builder.Ignore(t => t.DomainEvents);

            //// Properties            
            builder.Property(t => t.Id).ValueGeneratedNever().IsRequired();
            builder.Property(t => t.Nome).HasMaxLength(20).IsRequired();
            builder.Property(t => t.ContentType).HasMaxLength(20).IsRequired();
            builder.Property(t => t.Bytes).IsRequired();

            ////Propriedades Base
            builder.Property(t => t.TId);
            builder.Property(t => t.Ativo);
            builder.Property(t => t.DataCadastro);
            builder.Property(t => t.DataAtualizacao);
        }
    }
}
