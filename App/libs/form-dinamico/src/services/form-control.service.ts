import { Injectable } from '@angular/core';
import {
  FormControl,
  FormGroup,
  Validators,
  FormArray,
  ValidationErrors,
  AbstractControl
} from '@angular/forms';
import {
  FormularioGroup,
  FormularioControle,
  IControle,
  FormularioValidacao,
  ControleTipo,
  FormularioArray,
  Controle,
  Controles
} from '../model/interfaces';
import { ToastrService, ToastrConfig } from 'ngx-toastr';
import { ValidationFormService } from './validationForm.service';
import { Item } from '@myorg/core';

@Injectable()
export class FormControlService {
  formulario: FormularioGroup;
  formularioValidacoes: FormularioValidacao[];
  private idArray: number = 1;
  constructor(public _toastr: ToastrService) {
    this.formularioValidacoes = [];
  }

  toOptions(opcoes: string): any[] {
    let retorno = [];
    if (opcoes) {
      opcoes.split('|').forEach(texto => {
        retorno.push({
          Valor: texto,
          Nome: texto,
          Descricao: texto,
          Checado: false
        });
      });
    }
    //listaValores[chave.toUpperCase()] = session;
    //var listaValores = JSON.parse(sessionStorage.getItem('listaValores'));
    return retorno;
  }

  public controlesToArray(controles: Controles): Array<Controle> {
    var array = new Array<Controle>();
    Object.keys(controles).forEach(key => {
      array.push(controles[key]);
    });
    return array;
  }

  reset(controles: Controles, valor: any) {
    this.controlesToArray(controles).map(x => (x.Valor = valor[x.Key]));
  }

  buildFormulario(
    nome: string,
    label: string,
    estilo: string = '',
    ordem: number,
    controles: Controles = null,
    valor: any = null
  ) {
    let form = new FormularioGroup({
      key: nome,
      label: label,
      class: estilo,
      order: ordem
    });
    form.Valor = valor;
    if (controles && controles != null) {
      Object.keys(controles).forEach(key => {
        form.Controles[controles[key].Key] = controles[key];
      });
      this.toFormGroup(form);
      this.iniciarListaValores(form.Controles);
    }
    return form;
  }

  setControleCondicionado(controles: Controles) {
    let controle: Controle = null;
    Object.keys(controles).forEach(key => {
      let controle = controles[key];
      controle.Visivel = true;
      if (controle.Tipo == 1) {
        if ((controle as FormularioControle).ControleCondicaoKey != '') {
          controle.Visivel = false;
        }
      }
    });
  }

  iniciarListaValores(controles: Controles) {
    var listaValores = JSON.parse(sessionStorage.getItem('listaValores'));
    var valores = this.controlesToArray(controles).filter(x => x.AddLista);
    let valor: any;
    Object.keys(valores).forEach(key => {
      valor = valores[key];
    });
  }

  setItemListaValores(dados: any, controles: Controles, chave: string) {
    var listaSession = this.controlesToArray(controles).filter(x => x.AddLista);
    if (listaSession && listaSession.length > 0) {
      var listaValores = JSON.parse(sessionStorage.getItem('listaValores'));
      var session = listaValores[chave.toUpperCase()];
      if (session == undefined) {
        session = [];
      }

      var valor = listaSession
        .filter(x => x.AddLista)
        .find(x => x.AddListaChave == 'Valor');
      var nome = listaSession
        .filter(x => x.AddLista)
        .find(x => x.AddListaChave == 'Nome');
      var descricao = listaSession
        .filter(x => x.AddLista)
        .find(x => x.AddListaChave == 'Descricao');
      var index = listaSession.findIndex(x => x.Valor == dados.value['Id']);

      //if(dados.value && dados.value !="" && dados.value["Id"]!= null && dados.value[nome.Key]!= null )
      if (
        dados &&
        dados != '' &&
        dados['Id'] != null &&
        dados[nome.Key] != null
      ) {
        if (index < 0) {
          var novoItem = {
            Valor: dados['Id'],
            Nome: dados[nome.Key],
            Descricao: dados[descricao.Key],
            Checado: false
          } as Item;
          session.push(novoItem);
        } else {
          if (session[index] != null) {
            session[index].Nome = dados[nome.Key];
            session[index].Descricao = dados[descricao.Key];
          }
        }
        listaValores[chave.toUpperCase()] = session;
      }
      sessionStorage.setItem('listaValores', JSON.stringify(listaValores));
    }
  }

