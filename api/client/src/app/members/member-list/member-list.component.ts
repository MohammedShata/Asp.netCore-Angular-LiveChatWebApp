import { Component, OnInit } from '@angular/core';
import { Member } from 'src/app/_models/member';
import { Pagination } from 'src/app/_Models/pagination';
import { User } from 'src/app/_Models/user';
import { UserParams } from 'src/app/_Models/userParams';
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
  constructor(private memberServices:MembersService) 
  {
    this.userParams=this.memberServices.getUserParams();
 
  }
 

  ngOnInit(): void {
   this.loadMembers();
    }
    loadMembers(){
      this.memberServices.setUserParams(this.userParams);
      this.memberServices.getMembers(this.userParams).subscribe(response=>{
        this.members=response.result;
        this.pagination=response.pagination;
    })
  }
  resetFilter(){
    // this.userParams=new UserParams(this.user);
    this.userParams=this.memberServices.resetUserParams();
    this.loadMembers();
  }
  pageChanged(event:any)
  {
    this.userParams.pageNumber=event.page;
    this.memberServices.setUserParams(this.userParams);
    this.loadMembers();
  }

}
