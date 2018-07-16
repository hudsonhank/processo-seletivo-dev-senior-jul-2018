import { NgModule } from '@angular/core';
//3-PARTY
import { TextMaskModule } from 'angular2-text-mask';

//MODULOS/COMPONENTES
import { FormDinamicoComponent } from './components/form-dinamico/form-dinamico.component';
import { FormInputComponent } from './components/form-input/form-input.component';
import { FormModalComponent } from './components/form-modal/form-modal.component';
import { FormArrayComponent } from './components/form-array/form-array.component';
import { FormControlService } from './services/form-control.service';
import { FormGridComponent } from './components/form-grid/form-grid.component';
import { SharedModule } from '@myorg/shared';

//Export
/*export { FormDinamicoComponent } from './components/form-dinamico/form-dinamico.component';
export { FormInputComponent } from './components/form-input/form-input.component';
export { FormModalComponent } from './components/form-modal/form-modal.component';
export { FormArrayComponent } from './components/form-array/form-array.component';

export { FormControlService } from './services/form-control.service';
export * from './model/interfaces';*/

export * from './model/interfaces';
export { FormGridComponent } from './components/form-grid/form-grid.component';
export { FormControlService } from './services/form-control.service';

@NgModule({
  imports: [SharedModule, TextMaskModule],
  entryComponents: [FormModalComponent],
  declarations: [
    FormInputComponent,
    FormArrayComponent,
    FormDinamicoComponent,
    FormModalComponent,
    FormGridComponent
  ],

  providers: [FormControlService],

  exports: [
    FormInputComponent,
    FormDinamicoComponent,
    FormModalComponent,
    FormGridComponent
  ]
  //MensagensService
})
export class FormDinamicoModule {}
