import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
//
import { FooterComponent } from './components/footer.component';
import { LeftMenuComponent } from './components/left-menu.component';
import { MainContentComponent } from './components/main-content.component';
import { TopMenuComponent } from './components/top-menu.component';
import { NavbarComponent } from './components/navbar.component';

//import { FileSelectDirective, FileDropDirective } from 'ng2-file-upload/ng2-file-upload';
//import { FileUploadModule } from "ng2-file-upload";

//import { WorkSpaceComponent } from './components/workspace.component';

import {
  MatAutocompleteModule,
  MatButtonModule,
  MatButtonToggleModule,
  MatCardModule,
  MatCheckboxModule,
  MatChipsModule,
  MatDatepickerModule,
  MatDialogModule,
  MatDividerModule,
  MatExpansionModule,
  MatGridListModule,
  MatIconModule,
  MatInputModule,
  MatListModule,
  MatMenuModule,
  MatNativeDateModule,
  MatPaginatorModule,
  MatProgressBarModule,
  MatProgressSpinnerModule,
  MatRadioModule,
  MatRippleModule,
  MatSelectModule,
  MatSidenavModule,
  MatSliderModule,
  MatSlideToggleModule,
  MatSnackBarModule,
  MatSortModule,
  MatStepperModule,
  MatTableModule,
  MatTabsModule,
  MatToolbarModule,
  MatTooltipModule
} from '@angular/material';

//Components material
const material = [
  MatAutocompleteModule,
  MatButtonModule,
  MatButtonToggleModule,
  MatCardModule,
  MatCheckboxModule,
  MatChipsModule,
  MatDatepickerModule,
  MatDialogModule,
  MatDividerModule,
  MatExpansionModule,
  MatGridListModule,
  MatIconModule,
  MatInputModule,
  MatListModule,
  MatMenuModule,
  MatNativeDateModule,
  MatPaginatorModule,
  MatProgressBarModule,
  MatProgressSpinnerModule,
  MatRadioModule,
  MatRippleModule,
  MatSelectModule,
  MatSidenavModule,
  MatSliderModule,
  MatSlideToggleModule,
  MatSnackBarModule,
  MatSortModule,
  MatStepperModule,
  MatTableModule,
  MatTabsModule,
  MatToolbarModule,
  MatTooltipModule
];

@NgModule({
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    RouterModule,
    //FileUploadModule,
    material
  ],
  exports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    MainContentComponent,
    FooterComponent,
    TopMenuComponent,
    NavbarComponent,
    LeftMenuComponent,
    //FileUploadModule,
    material
  ],
  declarations: [
    MainContentComponent,
    //FileSelectDirective,
    FooterComponent,
    TopMenuComponent,
    NavbarComponent,
    LeftMenuComponent
  ]
})
export class SharedModule {}
