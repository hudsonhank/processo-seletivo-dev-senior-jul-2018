using Core.Abstractions.Domain.Mensagens;
using Core.Abstractions.Types.Exception;
using FluentAssertions;
using SGL.Application.Livro.Commands;
using SGL.Core.Domain.Entities;
using SGL.UnitTest.Setup;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SGL.UnitTest
{
    public class LivroServiceTests
    {        
        public LivroServiceTests()
        {
            SetupTest.Setup();
        }

        [Theory]
        [InlineData("Titulo")]
        [InlineData("Autor")]
        [InlineData("Genero")]
        [InlineData("Editora")]
        [InlineData("Descricao")]
        [InlineData("Sinopse")]
        [InlineData("Paginas")]
        public void DeveSerInvalidoCriarNovoLivroSemValorInseridoNoCampo(string campo)
        {
            //arrange
            var entity = SetupTest.CriarLivro();
            //act
            var erro = "";

            switch (campo)
            {
                case "Titulo":
                    erro = SetupTest.Mensagem.TituloObrigatorio.Texto;
                    entity.Titulo = "";
                    break;
                case "Autor":
                    erro = SetupTest.Mensagem.AutorObrigatorio.Texto;
                    entity.Autor = "";
                    break;
                case "Genero":
                    erro = SetupTest.Mensagem.GeneroObrigatorio.Texto;
                    entity.Genero = "";
                    break;
                case "Editora":
                    erro = SetupTest.Mensagem.EditoraObrigatorio.Texto;
                    entity.Editora = "";
                    break;
                case "Descricao":
                    erro = SetupTest.Mensagem.DescricaoObrigatorio.Texto;
                    entity.Descricao = "";
                    break;
                case "Sinopse":
                    erro = SetupTest.Mensagem.SinopseObrigatorio.Texto;
                    entity.Sinopse = "";
                    break;

                case "Paginas":
                    erro = SetupTest.Mensagem.PaginasObrigatorio.Texto;
                    entity.Paginas = 0;
                    break;

                case "DataPublicacao":
                    erro = SetupTest.Mensagem.DataPublicacaoObrigatorio.Texto;
                    entity.DataPublicacao = DateTime.MinValue;
                    break;
            }

            Func<Task> exc = async () => await SetupTest.LivroService.CriarNovoLivro(entity, nameof(CriarLivroCommand));

            //Assert                            
            exc.Should().Throw<RequisicaoInvalidaException>().Which.Mensagens.FirstOrDefault().Mensagem.Texto.Should().Be(erro);
            SetupTest.ProjetoContext.Dispose();
        }

        [Theory]
        [InlineData("Titulo")]
        [InlineData("Autor")]
        [InlineData("Genero")]
        [InlineData("Editora")]
        [InlineData("Descricao")]
        [InlineData("Sinopse")]
        public void DeveSerInvalidoCriarNovoLivroSemMinimoDeCaracteresInseridoNoCampo(string campo)
        {
            //arrange
            var entity = SetupTest.CriarLivro();
            //act
            var erro = "";

            switch (campo)
            {
                case "Titulo":
                    erro = SetupTest.Mensagem.TituloMinimo.Texto;
                    entity.Titulo = "123";
                    break;
                case "Autor":
                    erro = SetupTest.Mensagem.AutorMinimo.Texto;
                    entity.Autor = "123";
                    break;
                case "Genero":
                    erro = SetupTest.Mensagem.GeneroMinimo.Texto;
                    entity.Genero = "123";
                    break;
                case "Editora":
                    erro = SetupTest.Mensagem.EditoraMinimo.Texto;
                    entity.Editora = "123";
                    break;
                case "Descricao":
                    erro = SetupTest.Mensagem.DescricaoMinimo.Texto;
                    entity.Descricao = "123";
                    break;
                case "Sinopse":
                    erro = SetupTest.Mensagem.SinopseMinimo.Texto;
                    entity.Sinopse = "123";
                    break;

            }

            Func<Task> exc = async () => await SetupTest.LivroService.CriarNovoLivro(entity, nameof(CriarLivroCommand));

            //Assert                            
            exc.Should().Throw<RequisicaoInvalidaException>().Which.Mensagens.FirstOrDefault().Mensagem.Texto.Should().Be(erro);
            SetupTest.ProjetoContext.Dispose();
        }


        [Theory]
        [InlineData("Titulo")]
        [InlineData("Autor")]
        [InlineData("Genero")]
        [InlineData("Editora")]
        [InlineData("Descricao")]
        [InlineData("Sinopse")]
        public void DeveSerInvalidoCriarNovoLivroComMaximoDeCaracteresInseridoNoCampo(string campo)
        {
            //arrange
            var entity = SetupTest.CriarLivro();
            //act
            var erro = "";

            var textoMax = "0123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567891";
            switch (campo)
            {
                case "Titulo":
                    erro = SetupTest.Mensagem.TituloMaximo.Texto;
                    entity.Titulo = textoMax;
                    break;
                case "Autor":
                    erro = SetupTest.Mensagem.AutorMaximo.Texto;
                    entity.Autor = textoMax;
                    break;
                case "Genero":
                    erro = SetupTest.Mensagem.GeneroMaximo.Texto;
                    entity.Genero = textoMax;
                    break;
                case "Editora":
                    erro = SetupTest.Mensagem.EditoraMaximo.Texto;
                    entity.Editora = textoMax;
                    break;
                case "Descricao":
                    erro = SetupTest.Mensagem.DescricaoMaximo.Texto;
                    entity.Descricao = textoMax;
                    break;
                case "Sinopse":
                    erro = SetupTest.Mensagem.SinopseMaximo.Texto;
                    entity.Sinopse = textoMax;
                    break;
            }

            Func<Task> exc = async () => await SetupTest.LivroService.CriarNovoLivro(entity, nameof(CriarLivroCommand));

            //Assert                            
            exc.Should().Throw<RequisicaoInvalidaException>().Which.Mensagens.FirstOrDefault().Mensagem.Texto.Should().Be(erro);
            SetupTest.ProjetoContext.Dispose();
        }

        #region Teste do Título
        [Fact]
        public void DeveSerInvalidoCriarNovoLivroComMesmoTituloJaUtilizado()
        {
            //arrange
            var entity = SetupTest.CriarLivro();
            entity.Titulo = "Nome do LivroEntity - AAAAA";
            //act
            Func<Task> exc = async () => await SetupTest.LivroService.CriarNovoLivro(entity, nameof(CriarLivroCommand));
            
            //Assert                            
            exc.Should().Throw<RequisicaoInvalidaException>().Which.Mensagens.FirstOrDefault().Mensagem.Texto.Should().Be(SetupTest.Mensagem.TituloJaCadastrado.Texto);            
            SetupTest.ProjetoContext.Dispose();
        }

        #endregion Teste do Título

        [Fact]
        public async Task DeveCriarNovoLivroComTodosOsCamposPreenchidos()
        {
            //arrange
            var entity = SetupTest.CriarLivro();            
            //act
            var result = await SetupTest.LivroService.CriarNovoLivro(entity, nameof(CriarLivroCommand));
            //Assert                            
            result.Should().Be(entity.Id);

            //Assert                            
            //    exc.Should()..Mensagem.Texto.Should().Be(SetupTest.Mensagem.TituloJaCadastrado.Texto);
            SetupTest.ProjetoContext.Dispose();
        }

    }
}

