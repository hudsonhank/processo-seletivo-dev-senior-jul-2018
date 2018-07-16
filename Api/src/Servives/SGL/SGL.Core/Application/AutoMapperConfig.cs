using AutoMapper;
using SGL.Application.Livro.Commands;

namespace SGL.Application
{
    public class AutoMapperConfig
    {
        public static MapperConfiguration RegisterMappings2()
        {
            return new MapperConfiguration(cfg =>
            {                                
                cfg.AddProfile(new LivroAutoMapper());
                cfg.AddProfile(new DTOModelToLivroCommandAutoMapper());                
            });
        }
    }
}
