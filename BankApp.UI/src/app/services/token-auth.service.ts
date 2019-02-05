import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs/index';

const TOKEN = 'TOKEN';

@Injectable({
  providedIn: 'root'
})

export class TokenAuthService {
  public isLogged: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);

  constructor() {
    this.isLogged.next(this.hasToken());
  }

  setToken(token: string) {
    localStorage.setItem(TOKEN, token);
    this.isLogged.next(true);
  }

  getToken() {
    return localStorage.getItem(TOKEN);
  }

  removeToken() {
    localStorage.removeItem(TOKEN);
    this.isLogged.next(false);
  }

  hasToken() {
    return localStorage.getItem(TOKEN) != null;
  }
}
