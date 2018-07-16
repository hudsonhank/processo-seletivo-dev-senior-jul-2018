import { NgModule, Optional, SkipSelf } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
//
import { ConfigurationService } from './services/configuration.service';
import { ToastrModule } from 'ngx-toastr';
import { ToastCoreService } from './services/toast-core.service';
import { ArquivoService } from '../../livros-state/src/services/arquivo.service';

//Export
export * from './models/sistema';

export { ConfigurationService } from './services/configuration.service';
export { DataBase } from './services/database.service';
export { ToastCoreService } from './services/toast-core.service';

export class EnsureModuleLoadedOnceGuard {
  constructor(targetModule: any) {
    if (targetModule) {
      throw new Error(
        `${
          targetModule.constructor.name
        } has already been loaded. Import this module in the AppModule only.`
      );
    }
  }
}

@NgModule({
  imports: [
    CommonModule,
    BrowserAnimationsModule,
    //FileUploadModule,
    
    ToastrModule.forRoot({
      timeOut: 3000,
      closeButton: true,
      positionClass: 'toast-top-center',
      preventDuplicates: true
    })
  ],
  exports: [RouterModule, HttpClientModule],
  declarations: [],
  providers: [ConfigurationService, ArquivoService, ToastCoreService] // these should be singleton
})
export class CoreModule extends EnsureModuleLoadedOnceGuard {
  //Ensure that CoreModule is only loaded into AppModule
  //Looks for the module in the parent injector to see if it's already been loaded (only want it loaded once)
  constructor(
    @Optional()
    @SkipSelf()
    parentModule: CoreModule
  ) {
    super(parentModule);
  }
}
