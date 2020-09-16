import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { APIwithActionService } from '../../shared/APIwithAction.service';
import { GlobalAPIService } from '../../shared/GlobalAPI.service';
import { FormGroup, FormBuilder } from '@angular/forms';
@Component({
  selector: 'app-demographictreatmentcycle',
  templateUrl: './demographictreatmentcycle.component.html',
  styleUrls: ['./demographictreatmentcycle.component.css']
})
export class DemographictreatmentcycleComponent implements OnInit {
  forecastid:number=0;
  programid:number=0;
  //cycle:string="13"
  traetmentcycle:FormGroup
  methodstatus:boolean=false;
  constructor(private _fb: FormBuilder,private _avRoute:ActivatedRoute,
    private _router: Router,private _APIwithActionService: APIwithActionService,
    private _GlobalAPIService:GlobalAPIService) { 

      if (this._avRoute.snapshot.params["id"]) {  
        this.forecastid = this._avRoute.snapshot.params["id"];  
       
    
    }  
    if (this._avRoute.snapshot.params["pid"]) {  
      this.programid = this._avRoute.snapshot.params["pid"];  
     
  
    }
    if(this.forecastid>0)
    {
      this._APIwithActionService.getDatabyID(this.forecastid,'Forecsatinfo','GetbyId').subscribe((resp) => {                  
     
     console.log(resp);
     if(resp["months"] !=0)
     {
      this.traetmentcycle.patchValue({
     
        cycle: parseInt(resp["months"]).toString()
      
                   
    
    })
  this.traetmentcycle.get('cycle').disable();
     //  this.cycle=resp["months"]
        this.methodstatus=true;
     }
        else
        this.methodstatus=false;
      })
    }

  }  
  ngOnInit() {
    this.traetmentcycle = this._fb.group({
     
      cycle: '13'
    
                 
  
  })
  }
  changecycle(value:any)
  {

  //   let forecastyear= new Object();
  //   forecastyear={
  //     ProgramID:this.programid,
  //    forecastID:this.forecastid,
  //     UserId:localStorage.getItem("userid"),
  //  months:value
  //   }
    this._APIwithActionService.putAPI(this.forecastid,value,"MMProgram","updatedemographicprogram").subscribe((data)=>{

this._router.navigate(["/Forecast/Forecasttype", this.forecastid]);  

    })
  }
  save()
  {
    this._APIwithActionService.putAPI(this.forecastid,this.traetmentcycle.controls["cycle"].value,"MMProgram","updatedemographicprogram").subscribe((data)=>{

      this._router.navigate(["/Forecast/Forecasttype", this.forecastid]);  
      
          })
  }
}
