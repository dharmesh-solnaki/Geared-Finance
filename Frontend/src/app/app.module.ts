import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppHeaderModule } from './app-header/app-header.module';
import { RouterModule, Routes } from '@angular/router';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { GeneralInterceptor } from './general-interceptor.interceptor';
import { ToastrModule } from 'ngx-toastr';
import { AppLoginComponent } from './app-login/app-login.component';
import { ReactiveFormsModule } from '@angular/forms';
import { AuthService } from './Service/auth.service';
import { TokenService } from './Service/token.service';
import { AppForgotPasswwordComponent } from './app-forgot-passwword/app-forgot-passwword.component';

const appRoutes: Routes = [
  { path: '', component: AppComponent },
  { path: 'login', component: AppLoginComponent },
  { path: 'forgot-password', component: AppForgotPasswwordComponent },
  {
    path: 'settings',
    loadChildren: () =>
      import('./settings/settings.module').then((m) => m.SettingsModule),
  },
];

@NgModule({
  declarations: [AppComponent, AppLoginComponent, AppForgotPasswwordComponent],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: GeneralInterceptor, multi: true },
    AuthService,
    TokenService,
  ],
  bootstrap: [AppComponent],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppHeaderModule,
    ReactiveFormsModule,
    RouterModule.forRoot(appRoutes),
    NgbPaginationModule,
    HttpClientModule,
    ToastrModule.forRoot({
      timeOut: 3000,
      positionClass: 'toast-top-center',
      progressBar: true,
      preventDuplicates: true,
    }),
  ],
  // exports:[
  //   PhoneMaskingDirective
  // ]
})
export class AppModule {}
