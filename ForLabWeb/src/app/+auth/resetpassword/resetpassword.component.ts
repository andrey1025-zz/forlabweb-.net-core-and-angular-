import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';  
import { NgForm } from '@angular/forms';
import { APIwithActionService } from "../../shared/APIwithAction.service";
import { GlobalAPIService } from "../../shared/GlobalAPI.service";

@Component({
  selector: 'app-resetpassword',
  templateUrl: './resetpassword.component.html'
})
export class ResetpasswordComponent implements OnInit {
model:any;
id:any;
resetForm: NgForm;
  constructor(private _avRoute: ActivatedRoute,private router: Router, private _APIwithActionService: APIwithActionService, private _GlobalAPIService: GlobalAPIService) { 
    if (this._avRoute.snapshot.params["id"]) {  
      this.id = this._avRoute.snapshot.params["id"];  
  }  
    this.model = {
      password: '',
    
    }
  }

  ngOnInit() {
  }
  submit(){

   
    let updatepassword = new Object();
    updatepassword={
      newpassword:this.model.password
    }
    
    this._APIwithActionService.putAPI(this.id,updatepassword,"User","Updatepassword").subscribe((data)=>{
 
 
 this._GlobalAPIService.SuccessMessage("Password Reset Successfully");
    })
   }
}
