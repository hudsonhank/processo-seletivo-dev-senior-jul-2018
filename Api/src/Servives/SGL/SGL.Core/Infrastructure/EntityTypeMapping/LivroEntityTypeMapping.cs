using Core.Abstractions.Attribute;
using Core.Abstractions.Infrastructure.Data.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGL.Core.Domain.Entities;

namespace SGL.Infrastructure.EntityTypeMapping
{
    [Map]
    public sealed class LivroEntityTypeMapping : IEntityTypeMapping<LivroEntity>
    {
        public void Map(EntityTypeBuilder<LivroEntity> builder)
        {
            // table
            builder.ToTable("Livro");
            // keys
            builder.HasKey(t => t.Id);
            ////Events
            builder.Ignore(t => t.DomainEvents);

            //// Properties            
            builder.Property(t => t.Id).ValueGeneratedNever().IsRequired();
            builder.Property(t => t.CodigoUnico).HasMaxLength(20).IsRequired();
            builder.Property(t => t.Titulo).HasMaxLength(150).IsRequired();
            builder.Property(t => t.Autor).HasMaxLength(150).IsRequired();
            builder.Property(t => t.Genero).HasMaxLength(150).IsRequired();
            builder.Property(t => t.Editora).HasMaxLength(150).IsRequired();
            builder.Property(t => t.Descricao).HasMaxLength(500).IsRequired();
            builder.Property(t => t.Sinopse).HasMaxLength(500).IsRequired();            
            builder.Property(t => t.Paginas).IsRequired(); 
            builder.Property(t => t.DataPublicacao).IsRequired();
            builder.Property(t => t.Link).HasMaxLength(500);

            ////Referencia: Imagem
            builder.Property(t => t.CapaId);
            builder.HasOne(t => t.Capa).WithOne().HasForeignKey<LivroEntity>(e => e.CapaId).OnDelete(DeleteBehavior.Cascade);

            ////Propriedades Base
            builder.Property(t => t.TId);
            builder.Property(t => t.Ativo);
            builder.Property(t => t.DataCadastro);
            builder.Property(t => t.DataAtualizacao);
        }
    }
}
