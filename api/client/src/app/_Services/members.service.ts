import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable, OnInit } from '@angular/core';
import { of } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';
import { PaginatedResult, Pagination } from '../_Models/pagination';
import { UserParams } from '../_Models/userParams';

// const httpOptions={
//   headers:new HttpHeaders({
//     Authorization:'Bearer '+JSON.parse(localStorage.getItem('user'))?.token
//   })
  

@Injectable({
  providedIn: 'root'
})
export class MembersService implements OnInit {
members:Member[]=[];
  basicUrl=environment.ApiUrl;
  
  constructor(private http:HttpClient)
   { 
   }
   ngOnInit()
   {
   
   }
   getMembers(userParams:UserParams){
    let params=this.getPaginationHeaders(userParams.pageNumber,userParams.pageSize);
    params=params.append('minAge',userParams.minAge.toString());
    params=params.append('maxAge',userParams.maxAge.toString());
    params=params.append('gender',userParams.gender.toString());

     return this.getPaginationResult<Member[]>(this.basicUrl+'users',params);
    
   }
  
   getMember(username:string){
     const member=this.members.find(x=>x.userName==username);
     if(member !==undefined) return of(member);
return this.http.get<Member>(this.basicUrl+'users/'+username);

   }
   updateMember(member:Member)
   {
     return this.http.put(this.basicUrl+'users',member).pipe(
       map(()=>
       {const index=this.members.indexOf(member);
        this.members[index]=member;
       })
     )
   }
   setMainPhoto(photoId:number)
   {
     return this.http.put(this.basicUrl+'users/set-main-photo/'+photoId,{});
   }
   DeletePhoto(PhotoId:number)
{
    return this.http.delete(this.basicUrl+'users/delete-photo/'+PhotoId);
}
private getPaginationResult<T>(url,params) {
  const paginatedResult:PaginatedResult<T>=new PaginatedResult<T>();
   return this.http.get<T>(url, { observe: 'response', params }).pipe(
     map(response => {
       paginatedResult.result = response.body;
       if (response.headers.get('Pagination') !== null) {
         paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
       }
       return paginatedResult;
     }));
 }

  private getPaginationHeaders(pageNumber:number,pageSize:number){
             let params=new HttpParams();
             params=params.append('pageNumber',pageNumber.toString());
             params=params.append('pageSize',pageSize.toString());
             return params;
  }
}
