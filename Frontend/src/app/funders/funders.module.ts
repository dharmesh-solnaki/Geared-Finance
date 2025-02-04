import { NgModule } from '@angular/core';
import { CommonModule, DecimalPipe } from '@angular/common';
import { FundersComponent } from './funders.component';
import { Routes, RouterModule } from '@angular/router';
import { AddEditFunderComponent } from './add-edit-funder/add-edit-funder.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '../Shared/shared.module';
import { NgxGpAutocompleteModule } from '@angular-magic/ngx-gp-autocomplete';
import { CKEditorModule } from 'ng2-ckeditor';
import { FunderProductGuideComponent } from './funder-product-guide/funder-product-guide.component';
import { PdfViewerModule } from 'ng2-pdf-viewer';
import { FunderChartComponent } from './funder-chart/funder-chart.component';

const routes: Routes = [
  { path: '', component: FundersComponent },
  { path: 'add-funder', component: AddEditFunderComponent },
  { path: ':id/Edit', component: AddEditFunderComponent },
];

@NgModule({
  declarations: [
    FundersComponent,
    AddEditFunderComponent,
    FunderProductGuideComponent,
    FunderChartComponent,
  ],
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    ReactiveFormsModule,
    FormsModule,
    SharedModule,
    NgxGpAutocompleteModule,
    CKEditorModule,
    PdfViewerModule,
  ],
  providers: [DecimalPipe],
})
export class FundersModule {}
