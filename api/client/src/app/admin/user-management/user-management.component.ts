import { Component, OnInit } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { RolesModalComponent } from 'src/app/modals/roles-modal/roles-modal.component';
import { User } from 'src/app/_Models/user';
import { AdminService } from 'src/app/_Services/admin.service';

@Component({
  selector: 'app-user-management',
  templateUrl: './user-management.component.html',
  styleUrls: ['./user-management.component.css']
})
export class UserManagementComponent implements OnInit {
users:Partial<User[]>;
bsModalRef:BsModalRef;
  constructor(private adminServices:AdminService,private modalservice:BsModalService) 
   { }

  ngOnInit(): void {
    this.getUsersWithRoles();
  }
getUsersWithRoles()
{
  this.adminServices.getUserWithRoles().subscribe(users=>{
this.users=users;
  }
  )
}
openRolesModal(){
  const initialState = {
    list: [
      'Open a modal with component',
      'Pass your data',
      'Do something else',
      '...'
    ],
    title: 'Modal with component'
  };
  this.bsModalRef=this.modalservice.show(RolesModalComponent,{initialState});
  this.bsModalRef.content.closeBtnName = 'Close';
}
}
