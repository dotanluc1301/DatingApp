import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AccountService } from 'src/_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter();
  model:any ={}

  constructor(private accountService: AccountService) { }

  ngOnInit(): void {
  }

  register(){
    this.accountService.register(this.model).subscribe(
      response => {
        console.log("register successfully: " + JSON.stringify(response));
        this.cancel();
      },error => console.log("register failed: "+ JSON.stringify(error))
    );

  }

  cancel(){
    this.cancelRegister.emit(false);
  }

}
