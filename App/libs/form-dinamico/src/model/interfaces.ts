import { FormGroup, FormArray } from '@angular/forms';
import { Mensagem, Item } from '@myorg/core';

//import { Mensagem, Item } from '@myorg/core/models/sistema';

export class FormularioValidacao {
  Formato: string;
  Mensagem: Mensagem;
  NomePropriedade: string;
  Tipo: number;
  TipoCampo: number;

  constructor(
    options: {
      formato?: string;
      nomePropriedade?: string;
      tipo?: number;
      tipoCampo?: number;
      texto?: string;
    } = {}
  ) {
    this.Tipo = 1;
    this.Formato = options.formato || '';
    this.NomePropriedade = options.nomePropriedade || '';
    this.Tipo = options.tipo || 1;
    this.TipoCampo = options.tipoCampo || 1;
    this.Mensagem = new Mensagem(options.texto || 'Campo obrigatório.');
  }
}

export abstract class IControle {
  Key: string;
  GrupoName: string;
  Valor: any;
  Tipo: ControleTipo;
  ControleTipo: string;
  Label: string;
  Class: string;
  Order?: number;
  Validacoes?: Array<FormularioValidacao>;
  Visivel: boolean;
  Editavel: boolean;
  Listagem: boolean;
}

export class Controle extends IControle {
  Valor: any;
  PlaceHolder: string;
  Pattern: string;
  TypeData: string;
  Maximo: number;
  Type: string;
  /*ControleCondicaoKey: string;
	ControleCondicaoValor: string;*/
  AddLista: boolean;
  AddListaChave: string;

  constructor(
    options: {
      value?: any;
      key?: string;
      grupoName?: string;
      label?: string;
      class?: string;
      placeholder?: string;
      mask?: string;
      pattern?: string;
      order?: number;
      maximo?: number;
      type?: string;
      typeData?: string;
      controleTipo?: string;
      visivel?: boolean;
      listagem?: boolean;
      editavel?: boolean;
    } = {}
  ) {
    super();
    this.Tipo = ControleTipo.CONTROLE;
    this.Valor = options.value;
    this.Key = options.key || '';
    this.GrupoName = options.grupoName || 'Principal';
    this.Pattern = options.pattern || 'Text';
    this.Label = options.label || '';
    this.Class = options.class || 'col-md-4';
    this.PlaceHolder = options.placeholder || '';
    this.Order = options.order === undefined ? 1 : options.order;
    this.ControleTipo = options.controleTipo || 'textbox';
    this.Type = options.type || 'text';
    this.TypeData = options.typeData || 'text';
    this.AddLista = false;
    this.AddListaChave = '';
    /*this.ControleCondicaoKey = '';
		this.ControleCondicaoValor = '';*/
    this.Visivel = options.visivel || true; // options.visivel;
    this.Editavel = options.editavel;
    this.Listagem = options.listagem || true;
  }
}

export enum ControleTipo {
  CONTROLE = 1,
  GRUPO = 2,
  ARRAY = 3
}

export class Controles {
  [key: string]: Controle;
}

export class FormularioControle extends Controle {
  ListaValores: string;
  ListaOpcoes: Array<Item>;
  ControleCondicaoKey: string;
  ControleCondicaoValor: string;
  //ControleCondicao:Controle;

  constructor(
    options: {
      value?: any;
      key?: string;
      grupoName?: string;
      label?: string;
      class?: string;
      placeholder?: string;
      order?: number;
      maximo?: number;
      controleTipo?: string;
      typeData?: string;
      pattern?: string;
      type?: string;
      listaValores?: string;
      listaOpcoes?: Item[];
      editavel?: boolean;
      listagem?: boolean;
    } = {}
  ) {
    super(options);
    this.ListaOpcoes = options.listaOpcoes || new Array<Item>();
    this.ListaValores = options.listaValores || '';
    this.Validacoes = new Array<FormularioValidacao>();
    //this.ControleCondicao = new Controle();
    //
    /*this.ControleCondicao.Valor = "valor condicional:"+this.Valor;
			this.ControleCondicao.Type = "type condicional:"+this.Type;*/
    //
    this.ControleCondicaoKey = '';
    this.ControleCondicaoValor = '';
    this.Maximo = options.maximo === undefined ? 100 : options.maximo;
    this.Visivel = true;
    this.Editavel = options.editavel || true;
    this.Listagem = options.listagem || true;
  }
}

export class FormularioArray extends Controle {
  //Controles: Array<Controle>;
  Controles: Controles;
  ControlType: any;
  Formulario: FormGroup;
  Valores: FormArray;
  Default: any;
  constructor(
    options: {
      key?: string;
      default?: any;
      label?: string;
      grupoName?: string;
      class?: string;
      order?: number;
      controlType?: any;
      //controles?: Array<Controle>,
      controles?: Controles;
      validacoes?: Array<FormularioValidacao>;
    } = {}
  ) {
    super();
    this.Tipo = ControleTipo.ARRAY;
    this.Order = options.order === undefined ? 1 : options.order;
    this.Label = options.label || 'Lista de controles';
    this.Default = options.default || {};
    this.Class = options.class || 'panel box box-success';
    this.Key = options.key || 'Array' + this.Order;
    this.GrupoName = options.grupoName || 'Principal';
    this.Controles = options.controles || new Controles();
    this.Validacoes = options.validacoes || new Array<FormularioValidacao>();
    this.ControlType = options.controlType || new FormularioGroup();
    //
    this.Formulario = new FormGroup({});
    this.Valores = new FormArray([]);
  }
}

export class FormularioGroup extends Controle {
  Controles: Controles;
  //Controles: Array<Controle>;
  Formulario: FormGroup;
  ListaValores: string;
  constructor(
    options: {
      key?: string;
      grupoName?: string;
      label?: string;
      class?: string;
      order?: number;
      validacoes?: Array<FormularioValidacao>;
      //controles?: Array<Controle>
      controles?: Controles;
    } = {}
  ) {
    super();
    this.Tipo = ControleTipo.GRUPO;
    this.Key = options.key || 'Formulario';
    this.GrupoName = options.grupoName || 'Principal';
    this.Label = options.label || 'Dados Cadastrais';
    this.Class = options.class || 'panel box box-success';
    this.Order = options.order === undefined ? 1 : options.order;
    this.Controles = options.controles || new Controles(); //Array<Controle>();
    this.Validacoes = options.validacoes || new Array<FormularioValidacao>();
    this.Formulario = new FormGroup({});
    this.ListaValores = '';
  }
}

export class ColunaValor {
  [key: string]: any;
  constructor() {}
}

export class ColunaDefinicao {
  Key: string;
  Label: string;
  Valor: any;
  constructor(key: string, label?: string, valor?: any) {
    this.Key = key;
    this.Label = label || '';
    this.Valor = valor || '';
  }
}
