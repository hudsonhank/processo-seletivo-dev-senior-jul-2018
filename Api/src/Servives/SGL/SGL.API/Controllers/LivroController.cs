using Core.Abstractions.Api;
using Core.Abstractions.Domain.Mensagens;
using Core.Abstractions.Extension;
using MediatR;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGL.Application.Commands;
using SGL.Application.Livro.Commands;
using SGL.Application.Livro.Queries;
using SGL.Core.Domain.Entities.Queries;
using SGL.Domain.Mensagens;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SGL.APIControllers
{
    /// <summary>
    /// Api com os métodos de manipulação do dados da Livraria - Online
    /// </summary>
    /// 
    //[Route("api/v1/[controller]")]
    [Route("[controller]")]
    [EnableCors(policyName: "corsGlobalPolicy")]
    public class LivroController : BaseApi
    {
        /// <summary>
        /// Usado para efeturar buscas das Livros 
        /// </summary>       
        public readonly LivroQueries Queries;

        /// <summary>
        /// Orquestrador dos comandos 
        /// </summary>       
        public readonly IMediator Mediator;

        /// <summary>
        /// API de com métodos para manipular uma LivroEntity
        /// </summary>        
        /// <param name="mediator"></param>
        /// <param name="queries"></param>/// 
        public LivroController(IMediator mediator, ILivroQueries queries)
        {
            Queries = (LivroQueries)queries ?? throw new ArgumentNullException(nameof(queries));
            Mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// Cria uma nova livro
        /// </summary>        
        /// <param name="command"></param>   
        /// <param name="cancellationToken"></param>
        //[HttpPost("CriarNovoLivro")]
        [HttpPost]
        public async Task<IActionResult> CriarNovoLivro([FromBody] CriarLivroCommand command, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (ModelState.IsValid)
            {
                return await ExecuteActionAsync(async () => await Mediator.Send(command, cancellationToken));
            }
            return BadRequest(ModelStateErrors());
        }

        /// <summary>
        /// Edita as informações de uma livro existente
        /// </summary>        
        /// <param name="command"></param>   
        /// <param name="cancellationToken"></param>
        //[HttpPost("EditarInformacoesLivro")]
        //[Route("editar")]
        [HttpPut]
        public async Task<IActionResult> EditarInformacoesLivro([FromBody] EditarLivroCommand command, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (ModelState.IsValid)
            {
                return await ExecuteActionAsync(async () => await Mediator.Send(command, cancellationToken));
            }
            return BadRequest(ModelStateErrors());
        }

        /// <summary>
        /// Desativar uma livro pelo código unico
        /// </summary>        
        /// <param name="id"></param>   
        /// <param name="cancellationToken"></param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Desativar(long id, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (ModelState.IsValid)
            {
                return await ExecuteActionAsync(async () => await Mediator.Send(new DesativarLivroCommand(id), cancellationToken));
            }
            return BadRequest(ModelStateErrors());
        }

        /// <summary>
        /// Localizar uma livro pelo seu código unico
        /// </summary>                
        /// <param name="id"></param>           
        [HttpGet("{id}")]
        public async Task<IActionResult> Buscar(long id)
        {
            return await ExecuteActionAsync(async () => await Queries.FirstOrDefault(id.ToString()), true);
        }

        /// <summary>
        /// Localizar todas as Livros que atentem aos filtros selecionados
        /// </summary>        
        /// <param name="filtros">filtros de pesquisa</param>   
        /// <param name="textoSearch">Efetua o filtro full text na base de cache</param>   
        /// <param name="pageSize">itens por página</param>   
        /// <param name="currentPage">página atual</param>   
        [HttpGet]
        [Route("Listar")]
        //public async Task<IActionResult> BuscarLista([FromQuery]LivroQuery filtros, string texto, int pageSize = 10, int currentPage = 1)
        public async Task<IActionResult> Listar([FromQuery]LivroQuery filtros, string textoSearch, int pageSize = 10, int currentPage = 1)
        {
            return await ExecuteActionAsync(async () => await Queries.BuscarLista(filtros, textoSearch, pageSize, currentPage), true);
        }

        /// <summary>
        /// Localizar todas as Livros que atentem aos filtros selecionados
        /// </summary>        


        [HttpGet("Formulario/{id}")]
        public async Task<IActionResult> Formulario(long id)
        {
            var retorno = await Task.FromResult(FormularioExtension.Formulario((EditarLivroCommand)Activator.CreateInstance(typeof(EditarLivroCommand))));
            if (id > 0)
            {
                var dados = await Queries.Buscar(id);

                if (dados != null)
                {
                    retorno.Valor = dados;
                }
            }

            var js = Json(retorno);
            return js;
        }


        /// <summary>
        /// API de com métodos para upload da capa do livro
        /// </summary>        
        /// <param name="uploadedFile"></param>        
        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload(IFormFile uploadedFile)
        {
            if (ModelState.IsValid)
            {
                BaseMensagem resposta = new BaseMensagem(LivroMensagem.REQUISICAOINVALIDA)
                {
                    Tipo = TipoMensagemEnum.Erro
                };

                if (uploadedFile == null || (uploadedFile != null && uploadedFile.Length == 0))
                {
                    return BadRequest(resposta);
                }
                else
                {
                    var command = new ImagemCommand(uploadedFile.FileName, uploadedFile.ContentType);

                    using (var binaryReader = new BinaryReader(uploadedFile.OpenReadStream()))
                    {
                        command.Bytes = binaryReader.ReadBytes(Convert.ToInt32(uploadedFile.Length));
                    }

                    return await ExecuteActionAsync(async () => await Mediator.Send(command));

                }
            }
            return BadRequest(ModelStateErrors());
        }
    }
}
