using Core.Abstractions.Attribute.Enum;
using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Abstractions.Attribute
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class OptionsAttribute : ValidationAttribute
    //public class OptionsAttribute : System.Attribute
    {
        public string GrupoName { get; set; } = "Principal";
        public string Description { get; set; } = "";
        public string TypeData { get; set; } = "";
        public string Pattern { get; set; } = "";
        public string CSSClass { get; set; } = "col-md-6";
        public string ListaValores { get; set; } = "";
        public string ControleCondicaoKey { get; set; } = "";
        public string ControleCondicaoValor { get; set; } = "";        
        public bool AddLista { get; set; } = false;
        public string AddListaChave { get; set; } = "";
        public bool Editavel { get; set; } = false;
        public bool Listagem { get; set; } = false;
        public Type EntidadeTipo { get; set; } = null;
        public ComponenteTipoEnum TipoLista { get; set; } = ComponenteTipoEnum.Entidade;
        
        public OptionsAttribute(
           string grupoName = "Principal",
           string description = "",
           string listaValores = "",
           string typeData = "text",
           string controleCondicaoKey = "",
           string controleCondicaoValor = "",
           bool addLista = false,
           string addListaChave = "",
           string css = "col-md-6",
           string pattern = "",
           bool editavel = true,
           bool listagem = true,
           
           Type entidadeTipo = null,
           ComponenteTipoEnum tipoLista = ComponenteTipoEnum.Entidade
           )
        {
            GrupoName = grupoName;
            Pattern = pattern;
            CSSClass = string.IsNullOrEmpty(css) ? "col-md-6" : css;
            Description = description;
            ListaValores = listaValores;
            TypeData = typeData;
            EntidadeTipo = entidadeTipo;
            TipoLista = tipoLista;

            ControleCondicaoKey = controleCondicaoKey;
            ControleCondicaoValor = controleCondicaoValor;
            AddLista = addLista;
            AddListaChave = addListaChave;
            Editavel = editavel;
            Listagem = listagem;

        }

        public override bool IsValid(object value)
        {
            return true;
        }
    }
    
}
