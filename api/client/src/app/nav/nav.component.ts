import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../_Services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
model : any={}

  constructor(public Accountservices:AccountService, private router:Router,private toast:ToastrService) { }

  ngOnInit(): void {
  }
login()
{
this.Accountservices.login(this.model).subscribe(response=>{
  this.router.navigateByUrl('/members');
});
}
LogOut(){
  this.router.navigateByUrl('/')
  this.Accountservices.logout();
}
}
