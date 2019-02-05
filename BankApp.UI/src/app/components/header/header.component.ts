import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { TokenAuthService } from '../../services/token-auth.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  isLogged: boolean;

  constructor(private router: Router, private tokenAuthService: TokenAuthService) {
    this.tokenAuthService.isLogged.subscribe( value => {
      this.isLogged = value;
    });
  }

  ngOnInit() { }

  logout() {
    this.tokenAuthService.removeToken();
    this.router.navigateByUrl('/login');
  }
}
