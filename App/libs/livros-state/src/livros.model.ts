/*export interface LivrosStateMode {
  livros: Livro[];
}*/
export class Livro {
  Id: number;  
  Titulo: string;
  Autor: string;
  Genero: string;
  Editora: string;
  Descricao: string;
  Sinopse: string;
  Link: string;
  DataPublicacao: Date;
  Paginas: number;
  CapaId: number;  
  CapaNome: string;
  constructor() 
  {
    this.Id = 0;
    this.Titulo = '';
    this.Autor = '';
    this.Genero = '';
    this.Editora = '';    
    this.Descricao = '';
    this.Sinopse = '';
    this.Link = '';
    this.DataPublicacao = new Date();
    this.Paginas = 0;
    this.CapaId = 0;
    this.CapaNome = "";
  }
}