import { Component, OnInit, Input } from '@angular/core';
import { FormularioGroup } from '../../model/interfaces';

@Component({
  selector: 'form-dinamico',
  styleUrls: ['./form-dinamico.component.css'],
  templateUrl: './form-dinamico.component.html'
})
export class FormDinamicoComponent implements OnInit {
  @Input() formularioPrincipal: FormularioGroup;
  controles: Array<any> = [];
  constructor() {
    // Create 100 users
    //const users: UserData[] = [];
    //for (let i = 1; i <= 25; i++) { users.push(createNewUser(i)); }
    // Assign the data to the data source for the table to render
    //this.dataSource = new MatTableDataSource(users);
  }

  //HTML   [formularioPrincipal]="formularioPrincipal"

  ngOnInit() {
    if (
      this.formularioPrincipal &&
      this.formularioPrincipal != null &&
      this.formularioPrincipal.Controles != null
    ) {
      Object.keys(this.formularioPrincipal.Controles).forEach(key => {
        if (this.formularioPrincipal.Controles[key].ControleTipo != 'hidden') {
          if (
            this.formularioPrincipal.Controles[key].Tipo == 2 ||
            this.formularioPrincipal.Controles[key].Tipo == 3
          ) {
            this.formularioPrincipal.Controles[key].Class = 'col-md-12';
          }
          this.controles.push(this.formularioPrincipal.Controles[key]);
        }
      });
    }
  }
}
