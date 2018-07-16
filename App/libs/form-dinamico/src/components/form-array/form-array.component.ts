import { Component, OnInit, Input, ViewChild } from '@angular/core';
import {
  FormularioArray,
  FormularioGroup,
  Controles
} from '../../model/interfaces';
import { FormGroup, FormArray } from '@angular/forms';
import { FormControlService } from '../../services/form-control.service';
import { ToastrService } from 'ngx-toastr';

import { MatDialog } from '@angular/material';
import { FormModalComponent } from '../form-modal/form-modal.component';
import { FormGridComponent } from '../form-grid/form-grid.component';

@Component({
  selector: 'form-array',
  templateUrl: './form-array.component.html'
})
export class FormArrayComponent implements OnInit {
  @Input() formularioArray: FormularioArray;
  @Input() formularioPrincipal: FormularioGroup;
  @ViewChild('formGridArrayId') formGrid: FormGridComponent;
  maior: number = 1;
  valores: Array<any>;

  //HTML
  //[formulario]="formularioPrincipal.Formulario"

  constructor(
    public dialog: MatDialog,
    public _formService: FormControlService,
    public _toastr: ToastrService
  ) {}

  ngOnInit() {
    if (this.formularioArray && this.formularioArray != null) {
      if (this.formularioArray.Valores == null) {
        this.formularioArray.Valores = new FormArray([]);

        //this.valores = new Array<any>();
      } else {
        //this.valores = this.formularioArray.Valor as Array<any>;
      }

      if (
        this.formularioArray.Valores &&
        this.formularioArray.Valores.length > 0
      ) {
        //var lista = (this.formularioArray.Valor as Array<any>).map(x=>`${x.Id}`.toString().replace("@",""));

        var lista = (this.formularioArray.Valores.value as Array<any>).map(x =>
          `${x.Id}`.toString().replace('@', '')
        );

        this.maior = Math.max.apply(null, lista) + 1;
      }

      //this.formularioArray.Valores
      //this.valoresFormArray = (this.formularioPrincipal.Formulario.controls[this.formularioArray.Key] as FormArray);
      //this.valoresFormArray = (this.formularioPrincipal.Formulario.controls[this.formularioArray.Key] as FormArray);
    }
  }

  onAddLista() {
    const dialogRef = this.dialog.open(FormModalComponent, {
      disableClose: true,
      height: 'auto',
      width: '1024px',
      //data:  { principal:this.formularioPrincipal, formulario: this.formularioArray, valor: null}
      data: { formulario: this.formularioArray, valor: null }
    });
    dialogRef.afterClosed().subscribe(dados => {
      this.onAddDados(dados);
    });
  }

  onAddDados(dados: FormGroup) {
    if (dados && dados != null) {
      if (
        dados.value['Id'] == null ||
        dados.value['Id'] == '' ||
        dados.value['Id'] == '0'
      ) {
        dados.value['Id'] = '@' + this.maior++;
      }

      this.formularioArray.Valores.push(dados);
      this.formGrid.onAtualizarValores(this.formularioArray.Valores.value);
      //(this.formularioPrincipal.Formulario.controls[this.formularioArray.Key] as FormArray).push(dados);
      //this.formGrid.onAtualizarValores((this.formularioPrincipal.Formulario.controls[this.formularioArray.Key] as FormArray).value);

      this._formService.setItemListaValores(
        dados,
        this.formularioArray.ControlType.Controles as Controles,
        this.formularioArray.Key
      );

      /*(this.formularioPrincipal.Formulario.controls[this.formularioArray.Key] as FormArray).push(dados);
			this.formGrid.onAtualizarValores((this.formularioPrincipal.Formulario.controls[this.formularioArray.Key] as FormArray).value);
			this._formService.setItemListaValores(dados, this.formularioArray.ControlType.Controles as Array<Controle>,this.formularioArray.Key);*/
    }
  }
}
