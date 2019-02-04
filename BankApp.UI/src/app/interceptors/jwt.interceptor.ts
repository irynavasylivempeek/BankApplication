import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable } from 'rxjs';

import { UserService} from '../services/user.service';
const TOKEN = 'TOKEN';
@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  constructor(private userService: UserService) { }
  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // add authorization header with jwt token if available
    const isLogged = this.userService.isLogged();
    if (isLogged) {
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${localStorage.getItem(TOKEN)}`
        }
      });
    }
    return next.handle(request);
  }
}
