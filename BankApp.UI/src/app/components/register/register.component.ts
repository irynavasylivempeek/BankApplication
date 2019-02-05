import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { UserService } from '../../services/user.service';
import { TokenAuthService } from '../../services/token-auth.service';

import Login from '../../models/login.model';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})

export class RegisterComponent implements OnInit {

  error: string;
  user: Login;

  constructor(private userService: UserService, private router: Router, private tokenAuthService: TokenAuthService) { }

  register() {
    this.userService.register(this.user.userName, this.user.password).subscribe(
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
