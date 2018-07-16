using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SGL.API.Configuration
{

    /// <summary>
    /// Classe que faz o filro de upload para a API do Swashbuckle dos arquivos das capas dos livros do sistema
    /// </summary>
    /// 
    public class FileUploadOperation : IOperationFilter
    {
        /// <summary>
        /// Edita as informações de uma livro existente
        /// </summary>        
        /// <param name="operation"></param>   
        /// <param name="context"></param>
        public void Apply(Swashbuckle.AspNetCore.Swagger.Operation operation, OperationFilterContext context)
        {
            //if (operation.OperationId.ToLower() == "imagemuploadpost")
                if (operation.OperationId.ToLower() == "livrouploadpost")
                {
                operation.Parameters.Clear();
                operation.Parameters.Add(new NonBodyParameter
                {
                    Name = "uploadedFile",
                    In = "formData",
                    Description = "Arquivo de imagem com a capa do livro",
                    Required = true,
                    Type = "file"
                });
                operation.Consumes.Add("application/form-data");
            }
        }
    }
}
