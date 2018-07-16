using MediatR;
using SGL.Domain.Repository;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SGL.Domain.Events.Handlers
{
    public class AtualizarIndexPequisaLivroEventHandler : INotificationHandler<AtualizarIndexPequisaLivroEvent>
    {
        private readonly ILivroCacheRepository RedisRepository;

        public AtualizarIndexPequisaLivroEventHandler(ILivroCacheRepository redisLivroRepository)
        {
            RedisRepository = redisLivroRepository ?? throw new ArgumentNullException(nameof(redisLivroRepository));
        }

        public async Task Handle(AtualizarIndexPequisaLivroEvent command, CancellationToken cancellationToken)
        {
            await RedisRepository.SetAsync(command.Livro);
        }
    }
}
