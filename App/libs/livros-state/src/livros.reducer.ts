import { EntityState, EntityAdapter, createEntityAdapter } from '@ngrx/entity';
import { Livro } from './livros.model';
import { LivroActions, LivroActionTypes } from './livros.actions';
import { createFeatureSelector, createSelector } from '@ngrx/store';
import { Observable } from 'rxjs/Observable';
import { Research, AppError } from '@myorg/core';

export interface LivroState extends EntityState<Livro> {
  selectedLivroId: number | null;
  total: number | null;
}

export const livroAdapter: EntityAdapter<Livro> = createEntityAdapter<Livro>(
  { selectId: (livro: Livro) => livro.Id }
);

export const livroInitialState: LivroState = livroAdapter.getInitialState({
  ids: [],
  entities: {},
  total: 0,
  selectedLivroId: null
});

export const selectLivroState = createFeatureSelector<LivroState>('livros');

export const {
  selectIds,
  selectEntities,
  selectAll: selectAllLivros,
  selectTotal
} = livroAdapter.getSelectors(selectLivroState);

export const getSelectedLivro = createSelector(selectLivroState, state => {
  return state.entities[state.selectedLivroId];
});

export const getLivros = createSelector(selectLivroState, state => {
  var result = new Research();
  result.Total = 0;
  result.Resultado = new Array<Livro>();
  if (state != null) {
    result.Total = state.total;

    if (state.entities && result.Total) {
      Object.keys(state.entities).forEach(key => {
        result.Resultado.push(state.entities[key]);
      });
    }
  }

  return result;
});

export function livrosStateReducer(
  state: LivroState = livroInitialState,
  action: LivroActions
): LivroState | Observable<AppError> | Research {
  switch (action.type) {
    case LivroActionTypes.LoadLivrosFail: {
      return livroAdapter.removeAll(state);
    }

    case LivroActionTypes.LoadLivrosSuccess: {
      state.total = action.payload.Total;
      var retorno = livroAdapter.addAll(
        action.payload && action.payload.Total ? action.payload.Resultado : [],
        state
      );
      return retorno;
    }

    case LivroActionTypes.LoadLivroSuccess: {
      return livroAdapter.addOne(action.payload, state);
    }

    case LivroActionTypes.AddLivros: {
      return livroAdapter.addMany(action.payload, state);
    }

    case LivroActionTypes.UpdateLivroSuccess: {
      return livroAdapter.removeAll(state);
    }

    case LivroActionTypes.UpdateLivros: {
      return livroAdapter.updateMany(action.payload, state);
    }

    case LivroActionTypes.DeleteLivro: {
      return livroAdapter.removeOne(action.payload, state);
    }

    case LivroActionTypes.DeleteLivros: {
      return livroAdapter.removeMany(action.payload, state);
    }

    case LivroActionTypes.ClearLivros: {
      return livroAdapter.removeAll(state);
    }

    default: {
      return state;
    }
  }
}
