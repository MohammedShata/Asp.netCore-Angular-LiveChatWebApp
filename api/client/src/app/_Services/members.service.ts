import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable, OnInit } from '@angular/core';
import { of } from 'rxjs';
import { map, take } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';
import { PaginatedResult, Pagination } from '../_Models/pagination';
import { User } from '../_Models/user';
import { UserParams } from '../_Models/userParams';
import { AccountService } from './account.service';

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
  memberCache =new Map();
  user:User;
  userParams: UserParams;
  
  constructor(private http:HttpClient,private accountService:AccountService)
   {
    this.accountService.CurrentUser$.pipe(take(1)).subscribe(user=>{
      this.user=user;
      this.userParams=new UserParams(user);
   })
   }
   ngOnInit()
   {
   
   }
 
  getUserParams(){
    return this.userParams;
  }
  setUserParams(params:UserParams)
  {
    this.userParams=params;
  }
  resetUserParams(){
    this.userParams=new UserParams(this.user);
    return this.userParams;
  }

  
   getMembers(userParams:UserParams){
     var response=this.memberCache.get(Object.values(userParams).join('-'));
     if(response)
     {
       return of (response);
     }
    let params=this.getPaginationHeaders(userParams.pageNumber,userParams.pageSize);
    params=params.append('minAge',userParams.minAge.toString());
    params=params.append('maxAge',userParams.maxAge.toString());
    params=params.append('gender',userParams.gender.toString());
    params=params.append('orderBy',userParams.orderBy.toString());
     return this.getPaginationResult<Member[]>(this.basicUrl+'users',params).
     pipe(map(response =>
     {
       this.memberCache.set(Object.values(userParams).join('-'),response);
       return response;
     }))
    
   }
   addLike(username:string)
   {
     return this.http.post(this.basicUrl+'likes/'+username,{});
   }
    getLikes(predicate:string,pageNumber,pageSize)
   {
     let params=this.getPaginationHeaders(pageNumber,pageSize);
     params=params.append('predicate',predicate);
     return this.getPaginationResult<Partial<Member[]>>(this.basicUrl+'likes',params);
   }
   getMember(username:string){
    //  const member=this.members.find(x=>x.userName==username);
    //  if(member !==undefined) return of(member);
    const member=[...this.memberCache.values()]
    .reduce((arr,elem)=>arr.concat(elem.result),[]).
    find((member:Member)=>member.userName==username);
    if(member)
    {
      return of(member);
    }
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
             console.log(params);
             return params;
  }
}
