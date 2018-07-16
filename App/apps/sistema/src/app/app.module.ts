import { NgModule } from '@angular/core';
import { NxModule } from '@nrwl/nx';
import { AppComponent } from './app.component';
import { BrowserModule } from '@angular/platform-browser';
import { Routes, RouterModule, PreloadAllModules } from '@angular/router';
import { registerLocaleData } from '@angular/common';
import localePt from '@angular/common/locales/pt';
registerLocaleData(localePt);

import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { CoreModule } from '@myorg/core';
import { SharedModule } from '@myorg/shared';

const app_routes: Routes = [
  {
    path: 'principal',
    component: AppComponent
  },
  {
    path: 'modulo-livro',
    loadChildren: '@myorg/livros#LivrosModule'
  },
  { path: '**', redirectTo: 'principal', pathMatch: 'full' }
];

@NgModule({
  imports: [
    BrowserModule,
    CoreModule, //Singleton objects (services, components that are loaded only once, etc.)
    SharedModule, //Shared (multi-instance) objects
    //PessoasModule,
    NxModule.forRoot(),
    StoreModule.forRoot({}),
    EffectsModule.forRoot([]),
    RouterModule.forRoot(app_routes, {
      preloadingStrategy: PreloadAllModules,
      initialNavigation: true
    }),
    StoreDevtoolsModule.instrument()
  ],
  providers: [],
  declarations: [AppComponent],
  bootstrap: [AppComponent]
})
export class AppModule {}
