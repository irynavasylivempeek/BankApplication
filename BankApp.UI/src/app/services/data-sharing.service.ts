import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
const TOKEN = 'TOKEN';
@Injectable({
  providedIn: 'root'
})
export class DataSharingService {
  public isLogged: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  constructor() {
    this.isLogged.next(localStorage.getItem(TOKEN) != null);
  }
}
