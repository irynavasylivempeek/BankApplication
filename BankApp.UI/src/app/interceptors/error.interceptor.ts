import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Router } from '@angular/router';

import { TokenAuthService } from '../services/token-auth.service';

@Injectable()

export class ErrorInterceptor implements HttpInterceptor {
  constructor(private router: Router, private tokenAuthService: TokenAuthService) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(request).pipe(catchError(err => {
      if (err.status === 401) {
        this.tokenAuthService.removeToken();
        this.router.navigateByUrl('/login');
      } else {
        const error = err.error.message || err.statusText;
        return throwError(error);
      }
    }));
  }
}
