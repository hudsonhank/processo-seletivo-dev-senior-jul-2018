using Core.Abstractions.Attribute.Enum;
using Core.Abstractions.Extension;
using System.Collections.Generic;

namespace Core.Abstractions.Types.Formulario
{
    public class FormularioControle : IControle
    {
        public virtual string Key { get; set; }
        public virtual object Valor { get; set; } = string.Empty;
        public virtual string GrupoName { get; set; } = string.Empty;
        public virtual string Label { get; set; } = "Dados Cadastrais";
        public virtual string Class { get; set; } = "col-md-6";
        public virtual int Order { get; set; } = 1;
        public virtual int Maximo { get; set; } = 100;
        
        public virtual ControleTypeEnum Tipo { get; } = ControleTypeEnum.Controle;
        public virtual IList<FormularioValidacao> Validacoes { get; set; }
        //

        public virtual string PlaceHolder { get; set; } = "";
        public virtual string Pattern { get; set; } = "";
        public virtual string TypeData { get; set; } = "text";
        public virtual string ListaValores { get; set; } = "";
    
        public string ControleTipo { get; set; } = ComponenteTipoEnum.TextBox.GetDescription();


        //public virtual IControle ControleCondicao { get; set; }

        public virtual string ControleCondicaoKey { get; set; } = "";
        public virtual string ControleCondicaoValor { get; set; } = "";

        public virtual bool AddLista { get; set; } = false;
        public virtual string AddListaChave { get; set; } = "";
        public virtual bool Visivel { get; set; } = true;
        public virtual bool Editavel { get; set; }
        public virtual bool Listagem { get; set; }


        public FormularioControle(string key)
        {
            Key = key;
            Tipo = ControleTypeEnum.Controle;
            Validacoes = new List<FormularioValidacao>();
            ControleCondicaoKey = "";
            ControleCondicaoValor = "";
            Visivel = true;
            //    ControleCondicao = new FormularioControle(key);
        }

    }
}
