using AutoMapper;
using SGL.Application.Commands;
using SGL.Core.Domain.Entities;
using SGL.Core.Domain.Entities.Queries;

namespace SGL.Application.Livro.Commands
{
    public class LivroAutoMapper : Profile
    {
        public LivroAutoMapper()
        {
            //LivroEntity            
            CreateMap<LivroEntity, CriarLivroCommand>();
            CreateMap<CriarLivroCommand, LivroEntity>();
            //
            CreateMap<LivroEntity, EditarLivroCommand>();
            CreateMap<EditarLivroCommand, LivroEntity>();

            CreateMap<Imagem, ImagemCommand>();
            CreateMap<ImagemCommand, Imagem>();


        }
    }

    public class DTOModelToLivroCommandAutoMapper : Profile
    {
        public DTOModelToLivroCommandAutoMapper()
        {
            //CreateMap<LivroDto, CriarLivroCommand>();
            //CreateMap<LivroDto, EditarLivroCommand>();
            //CreateMap<LivroDto, DesativarLivroCommand>();
            CreateMap<LivroEntity, LivroQuery>().ConstructUsing(entity => new LivroQuery(entity.Id.ToString(), entity.CodigoUnico, entity.Titulo, entity.Autor, entity.Genero, entity.Editora, entity.DataPublicacao,""));
            
        }
    }
}




