import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppHeaderModule } from './app-header/app-header.module';
import { RouterModule, Routes } from '@angular/router';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';



const appRoutes: Routes = [
  { path: '', component: AppComponent },
  {
    path: 'settings',
    loadChildren: () =>
      import('./settings/settings.module').then((m) => m.SettingsModule),
  },
];

@NgModule({
  declarations: [
    AppComponent,

    ],
  providers: [],
  bootstrap: [AppComponent],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppHeaderModule,
    RouterModule.forRoot(appRoutes),  
    NgbPaginationModule
    
  ],
  // exports:[
  //   PhoneMaskingDirective
  // ]
})
export class AppModule {}
