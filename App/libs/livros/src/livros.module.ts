import { CommonModule } from '@angular/common';
//ANGULAR
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
//3-PARTY
import { TextMaskModule } from 'angular2-text-mask';
//MODULOS/COMPONENTES
import { LivroListarComponent } from './components/livros-listar/livros-listar.component';
import { LivroFormComponent } from './components/livros-form/livros-form.component';
import { SharedModule } from '@myorg/shared';
import { FormDinamicoModule } from '@myorg/form-dinamico';
import { LivrosStateModule, LivroFormResolver } from '@myorg/livros-state';

import { FileSelectDirective, FileDropDirective } from 'ng2-file-upload/ng2-file-upload';
import { FileUploadModule } from "ng2-file-upload";


/*@NgModule({
  imports: [
    CommonModule,

    RouterModule.forChild([
      /* {path: '', pathMatch: 'full', component: InsertYourComponentHere} 
    ])
  ]
})*/

@NgModule({
  imports: [
    CommonModule,
    TextMaskModule,
    SharedModule,
    FormDinamicoModule,
    LivrosStateModule,
    RouterModule.forChild([
      {
        path: '',
        pathMatch: 'full',
        component: LivroListarComponent,
        resolve: { LivroForm: LivroFormResolver }
      },
      {
        path: 'livro-listar',
        pathMatch: 'full',
        component: LivroListarComponent,
        resolve: { LivroForm: LivroFormResolver }
      },
      {
        path: 'formulario',
        component: LivroFormComponent,
        resolve: { LivroForm: LivroFormResolver }
      },
      {
        path: 'formulario/edi',
        component: LivroFormComponent,
        resolve: { LivroForm: LivroFormResolver }
      }
    ])
  ],
  declarations: [LivroListarComponent,FileSelectDirective, LivroFormComponent],
  providers: []
})

export class LivrosModule {}

