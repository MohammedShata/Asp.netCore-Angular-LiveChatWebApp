import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../_Models/user';
import { AccountService } from '../_Services/account.service';
import { take } from 'rxjs/operators';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {

  constructor(private accountServices:AccountService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    let CurrentUser:User;
    this.accountServices.CurrentUser$.pipe(take(1)).subscribe(user=>CurrentUser=user);
    if(CurrentUser)
    request=request.clone({
      setHeaders:{
      Authorization: `Bearer ${CurrentUser.token}`
    }
    })
    return next.handle(request);
  }
}
