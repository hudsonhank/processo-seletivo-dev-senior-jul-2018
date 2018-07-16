import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Rx';
import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { FormularioGroup } from '@myorg/form-dinamico';
import { DataBase, ConfigurationService } from '@myorg/core';
import { Livro } from '../livros.model';

@Injectable()
export class LivroService extends DataBase<Livro> {
  constructor(
    public _http: HttpClient,
    private _configuration: ConfigurationService,
    public _toastr: ToastrService
  ) {
    super(_http, _toastr);
    this.authorization = true;
    var name = Livro.name ? Livro.name : 'Livro';
    this.url = this._configuration.UrlApi + `${name}`;
  }

  buildModel(): Observable<Livro> {
    return this._http.get<Livro>(this.url + 'Modelo');
  }

  formulario(id: any): any {    
    return this._http
      .get<FormularioGroup>(
        this.url + `/Formulario/${id}`,
        this.onCreateRequestOptions()
      ).catch(this.onHandleError);      
  }

  ListaValores(id: number): any {
    return this._http
      .get(this.url + `/ListaValores/${id}`, this.onCreateRequestOptions())
      .catch(this.onHandleError);
  }
}
