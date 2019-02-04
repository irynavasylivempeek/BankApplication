import { Component, OnInit } from '@angular/core';
import {UserService} from '../../services/user.service';
import {Router} from '@angular/router';
import {DataSharingService} from '../../services/data-sharing.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {
  isLogged: boolean;
  constructor(private userService: UserService, private router: Router, private dataSharingService: DataSharingService) {
    this.dataSharingService.isLogged.subscribe( value => {
      this.isLogged = value;
    });
  }

  ngOnInit() {

  }
  logout() {
    this.userService.logout();
    this.router.navigateByUrl('/login');
    this.dataSharingService.isLogged.next(false);
  }


}
