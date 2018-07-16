import {
  Resolve
  //ActivatedRouteSnapshot,
  //RouterStateSnapshot
} from '@angular/router';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Rx';
//import { FormularioGroup } from '@sdkrepo/modulo-form-dinamico/src/model/interfaces';
import { FormularioGroup } from '@myorg/form-dinamico';
import { LivroService } from './livros.service';

@Injectable()
export class LivroFormResolver implements Resolve<FormularioGroup> {
  constructor(public _service: LivroService) {}
  resolve(): //route: ActivatedRouteSnapshot,
  //state: RouterStateSnapshot


    | Observable<FormularioGroup>
    | Promise<FormularioGroup>
    | FormularioGroup
    | any {
    let id = sessionStorage.getItem('livroId');
    if(id)
    {
      sessionStorage.removeItem('livroId');
      return this._service.formulario(id);
    }
    return this._service.formulario(0);
  }
}
