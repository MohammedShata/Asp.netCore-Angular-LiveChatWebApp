import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { User } from './_Models/user';
import { AccountService } from './_Services/account.service';
import { PresenceService } from './_Services/presence.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  title = 'Dating App';
  constructor(private http:HttpClient,
    private AccountServices:AccountService,
    private presceneServices:PresenceService){}
ngOnInit(){

this.GetCurrentUser();
}
GetCurrentUser()
{
  const user:User=JSON.parse(localStorage.getItem('user'));
  if(user)
  { 
    this.AccountServices.SetCurrentUser(user);
    this.presceneServices.createHubConnection(user);
  }


}


}

