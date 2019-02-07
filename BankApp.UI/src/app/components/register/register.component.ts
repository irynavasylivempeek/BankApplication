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
  existingUserNames: string[];
  confirmPassword: string;

  constructor(private userService: UserService, private router: Router, private tokenAuthService: TokenAuthService) {
    this.user = new Login();
    this.confirmPassword = '';
    userService.getRegisteredUserNames().subscribe(result => {
      this.existingUserNames = result;
    });
  }

  register() {
    this.userService.register(this.user.userName, this.user.password).subscribe(
      result => {
        if (result.success) {
          this.tokenAuthService.setToken(result.token);
          this.router.navigateByUrl('/dashboard');
        } else {
          this.error = result.errorMessage;
        }
      },
      result => {
        console.error(result.error);
      });
  }

  ngOnInit() { }
}
