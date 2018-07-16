import { ActivatedRoute, Router } from '@angular/router';
import {
  Component,
  OnInit,
  ViewChild,
  OnChanges,
  OnDestroy,
  ChangeDetectionStrategy
} from '@angular/core';
import { Subscription } from 'rxjs/Rx';
import { ToastrService } from 'ngx-toastr';
import { FormArray } from '@angular/forms';
import { Store } from '@ngrx/store';
import 'rxjs/add/operator/let';
import { WorkSpaceComponent } from '@myorg/shared';

import {
  FormularioArray,
  FormularioGroup,
  FormControlService,
  Controles,
  FormularioControle,
  FormGridComponent
} from '@myorg/form-dinamico';

import {
  LivroService,
  LivroState,
  Livro,
  DeleteLivro,
  getLivros,
  LoadLivros
} from '@myorg/livros-state';

//export interface TodosState extends Todos, Filter { }

@Component({
  selector: 'app-livro-listar',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './livros-listar.component.html'
})
export class LivroListarComponent extends WorkSpaceComponent<Livro>
  implements OnInit, OnChanges, OnDestroy {
  @ViewChild('formGridId') formGrid: FormGridComponent;
  filtros: FormularioGroup;
  valores: FormularioArray = new FormularioArray();
  acoes: any;
  subscription: Subscription;
  filtroQuery: string;
  constructor(
    public _service: LivroService,
    public _formService: FormControlService,
    public _router: Router,
    public _route: ActivatedRoute,
    public _toastr: ToastrService,
    private _store: Store<LivroState>
  ) {
    super(_router, _toastr, null);

    this.subscription = this._route.data.subscribe(
      (resolver: { LivroForm: FormularioGroup }) => {
        this.valores = new FormularioArray();
        this.valores.ControlType = resolver.LivroForm;
      },
      err => this.handleError(err)
    );
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }
  ngOnInit() {
    //Filtros
    this.filtros = this._formService.buildFormulario(
      'Filtros',
      'Livros',
      'panel box box-danger',
      1,
      this.getCamposFiltros()
    );
    this.acoes = {
      Editar: '/modulo-livro/formulario/edi',
      Chave: 'livroId',
      Store: this._store,
      Action: DeleteLivro
    };
    this.formGrid = null;
    this.getAll();
  }

  getAll(): void {
    this.subscription = this._store.select(getLivros).subscribe(result => {
      this.valores = this._formService.toFormArrayFromListaValor(this.valores,result.Resultado);
      if (this.formGrid) {
        this.formGrid.onAtualizarValores(this.valores.Valores.value, result);
      }
    });
  }

  getCamposFiltros(): Controles {
    let controles: Controles = new Controles();
    controles['TextoExtra'] = new FormularioControle({ key: 'TextoExtra',  label: 'Pesquisa', placeholder: 'Título, Autor, Editora...',   class: 'col-md-4',  value: '',   editavel: true,   order: 1  });
    controles['Titulo'] = new FormularioControle({ key: 'Titulo',  controleTipo: 'textbox', typeData: 'text', label: 'Título', placeholder: 'Título, Autor, Editora...',   class: 'col-md-4',  value: '',   editavel: true,   order: 2  });
    controles['Autor'] = new FormularioControle({ key: 'Autor',  controleTipo: 'textbox', typeData: 'text', label: 'Autor', placeholder: 'Autor',   class: 'col-md-4',  value: '',   editavel: true,   order: 3  });
    controles['Genero'] = new FormularioControle({ key: 'Genero',  label: 'Gênero', placeholder: 'Gênero',   class: 'col-md-4',  value: '',   editavel: true,   order: 4  });
    controles['Editora'] = new FormularioControle({ key: 'Editora',  label: 'Editora', placeholder: 'Editora',   class: 'col-md-4',  value: '',   editavel: true,   order: 5  });
    controles['PublicacaoAno'] = new FormularioControle({ key: 'PublicacaoAno',  label: 'Ano',typeData: 'number', placeholder: 'Ano de Publicação', maximo:4,   class: 'col-md-4',  value: '',    editavel: true,   order: 1  });    
    return controles;
  }

  filtrosModel(): boolean {
    this.filtroQuery = '';
    var temFiltro = false;    
    if (this.filtros.Formulario.get('TextoExtra').value.trim() != '') {
      this.filtroQuery +=
        (this.filtroQuery != '' ? '&' : '?') +
        `TextoExtra=${this.filtros.Formulario.get('TextoExtra').value.trim()}`;
      temFiltro = true;
    }

    if (this.filtros.Formulario.get('Titulo').value.trim() != '') {
      this.filtroQuery +=
        (this.filtroQuery != '' ? '&' : '?') +
        `Titulo=${this.filtros.Formulario.get('Titulo').value.trim()}`;
      temFiltro = true;
    }

    if (this.filtros.Formulario.get('Autor').value.trim() != '') {
      this.filtroQuery +=
        (this.filtroQuery != '' ? '&' : '?') +
        `Autor=${this.filtros.Formulario.get('Autor').value.trim()}`;
      temFiltro = true;
    }

    if (this.filtros.Formulario.get('Genero').value.trim() != '') {
      this.filtroQuery +=
        (this.filtroQuery != '' ? '&' : '?') +
        `Genero=${this.filtros.Formulario.get('Genero').value.trim()}`;
      temFiltro = true;
    }
    
    if (this.filtros.Formulario.get('PublicacaoAno').value.trim() != '') {
      this.filtroQuery +=
        (this.filtroQuery != '' ? '&' : '?') +
        `PublicacaoAno=${this.filtros.Formulario.get('PublicacaoAno').value.trim()}`;
      temFiltro = true;
    }

    if (this.filtros.Formulario.get('Editora').value.trim() != '') {
      this.filtroQuery +=
        (this.filtroQuery != '' ? '&' : '?') +
        `Editora=${this.filtros.Formulario.get('Editora').value.trim()}`;
      temFiltro = true;
    }
    this.filtroQuery +=
      (this.filtroQuery != '' ? '&' : '?') + `pageSize=50&currentPage=1`;
    return temFiltro;
  }

  onListar() {
    this.valores.Valores = new FormArray([]);
    if (this.filtrosModel()) {
      this._store.dispatch(new LoadLivros(this.filtroQuery));
      return true;
    }
    this._toastr.info('Nenhum filtro selecionado.', 'Atenção!');
    return false;
  }

  onExcluir(id: number) {
    console.log('onExcluir', id);
    this._store.dispatch(new DeleteLivro(id));
  }
}