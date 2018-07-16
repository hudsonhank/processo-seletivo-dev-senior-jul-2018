using Core.Abstractions.Attribute;
using Core.Abstractions.Domain.Enums;
using Core.Abstractions.Domain.Mensagens;
using Core.Abstractions.Extension;
using System;
using System.Linq.Expressions;

namespace Core.Abstractions.Domain.Validation
{
    public class ObrigatorioValidacao<T> : Validacao 
    {
        public ObrigatorioValidacao(Expression<Func<object>> atributo, IMensagem mensagem) 
            : base(
                  TipoValidacao.Requerido, 
                  new ObrigatorioAttribute(),
                  atributo.GetMemberName<T, object>(),
                  atributo.GetMemberValue<T, object>(),
                  mensagem)
        {
        }
    }
}