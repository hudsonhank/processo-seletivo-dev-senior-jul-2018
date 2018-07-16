import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import {
  Controle,
  FormularioGroup,
  FormularioArray
} from '../../model/interfaces';
import { FormControlService } from '../../services/form-control.service';
import { FormGroup } from '@angular/forms';

@Component({
  selector: 'form-modal',
  templateUrl: './form-modal.component.html',
  styleUrls: ['./form-modal.component.css']
})
export class FormModalComponent implements OnInit {
  controle: FormularioGroup;
  valor: any;
  titulo: string = '';
  formulario: FormularioArray;
  textoAcao = 'Adicionar';

  constructor(
    public dialogRef: MatDialogRef<FormModalComponent>,
    @Inject(MAT_DIALOG_DATA) public dados: any,
    public _formService: FormControlService
  ) {
    this.formulario = dados.formulario as FormularioArray;
    this.valor = dados.valor;
    this.titulo = this.formulario.Label;
  }

  ngOnInit() {
    if (this.valor == null)
      this._formService.reset(
        this.formulario.ControlType.Controles,
        JSON.parse(
          this.formulario.Default && this.formulario.Default != null
            ? this.formulario.Default
            : '[]'
        )
      );
    else this.textoAcao = 'Atualizar';

    this.controle = this._formService.buildFormulario(
      'formularioModal',
      this.titulo,
      '',
      1,
      this.formulario.ControlType.Controles,
      this.valor
    );
    this.controle.Label = '';
  }

  onCloseConfirm() {
    var value = this._formService.validarFormulario(this.controle);
    if (value) {
      //let formValue = JSON.parse(value);
      //this.controle.Valor = formValue;
      //console.log(this.controle);
      this.dialogRef.close(this.controle.Formulario);
      //this.dialogRef.close(formValue);
    }
  }

  onCloseCancel() {
    this.dialogRef.close(null);
    return null;
  }
}
