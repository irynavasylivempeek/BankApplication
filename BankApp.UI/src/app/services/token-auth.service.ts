import { Injectable } from '@angular/core';

import { DataSharingService } from './data-sharing.service';

const TOKEN = 'TOKEN';

@Injectable({
  providedIn: 'root'
})

export class LocalStorageService {
  constructor(private dataSharingService: DataSharingService) { }

  setToken(token: string) {
    localStorage.setItem(TOKEN, token);
    this.dataSharingService.isLogged.next(true);
  }

  getToken() {
    return localStorage.getItem(TOKEN);
  }

  removeToken() {
    localStorage.removeItem(TOKEN);
    this.dataSharingService.isLogged.next(false);
  }

  hasToken() {
    return localStorage.getItem(TOKEN) != null;
  }
}
