import { Component, OnChanges } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { FileUploader } from 'ng2-file-upload';
import { ToastrService } from 'ngx-toastr';
import { Store } from '@ngrx/store';
import { WorkSpaceComponent } from '@myorg/shared';
import {
  Livro,
  LivroService,
  LivroState,
  UpdateLivro,
  AddLivro,
  ArquivoService
} from '@myorg/livros-state';
import { FormularioGroup, FormControlService } from '@myorg/form-dinamico';
import { Arquivo} from '@myorg/core';

@Component({
  selector: 'app-livro-form',
  templateUrl: './livros-form.component.html'
})
export class LivroFormComponent extends WorkSpaceComponent<Livro>
  implements OnChanges {
  livroId : any;
  principal: FormularioGroup;
  titulo: string = 'Formulário Livro';

  private CapaNome= '';

  constructor(
    public _fb: FormBuilder,
    public _service: LivroService,
    public arquivoService: ArquivoService,
    public _formService: FormControlService,
    public _route: ActivatedRoute,
    public _router: Router,
    public _toastr: ToastrService,
    private _store: Store<LivroState>
  ) {
    super(_router, _toastr, null);

    this.subscription = this._route.data.subscribe(
      (resolver: { LivroForm: FormularioGroup }) => {
        this.principal = resolver.LivroForm;
      },
      err => this.handleError(err)
    );
  }
  ngOnInit() {
    
    this.arquivoService.onLimparQueue();

    this.livroId =
      this.principal.Valor != null && this.principal.Valor['Id'] != null
        ? this.principal.Valor['Id']
        : 0;
    let name = 'Cadastro';
    if (this.livroId > 0) {
      name = 'Edição';
    }
    this.principal = this._formService.buildFormulario(
      'principal',
      name,
      '',
      1,
      this.principal.Controles,
      this.principal.Valor
    );
  }

	get NomeDoArquivo(): string 
	{
      if(this.arquivoService.isEnviado && this.arquivoService.successResponse)
      {
         let arquivo = JSON.parse(this.arquivoService.successResponse); //success server response
         return arquivo.Data.Nome;
      }
      return ' Nenhum arquivo selecionado';      
   };	

  onSubmit() {
    
    if(!this.arquivoService.isEnviado)
    {
      this._toastr.warning(" Nenhum arquivo para capa do livro selecionado");     
    }
    else{

      var value = this._formService.validarFormulario(this.principal);

      if (value) {

        if(this.arquivoService.isEnviado && this.arquivoService.successResponse)
        {
          let arquivo = JSON.parse(this.arquivoService.successResponse); //success server response
          value.CapaId = arquivo.Data.capaId;
        }	

        value.Id = this.livroId;
        
        this._store.dispatch(
          this.livroId > 0
            ? new UpdateLivro({ livro: value, partial: value })
            : new AddLivro(value)
        );
      }

    }
    
    
  }
}
