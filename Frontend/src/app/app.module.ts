import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppHeaderModule } from './app-header/app-header.module';
import { RouterModule, Routes } from '@angular/router';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { GeneralInterceptorInterceptor } from './general-interceptor.interceptor';
import { ToastrModule } from 'ngx-toastr';



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
  providers: [ {provide:HTTP_INTERCEPTORS,useClass:GeneralInterceptorInterceptor,multi:true},],
  bootstrap: [AppComponent],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppHeaderModule,
    RouterModule.forRoot(appRoutes),  
    NgbPaginationModule,
    ToastrModule.forRoot(),
  ],
  // exports:[
  //   PhoneMaskingDirective
  // ]
})
export class AppModule {}
