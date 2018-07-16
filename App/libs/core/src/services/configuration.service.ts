import { Injectable } from '@angular/core';

@Injectable()
export class ConfigurationService {
  //ng build --prod --aot  --output-path=C:\IIS\SDKAPP --base-href http://localhost/SDKAPP/
  public Portal = 'http://localhost:4200/';
  public UrlApi = 'http://localhost:50000/';  
  //API
  public ListaValoresApi = this.UrlApi + 'ListaValores/';
  public ArquivoApi = this.UrlApi + 'Arquivo/';
}
