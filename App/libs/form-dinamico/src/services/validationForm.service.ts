import {
  AbstractControl,
  FormArray,
  FormGroup,
  FormControl,
  ValidatorFn
} from '@angular/forms';
import { Injectable } from '@angular/core';
//import { FormularioControle } from '@sdkrepo/modulo-form-dinamico/src/model/interfaces';
import { DatePipe } from '@angular/common';
import { FormularioControle } from '@myorg/form-dinamico';

function ValidarCnpjFunction(valor: string) {
  var cnpj = valor.replace(/[^0-9]+/g, '');

  if (cnpj && cnpj.length != 14) {
    return null;
  }
  let multiplicador1 = [5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
  let multiplicador2 = [6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];

  let soma = 0;
  let digito;

  for (var i = 0; i < 12; i++) {
    soma += Number.parseInt(cnpj[i].toString()) * multiplicador1[i];
  }

  soma = soma % 11;

  if (soma < 2) {
    digito = '0';
  } else {
    digito = (11 - soma).toString();
  }

  let tempCnpj = cnpj.substring(0, 12) + digito;
  soma = 0;

  for (var j = 0; j < 13; j++) {
    soma += Number.parseInt(tempCnpj[j].toString()) * multiplicador2[j];
  }

  soma = soma % 11;

  if (soma < 2) {
    digito = '0';
  } else {
    digito = (11 - soma).toString();
  }

  let erro = cnpj.endsWith(digito) ? null : { invalidarCnpj: true };
  return erro;
}

function ValidarCpfFunction(valor: string) {
  var teste = false;
  var numeros, digitos, soma, i, resultado, digitos_iguais;
  digitos_iguais = 1;

  var cpf = valor.replace(/[^0-9]+/g, '');

  if (cpf && cpf.length != 11) {
    return null;
  }

  var triviais = [
    '00000000000',
    '11111111111',
    '22222222222',
    '33333333333',
    '44444444444',
    '55555555555',
    '66666666666',
    '77777777777',
    '88888888888',
    '99999999999'
  ];

  if (triviais.indexOf(cpf) > -1) {
    return true;
  }

  for (i = 0; i < cpf.length - 1; i++) {
    if (cpf.charAt(i) != cpf.charAt(i + 1)) {
      digitos_iguais = 0;
      break;
    }
  }

  if (!digitos_iguais) {
    numeros = cpf.substring(0, 9);
    digitos = cpf.substring(9);
    soma = 0;
    for (i = 10; i > 1; i--) soma += numeros.charAt(10 - i) * i;

    resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;

    if (resultado != digitos.charAt(0)) {
      return true;
    }

    numeros = cpf.substring(0, 10);
    soma = 0;
    for (i = 11; i > 1; i--) soma += numeros.charAt(11 - i) * i;
    resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;

    if (resultado != digitos.charAt(1)) {
      return true;
    }

    return null;
  } else {
    return true;
  }
}

@Injectable()
export class ValidationFormService {
  static validarCpf(c: FormControl) {
    let cpf_regex = /^\d{3}\.\d{3}\.\d{3}\-\d{2}$/;
    if (!cpf_regex.test(c.value)) {
      c.setValue('');
      return { validarCpf: true };
    }
    return null;
  }

  static validarCnpj(c: FormControl) {
    let cnpj_regex = /^\d{2}\.\d{3}\.\d{3}\/\d{4}\-\d{2}$/;
    if (!cnpj_regex.test(c.value)) {
      c.setValue('');
      return { validarCnpj: true };
    }
    return null;
  }

  static ValidandoCpf(AC: AbstractControl): ValidatorFn {
    return (AC): { [key: string]: any } => {
      if (AC.value) {
        return ValidarCpfFunction(AC.value) ? { invalidarCpf: true } : null;
      }
      return null;
    };
  }

  static ValidandoCnpj(AC: AbstractControl): ValidatorFn {
    return (AC): { [key: string]: any } => {
      if (AC.value) {
        return ValidarCnpjFunction(AC.value) ? { invalidarCnpj: true } : null;
      }
      return null;
    };
  }

  static ValiadarPassword(AC: AbstractControl) {
    let password = AC.get('Senha').value; // to get value in input tag
    let confirmPassword = AC.get('ConfirmaNovaSenha').value; // to get value in input tag
    if (password != confirmPassword)
      AC.get('ConfirmaNovaSenha').setErrors({ MatchPassword: true });
    else return null;
  }

  static minLengthArray(min: number) {
    return (c: AbstractControl): { [key: string]: any } => {
      if (c.value.length >= min) return null;
      return { minLengthArray: { valid: false } };
    };
  }

  static multipleCheckboxRequireOne(fa: FormArray) {
    let valid = false;
    for (let x = 0; x < fa.length; ++x) {
      if (fa.at(x).value) {
        valid = true;
        break;
      }
    }

    return valid
      ? null
      : {
          multipleCheckboxRequireOne: true
        };
  }

  static validateCheckboxVeio(formGroup: FormGroup) {
    for (let key in formGroup.controls) {
      if (formGroup.controls.hasOwnProperty(key)) {
        let control: FormControl = <FormControl>formGroup.controls[key];
        if (control.value) {
          return null;
        }
      }
    }
    return {
      validateCheckbox: {
        valid: false
      }
    };
  }

  static validateCheckboxFormArray(formGroup: FormArray) {
    for (let key in formGroup.controls) {
      if (formGroup.controls.hasOwnProperty(key)) {
        let control: FormControl = <FormControl>formGroup.controls[key];
        if (control.value) {
          return null;
        }
      }
    }
    return {
      validateCheckboxFormArray: {
        valid: false
      }
    };
  }

  static validateCheckboxFormArrayGroup(formGroup: FormArray) {
    for (let key in formGroup.controls) {
      if (formGroup.controls.hasOwnProperty(key)) {
        let control: FormGroup = <FormGroup>formGroup.controls[key];

        for (let keyControl in control.controls) {
          if (control.controls.hasOwnProperty(keyControl)) {
            let ctr: FormControl = <FormControl>control.controls[keyControl];
            if (ctr.valid) {
              return null;
            }
          }
        }
        return {
          validateCheckboxFormArrayGroup: {
            valid: false
          }
        };
      }
    }

    return {
      validateCheckboxFormArrayGroup: {
        valid: false
      }
    };
  }

  static getValidatorErrorMessage(code: string) {
    let config = {
      required: 'Required',
      invalidCreditCard: 'Is invalid credit card number',
      invalidEmailAddress: 'Invalid email address',
      invalidPassword:
        'Invalid password. Password must be at least 6 characters long, and contain a number.'
    };
    return config[code];
  }

  static creditCardValidator(control: AbstractControl) {
    // Visa, MasterCard, American Express, Diners Club, Discover, JCB
    if (
      control.value.match(
        /^(?:4[0-9]{12}(?:[0-9]{3})?|5[1-5][0-9]{14}|6(?:011|5[0-9][0-9])[0-9]{12}|3[47][0-9]{13}|3(?:0[0-5]|[68][0-9])[0-9]{11}|(?:2131|1800|35\d{3})\d{11})$/
      )
    ) {
      return null;
    } else {
      return { invalidCreditCard: true };
    }
  }

  static emailValidator(control: AbstractControl) {
    // RFC 2822 compliant regex
    if (
      control.value.match(
        /[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?/
      )
    ) {
      return null;
    } else {
      return { invalidEmailAddress: true };
    }
  }

  static passwordValidator(control: AbstractControl) {
    // {6,100}           - Assert password is between 6 and 100 characters
    // (?=.*[0-9])       - Assert a string has at least one number
    // (?!.*\s)          - Spaces are not allowed
    if (
      control.value.match(/^(?=.*\d)(?=.*[a-zA-Z!@#$%^&*])(?!.*\s).{6,100}$/)
    ) {
      return null;
    } else {
      return { invalidPassword: true };
    }
  }

  static loginValidator(control: AbstractControl) {
    // {6,100}           - Assert password is between 6 and 100 characters
    // (?=.*[0-9])       - Assert a string has at least one number
    // (?!.*\s)          - Spaces are not allowed
    if (
      control.value.match(/^(?=.*\d)(?=.*[a-zA-Z!@#$%^&*])(?!.*\s).{6,100}$/)
    ) {
      return null;
    } else {
      return { invalidLogin: true };
    }
  }

  static FormatDateValue(AC: FormularioControle) {
    if (AC.Valor) {
      var dateValor = AC.Valor instanceof Date ? AC.Valor : new Date(AC.Valor);
      var datePipe = new DatePipe('pt');
      
      AC.Valor = datePipe.transform(dateValor, 'MM/dd/yyyy');


    }
  }
}
