using MediatR;
using SGL.Core.Domain.Entities.Queries;

namespace SGL.Domain.Events
{
    public class AtualizarIndexPequisaLivroEvent : INotification
    {
        public virtual LivroQuery Livro { get; set; }

        public AtualizarIndexPequisaLivroEvent(LivroQuery livro)
        {
            Livro = livro;
        }
    }
}
