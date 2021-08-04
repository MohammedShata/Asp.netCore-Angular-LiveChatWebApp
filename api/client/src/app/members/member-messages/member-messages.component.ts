import { Input, ViewChild } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Message } from 'src/app/_Models/message';
import { MessagesService } from 'src/app/_Services/message.service';

@Component({
  selector: 'app-member-messages',
  templateUrl: './member-messages.component.html',
  styleUrls: ['./member-messages.component.css']
})
export class MemberMessagesComponent implements OnInit {
  @ViewChild('messageForm') messageForm:NgForm;
  @Input() messages:Message[]=[];
  @Input () username:string;
  messageContent:string;
    constructor(public messageServices:MessagesService) { }
  
    ngOnInit(): void {
  
    }
    sendMessage(){
      this.messageServices.sendMessage(this.username,this.messageContent).then(()=>
     {
       this.messageForm.reset();
      });
       
    }
  
  
}
