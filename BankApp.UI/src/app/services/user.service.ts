import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {User} from '../models/user.model';


const TOKEN = 'TOKEN';
@Injectable({
  providedIn: 'root'
})
export class UserService {
  constructor(private http: HttpClient) {
  }
  login(userName: string, password: string): Observable<any> {
    return this.http.post<any>('/api/user/login', {
      userName,
      password
    });
  }

  register(userName: string, password: string): Observable<any> {
    console.log('lol');
    return this.http.post<any>('/api/user/register', {
      userName,
      password
    });
  }

  setToken(token: string): void {
    localStorage.setItem(TOKEN, token);
  }
  isLogged() {
    return localStorage.getItem(TOKEN) != null;
  }
  getUserInfo(): Observable<User> {
    return this.http.get<User>('/api/user/userInfo');
  }
  getAllUsers(): Observable<User[]> {
    return this.http.get<User[]>('/api/user/getAll');
  }
  logout() {
    localStorage.removeItem(TOKEN);
  }
}

