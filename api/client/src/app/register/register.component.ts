import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../_Services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
model :any= {};
registerForm:FormGroup;
maxDate: Date;
validationErrors:string[]=[];
@Output() cancelRegister=new EventEmitter();
  constructor(private accountServices:AccountService,private toast:ToastrService,private fb:FormBuilder,private router:Router) { }

  ngOnInit(): void {
    this.initializeForm();
    this.maxDate=new Date();
    this.maxDate.setFullYear(this.maxDate.getFullYear()-18);
  }
  initializeForm()
  {
    this.registerForm=this.fb.group({
      gender:['male'],
      username:['',Validators.required],
      knownAs:['',Validators.required],
      dateOfbirth:['',Validators.required],
      city:['',Validators.required],
      country:['',Validators.required],
      password:['',[Validators.required,Validators.minLength(4),Validators.maxLength(8)]],
      confirmPassword:['',[Validators.required,this.matchValues('password')]]
    });
 this.registerForm.controls.password.valueChanges.subscribe(()=>{
   this.registerForm.controls.confirmPassword.updateValueAndValidity();
 })
  }
  matchValues(matchTo:string) :ValidatorFn{
    return (control:AbstractControl)=>{
      return control?.value===control?.parent?.controls[matchTo].value ? null : {isMatching:true}
    }
  }
  register()
  {
   
    this.accountServices.register(this.registerForm.value).subscribe(response=>{
this.router.navigateByUrl('/members');

    },error=>{
      this.validationErrors=error;
    })
  }
  cancel(){
    this.cancelRegister.emit(false);
  }

}