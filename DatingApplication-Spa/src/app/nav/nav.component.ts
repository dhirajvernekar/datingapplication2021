import { AuthService } from './../_services/Auth.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss']
})
export class NavComponent implements OnInit {
  model: any = {};
  constructor(private authService: AuthService) { }

  ngOnInit() {
  }
  login() {
  // tslint:disable-next-line: deprecation
  this.authService.login(this.model).subscribe(next => {
    console.log('logged in successfully');
  }, error => {
    console.log('filed');
  }
  );
}
  loggedIn() {
  const token = localStorage.getItem('token');
  return !!token;
  }
  logOut() {
  localStorage.removeItem('token');
  }
}
