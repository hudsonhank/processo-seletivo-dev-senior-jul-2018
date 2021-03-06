import {
  Component,
  EventEmitter,
  OnInit,
  Output,
  ChangeDetectorRef
} from '@angular/core';
import { MediaMatcher } from '@angular/cdk/layout';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navbar',
  templateUrl: 'navbar.component.html',
  styleUrls: ['navbar.component.css']
  //template: `<mat-toolbar>My App</mat-toolbar>`
})
export class NavbarComponent implements OnInit {
  @Output() avisaLogado = new EventEmitter(false);
  public logado: boolean;
  public nomeLoginUsuarioLogado: string;
  mobileQuery: MediaQueryList;

  fillerNav = Array(15)
    .fill(0)
    .map((_, i) => `Nav Item ${i + 1}`);

  fillerContent = Array(15)
    .fill(0)
    .map(
      () =>
        `Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut
       labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco
       laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in
       voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat
       cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.`
    );

  private _mobileQueryListener: () => void;

  constructor(
    changeDetectorRef: ChangeDetectorRef,
    media: MediaMatcher,
    private _router: Router
  ) {
    this.mobileQuery = media.matchMedia('(max-width: 600px)');
    this._mobileQueryListener = () => changeDetectorRef.detectChanges();
    this.mobileQuery.addListener(this._mobileQueryListener);
  }

  ngOnDestroy(): void {
    this.mobileQuery.removeListener(this._mobileQueryListener);
  }

  ngOnInit() {
    this.nomeLoginUsuarioLogado = 'Hudson';
  }

  onVoltar() {
    window.location.href = encodeURI('/');
  }

  onSignOff() {
    window.location.href = encodeURI('/');
  }

  shouldRun = [/(^|\.)plnkr\.co$/, /(^|\.)stackblitz\.io$/].some(h =>
    h.test(window.location.host)
  );
}
