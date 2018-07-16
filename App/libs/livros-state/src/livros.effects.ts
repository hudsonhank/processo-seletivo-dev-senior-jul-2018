import { Injectable } from '@angular/core';
import { Effect } from '@ngrx/effects';
import { DataPersistence } from '@nrwl/nx';
import { map } from 'rxjs/operators';
import { Router } from '@angular/router';
import { ToastCoreService } from '@myorg/core';

import * as livroActions from './livros.actions';
import { LivroState } from './livros.reducer';

import { Livro } from './livros.model';
import { LivroService } from './services/livros.service';

@Injectable()
export class LivroEffects {
  constructor(
    //private actions: Actions,
    private d: DataPersistence<LivroState>,
    private livroService: LivroService,
    public toastCoreService: ToastCoreService,
    public router: Router
  ) {}
  @Effect()
  onLoadLivros = this.d.fetch(livroActions.LivroActionTypes.LoadLivros, {
    run: (action: livroActions.LoadLivros, state: LivroState) => {
      return this.livroService.onListar(action.payload).pipe(
        map(result => {
          return new livroActions.LoadLivrosSuccess(result);
        })
      );
    },
    onError: (a: livroActions.LoadLivros, result) => {
      this.toastCoreService.handleError(result, 'Listar Livros');
      return new livroActions.LoadLivrosFail(result);
    }
  });

  @Effect()
  onLoadLivro = this.d.fetch(livroActions.LivroActionTypes.LoadLivro, {
    run: (action: livroActions.LoadLivro, state: LivroState) => {
      return this.livroService.onBuscar(action.payload).pipe(
        map(result => {
          return new livroActions.LoadLivroSuccess(result as Livro);
        })
      );
    },
    onError: (a: livroActions.LoadLivro, result) => {
      return this.toastCoreService.handleError(
        result,
        livroActions.LivroActionTypes.LoadLivro
      );
    }
  });

  @Effect()
  onAddLivro = this.d.fetch(livroActions.LivroActionTypes.AddLivro, {
    run: (action: livroActions.AddLivro, state: LivroState) => {
      return this.livroService.onCadastrar(action.payload).pipe(
        map(result => {
          this.toastCoreService.success(
            'Cadastro efetuado com sucesso.',
            'Sucesso!',
            livroActions.LivroActionTypes.AddLivro
          );
          this.router.navigate(['/modulo-livro/livro-listar']);
          return new livroActions.AddLivroSuccess();
        })
      );
    },
    onError: (a: livroActions.AddLivro, result) => {
      return this.toastCoreService.handleError(
        result,
        livroActions.LivroActionTypes.AddLivro
      );
    }
  });

  @Effect()
  onUpdateLivro = this.d.fetch(livroActions.LivroActionTypes.UpdateLivro, {
    run: (action: livroActions.UpdateLivro, state: LivroState) => {
      return this.livroService.onEditar(action.payload.livro).pipe(
        map(result => {
          this.toastCoreService.success(
            'Editação efetuada com sucesso.',
            'Sucesso!',
            livroActions.LivroActionTypes.UpdateLivro
          );

          this.router.navigate(['/modulo-livro/livro-listar']);
          return new livroActions.UpdateLivroSuccess();
        })
      );
    },
    onError: (a: livroActions.UpdateLivro, result) => {
      return this.toastCoreService.handleError(
        result,
        livroActions.LivroActionTypes.UpdateLivro
      );
    }
  });

  @Effect()
  onDeleteLivro = this.d.fetch(livroActions.LivroActionTypes.DeleteLivro, {
    run: (action: livroActions.DeleteLivro, state: LivroState) => {
      console.log('onDeleteLivro', action);

      return this.livroService.onDeletar(action.payload).pipe(
        map(result => {
          this.toastCoreService.success(
            'Exclusão efetuada com sucesso.',
            'Sucesso!',
            livroActions.LivroActionTypes.DeleteLivro
          );
          return new livroActions.DeleteLivroSuccess();
        })
      );
    },
    onError: (a: livroActions.DeleteLivro, result) => {
      return this.toastCoreService.handleError(
        result,
        livroActions.LivroActionTypes.DeleteLivro
      );
    }
  });
}
