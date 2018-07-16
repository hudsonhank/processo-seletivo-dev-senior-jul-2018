import {
  Component,
  ViewChild,
  Input,
  OnInit,
  Output,
  EventEmitter
} from '@angular/core';
import {
  MatPaginator,
  MatSort,
  MatTableDataSource,
  MatDialog,
  MatPaginatorIntl
} from '@angular/material';

import { FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { Research } from '@myorg/core';
import {
  FormularioArray,
  FormularioGroup,
  ColunaDefinicao,
  ColunaValor,
  FormularioControle,
  Controle
} from '../../model/interfaces';
import { FormControlService } from '../../services/form-control.service';
import { FormModalComponent } from '../form-modal/form-modal.component';
import { ValidationFormService } from '../../services/validationForm.service';

@Component({
  selector: 'form-grid',
  templateUrl: './form-grid.component.html',
  styleUrls: ['./form-grid.component.css']
})
export class FormGridComponent implements OnInit {
  @Input() formularioArray: FormularioArray;
  @Input() formularioPrincipal: FormularioGroup;
  @Input() acoes: any;
  @Output() excluir = new EventEmitter(false);

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  //@ViewChild(MatPaginatorIntl) paginatorLabel: MatPaginatorIntl;

  dataSource: MatTableDataSource<ColunaValor> = null;
  colunas: ColunaDefinicao[] = [];
  displayedColumns: string[] = [];
  controlesTypes: Controle[] = [];

  constructor(
    public dialog: MatDialog,
    public _formService: FormControlService,
    public _router: Router,
    public _toastr: ToastrService
  ) {}

  ngOnInit() {
    this.controlesTypes= this._formService.controlesToArray(
      this.formularioArray.ControlType.Controles
    );
    this.controlesTypes = this.controlesTypes.filter(x => x.Listagem || x.Key=="Id");
    Object.keys(this.controlesTypes).forEach(key => {
      var prop = this.controlesTypes[key];
      /*if (prop.Key == 'Id') {
        prop.Label = '#';
      }*/
      //if( prop.Key == "Id" ||  (prop.ControleTipo != "hidden" && prop.ControleTipo.indexOf('lista')==-1) )
      if ((prop.ControleTipo != 'hidden' &&  prop.ControleTipo.indexOf('lista') == -1)
      ) {
        this.colunas.push(
          new ColunaDefinicao(
            prop.Key,
            prop.Label,
            (row: ColunaValor, index: string) => `${row[index]}`
          )
        );
      }
    });
    /** Column definitions in order */
    this.displayedColumns = this.colunas.map(x => x.Key);

    //this.colunas.push(new ColunaDefinicao("Acoes","Ações", (row: ColunaValor, index: string) => `${row[index]}`));
    this.displayedColumns.push('editar');
    this.displayedColumns.push('excluir');

    this.onAtualizarValores(this.formularioArray.Valores.value);
  }

  ngAfterViewInit() {
    this.dataSource.sort = this.sort;
    var label = new MatPaginatorIntl();
    label.firstPageLabel = 'Primeira página';
    label.nextPageLabel = 'Próxima';
    label.previousPageLabel = 'Anterior';
    label.itemsPerPageLabel = 'Items por página';
    label.lastPageLabel = 'Última página';
    this.paginator._intl = label;
    this.paginator.showFirstLastButtons = true;
    this.dataSource.paginator = this.paginator;
  }

  applyFilter(filterValue: string) {
    filterValue = filterValue.trim(); // Remove whitespace
    filterValue = filterValue.toLowerCase(); // Datasource defaults to lowercase matches
    this.dataSource.filter = filterValue;
  }

  onAtualizarValores(valores: Array<any>, research: Research = null) {

    let items = [];
    var i = 0;
    var listaValores = JSON.parse(sessionStorage.getItem('listaValores'));
    if(valores!=null && valores.length>0)
    {      
      Object.keys(valores).forEach(key => {
        items.push(this.transportaValores(i + 1, valores[key], listaValores));
      });
    }

    if (this.dataSource == null) {
      this.dataSource = new MatTableDataSource(items);
    } else {
      
      //this.dataSource.data = research.Itens;
      this.dataSource.data = items;
      this.dataSource.filter = '';
      this.ngAfterViewInit();
    }
  }
  transportaValores(id: number, controle: any, listaSession: any): ColunaValor {    
    let props: ColunaValor = new ColunaValor();    
    let campoLista: any;
    let valor: any;

    Object.keys(controle).forEach(key => {
      valor = controle[key];      
      campoLista =this.controlesTypes.find(x => x.Key == key);
      if (campoLista != undefined && campoLista.TypeData && campoLista.TypeData == 'Date') 
      {
        campoLista.Valor =valor;
        ValidationFormService.FormatDateValue(campoLista as FormularioControle);
        valor = campoLista.Valor;
      }

      if (valor &&  campoLista != undefined  && campoLista.ListaValores != null && campoLista.ListaValores != '' ) {
        var lista = listaSession[campoLista.ListaValores];
        var valorLista = null;
        if (lista) {
          valorLista = lista.find(x => x.Valor == valor);
          if (valorLista) {
            valor = valorLista.Nome;
          }
        }
      }

      props[key] = '';
      if (valor) {
        props[key] = valor;
      }
    });
    return props;
  }

  excluirItem(item) {
    var index = this.formularioArray.Valores.value.findIndex(
      x => x.Id == item.Id
    );
    this.formularioArray.Valores.controls.splice(index, 1);
    this.formularioArray.Valores.value.splice(index, 1);
    this.onAtualizarValores(this.formularioArray.Valores.value);

    if (item.Id != null && item.Id != '') {
      this.excluir.emit(item.Id);
    }
  }

  editarItem(item) {
    if (this.acoes != null) {
      sessionStorage.setItem(this.acoes.Chave, item.Id);
      this._router.navigate([this.acoes.Editar]);
    } else {
      const dialogRef = this.dialog.open(FormModalComponent, {
        disableClose: true,
        height: 'auto',
        width: '1024px',
        data: { formulario: this.formularioArray, valor: item }
      });

      dialogRef.afterClosed().subscribe(dados => {
        this.onEdiTarDados(dados);
      });
    }
  }

  onEdiTarDados(dados: FormGroup) {
    if (dados && dados != null) {
      var index = this.formularioArray.Valores.value.findIndex(
        x => x.Id == dados.value['Id']
      );

      if (index >= 0) {
        this.formularioArray.Valores.value[index] = dados.value;
      } else {

        this.formularioArray.Valores.push(dados);
      }

      this.onAtualizarValores(this.formularioArray.Valores.value);
    }
  }
}
