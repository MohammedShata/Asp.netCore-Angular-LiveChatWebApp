import { Component, OnInit,Input } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { take } from 'rxjs/operators';
import { Member } from 'src/app/_models/member';
import { Photo } from 'src/app/_models/photo';
import { User } from 'src/app/_Models/user';
import { AccountService } from 'src/app/_Services/account.service';
import { MembersService } from 'src/app/_Services/members.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent implements OnInit {
@Input() member:Member;
uploader:FileUploader;
hasBaseDropzoneOver=false;
baseUrl=environment.ApiUrl;
user:User;
  constructor(private accountServices:AccountService,private memberServices:MembersService) {
    this.accountServices.CurrentUser$.pipe(take(1)).subscribe(user=> this.user=user);
   }

  ngOnInit(): void {
    this.initializeUploader();
  }
  fileOverBase(e:any)
  {
    this.hasBaseDropzoneOver=e;
  }
  initializeUploader(){
    this.uploader=new FileUploader({
      url:this.baseUrl+'users/add-photo',
      authToken:'Bearer '+this.user.token,
      isHTML5:true,
      allowedFileType:['image'],
      removeAfterUpload:true,
      autoUpload:false,
      maxFileSize:10*1024*1024
    });
    this.uploader.onAfterAddingFile=(file)=>{
      file.withCredentials=false;
    }
this.uploader.onSuccessItem=(item,response,status,headers)=>{
  if(response){
    const photo:Photo=JSON.parse(response);
    this.member.photos.push(photo);
    if(photo.isMain)
    {
      this.user.photoUrl=photo.url;
      this.member.photoUrl=photo.url;
      this.accountServices.SetCurrentUser(this.user);
    }
  }
}
  }
  setMainPhoto(photo:Photo)
  {
    this.memberServices.setMainPhoto(photo.id).subscribe(()=>{
      this.user.photoUrl=photo.url;
      this.accountServices.SetCurrentUser(this.user);
      this.member.photoUrl=photo.url;
      this.member.photos.forEach(p =>{
        if(p.isMain) p.isMain=false;
        if(p.id==photo.id) p.isMain=true;
      })
    })

  }
  DeletePhoto(photoId:number)
  {
    this.memberServices.DeletePhoto(photoId).subscribe(()=>{
      this.member.photos=this.member.photos.filter(x=>x.id !==photoId)
    })
  }

}
