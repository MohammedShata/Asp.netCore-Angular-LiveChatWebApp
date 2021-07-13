import {Message} from '../_Models/message';
import { Component, OnInit } from '@angular/core';
import { Pagination } from '../_Models/pagination';
import { MessagesService } from '../_Services/message.service';


@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit {
messages:Message[]=[];
pagination:Pagination;
container='Inbox';
pageNumber=1;
pageSize=5;
loading=false;
  constructor(private messageService:MessagesService) { }

  ngOnInit(): void {
    this.loadMessages();
  }
  loadMessages()
  {
    this.loading=true;
    this.messageService.getMessages(this.pageNumber,this.pageSize,this.container)
    .subscribe(response=>{
      console.log(response);
      this.messages=response.result;
      this.pagination=response.pagination;
      this.loading=false;
    })
  }
  pageChanged(event:any)
  {
    this.pageNumber=event.page;
    this.loadMessages();
  }

}
