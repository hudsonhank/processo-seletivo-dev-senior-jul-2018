using Core.Abstractions.Domain.Mensagens;
using Core.Abstractions.Extension;
using Core.Abstractions.Types.Exception;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Abstractions.Api
{
    public interface IBaseApi
    {
    }

    public class BaseApi : Controller, IBaseApi
    {
        public BaseApi()
        {
        }

        [HttpGet("Formulario")]
        public virtual async Task<IActionResult> FormularioAsync<TEntity>()
        {
            var retorno = await Task.FromResult(FormularioExtension.Formulario((TEntity)Activator.CreateInstance(typeof(TEntity))));
           
            return Json(retorno);
        }

        protected virtual List<BaseMensagem> ModelStateErrors()
        {
            var modelStateErrors = ModelState.ToList();
            var listaErro = new List<BaseMensagem>();

            if (modelStateErrors.Any())
            {
                foreach (var erro in modelStateErrors)
                {
                    if (erro.Value != null && erro.Value.Errors != null && erro.Value.Errors.Count > 0)
                    {
                        listaErro.Add(new BaseMensagem(erro.Value.Errors[0].ErrorMessage));
                    }
                }

            }
            return listaErro;
        }

        protected async Task<object> ExecuteAsync(Func<Task<object>> action, bool retornarJson = false, bool actionResult = false)
        {
            try
            {
                var res = await action();
                if (retornarJson)
                {
                    return Json(res);
                }
                if (res != null && res.GetType() != typeof(NotFoundObjectResult) && res.GetType() != typeof(BadRequestObjectResult))
                {
                    return actionResult ? Ok(res) : res;
                }
                return null;
            }
            catch (NaoEncontradoException)
            {
                return NotFound(JsonConvert.SerializeObject(new BaseMensagem("Nenhum registro encontrado."), Formatting.Indented));
            }
            catch (RequisicaoInvalidaException exc)
            {
                var erro = exc.Mensagens.FirstOrDefault().Mensagem ?? new BaseMensagem(exc.Message);
                return BadRequest(JsonConvert.SerializeObject(erro, Formatting.Indented));
            }
            catch (System.Exception exc)
            {
                return BadRequest(JsonConvert.SerializeObject(new BaseMensagem(exc.Message), Formatting.Indented));
            }
        }

        protected async Task<IActionResult> ExecuteActionAsync(Func<Task<object>> action, bool retornarJson = false)
        {
            return await ExecuteAsync(action, retornarJson, true) as IActionResult;
        }

        protected IActionResult ExecuteAction(Func<Task<object>> action, bool retornarJson = false)
        {
            return ExecuteAsync(action, retornarJson, true).Result as IActionResult;
        }
    }
}
