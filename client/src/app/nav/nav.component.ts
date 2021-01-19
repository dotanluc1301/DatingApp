import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from 'src/_model/user';
import { AccountService } from 'src/_services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css'],
})
export class NavComponent implements OnInit {
  model: any = {};
  constructor(public accountService: AccountService) {}

  ngOnInit(): void {
    this.getCurrentUser();
  }

  login() {
    this.accountService.login(this.model).subscribe()
  }

  logout(){
    this.accountService.logout();
  }

  getCurrentUser(){
    this.accountService.currentUser$.subscribe(user =>{
      console.log("nav - getCurrentUser response - " + JSON.stringify(user));
    },error =>{
      console.log("nav - getCurrentUser error - " + error);
    })
  }
}
