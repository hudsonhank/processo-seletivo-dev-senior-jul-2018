import { Action } from '@ngrx/store';

import { Observable } from 'rxjs/Observable';

import { Update } from '@ngrx/entity';

import { Research } from '@myorg/core';
import { Livro } from './livros.model';

export enum LivroActionTypes {
  LoadLivros = '[Livro] Load Livros',
  LoadLivrosSuccess = '[Livro] Load Livros Success',
  LoadLivrosFail = '[Livro] Load Livros Fail',
  LoadLivro = '[Livro] Load Livro',
  LoadLivroSuccess = '[Livro] Load Livro Success',
  AddLivro = '[Livro] Add Livro',
  AddLivroSuccess = '[Livro] Add Livro Success',
  UpsertLivro = '[Livro] Upsert Livro',
  AddLivros = '[Livro] Add Livros',
  UpsertLivros = '[Livro] Upsert Livros',
  UpdateLivro = '[Livro] Update Livro',
  UpdateLivroSuccess = '[Livro] Update Livro Success',
  UpdateLivros = '[Livro] Update Livros',
  DeleteLivro = '[Livro] Delete Livro',
  DeleteLivroSuccess = '[Livro] Delete Livro Success',
  DeleteLivros = '[Livro] Delete Livros',
  ClearLivros = '[Livro] Clear Livros'
}

export class LoadLivros implements Action {
  readonly type = LivroActionTypes.LoadLivros;
  constructor(public payload: string) {}
}

export class LoadLivrosSuccess implements Action {
  readonly type = LivroActionTypes.LoadLivrosSuccess;
  constructor(public payload: Research) {}
}

export class LoadLivroSuccess implements Action {
  readonly type = LivroActionTypes.LoadLivroSuccess;
  constructor(public payload: Livro) {}
}

export class LoadLivrosFail implements Action {
  readonly type = LivroActionTypes.LoadLivrosFail;
  constructor(public payload: Observable<any>) {}
}

export class LoadLivro implements Action {
  readonly type = LivroActionTypes.LoadLivro;

  constructor(public payload: number) {}
}

export class AddLivro implements Action {
  readonly type = LivroActionTypes.AddLivro;
  constructor(public payload: Livro) {}
}

export class AddLivroSuccess implements Action {
  readonly type = LivroActionTypes.AddLivroSuccess;
}

export class UpdateLivroSuccess implements Action {
  readonly type = LivroActionTypes.UpdateLivroSuccess;
}

export class UpsertLivro implements Action {
  readonly type = LivroActionTypes.UpsertLivro;

  constructor(public payload: Livro) {}
}

export class AddLivros implements Action {
  readonly type = LivroActionTypes.AddLivros;

  constructor(public payload: Livro[]) {}
}

export class UpsertLivros implements Action {
  readonly type = LivroActionTypes.UpsertLivros;

  constructor(public payload: Livro[]) {}
}

export class UpdateLivro implements Action {
  readonly type = LivroActionTypes.UpdateLivro;

  constructor(public payload: { livro: Livro; partial: Update<Livro> }) {}
}

export class UpdateLivros implements Action {
  readonly type = LivroActionTypes.UpdateLivros;

  constructor(public payload: Update<Livro>[]) {}
}

export class DeleteLivro implements Action {
  readonly type = LivroActionTypes.DeleteLivro;
  constructor(public payload: number) {}
}

export class DeleteLivroSuccess implements Action {
  readonly type = LivroActionTypes.DeleteLivroSuccess;
}

export class DeleteLivros implements Action {
  readonly type = LivroActionTypes.DeleteLivros;

  constructor(public payload: number[]) {}
}

export class ClearLivros implements Action {
  readonly type = LivroActionTypes.ClearLivros;
}

export type LivroActions =
  | LoadLivros
  | LoadLivrosSuccess
  | LoadLivro
  | LoadLivroSuccess
  | LoadLivrosFail
  | AddLivro
  | AddLivroSuccess
  | UpsertLivro
  | AddLivros
  | UpsertLivros
  | UpdateLivro
  | UpdateLivroSuccess
  | UpdateLivros
  | DeleteLivro
  | DeleteLivros
  | ClearLivros;
