import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { User } from './_Models/user';
import { AccountService } from './_Services/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  title = 'Dating App';
  constructor(private http:HttpClient,private AccountServices:AccountService){}
ngOnInit(){

this.GetCurrentUser();
}
GetCurrentUser()
{
  const user:User=JSON.parse(localStorage.getItem('user'));
  this.AccountServices.SetCurrentUser(user);

}


}

