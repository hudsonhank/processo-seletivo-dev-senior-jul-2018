using Core.Abstractions.Infrastructure;
using SGL.Core.Domain.Entities;
using SGL.Core.Domain.Entities.Queries;
using System.Threading.Tasks;

namespace SGL.Domain.Repository
{

    public interface IImagemCacheRepository: ICacheRepository<Imagem>
    {
    }
}