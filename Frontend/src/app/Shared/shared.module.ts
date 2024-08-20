import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CommonGridComponent } from './common-grid/common-grid.component';
import { CommonDialogComponent } from './common-dialog/common-dialog.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { PhoneMaskingDirective } from '../Directives/phone-masking.directive';
import { PhonePipe } from '../Pipes/phone.pipe';
import { CommonSelectmenuComponent } from './common-selectmenu/common-selectmenu.component';
import { RemoveNondigitsDirective } from '../Directives/remove-nondigits.directive';

@NgModule({
  declarations: [
    CommonGridComponent,
    CommonDialogComponent,
    PhoneMaskingDirective,
    PhonePipe,
    CommonSelectmenuComponent,
    RemoveNondigitsDirective,
  ],
  imports: [CommonModule, ReactiveFormsModule, FormsModule],
  exports: [
    CommonGridComponent,
    CommonDialogComponent,
    PhoneMaskingDirective,
    PhonePipe,
    CommonSelectmenuComponent,
    RemoveNondigitsDirective,
  ],
})
export class SharedModule {}
