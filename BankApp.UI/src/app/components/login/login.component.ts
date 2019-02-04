import { Component, OnInit } from '@angular/core';
import {Login} from '../../models/login.model';
import {UserService} from '../../services/user.service';
import {Router} from '@angular/router';
import {DataSharingService} from '../../services/data-sharing.service'
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  error: string;
  user: Login;
  constructor(private userService: UserService, private router: Router, private dataSharingService: DataSharingService) { }

  login() {
    this.userService.login(this.user.userName, this.user.password).subscribe(
      r => {
        if (r.success) {
          this.userService.setToken(r.token);
          this.router.navigateByUrl('/dashboard');
          this.dataSharingService.isLogged.next(true);
        } else {
          this.error = r.errorMessage;
        }
      },
      r => {
        alert(r.error);
      });
  }
  ngOnInit() {
    this.user = {
      userName: '',
      password: ''
    };
  }
}
