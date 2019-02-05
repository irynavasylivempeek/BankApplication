import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { UserService } from '../../services/user.service';
import { TokenAuthService } from '../../services/token-auth.service';

import Login from '../../models/login.model';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  error: string;
  user: Login;

  constructor(private userService: UserService, private router: Router, private tokenAuthService: TokenAuthService) { }

  login() {
    this.userService.login(this.user.userName, this.user.password).subscribe(
      r => {
        if (r.success) {
          this.tokenAuthService.setToken(r.token);
          this.router.navigateByUrl('/dashboard');
        } else {
          this.error = r.errorMessage;
        }
      },
      r => {
        console.error(r.error);
      });
  }

  ngOnInit() {
    this.user = {
      userName: '',
      password: ''
    };
  }
}
