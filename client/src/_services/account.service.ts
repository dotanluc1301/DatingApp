import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { User } from 'src/_model/user';
import { URL } from '../_constant/URL.const';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private currentUser = new ReplaySubject<User>(1);
  currentUser$ = this.currentUser.asObservable();


  constructor(private http: HttpClient) {  }

  login(model:any){
    return this.http.post(URL.baseUrl+URL.login,model).pipe(
      map((response:User) =>{
        const user = response;
        if(user){
          localStorage.setItem("user", JSON.stringify(user));
          this.currentUser.next(user);
        }
      })
    )
  }

  register(model:any){
    return this.http.post(URL.baseUrl + URL.register, model).pipe(
      map((response:any)=>{
        if(response){
          localStorage.setItem('user',JSON.stringify(response));
          this.currentUser.next(response);
        }
        return response;
      }
    )
  )};

  setCurrentUser(user:User){
    this.currentUser.next(user);
  }

  logout(){
    localStorage.removeItem("user");
    this.currentUser.next(null);
  }
}
