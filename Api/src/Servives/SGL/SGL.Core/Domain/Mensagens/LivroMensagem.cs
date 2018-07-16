
using Core.Abstractions.Domain.Mensagens;
namespace SGL.Domain.Mensagens
{
    public interface ILivroMensagem : IMensagemLivroiner<IMensagem>
    {
        //Sucesso
        BaseMensagem SucessoCriar { get; }
        BaseMensagem SucessoUploadCapa { get; }
        BaseMensagem SucessoEditar { get; }
        BaseMensagem SucessoDesativar { get; }

        BaseMensagem IdObrigatorio { get; }
        BaseMensagem LivroNaoExiste { get; }
        BaseMensagem RequisicaoInvalida { get; }
        BaseMensagem CapaIdObrigatorio { get; }
        BaseMensagem ImagemCapaJaCadastrado { get; }
        BaseMensagem ImagemTipoCapaInvalido { get; }

        BaseMensagem TituloObrigatorio { get; }
        BaseMensagem TituloJaCadastrado { get; }
        BaseMensagem TituloMaximo { get; }
        BaseMensagem TituloMinimo { get; }

        BaseMensagem AutorObrigatorio { get; }
        BaseMensagem AutorMaximo { get; }
        BaseMensagem AutorMinimo { get; }

        BaseMensagem GeneroObrigatorio { get; }
        BaseMensagem GeneroMaximo { get; }
        BaseMensagem GeneroMinimo { get; }

        BaseMensagem EditoraObrigatorio { get; }
        BaseMensagem EditoraMaximo { get; }
        BaseMensagem EditoraMinimo { get; }

        BaseMensagem DescricaoObrigatorio { get; }
        BaseMensagem DescricaoMaximo { get; }
        BaseMensagem DescricaoMinimo { get; }

        BaseMensagem SinopseObrigatorio { get; }
        BaseMensagem SinopseMaximo { get; }
        BaseMensagem SinopseMinimo { get; }

        BaseMensagem PaginasObrigatorio { get; }

        BaseMensagem DataPublicacaoObrigatorio { get; }
    }


    public class LivroMensagem: BaseMensagemLivroiner<BaseMensagem>, ILivroMensagem
    {
        ////Sucesso        
        public const string SUCESSOCRIAR = "Livro cadastrado.";
        public const string SUCESSOEDITAR = "Livro excluído.";
        public const string SUCESSODESATIVAR = "Informações do livro atualizadas.";
        public const string SUCESSOUPLOADCAPA = "Upload da capa executado com sucesso:";

        public const string LIVRONAOEXISTE = "Livro não localizado.";
        public const string REQUISICAOINVALIDA = "A requisição não encontrou uma imagem para a capa do livro.";
        public const string CAPAIDOBRIGATORIO = "Informe o ID do arquivo da capa do livro.";
        public const string IMAGEMCAPAJACADASTRADO = "A imagem já esta sendo utilizada por outro Livro.";
        public const string IMAGEMTIPOCAPAINVALIDO= "Tipo do arquivo da imagem da capa inválido utilize (JEPG, JPG, BMP ou GIF).";
        public const string IDOBRIGATORIO = "É obrigatório informar o ID do livro.";


        public const string TITULOOBRIGATORIO =  "É obrigatório informar o Título.";
        public const string TITULOJACADASTRADO = "O Título informado já foi cadastrado.";
        public const string TITULOMAXIMO = "Informe no máximo 150 caracteres no campo Título.";
        public const string TITULOMINIMO = "Informe no mínimo 10 caracteres no campo Título.";

        public const string AUTOROBRIGATORIO = "É obrigatório informar o Autor.";
        public const string AUTORMAXIMO = "Informe no máximo 150 caracteres no campo Autor.";
        public const string AUTORMINIMO = "Informe no mínimo 10 caracteres no campo Autor.";

        public const string EDITORAOBRIGATORIO = "É obrigatório informar a Editora.";
        public const string EDITORAMAXIMO = "Informe no máximo 150 caracteres no campo Editora.";
        public const string EDITORAMINIMO = "Informe no mínimo 10 caracteres no campo Editora.";

        public const string GENEROOBRIGATORIO = "É obrigatório informar o Gênero.";
        public const string GENEROMAXIMO = "Informe no máximo 150 caracteres no campo Gênero.";
        public const string GENEROMINIMO = "Informe no mínimo 10 caracteres no campo Gênero.";

        public const string DESCRICAOOBRIGATORIO = "É obrigatório informar a Descrição.";
        public const string DESCRICAOMAXIMO = "Informe no máximo 500 caracteres no campo Descrição.";
        public const string DESCRICAOMINIMO = "Informe no mínimo 10 caracteres no campo a Descrição.";

