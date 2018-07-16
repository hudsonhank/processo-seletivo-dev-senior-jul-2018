import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-footer',
  template: `  
   <footer class="main-footer" style="min-height: 51px">    
      <label class="col-md-6">Form Dinâmico - Copyright Hudson Ricardo</label>  
      <label class="col-md-3">1.0.0.0</label>  
      <label class="col-md-3">Último acesso: {{ultima}}</label>    
   </footer>
 `
})
export class FooterComponent implements OnInit {
  ultima: string;

  constructor() {}

  ngOnInit() {
    this.ultima = new Date().toLocaleDateString();
  }
}
