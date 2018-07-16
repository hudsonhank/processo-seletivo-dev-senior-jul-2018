import { OnChanges } from '@angular/core';
import { FormGroup, FormControl, ValidationErrors } from '@angular/forms';
import { Subscription } from 'rxjs/Rx';
import { Router } from '@angular/router/src';

import { ToastrService, ToastrConfig } from 'ngx-toastr';
import { Mensagem, NotFound, NotAuthorized, BadInput } from '@myorg/core';

export class WorkSpaceComponent<TModel> implements OnChanges {
  model: TModel;
  subscription: Subscription;
  title: string;
  validationErrors: ValidationErrors[];
  formErrors: ValidationErrors;
  submited: boolean = false;
  modelForm: FormGroup;

  constructor(
    public _router: Router,
    public _toastr: ToastrService,
    public _toastOptions: ToastrConfig
  ) {}

  listaValores(listaValores: any) {
    sessionStorage.removeItem('listaValores');
    sessionStorage.setItem('listaValores', JSON.stringify(listaValores));
  }

  onReset() {
    this.modelForm.reset();
  }

  onVoltar(path: string) {
    this._router.navigate([path]);
  }

  ngOnChanges() {
    this.modelForm.reset({});
  }

  onValidarSubmit(): boolean {
    this.submited = true;
    var retorno = this.modelForm.valid;
    if (!this.modelForm.valid) {
      this._toastr.error(
        'Por favor, verifique o(s) campo(s) com erro no formulário.',
        'ATENÇÃO'
      );
    }

    for (var key in this.modelForm.controls) {
      const control = this.modelForm.get(key) as FormControl;
      if (control && !control.valid) {
        control.markAsTouched();
        control.markAsDirty();
      }
    }
    this.onFormValueChanged();
    return this.modelForm.valid;
  }

  onFormValueChanged() {
    this.modelForm.valueChanges.subscribe(data => this.onValueChanged(data));
    this.onValueChanged();
  }

  onValueChanged(data?: any) {
    if (!this.modelForm) {
      return;
    }
    const form = this.modelForm;
    for (const field in this.formErrors) {
      this.formErrors[field] = '';
      const control = form.get(field);
      if (control && control.dirty && !control.valid) {
        const messages = this.validationErrors[field];
        for (const key in control.errors) {
          this.formErrors[field] += messages[key] + ' ';
        }
      }
    }
  }

  handleError(resposta: any): any {
    console.log('TELA:', resposta);

    let mensagem = (resposta.originalError.error.length
      ? resposta.originalError.error[0]
      : resposta.originalError.error) as Mensagem;
    if (resposta instanceof NotFound) {
      this._toastr.info(`Informação: ${mensagem.Texto}`, 'Não encontrado.');
    } else if (resposta instanceof NotAuthorized) {
      this._toastr.warning(`Alerta: ${mensagem.Texto}`, 'Não Autorizado.');
    } else if (resposta instanceof BadInput) {
      this._toastr.warning(`Alerta: ${mensagem.Texto}`, 'Advertência.');
    } else {
      //throw error;
      this._toastr.info(
        'Erro (Desconhecido) no servidor.',
        'Erro Desconhecido.'
      );
    }
  }

  handleErrorOld(resposta: any): any {
    console.log('handleError TELA', resposta);

    let mensagem = (resposta.originalError.error.length
      ? resposta.originalError.error[0]
      : resposta.originalError.error) as Mensagem;
    //let mensagem  =resposta.originalError.error as Mensagem;
    if (resposta instanceof NotFound) {
      this._toastr.info(`Informação: ${mensagem.Texto}`, 'Não encontrado.');
    } else if (resposta instanceof NotAuthorized) {
      this._toastr.warning(`Alerta: ${mensagem.Texto}`, 'Não Autorizado.');
    } else if (resposta instanceof BadInput) {
      this._toastr.warning(`Alerta: ${mensagem.Texto}`, 'Advertência.');
    } else {
      //throw error;
      this._toastr.info(
        'Erro (Desconhecido) no servidor.',
        'Erro Desconhecido.'
      );
      //this._toastr.warning(`Erro ${mensagem.Texto}.`, "Erro Desconhecido.");
    }
  }
}
