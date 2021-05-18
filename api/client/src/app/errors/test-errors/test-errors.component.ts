import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-test-errors',
  templateUrl: './test-errors.component.html',
  styleUrls: ['./test-errors.component.css']
})
export class TestErrorsComponent implements OnInit {
basicUrl='https://localhost:5001/api/';
validationErrors:string[]=[];

  constructor(private https:HttpClient) { }

  ngOnInit(): void {
  }
  get404error()
  {
    this.https.get(this.basicUrl+'buggy/not-found').subscribe(response=>{
      console.log(response);
    },error=>{
      console.log(error);
    })
  }
  get500error()
  {
    this.https.get(this.basicUrl+'buggy/server-error').subscribe(response=>{
      console.log(response);
    },error=>{
      console.log(error);
    })
  }
  get401error()
  {
    this.https.get(this.basicUrl+'buggy/auth').subscribe(response=>{
      console.log(response);
    },error=>{
      console.log(error);
    })
  }
  get400error()
  {
    this.https.get(this.basicUrl+'buggy/bad-request').subscribe(response=>{
      console.log(response);
    },error=>{
      console.log(error);
    })
  }
  get400Validationerror()
  {
    this.https.post(this.basicUrl+'account/register',{}).subscribe(response=>{
      console.log(response);
    },error=>{
      console.log(error);
      this.validationErrors=error;
    })
  }

}
