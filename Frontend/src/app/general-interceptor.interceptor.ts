import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse,
} from '@angular/common/http';
import { Observable, catchError, switchMap, throwError } from 'rxjs';
import { errorResponses } from './Shared/constants';
import { AuthService } from './Service/auth.service';
import { TokenService } from './Service/token.service';
import { Router } from '@angular/router';

@Injectable()
export class GeneralInterceptor implements HttpInterceptor {
  constructor(
    private _authService: AuthService,
    private _tokenService: TokenService,
    private _route: Router
  ) {}

  intercept(
    request: HttpRequest<unknown>,
    next: HttpHandler
  ): Observable<HttpEvent<unknown>> {
    const accessToken = this._tokenService.getAccessToken();
    if (accessToken) {
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${accessToken}`,
        },
      });
    }
    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        let errorMessage = '';
        if (error.error instanceof ErrorEvent) {
          errorMessage = `${errorResponses.CLIEENTSIDE_ERROR}: ${error.error.message}`;
        } else {
          switch (error.status) {
            case 400:
              errorMessage = `${errorResponses.BAD_REQUEST}: ${error.message}`;
              break;
            case 401:
              if (!this._tokenService.isAccessTokenExpired()) {
                this._route.navigate(['access-denied']);
                return throwError(errorResponses.UNAUTHORIZED);
              } else {
                return this.handle401Error(request, next);
              }
            case 404:
              errorMessage = `${errorResponses.NOT_FOUND}: ${error.message}`;
              break;
            case 409:
              errorMessage = `${errorResponses.CONFLICT_ERROR}: ${error.message}`;
              break;
            case 500:
              errorMessage = `${errorResponses.INTERNAL_SERVER_ERROR}: ${error.message}`;
              break;
            default:
              errorMessage = `${errorResponses.UNKNOWN_ERROR}: ${error.message}`;
              break;
          }
        }
        alert(errorMessage);
        return throwError(() => new Error(errorMessage));
      })
    );
  }
  private handle401Error(request: HttpRequest<any>, next: HttpHandler) {
    return this._authService.validateToken().pipe(
      switchMap((res: { accessToken: string }) => {
        this._tokenService.setToken(res.accessToken);
        request = request.clone({
          setHeaders: { Authorization: `Bearer ${res.accessToken}` },
        });
        return next.handle(request);
      }),
      catchError((err) => {
        this._tokenService.clearToken();
        return throwError(() => new Error(err));
      })
    );
  }
}