  addEntidade(array: FormArray, model: any) {
    let entidade = new Item();
    let grupo = new FormGroup({});
    var valor = '';
    var texto = '';
    Object.keys(model).forEach(key => {
      valor = model[key];
      //if(valor && valor!=null && valor!=undefined && key !="TId" && key !="Ativo" && key !="DataCadastro" && key !="DataAtualizacao")
      if (
        key != 'TId' &&
        key != 'Ativo' &&
        key != 'DataCadastro' &&
        key != 'DataAtualizacao'
      ) {
        texto += `${key}:'${valor}'; `;
        grupo.addControl(key, new FormControl(valor));
      }
    });
    array.push(grupo);
  }

  toFormGroup(grupo: FormularioGroup): FormGroup {
    if (grupo.Controles) {
      if (!grupo.Formulario) {
        grupo.Formulario = new FormGroup({});
      }
      var listaControles = this.controlesToArray(grupo.Controles).sort(
        (a, b) => a.Order - b.Order
      );
      Object.keys(listaControles).forEach(key => {
        var controle = listaControles[key];
        if (grupo.Valor && grupo.Valor[controle.Key]) {
          controle.Valor = grupo.Valor[controle.Key];
        }
        switch (controle.Tipo) {
          case ControleTipo.CONTROLE:
            var crtForm = controle as FormularioControle;
            crtForm.Type = controle.Type || 'text';
            crtForm.ControleCondicaoKey = controle.ControleCondicaoKey || '';
            crtForm.ControleCondicaoValor =
              controle.ControleCondicaoValor || '';
            crtForm.Editavel = grupo.Valor == null ? false : !controle.Editavel;
            crtForm.Listagem = controle.Listagem;
            crtForm.Visivel = controle.Visivel || true;
            grupo.Formulario.addControl(crtForm.Key, this.toControl(crtForm));
            break;

          case ControleTipo.ARRAY:
            var array = controle as FormularioArray;
            var crt = this.toFormArray(controle as FormularioArray);
            array.Valores = crt;
            grupo.Formulario.addControl(controle.Key, crt);
            break;

          case ControleTipo.GRUPO:
            grupo.Formulario.addControl(
              controle.Key,
              this.toFormGroupFilho(controle as FormularioGroup)
            );
            break;
        }
      });
      this.formulario = grupo;
    }

    return grupo.Formulario;
  }

  toFormGroupFilho(
    group: FormularioGroup,
    controles: Array<Controle> = null
  ): FormGroup {
    var lista = group.Controles;
    group.Controles = new Controles();
    Object.keys(lista).forEach(key => {
      let controle = lista[key];
      group.Controles[lista[key].Key] = lista[key];
    });

    group.Formulario = this.toFormGroup(group);

    return group.Formulario;
  }

  toFormArray(array: FormularioArray): FormArray {
    array.Formulario = new FormGroup({});
    var formArray = new FormArray(
      [],
      ValidationFormService.validateCheckboxFormArray
    );
    if (array.ControleTipo == 'listaentidade') {
      let crt = array.ControlType as FormularioGroup;
      array.Formulario.addControl(crt.Key, this.toFormGroupFilho(crt));
      let lista = array.Valor as Array<any>;
      if (lista != null && lista.length > 0) {
        Object.keys(lista).forEach(key => {
          this.addEntidade(formArray, lista[key]);
        });
      }
      this.setValidatorControl(array.Validacoes, array.Key, array.Formulario);
    }

    if (array.ControleTipo == 'listavalor') {
      let crt = array.ControlType as FormularioControle;
      array.Formulario.addControl(crt.Key, this.toControl(crt));
    }
    return formArray;
  }

