import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { APIwithActionService } from '../../shared/APIwithAction.service';
import { GlobalAPIService } from '../../shared/GlobalAPI.service';
import { Router, ActivatedRoute } from '@angular/router';

import { FormBuilder, FormGroup, FormControl, FormArray, Validators } from "@angular/forms";
@Component({
  selector: 'app-demographiclineargrowth',
  templateUrl: './demographiclineargrowth.component.html',
  styleUrls: ['./demographiclineargrowth.component.css']
})
export class DemographiclineargrowthComponent implements OnInit {
  demographiclineargrowth:FormGroup;
  forecastid:number;
  programid:number;
  parameterlist=new Array();
  controlArray=new Array();
  constructor(private _fb: FormBuilder, private _avRoute: ActivatedRoute,
    private _router: Router, private _APIwithActionService: APIwithActionService,
    private _GlobalAPIService: GlobalAPIService) { }

  ngOnInit() {


    if (this._avRoute.snapshot.params["id"]) {  
      this.forecastid = this._avRoute.snapshot.params["id"];  
     
  
  }  
  if (this._avRoute.snapshot.params["pid"]) {  
    this.programid = this._avRoute.snapshot.params["pid"];  
   

  }

    this.demographiclineargrowth = this._fb.group({
      _lineargrowth: this._fb.array([]),

    })


    if (this.forecastid > 0) {
      
      this._APIwithActionService.getDatabyID(this.forecastid, 'Assumption', 'Getlineargrowth').subscribe((data) => {

        this.parameterlist = data[0].table;
        this._APIwithActionService.getDatabyID(this.forecastid, 'Assumption', 'GetlinearDynamiccontrol').subscribe((data) => {
          this.controlArray = data
      
          let ss = <FormArray>this.demographiclineargrowth.controls["_lineargrowth"];
          for (let index = 0; index < this.parameterlist.length; index++) {
            ss.push(this._fb.group(this.parameterlist[index]))


          }
        })


      })

      // 

      //    
    }

  }

  savepatientlineargrowth()
  {
    let lineargrowth = <FormArray>this.demographiclineargrowth.controls["_lineargrowth"];
    let lineargrowthlist1:Object;
let currentpatient:number=0
let targetpatient:number=0
    let lineargrowthvalue = new Array();
    lineargrowth.getRawValue().forEach(x => {
      currentpatient=currentpatient+parseFloat(x.currentpatient)
      targetpatient=targetpatient+parseFloat(x.targetpatient)
      for (let index = 0; index < this.controlArray.length; index++) {
      if(this.controlArray[index].name!="sitecategoryname")
      {
          lineargrowthvalue.push({

            Columnname: this.controlArray[index].name,
            Serial: x[this.controlArray[index].name],
            ForeCastId: this.forecastid,
            SiteCategoryId:x.siteCategoryId
          })

        }

      }
   
    })
    lineargrowthlist1={
      ForecastinfoID:this.forecastid,

      CurrentPatient:currentpatient,
      TargetPatient:targetpatient,
      patientdetaillist:lineargrowthvalue
    }
     this._APIwithActionService.postAPI(lineargrowthlist1,'Assumption','Savelineargrowth').subscribe((data)=>{
//  this._router.navigate(["Demographic/Demographicchart",this.forecastid])
  
  this._router.navigate(["/Forecast/Demographicchart", this.forecastid,this.programid]);  
  //  this.nextStep.emit('step7,N,'+this.forecastid);
     })

  }

}
