import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AlertifyService } from '../_services/alertify.service';
import { AuthService } from '../_services/Auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  @Input() valuesFromHome: any;
  @Output() cancelRegister = new EventEmitter();
  model: any = {};
  constructor(private authService: AuthService, private alertify: AlertifyService) { }


  ngOnInit() {
  }
  register() {
    this.authService.register(this.model).subscribe(() => {
      // console.log('registration');
      this.alertify.success('success');
     } ,
      error => {
        // console.log('registration error');
        this.alertify.success('failure');
      } );
  }
  cancel() {
  this.cancelRegister.emit(false);

}

}
