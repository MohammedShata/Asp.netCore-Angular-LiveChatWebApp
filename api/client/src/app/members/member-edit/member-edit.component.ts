import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { Member } from 'src/app/_models/member';
import { User } from 'src/app/_Models/user';
import { AccountService } from 'src/app/_Services/account.service';
import { MembersService } from 'src/app/_Services/members.service';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
 @ViewChild('editForm') editform:NgForm;
  member:Member;
 user:User;
 @HostListener('window:beforeunload',['$event']) unloadNotification($event:any)
 {
if(this.editform.dirty)
{
  $event.returnValue=true;
}
 }
  constructor(private accountServices:AccountService,private memberServices:MembersService,private toast:ToastrService) {
    this.accountServices.CurrentUser$.pipe(take(1)).subscribe(user=>this.user=user);
   }
    
  ngOnInit(): void {
  this.loadMember();
  }
loadMember(){
  this.memberServices.getMember(this.user.userName).subscribe(member=>{
    this.member=member;
  })
}

UpdateMember(){
this.memberServices.updateMember(this.member).subscribe(()=>{
  this.toast.success('Profile Updated Successfully');
this.editform.reset(this.member);
})

}
}