  toFormArrayFromListaValor(
    array: FormularioArray,
    lista: Array<any>
  ): FormularioArray {
    array.Formulario = new FormGroup({});
    var formArray = new FormArray(
      [],
      ValidationFormService.validateCheckboxFormArray
    );
    let crt = array.ControlType as FormularioGroup;
    array.Formulario.addControl(crt.Key, this.toFormGroupFilho(crt));
    if (lista != null && lista.length > 0) {
      Object.keys(lista).forEach(key => {
        this.addEntidade(formArray, lista[key]);
      });
    }

    array.Valores = formArray;
    return array;
  }

  toFormArrayItens(itens: Item[], valor: string = ''): FormArray {
    var formArray = new FormArray(
      [],
      ValidationFormService.validateCheckboxFormArray
    );
    const valores = valor.split(',');
    itens.map(x =>
      formArray.push(new FormControl(valores.indexOf(x.Valor) > -1))
    );
    return formArray;
  }

  toControl(controle: FormularioControle): AbstractControl {
    //var formControl:AbstractControl = new FormControl({value: controle.Valor || '', disabled: !controle.Editavel});
    var formControl: AbstractControl = new FormControl();
    formControl.setValue(controle.Valor || '');
    if (
      controle.ControleTipo == 'dropdown' ||
      controle.ControleTipo == 'radiobutton'
    ) {
      if (controle.ListaOpcoes == null || controle.ListaOpcoes.length == 0) {
        controle.ListaOpcoes = this.toOptions(controle.ListaValores);
      }
    }

    if (controle.ControleTipo == 'checkbox') {
      if (controle.ListaOpcoes == null || controle.ListaOpcoes.length == 0) {
        controle.ListaOpcoes = this.toOptions(controle.ListaValores);
      }

      this.setValidatorControl(
        controle.Validacoes,
        controle.Key,
        formControl,
        controle
      );
      return this.toFormArrayItens(controle.ListaOpcoes, controle.Valor);
    }
    this.setValidatorControl(
      controle.Validacoes,
      controle.Key,
      formControl,
      controle
    );
    return formControl;
  }

  setValidatorControl(
    validacaoes: FormularioValidacao[],
    controlKey: string,
    control: AbstractControl,
    input: FormularioControle = null
  ) {
    let validators: any[] = [];
    if (validacaoes != null) {
      var validaCampo = validacaoes.filter(
        x => x.NomePropriedade == controlKey
      );
      if (validaCampo) {
        validaCampo.forEach(validacao => {
          switch (validacao.Tipo) {
            case 1: //
              validators.push(Validators.required);
              break;

            case 2:
              validators.push(
                Validators.maxLength(Number.parseInt(validacao.Formato))
              );
              break;

            case 3:
              validators.push(
                Validators.minLength(Number.parseInt(validacao.Formato))
              );
              break;

            case 4:
              if (input.TypeData == 'cpf' && validacao.Formato) {
                validators.push(ValidationFormService.ValidandoCpf(control));
              } else if (input.TypeData == 'cnpj') {
                validators.push(ValidationFormService.ValidandoCnpj(control));
              } else {
                validators.push(Validators.pattern(validacao.Formato));
              }
              break;

            case 5:
              validators.push(ValidationFormService.emailValidator);
              break;
          }

          var valida = this.formularioValidacoes.findIndex(
            x => x.Mensagem.Chave == validacao.Mensagem.Chave
          );
          if (valida == -1) {
            this.formularioValidacoes.push(validacao);
          }
        });
      }
    }
    //Insere os validators
    control.setValidators(validators);
  }
  onValidarFormulario() {
    this.validarFormulario(this.formulario);
  }

