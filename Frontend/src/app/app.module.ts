import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule, Routes } from '@angular/router';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { GeneralInterceptor } from './general-interceptor.interceptor';
import { ToastrModule } from 'ngx-toastr';
import { AppLoginComponent } from './app-login/app-login.component';
import { ReactiveFormsModule } from '@angular/forms';
import { AuthService } from './Service/auth.service';
import { TokenService } from './Service/token.service';
import { AppForgotPasswwordComponent } from './app-forgot-password/app-forgot-password.component';
import { AccessDeniedPageComponent } from './Shared/access-denied-page/access-denied-page.component';
import { AppHeaderComponent } from './app-header/app-header.component';
import { SharedTemplateService } from './Models/shared-template.service';

const appRoutes: Routes = [
  { path: '', component: AppComponent },
  { path: 'login', component: AppLoginComponent },
  { path: 'forgot-password', component: AppForgotPasswwordComponent },
  {
    path: 'settings',
    loadChildren: () =>
      import('./settings/settings.module').then((m) => m.SettingsModule),
  },
  {
    path: 'funders',
    loadChildren: () =>
      import('./funders/funders.module').then((m) => m.FundersModule),
  },
  { path: 'access-denied', component: AccessDeniedPageComponent },
];

@NgModule({
  declarations: [
    AppComponent,
    AppLoginComponent,
    AppForgotPasswwordComponent,
    AccessDeniedPageComponent,
    AppHeaderComponent,
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: GeneralInterceptor, multi: true },
    AuthService,
    TokenService,
    SharedTemplateService,
  ],
  bootstrap: [AppComponent],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
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
})
export class AppModule {}
