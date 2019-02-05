import { Component } from '@angular/core';
import { Router } from '@angular/router';

import { TokenAuthService } from './services/token-auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

export class AppComponent {
  constructor(private tokenAuthService: TokenAuthService, private router: Router) {
  }

  ngOnInit () {
    if (this.tokenAuthService.hasToken()) {
      this.router.navigateByUrl('/dashboard');
    } else {
      this.router.navigateByUrl('/login');
    }
  }

}
