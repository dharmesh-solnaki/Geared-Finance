import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AppHeaderComponent } from './app-header.component';
import { RouterModule } from '@angular/router';

 


@NgModule({
  declarations: [
    AppHeaderComponent
  ],
  imports: [
    RouterModule,
    CommonModule
  ],
  exports: [
    AppHeaderComponent
  ]
})
export class AppHeaderModule { }
