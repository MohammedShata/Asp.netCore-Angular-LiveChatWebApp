import { Component, OnInit } from '@angular/core';
import { Member } from '../_models/member';
import { Pagination } from '../_Models/pagination';
import { MembersService } from '../_Services/members.service';

@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.css']
})
export class ListsComponent implements OnInit {
members:Partial<Member[]>;
predicate='liked';
pageNumber=1;
pageSize=4;
pagination:Pagination;
  constructor(private memberServices:MembersService) { }

  ngOnInit(): void {
    this.loadLikes();
  }
  loadLikes(){
    this.memberServices.getLikes(this.predicate,this.pageNumber,
      this.pageSize).subscribe(response=>{
      this.members=response.result;
 
      this.pagination=response.pagination;
     
    })
  }
  pageChanged(event:any)
  {
    this.pageNumber=event.page;
  
     this.loadLikes();
  }

}
