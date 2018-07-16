import { Component, OnInit, ViewChild } from '@angular/core';
import { Menu } from '@myorg/core';
import { LeftMenuComponent } from '@myorg/shared';

@Component({
  selector: 'app-root',
  template: `  
  <div class="wrapper-content">              
      <app-top-menu></app-top-menu>            
      <app-left-menu #appMenu [menus]="menus" ></app-left-menu>          
      <app-main-content></app-main-content>    
      <app-footer></app-footer>
   </div>
  `
  //templateUrl: './app.component.html',
})
export class AppComponent implements OnInit {
  @ViewChild('appMenu') appMenu: LeftMenuComponent;
  menus: Array<Menu> = new Array<Menu>();
  constructor() {}

  ngOnInit() {
    //Pessoa
    let menu = new Menu({
      Id: 1,
      Nome: 'Livros',
      Descricao: 'Módulo de Cadastro',
      Uri: '/modulo-livro',
      Classe: 'fa fa-edit'
    });
    menu.SubMenu.push(
      new Menu({
        Id: 2,
        Nome: 'Formulário',
        Descricao: 'Formulário de Livro',
        Uri: '/modulo-livro/formulario',
        Classe: 'fa fa-files-o'
      })
    );
    menu.SubMenu.push(
      new Menu({
        Id: 3,
        Nome: 'Listar livros',
        Descricao: 'Listagem dos livros',
        Uri: '/modulo-livro/livro-listar',
        Classe: 'fa fa-th'
      })
    );
    this.menus.push(menu);

    var listaValores = JSON.parse(
      sessionStorage.getItem('listaValores')
    ) as Array<any>;

    if (listaValores == null || listaValores == undefined) {
      listaValores = [];
      var simnao = [];
      simnao.push({
        Valor: 'Sim',
        Nome: 'Sim',
        Descricao: 'Sim',
        Checado: false
      });
      simnao.push({
        Valor: 'Não',
        Nome: 'Não',
        Descricao: 'Não',
        Checado: false
      });
      listaValores['SIMNAO'] = simnao;
      sessionStorage.setItem('listaValores', JSON.stringify(listaValores));
    }
  }
}
