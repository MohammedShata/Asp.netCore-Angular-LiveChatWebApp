import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import {map} from 'rxjs/operators'; 
import {User} from '../_Models/user'

@Injectable({
  providedIn: 'root'
})
export class AccountService {
 basicUrl='https://localhost:5001/api/';
 private CurrentUserSource =new ReplaySubject<User>(1);
 CurrentUser$= this.CurrentUserSource.asObservable();
  constructor(private http:HttpClient) {
   }
   login(model:any)
  {
   return this.http.post(this.basicUrl+'account/login',model).pipe(
     map((response : User)=>{
           const user=response;
           if(user)
           {
            this.SetCurrentUser(user);
           }
     })
   );

  }
  register(model:any)
  {
    return this.http.post(this.basicUrl+'account/register',model).pipe(
      map((user:User)=>{
      this.SetCurrentUser(user);
      })
    )
  }
  SetCurrentUser(user :User)
  {
    user.roles=[];
    const roles=this.getDecodedToken(user.token).role;
    Array.isArray(roles) ? user.roles=roles:user.roles.push(roles);
    localStorage.setItem('user',JSON.stringify(user));
 
    this.CurrentUserSource.next(user);
  }
  logout()
  {
    localStorage.removeItem('user');
    this.CurrentUserSource.next(null);
  }
  getDecodedToken(token)
  {
    return JSON.parse(atob(token.split('.')[1]));
  }
}
