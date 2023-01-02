import { AuthService } from '@abp/ng.core';
import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  //for disabled button and abp loading spinner
  isLog: boolean = false;
  get hasLoggedIn(): boolean {
    return this.authService.isAuthenticated;
  }

  constructor(private authService: AuthService, private router: Router) {
    // this.router.events.subscribe(event => {
    //   if (event instanceof NavigationStart) {
    //     console.log(event);
    //   }
    // });
  }

  login() {
    //when clicked button spinner and disabled will be active
    this.isLog = true;
    setTimeout(a => {
      this.authService.navigateToLogin();
      //all functions is over here
      this.isLog = false;
    }, 1000);
  }
}
