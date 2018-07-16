
export class Arquivo{
  Id: number;
  
  Nome: string;
  Extensao: string;
  Tipo: string;
  Bytes: any;
  NomeExtensao: string;

  constructor() {   
      this.Id= 0;
      this.Nome= '';
      this.Extensao= '';
      this.Tipo= '';
      this.NomeExtensao= '';
  }
}
export class Menu {
  Id?: number;
  Nome?: string;
  Descricao?: string;
  Uri?: string;
  //Icone?: string;
  Classe?: string;
  Selecionado?: boolean;
  SubMenu: Array<Menu>;

  constructor(
    options: {
      Id?: number;
      Nome?: string;
      Descricao?: string;
      Uri?: string;
      //Icone?: string,
      Classe?: string;
      Selecionado?: boolean;
      SubMenu?: Array<Menu>;
    } = {}
  ) {
    this.Id = options.Id || 0;
    this.Nome = options.Nome || '';
    this.Uri = options.Uri || '';
    this.Classe = options.Classe || 'fa fa-edit';
    //this.Icone = options.Icone || 'ion-person-add';
    this.Selecionado = false;
    this.SubMenu = options.SubMenu || new Array<Menu>();
  }
}

export class Campo {
  Chave: string;
  Valor: string;
  constructor(chave: string, valor: string = '') {
    this.Chave = chave;
    this.Valor = valor;
  }
}

export class Campos {
  [key: string]: Campo;
}

export class Filtro {
  Campos: Campos;
  PageSize: number;
  CurrentPage: number;

  constructor() {
    this.Campos = new Campos();
  }
}

export class Item {
  Valor: string;
  Nome: string;
  Descricao: string;
  Checado: boolean;

  constructor(
    options: {
      Valor?: string;
      Nome?: string;
      Descricao?: string;
      Checado?: boolean;
    } = {}
  ) {
    this.Valor = options.Valor || '';
    this.Nome = options.Nome || '';
    this.Descricao = options.Descricao || '';
    this.Checado = options.Checado;
  }
}

export class Mensagem {
  Chave: string;
  Tipo: number;
  TipoTexto: string;
  Texto: string;
  Token: string;

  constructor(texto: string = '') {
    this.Tipo = 0;
    this.Chave = '';
    this.TipoTexto = '';
    this.Texto = texto;
  }
}

export class Research {
  CurrentPage: number;
  PageSize: number;
  Total: number;
  Resultado: any[] = [];
}

export class AppError {
  constructor(public originalError?: any) {}
}

export class NotFound extends AppError {
  constructor(public originalError?: any) {
    super(originalError);
  }
}

export class NotAuthorized extends AppError {
  constructor(public originalError?: any) {
    super(originalError);
  }
}

export class BadInput extends AppError {
  constructor(public originalError?: any) {
    super(originalError);
  }
}
