import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-main-content',
  //templateUrl: './main-content.component.html'
  template: `  
   <div class="content-wrapper">                      
      <div class="content">            
         <router-outlet></router-outlet>
      </div>                        
   </div>
  `
})
export class MainContentComponent implements OnInit {
  constructor() {}

  ngOnInit() {}
}
