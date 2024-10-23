import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LeadsComponent } from './leads/leads.component';
import { RouterModule, Routes } from '@angular/router';
import { SharedModule } from '../Shared/shared.module';
import { ReactiveFormsModule } from '@angular/forms';

const applicationRoutes: Routes = [{ path: '', component: LeadsComponent }];

@NgModule({
  declarations: [LeadsComponent],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterModule.forChild(applicationRoutes),
    SharedModule,
  ],
})
export class ApplicationModule {}
