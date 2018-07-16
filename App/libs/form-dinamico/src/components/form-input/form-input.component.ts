import { Component, OnInit, Input } from '@angular/core';
import { FormControl, FormArray } from '@angular/forms';
import { FormularioControle, FormularioGroup } from '../../model/interfaces';
import { FormControlService } from '../../services/form-control.service';
import { ValidationFormService } from '../../services/validationForm.service';

@Component({
  selector: 'form-input',
  styleUrls: ['./form-input.component.css'],
  templateUrl: './form-input.component.html'
})
export class FormInputComponent implements OnInit {
  @Input() formularioControle: FormularioControle;
  @Input() formularioPrincipal: FormularioGroup;

  erro: string;

  cpfMask = [
    /\d/,
    /\d/,
    /\d/,
    '.',
    /\d/,
    /\d/,
    /\d/,
    '.',
    /\d/,
    /\d/,
    /\d/,
    '-',
    /\d/,
    /\d/
  ];
  cnpjMask = [
    /\d/,
    /\d/,
    '.',
    /\d/,
    /\d/,
    /\d/,
    '.',
    /\d/,
    /\d/,
    /\d/,
    '/',
    /\d/,
    /\d/,
    /\d/,
    /\d/,
    '-',
    /\d/,
    /\d/
  ];
  foneMask = [
    '(',
    /\d/,
    /\d/,
    ')',
    ' ',
    /\d/,
    /\d/,
    /\d/,
    /\d/,
    '-',
    /\d/,
    /\d/,
    /\d/,
    /\d/
  ];
  dataMask = [/\d/, /\d/, '/', /\d/, /\d/, '/', /\d/, /\d/, /\d/, /\d/];
  cepMask = [/\d/, /\d/, /\d/, /\d/, /\d/, '-', /\d/, /\d/, /\d/];
  numberMask = [/\d/, /\d/, /\d/, /\d/, /\d/, /\d/, /\d/, /\d/, /\d/, /\d/];
  //
  pattern = /[a-zA-Z]/;
  patternNumero = /^(\-[0-9]{0,10})|([0-9]{1,10})$/;
  patternTexto = /[\w\d Çç~^áàéèíìóòúùÁÀÉÈÍÌÓÒÚÙâãêîôõûÂÃÊÎÔÕÛäëïöüÄËÏÖÜñÿýÝ'"!@#$%¨&*()_+,.:?\\º¹²³£¢¬/¼½¾<>®±©]/;
  patternTextoPonto = /[a-zA-Z.]/;
  patternTextoObjeto = /^([a-zA-Z][a-zA-Z_0-9]{0,1000})|([a-zA-Z_0-9]{1,1000})$/;

  constructor(private _service: FormControlService) {}

  carregarValoresSession() {
    if (
      this.formularioControle &&
      (this.formularioControle.ControleTipo == 'dropdown' ||
        this.formularioControle.ControleTipo == 'checkbox' ||
        this.formularioControle.ControleTipo == 'radiobutton')
    ) {
      var listaValores = JSON.parse(sessionStorage.getItem('listaValores'));

      if (
        listaValores &&
        this.formularioControle.ListaValores != '' &&
        listaValores[this.formularioControle.ListaValores.toUpperCase()] != null
      ) {
        this.formularioControle.ListaOpcoes =
          listaValores[this.formularioControle.ListaValores.toUpperCase()];
      }

      //this.tratarCondicaoChecado();
    }

    this.tratarCondicaoVisivel(
      this.formularioControle.Valor,
      this.formularioControle.Key
    );
  }

  tratarCondicaoVisivel(valor: string, chave: string) {
    Object.keys(this.formularioPrincipal.Controles).forEach(key => {
      let ctr = this.formularioPrincipal.Controles[key] as FormularioControle;
      if (
        ctr.ControleCondicaoKey &&
        ctr.ControleCondicaoKey != '' &&
        ctr.ControleCondicaoKey == chave
      ) {
        ctr.Visivel =
          (ctr.ControleCondicaoValor.split('|') as Array<string>).findIndex(
            x => x == valor
          ) >= 0;
      }
    });
  }

  tratarCondicaoChecado() {
    if (
      this.formularioControle.ListaOpcoes != null &&
      this.formularioControle.ControleTipo == 'checkbox'
    ) {
      var arrayCheck = this.formularioPrincipal.Formulario.get(
        this.formularioControle.Key
      ) as FormArray;

      arrayCheck.controls.splice(arrayCheck.controls.length);

      var checado = this.formularioControle.ListaOpcoes.find(
        x =>
          x.Valor == this.formularioControle.Valor ||
          x.Nome == this.formularioControle.Valor
      );
    }
  }

  dropChange(event: any, chave: string) {
    this.tratarCondicaoVisivel(event.value, chave);
  }

  get isInValid() {
    const control = this.formularioPrincipal.Formulario.controls[
      this.formularioControle.Key
    ];
    this.erro = '';

    if (
      this.formularioControle.TypeData == 'cpf' &&
      (control.errors && !control.errors.required) &&
      control.value.length >= 14
    ) {
      if (control && control.dirty && !control.valid) {
        this.erro = this.getValidacao(4, control.errors);
        if (!this.erro) {
          this.erro = ` Existe um erro no campo: ${
            this.formularioControle.Label
          }.`;
        }
        return true;
      }
      return null;
    }

    if (
      this.formularioControle.TypeData == 'cnpj' &&
      (control.errors && !control.errors.required) &&
      control.value.length >= 18
    ) {
      if (control && control.dirty && !control.valid) {
        this.erro = this.getValidacao(4, control.errors);
        if (!this.erro) {
          this.erro = ` Existe um erro no campo: ${
            this.formularioControle.Label
          }.`;
        }
        return true;
      }
      return null;
    }

    let retorno = control && control.dirty && !control.valid;

    if (retorno) {
      this.mensagemErro();
      return true;
    }
    return null;
  }

  private getValidacao(tipo: number, errors: any) {
    var validacoes = this._service.formularioValidacoes.filter(
      x => x.NomePropriedade == this.formularioControle.Key && x.Tipo == tipo
    );

    if (validacoes && validacoes.length > 0) {
      this.erro = validacoes[0].Mensagem.Texto;

      if (!this.erro) {
        switch (tipo) {
          case 1:
            this.erro = `Campo ${this.formularioControle.Label} com erro.`;
            break;
          case 2:
            this.erro = `deve ser informado no máximo ${
              errors.maxlength.requiredLength
            } caracteres.`;
            break;
          case 3:
            this.erro = `deve ser informado no minimo ${
              errors.minlength.requiredLength
            } caracteres.`;
            break;
        }
      }
      return this.erro;
    }
    return null;
  }

  mensagemErro() {
    var control = this.formularioPrincipal.Formulario.controls[
      this.formularioControle.Key
    ];
    var errors = control.errors;

    this.erro = '';
    if (errors) {
      if (errors.required) {
        return this.getValidacao(1, control.errors);
      }

      if (errors.maxlength) {
        return this.getValidacao(2, control.errors);
      }

      if (errors.minlength) {
        return this.getValidacao(3, control.errors);
      }

      if (errors.validateCheckbox) {
        this.erro = this.getValidacao(4, control.errors);
        if (!this.erro) {
          this.erro = ` Selecione um ${this.formularioControle.Label}.`;
        }
        return this.erro;
      }
      if (errors.invalidEmailAddress) {
        this.erro = this.getValidacao(4, control.errors);
        if (!this.erro) {
          this.erro = ` Existe um erro no campo: ${
            this.formularioControle.Label
          }.`;
        }
        return this.erro;
      }

      if (errors.validateCheckboxFormArray) {
        this.erro = this.getValidacao(1, control.errors);
        if (!this.erro) {
          this.erro = ` Existe um erro no campo: ${
            this.formularioControle.Label
          }.`;
        }
        return this.erro;
      }

      if (errors.pattern) {
        this.erro = `erro pattern requer: ${errors.pattern.requiredPattern} .`;
        return this.erro;
      }
    }
    return this.erro;
  }

  onBlur() {
    var control = this.formularioPrincipal.Formulario.controls[
      this.formularioControle.Key
    ] as FormControl;

    if (control && this.formularioControle.TypeData == 'cpf') {
      return ValidationFormService.validarCpf(control);
    }

    if (control && this.formularioControle.TypeData == 'cnpj') {
      return ValidationFormService.validarCnpj(control);
    }
  }

  onKeyPress(event: any, tipo: string = null) {
    var inputChar = String.fromCharCode(
      !event.charCode ? event.which : event.charCode
    );
    var valor: string = event.target.value;

    if (tipo == 'number' && inputChar == '-' && valor.length) {
      valor += inputChar;
      if (valor.indexOf(inputChar) >= 0) {
        event.preventDefault();
        return;
      }
    }

    if (!this.pattern.test(inputChar)) {
      event.preventDefault();
    }
  }

  ngOnInit() {
    //console.log(this.formularioControle.Label, this.formularioControle.PlaceHolder);

    if (this.formularioControle && this.formularioControle.TypeData) {
      switch (this.formularioControle.TypeData) {
        case 'email':
          this.pattern = /[a-zA-Z._@]/;
          break;
        case 'Date':
          ValidationFormService.FormatDateValue(this.formularioControle);
          break;
        case 'number':
          this.pattern = this.patternNumero;
          break;
        case 'objeto':
          console.log(this.formularioControle);
          this.pattern = this.patternTextoObjeto;
          break;

        default:
          this.pattern = this.patternTexto;
          break;
      }
    }
    this.carregarValoresSession();
    this.erro = '';
  }
}
