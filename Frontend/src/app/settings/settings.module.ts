import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SettingsComponent } from './settings.component';
import { RouterModule, Routes } from '@angular/router';
import { UserManagementComponent } from './user-management/user-management.component';
import { CommonSelectmenuComponent } from '../Shared/common-selectmenu/common-selectmenu.component';
import { ReactiveFormsModule } from '@angular/forms';
import { AddSiteUserComponent } from './user-management/add-site-user/add-site-user.component';
import { PhoneMaskingDirective } from '../Directives/phone-masking.directive';
import { CommonGridComponent } from '../Shared/common-grid/common-grid.component';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { UserService } from '../Service/user.service';
import {  HttpClientModule } from '@angular/common/http';
import { VendorService } from '../Service/vendor.service';
import { PhonePipe } from '../Pipes/phone.pipe';
import { FundingCategoriesComponent } from './funding-categories/funding-categories.component';
import { CommonDialogComponent } from '../Shared/common-dialog/common-dialog.component';
import { EquipmentService } from '../Service/equipment.service';




const routes: Routes = [
  {
    path: '',
    component: SettingsComponent,
    children: [
      { path: 'user-management', component: UserManagementComponent },
      { path: 'user-management/0/add', component: AddSiteUserComponent  },
      { path: 'user-management/:id/Edit', component: AddSiteUserComponent  },
      {path:'equipmentType',component:FundingCategoriesComponent}
    ],
  },
];

@NgModule({
  declarations: [
    SettingsComponent,
    UserManagementComponent,
    CommonSelectmenuComponent,
    AddSiteUserComponent,
    CommonGridComponent,
    PhoneMaskingDirective,
    PhonePipe,
    FundingCategoriesComponent,
    CommonDialogComponent
  ],
  imports: [ReactiveFormsModule, RouterModule.forChild(routes), CommonModule,
    NgbPaginationModule,HttpClientModule
  ],
  providers:[ UserService,VendorService,EquipmentService],
  exports: [SettingsComponent, CommonSelectmenuComponent,
    PhoneMaskingDirective,CommonGridComponent,CommonDialogComponent
  ]
})
export class SettingsModule {}
