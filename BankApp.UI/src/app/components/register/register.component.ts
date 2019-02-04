import { Component, OnInit } from '@angular/core';
import {UserService} from '../../services/user.service';
import {Login} from '../../models/login.model';
import {Router} from '@angular/router';
import {DataSharingService} from '../../services/data-sharing.service';
@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  error: string;
  user: Login;
  constructor(private userService: UserService, private router: Router, private dataSharingService: DataSharingService) { }
  register() {
    this.userService.register(this.user.userName, this.user.password).subscribe(
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