        public const string SINOPSEOBRIGATORIO = "É obrigatório informar a Sinopse.";
        public const string SINOPSEMAXIMO = "Informe no máximo 500 caracteres no campo Sinopse.";
        public const string SINOPSEMINIMO = "Informe no mínimo 10 caracteres no campo a Sinopse.";

        public const string DATAPUBLICACAOOBRIGATORIO = "É obrigatório informar a Data de Publicação.";

        public const string PAGINASOBRIGATORIO = "É obrigatório informar o número de páginas.";

        //Sucesso
        public BaseMensagem SucessoCriar => BaseMensagem.Personalizada(SUCESSOCRIAR);        
        public BaseMensagem SucessoEditar => BaseMensagem.Personalizada(SUCESSOEDITAR);
        public BaseMensagem SucessoDesativar => BaseMensagem.Personalizada(SUCESSODESATIVAR);
        public BaseMensagem SucessoUploadCapa => BaseMensagem.Personalizada(SUCESSOUPLOADCAPA);
        //Alertas
        public BaseMensagem IdObrigatorio => BaseMensagem.Personalizada(IDOBRIGATORIO);
        public BaseMensagem LivroNaoExiste => BaseMensagem.Personalizada(LIVRONAOEXISTE);
        public BaseMensagem RequisicaoInvalida => BaseMensagem.Personalizada(REQUISICAOINVALIDA);
        
        public BaseMensagem CapaIdObrigatorio => BaseMensagem.Personalizada(CAPAIDOBRIGATORIO);
        public BaseMensagem ImagemCapaJaCadastrado => BaseMensagem.Personalizada(IMAGEMCAPAJACADASTRADO);
        public BaseMensagem ImagemTipoCapaInvalido => BaseMensagem.Personalizada(IMAGEMTIPOCAPAINVALIDO);
        //Título
        public BaseMensagem TituloObrigatorio => BaseMensagem.Personalizada(TITULOOBRIGATORIO);
        public BaseMensagem TituloJaCadastrado => BaseMensagem.Personalizada(TITULOJACADASTRADO);
        public BaseMensagem TituloMaximo => BaseMensagem.Personalizada(TITULOMAXIMO);
        public BaseMensagem TituloMinimo => BaseMensagem.Personalizada(TITULOMINIMO);
        //Autor
        public BaseMensagem AutorObrigatorio => BaseMensagem.Personalizada(AUTOROBRIGATORIO);        
        public BaseMensagem AutorMaximo => BaseMensagem.Personalizada(AUTORMAXIMO);
        public BaseMensagem AutorMinimo => BaseMensagem.Personalizada(AUTORMINIMO);
        //Gênero
        public BaseMensagem GeneroObrigatorio => BaseMensagem.Personalizada(GENEROOBRIGATORIO);
        public BaseMensagem GeneroMaximo => BaseMensagem.Personalizada(GENEROMAXIMO);
        public BaseMensagem GeneroMinimo => BaseMensagem.Personalizada(GENEROMINIMO);
        //Editora
        public BaseMensagem EditoraObrigatorio => BaseMensagem.Personalizada(EDITORAOBRIGATORIO);
        public BaseMensagem EditoraMaximo => BaseMensagem.Personalizada(EDITORAMAXIMO);
        public BaseMensagem EditoraMinimo => BaseMensagem.Personalizada(EDITORAMINIMO);
        //Descricao
        public BaseMensagem DescricaoObrigatorio => BaseMensagem.Personalizada(DESCRICAOOBRIGATORIO);
        public BaseMensagem DescricaoMaximo => BaseMensagem.Personalizada(DESCRICAOMAXIMO);
        public BaseMensagem DescricaoMinimo => BaseMensagem.Personalizada(DESCRICAOMINIMO);
        //Sinopse
        public BaseMensagem SinopseObrigatorio => BaseMensagem.Personalizada(SINOPSEOBRIGATORIO);
        public BaseMensagem SinopseMaximo => BaseMensagem.Personalizada(SINOPSEMAXIMO);
        public BaseMensagem SinopseMinimo => BaseMensagem.Personalizada(SINOPSEMINIMO);
        //Páginas
        public BaseMensagem PaginasObrigatorio => BaseMensagem.Personalizada(PAGINASOBRIGATORIO);
        //Data de publicação
        public BaseMensagem DataPublicacaoObrigatorio => BaseMensagem.Personalizada(DATAPUBLICACAOOBRIGATORIO);

        public LivroMensagem()
        {
        }
    }
}

