import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { Observer } from 'rxjs/Observer';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/map';
import 'rxjs/add/observable/throw';
import { catchError, map, tap } from 'rxjs/operators';

import {
  Research,
  AppError,
  NotAuthorized,
  BadInput,
  NotFound
} from '../models/sistema';
import * as FileSaver from 'file-saver';
import { ToastrService } from 'ngx-toastr';
import {
  HttpClient,
  HttpHeaders,
  HttpErrorResponse
} from '@angular/common/http';
import { ResponseContentType } from '@angular/http';

@Injectable()
export class DataBase<TModel> {
  public authorization: boolean = false;
  public storage: Storage;
  public url: string;

  constructor(public _http: HttpClient, public _toastr: ToastrService) {
    this.storage = localStorage;
  }
  resultadoRequisicao(response: Observable<any>): Observable<any> {
    return response
      .map((res: Response) => {
        return res.json();
      })
      .catch(this.onHandleError);
  }

  onExtrairDados(response: Response) {
    let data = response.json();
    return data || {};
  }

  onCreateRequestOptions() {
    return { headers: this.onCreateHeader() };
  }

  onCreateRequestOptionsArquivo(
    authorization: boolean = false,
    mimetype: string
  ): any {
    return {
      headers: this.onCreateHeaderArquivo(authorization, mimetype),
      responseType: ResponseContentType.Blob
    };
  }


  onCreateHeader(): HttpHeaders {
    let headers = new HttpHeaders().set('Content-Type', 'application/json');
    if (this.authorization) {
      headers.set('Authorization', 'Bearer ' + this.storage.getItem('token'));
    }
    return headers;
  }

  onCreateHeaderArquivo(
    authorization: boolean = false,
    mimetype: string
  ): HttpHeaders {
    let headers = new HttpHeaders().set('Content-Type', 'application/json');
    headers.set('Accept', "'" + mimetype + "'");
    if (authorization) {
      headers.append(
        'Authorization',
        'Bearer ' + localStorage.getItem('token')
      );
    }
    return headers;
  }

  //Observable<any>
  onHandleError(resposta: HttpErrorResponse): Observable<any> {
    //console.log('onHandleError - SERV:',resposta );
    switch (resposta.status) {
      case 0:
        return Observable.throw(new AppError(resposta));
      case 302:
        return Observable.throw(new NotAuthorized(resposta));
      case 400:
        return Observable.throw(new BadInput(resposta));
      case 404:
        return Observable.throw(new NotFound(resposta));
      default:
        return Observable.throw(new AppError(resposta));
    }
  }

  onBaixarComHeader(
    url: string,
    nomeExtensaoArquivo: string,
    mimetype: string
  ) {
    return this.getArquivo(url, false, true, mimetype).subscribe(
      (response: any) =>
        FileSaver.saveAs(
          new Blob([response._body], { type: mimetype }),
          nomeExtensaoArquivo
        ),
      erro =>
        this._toastr.warning(
          'Nenhum dado encontrado com os filtros utilizados.',
          'Atenção!'
        )
    );
  }

  getArquivo(
    url: string,
    refreshToken: boolean = false,
    authorization: boolean = true,
    mimetype: string = ''
  ): Observable<any> {
    /*return this._http.get(url, this.onCreateRequestOptionsArquivo(authorization,mimetype)).map((response: any) => 
		{
			return response;            
		}).catch(this.onHandleError);*/

    return this._http
      .get<TModel>(
        url,
        this.onCreateRequestOptionsArquivo(authorization, mimetype)
      )
      .catch(this.onHandleError);
  }

  //ListarAsync
  onListar(filtros: string = ''): Observable<Research> {
    return this._http
      .get<Research>(
        this.url + `/Listar${filtros}`,        
        this.onCreateRequestOptions()
      )
      .pipe(
        //tap(result => console.log(`fetched `, result)),
        catchError(this.onHandleError)
      )
      .catch(this.onHandleError);
  }

  onBuscar(id: number): Observable<AppError> {
    //Get
    return this._http
      .get<TModel>(
        this.url + `${id}`,
        this.onCreateRequestOptions()
      )
      .catch(this.onHandleError);
  }

  onCadastrar(model: TModel): any {
    return this._http
      .post(
        this.url,
        JSON.stringify(model),
        this.onCreateRequestOptions()
      )
      .catch(this.onHandleError);
  }

  //UpdateAsync
  onEditar(model: TModel): any {
    return this._http
      .put(
        //this.url + 'UpdateAsync',
        this.url,
        JSON.stringify(model),
        this.onCreateRequestOptions()
      )
      .catch(this.onHandleError);
  }

  onDeletar(id: number): Observable<any> {
    return (
      this._http
        .delete(this.url + `/${id}`, this.onCreateRequestOptions())
        .catch(this.onHandleError)
    );
  }
}