import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FundersComponent } from './funders.component';
import { Routes, RouterModule } from '@angular/router';
import { AddEditFunderComponent } from './add-edit-funder/add-edit-funder.component';
import { SettingsModule } from '../settings/settings.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
const routes: Routes = [
  {
    path: '',
    component: FundersComponent,
    children: [{ path: 'add-funder', component: AddEditFunderComponent }],
  },
];

@NgModule({
  declarations: [FundersComponent, AddEditFunderComponent],
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    SettingsModule,
    ReactiveFormsModule,
    FormsModule,
  ],
})
export class FundersModule {}
