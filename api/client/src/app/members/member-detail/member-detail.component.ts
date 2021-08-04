import { OnDestroy, ViewChild } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs';
import { take } from 'rxjs/operators';
import { Member } from 'src/app/_models/member';
import { Message } from 'src/app/_Models/message';
import { User } from 'src/app/_Models/user';
import { AccountService } from 'src/app/_Services/account.service';
import { MembersService } from 'src/app/_Services/members.service';
import { MessagesService } from 'src/app/_Services/message.service';
import { PresenceService } from 'src/app/_Services/presence.service';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit,OnDestroy {
  @ViewChild('memberTabs',{static:true}) memberTabs:TabsetComponent;
member:Member;
galleryOptions: NgxGalleryOptions[];
galleryImages:NgxGalleryImage[];
activeTap:TabDirective;
messages:Message[]=[];
user:User;
ngOnDestroy():void{
this.messageServices.stopHubConnection();
}
  constructor(public presence:PresenceService,private route:ActivatedRoute,
    private messageServices:MessagesService,private accountServices:AccountService,
    private router:Router) {
      this.accountServices.CurrentUser$.pipe(take(1)).subscribe(user => this.user=user);
        this.router.routeReuseStrategy.shouldReuseRoute = () => false;
     }

  ngOnInit(): void {
  this.route.data.subscribe(data=>{
    this.member=data.member;
  })

  this.route.queryParams.subscribe(params=>{
   params.tab ? this.onSelectedTab(params.tab) : this.onSelectedTab(0);
  }
  )

  this.galleryOptions=[{
    width:'500px',
    height:'500px',
    imagePercent:100,
    thumbnailsColumns:4,
    imageAnimation: NgxGalleryAnimation.Slide,
    preview:false
  }]
  this.galleryImages=this.getImages();
 
  }
getImages():NgxGalleryImage[]{
  const imageUrl=[];
  for(const photo of this.member.photos){
    imageUrl.push({
      small:photo?.url,
      medium:photo?.url,
      big:photo?.url
    })
  }
  return imageUrl;
}

  loadMessages(){
    this.messageServices.getMessageThread(this.member.userName).
    subscribe(messages=>{
      this.messages=messages;
    });
  }
  onSelectedTab(tabId:number)
{
  this.memberTabs.tabs[tabId].active=true;
}
  onTabsActivited(data:TabDirective){
    this.activeTap=data;
    if(this.activeTap.heading==='Messages'&& this.messages.length===0)
    {
          this.messageServices.createHubConnection(this.user,this.member.userName)
    }
    else{
      this.messageServices.stopHubConnection();
    }
    
  }
}
