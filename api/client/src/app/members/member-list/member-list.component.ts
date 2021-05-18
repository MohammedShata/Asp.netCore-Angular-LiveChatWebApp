import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { take } from 'rxjs/operators';
import { Member } from 'src/app/_models/member';
import { Pagination } from 'src/app/_Models/pagination';
import { User } from 'src/app/_Models/user';
import { UserParams } from 'src/app/_Models/userParams';
import { AccountService } from 'src/app/_Services/account.service';
import { MembersService } from 'src/app/_Services/members.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
members:Member[];
pagination:Pagination;
userParams:UserParams;
user:User;
genderList=[{value:'male',display:'Males'},{value:'female',display:'Females'}]
  constructor(private memberServices:MembersService,private account:AccountService) 
  {
    this.account.CurrentUser$.pipe(take(1)).subscribe(user=>{
     this.user=user;
     this.userParams=new UserParams(user);
  })
 }

  ngOnInit(): void {
   this.loadMembers();
    }
    loadMembers(){
      this.memberServices.getMembers(this.userParams).subscribe(response=>{
        this.members=response.result;
        this.pagination=response.pagination;
    })
  }
  resetFilter(){
    this.userParams=new UserParams(this.user);
    this.loadMembers();
  }
  pageChanged(event:any)
  {
    this.userParams.pageNumber=event.page;
    this.loadMembers();
  }

}
