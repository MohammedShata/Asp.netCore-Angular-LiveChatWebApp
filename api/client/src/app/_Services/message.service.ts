import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Message } from '../_Models/message';
import { getPaginationHeaders, getPaginationResult } from './paginationHelper.services';

// const httpOptions={
//   headers:new HttpHeaders({
//     Authorization:'Bearer '+JSON.parse(localStorage.getItem('user'))?.token
//   })
  

@Injectable({
  providedIn: 'root'
})
export class MessagesService implements OnInit {
  baseUrl=environment.ApiUrl;
  constructor(private http:HttpClient) { }
  ngOnInit(): void {

  }
  getMessages(pageNumber,pageSize,container)
  {
    let params=getPaginationHeaders(pageNumber,pageSize);
    params=params.append('Container',container);
    console.log(params);
    return getPaginationResult<Message[]>(this.baseUrl+'messages',params,this.http);
  }
  getMessageThread(userName:string)
  {
   
    return this.http.get<Message[]>(this.baseUrl+ 'messages/thread/' + userName);
  }
  sendMessage(username:string ,content:string){
    return this.http.post<Message>(this.baseUrl+'messages',{RecipientUsername:username,content});

  }
}