  onFormatarJsonControls(input: any, selecionados: any, valor: any) {
    let listaControles;
    if (input instanceof FormGroup) {
      listaControles = selecionados.map(x => valor[x.Key]);
      if (listaControles && listaControles.length > 0) {
        //let chave = "";
        let newValor = '';
        Object.keys(listaControles).forEach(selKey => {
          if (listaControles[selKey] instanceof Array) {
            newValor = '';
            for (
              let index = 0;
              index < listaControles[selKey].length;
              index++
            ) {
              if (listaControles[selKey][index]) {
                newValor += selecionados[selKey].ListaOpcoes[index].Valor + ',';
              }
            }
            newValor =
              newValor.length > 0
                ? newValor.substr(0, newValor.length - 1)
                : '';
          }

          if (newValor != '') {
            valor[selecionados[selKey].Key] = newValor;
          }
        });
      }
    }

    if (input instanceof FormArray) {
      for (let index = 0; index < input.controls.length; index++) {
        this.onFormatarJsonControls(
          input.controls[index] as FormGroup,
          selecionados,
          input.value[index]
        );
      }
    }
  }

  onFormatarJson(formulario: FormularioGroup) {
    let listaCrt = this.controlesToArray(formulario.Controles).filter(
      x =>
        x.Tipo == 3 ||
        x.Tipo == 2 ||
        (x.Tipo == 1 && x.ControleTipo == 'checkbox')
    );
    if (listaCrt) {
      Object.keys(listaCrt).forEach(key => {
        let crtCheckBox;
        let crt = formulario.Formulario.get(listaCrt[key].Key);
        if (listaCrt[key].ControleTipo == 'entidade') {
          let crtCheckBox = this.controlesToArray(
            listaCrt[key].Controles
          ).filter(x => x.ControleTipo == 'checkbox');
          if (crtCheckBox.length > 0) {
            this.onFormatarJsonControls(
              crt as FormGroup,
              crtCheckBox,
              (crt as FormGroup).value
            );
          }
        } else if (listaCrt[key].ControleTipo == 'listaentidade') {
          let crtCheckBox = this.controlesToArray(
            listaCrt[key].ControlType.Controles
          ).filter(x => x.ControleTipo == 'checkbox');
          if (crtCheckBox.length > 0) {
            this.onFormatarJsonControls(
              listaCrt[key].Valores,
              crtCheckBox,
              (crt as FormGroup).value
            );
          }
        } else {
          let list = new Array<Controle>();
          list.push(listaCrt[key]);
          this.onFormatarJsonControls(crt.parent, list, crt.parent.value);
        }
      });
    }
    let jsonValue = JSON.stringify(formulario.Formulario.value).replace(
      /("Id":"@\d{0,30}"+?)/g,
      '"Id":0'
    );
    return jsonValue;
  }

  validarFormulario(controle: FormularioGroup): any {
    if (!controle) return false;
    var existeErro = this.formValidationErrors(controle.Formulario);

    if (controle.Formulario.invalid) {
      this._toastr.warning(
        'Por favor, verifique o(s) campo(s) com erro no formulário.',
        'Atenção'
      );
      return false;
    }
    let jsonValue = this.onFormatarJson(controle);
    return JSON.parse(jsonValue);
  }

  formValidationErrors(controle: AbstractControl, key: string = 'Principal') {
    let form: any;
    if (controle instanceof FormControl) {
      this.controlErrors(controle, 'controle');
    } else if (controle instanceof FormGroup) {
      form = controle as FormGroup;
    } else if (controle instanceof FormArray) {
      form = controle as FormArray;
    }
    if (form) {
      Object.keys(form.controls).forEach(key => {
        let crt = form.get(key);

        if (crt instanceof FormControl) {
          this.controlErrors(crt, key);
        } else if (crt instanceof FormArray || crt instanceof FormGroup) {
          this.controlErrors(crt, key);
          if (crt.invalid) {
            this.formValidationErrors(crt, key);
          }
        }
      });
    }
  }

  controlErrors(controle: AbstractControl, key: string): boolean {
    //if (!controle.disable) {
    if (controle && (controle.errors != null || controle.invalid)) {
      controle.markAsTouched();
      controle.markAsDirty();
      if (controle.errors && controle instanceof FormArray) {
        var validaCampo = this.formularioValidacoes.filter(
          x => x.NomePropriedade == key && x.Tipo > 0
        );
        if (validaCampo != null && validaCampo.length) {
          this._toastr.info(validaCampo[0].Mensagem.Texto, 'Informe.');
        }
      }
      return false;
    }
    return false;
  }
}
