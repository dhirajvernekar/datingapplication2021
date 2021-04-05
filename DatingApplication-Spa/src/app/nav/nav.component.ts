import { Router, Routes } from '@angular/router';
import { AlertifyService } from './../_services/alertify.service';
import { AuthService } from './../_services/Auth.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss']
})
export class NavComponent implements OnInit {
  model: any = {};
  constructor(public authService: AuthService, private alertify: AlertifyService, private router: Router) { }

  ngOnInit() {
  }
  login() {
  // tslint:disable-next-line: deprecation
  this.authService.login(this.model).subscribe(next => {
    // console.log('logged in successfully');
    this.alertify.success('logged in successfully');
  }, error => {
    // console.log('filed');
    this.alertify.error('logged error');
  }, () => {
   this.router.navigate(['/members']);
  });
}
  loggedIn() {
  // const token = localStorage.getItem('token');
  // return !!token;
  return this.authService.loggedIn();
  }
  logOut() {
  localStorage.removeItem('token');
  this.router.navigate(['/home']);
  }
}
