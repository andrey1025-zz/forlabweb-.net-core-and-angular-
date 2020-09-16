import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';  
import { APIwithActionService } from "../../shared/APIwithAction.service";
@Component({
  selector: 'app-verifylink',
  templateUrl: './verifylink.component.html'
})
export class VerifylinkComponent implements OnInit {
  id: number;  
  constructor( private _avRoute: ActivatedRoute,private _APIwithActionService:APIwithActionService) {


    if (this._avRoute.snapshot.params["id"]) {  
      this.id = this._avRoute.snapshot.params["id"];  
  }  
this._APIwithActionService.getDatabyID(this.id,"User","Verifyaccount").subscribe((data)=>

{
 
})

   }

  ngOnInit() {
  }

}
