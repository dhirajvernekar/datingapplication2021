import { AuthService } from './_services/Auth.service';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'DatingApplication-Spa';
  jwtHelper = new JwtHelperService();
  /**
   *
   */
  constructor(private authService: AuthService) {}

  ngOnInit() {
    const token = localStorage.getItem('token');
    if (token) {
      this.authService.decodedToken = this.jwtHelper.decodeToken(token);
    }

  }
}

