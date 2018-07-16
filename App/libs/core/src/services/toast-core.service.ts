import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { isE2E } from './e2e-check';
import { ToastrService } from 'ngx-toastr';
import {
  NotFound,
  Mensagem,
  NotAuthorized,
  BadInput,
  AppError
} from '../models/sistema'
import { HttpErrorResponse } from '@angular/common/http';

@Injectable()
export class ToastCoreService {
  constructor(public snackBar: MatSnackBar, public Toaster: ToastrService) {}

  openSnackBar(message: string, action: string) {
    if (isE2E) {
      console.log(`${message} - ${action}`);
    } else {
      this.snackBar.open(message, action, {
        duration: 2000
      });
    }
  }

  success(message: string, title: string, action: string) {
    if (isE2E) {
      console.log(`${message} - ${action}`);
    } else {

      if(action)
      {
        this.Toaster.success(message, title);
      }      
      this.snackBar.open(message, action, {
        duration: 2000
      });
    }
  }

  handleError(resposta: AppError, method: string = ''): any {
    let mensagem = new Mensagem('Erro (Desconhecido) no servidor.');
    var erroResponse = resposta.originalError as HttpErrorResponse;

    if (erroResponse.error) {
      mensagem = (erroResponse.error.length
        ? erroResponse.error[0]
        : erroResponse.error) as Mensagem;
    }

    if (erroResponse['originalError']) {
      mensagem = erroResponse['originalError']['error'] as Mensagem;
    }

    this.openSnackBar(mensagem.Texto, method);

    if (resposta instanceof NotFound || erroResponse instanceof NotFound) {
      this.Toaster.info(
        `Informação: ${mensagem.Texto}`,
        'Nada foi encontrado.'
      );
    } else if (resposta instanceof NotAuthorized) {
      this.Toaster.warning(`Alerta: ${mensagem.Texto}`, 'Não Autorizado.');
    } else if (resposta instanceof BadInput) {
      this.Toaster.warning(`Alerta: ${mensagem.Texto}`, 'Advertência.');
    } else {
      //throw error;
      this.Toaster.info(
        'Erro (Desconhecido) no servidor.',
        'Erro Desconhecido.'
      );
    }
  }
}
