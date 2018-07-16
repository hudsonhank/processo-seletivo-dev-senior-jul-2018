import { NgModule } from '@angular/core';
import { ToastCoreService } from '@myorg/core';
import { livrosStateReducer, livroInitialState } from './livros.reducer';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { LivroEffects } from './livros.effects';
import { LivroService } from './services/livros.service';
import { LivroFormResolver } from './services/livros-form.resolver';
import { ArquivoService } from './services/arquivo.service';




export { ArquivoService } from './services/arquivo.service';
export { LivroService } from './services/livros.service';
export { LivroFormResolver } from './services/livros-form.resolver';
export { Livro } from './livros.model';
export { LivroState, getLivros } from './livros.reducer';
export {
  DeleteLivro,
  LoadLivros,
  UpdateLivro,
  AddLivro
} from './livros.actions';

@NgModule({
  imports: [    
    StoreModule.forFeature('livros', livrosStateReducer, {
      initialState: livroInitialState
    }),
    EffectsModule.forFeature([LivroEffects])
  ],

  providers: [
    ToastCoreService,
    LivroEffects,
    LivroService,
    ArquivoService,
    LivroFormResolver
  ]
})
export class LivrosStateModule {}
