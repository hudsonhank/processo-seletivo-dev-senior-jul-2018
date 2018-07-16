import { Component, OnInit, Input } from '@angular/core';
import { Router } from '@angular/router';
import { Menu } from '@myorg/core';

@Component({
  selector: 'app-left-menu',
  templateUrl: './left-menu.component.html'
})
export class LeftMenuComponent implements OnInit {
  @Input() menus: Array<Menu>;
  onLogado: any;

  constructor() {}

  ngOnInit() {
    //console.log('Menus:',this.menus);
    //this.ultima = new Date().toLocaleDateString();
  }

  constructorprivate(_router: Router) {
    //var menus = [];
    console.log('Menu APP:', this.onLogado);
  }

  menu() {
    //var menus = [];
    console.log('Menu APP:', this.menus);
  }
}
