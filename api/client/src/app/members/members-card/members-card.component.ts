import { Component, Input, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Member } from 'src/app/_models/member';
import { MembersService } from 'src/app/_Services/members.service';

@Component({
  selector: 'app-members-card',
  templateUrl: './members-card.component.html',
  styleUrls: ['./members-card.component.css']
})
export class MembersCardComponent implements OnInit {
@Input() member:Member;
  constructor(private memberServices:MembersService,private toast:ToastrService) { }

  ngOnInit(): void {
  }
addLike(member:Member)
{
this.memberServices.addLike(member.userName).subscribe(()=>{
  this.toast.success('You have liked'+member.knownAs);
})
}
}
