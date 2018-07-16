import { Component, EventEmitter, OnInit, Output } from '@angular/core';

import { Router } from '@angular/router';

@Component({
  selector: 'app-top-menu',
  //templateUrl: './top-menu.component.html'
  template: `  
	<header class="main-header">
		<a href="index2.html" class="logo">
			<span class="logo-mini"><b>SGL</b></span>  
			<span class="logo-lg">Livraria<b> On line</b></span>
		</a>

		<nav class="navbar navbar-static-top">
			<a href="#" class="sidebar-toggle" data-toggle="push-menu" role="button">
				<span class="sr-only">Toggle navigation</span>
			</a>
			<div class="navbar-custom-menu">
			</div>
  		</nav>
	</header>`
})
export class TopMenuComponent implements OnInit {
  @Output() avisaLogado = new EventEmitter(false);
  public logado: boolean;
  public nomeLoginUsuarioLogado: string;

  constructor(private _router: Router) {}

  ngOnInit() {
    this.nomeLoginUsuarioLogado = 'Hudson';
  }

  onVoltar() {
    window.location.href = encodeURI('/');
  }

  onSignOff() {
    window.location.href = encodeURI('/');
  }
}
