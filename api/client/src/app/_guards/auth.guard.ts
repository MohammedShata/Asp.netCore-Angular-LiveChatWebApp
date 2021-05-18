import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AccountService } from '../_Services/account.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private accountservices:AccountService,private toast:ToastrService){}
  canActivate(): Observable<boolean>{
    return this.accountservices.CurrentUser$.pipe(
      map(user=>{
        if(user)
        return true;
        this.toast.error('you shall not pass!');
      })
    )
  }
  
}
