import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, catchError, of } from 'rxjs';
import { errorResponses } from './Models/constants.model';

@Injectable()
export class GeneralInterceptorInterceptor implements HttpInterceptor {

  constructor() {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(

      catchError((error: HttpErrorResponse) => {
        let errorMessage = '';
     console.log("hi from itercepto")
        if (error.error instanceof ErrorEvent) {
          // Client-side errors
          errorMessage = `${errorResponses.CLIEENTSIDE_ERROR}: ${error.error.message}`;
        } else {
          // Server-side errors
          switch (error.status) {
            case 400:
              errorMessage = `${errorResponses.BAD_REQUEST}: ${error.message}`;
              break;
            case 401:
              errorMessage = `${errorResponses.UNAUTHORIZED}: ${error.message}`;
              break;
            case 403:
              errorMessage = `${errorResponses.FORBIDDEN}: ${error.message}`;
              break;
            case 404:
              errorMessage = `${errorResponses.NOT_FOUND}: ${error.message}`;
              break;
            case 500:
              errorMessage = `${errorResponses.INTERNAL_SERVER_ERROR}: ${error.message}`;
              break;
            default:
              errorMessage = `${errorResponses.UNKNOWN_ERROR}: ${error.message}`;
              break;
          }
        }
        alert(errorMessage)
        return of()
      })

    );
  }
}
