import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { APIwithActionService } from '../../shared/APIwithAction.service';
import { GlobalAPIService } from '../../shared/GlobalAPI.service';
import { Router, ActivatedRoute } from '@angular/router';

import { FormBuilder, FormGroup, FormControl, FormArray, Validators } from "@angular/forms";
@Component({
  selector: 'app-patientassumption',
  templateUrl: './patientassumption.component.html',
  styleUrls: ['./patientassumption.component.css']
})
export class PatientassumptionComponent1 implements OnInit {
  Patientass: FormGroup
  forecastid: number;
  programid: number;
  parameterlist = new Array();
  controlArray = new Array();
  outsideRange: boolean;
  forecasttype: string;
  disableinput:boolean;
  HeaderArray= new Array();
  constructor(private _fb: FormBuilder, private _avRoute: ActivatedRoute,
    private _router: Router, private _APIwithActionService: APIwithActionService,
    private _GlobalAPIService: GlobalAPIService) { }
    comparecurrentpatient(group: FormGroup) {
      for (let index = 0; index < this.controlArray.length; index++) {
        if (this.controlArray[index].type == "number") {
          if (group.value[this.controlArray[index].name] == 0) { this.outsideRange = true; }
          else {
            this.outsideRange = false;
          }
  
        }
        else {
          this.outsideRange = false;
        }
  
      }
  
    }
  ngOnInit() {

    if (this._avRoute.snapshot.params["pid"]) {
      this.programid = this._avRoute.snapshot.params["pid"];

    }
    if (this._avRoute.snapshot.params["id"]) {
      this.forecastid = this._avRoute.snapshot.params["id"];

    }


    this.Patientass = this._fb.group({
      _patientAssumption: this._fb.array([]),

    })
  
    if (this.forecastid > 0) {
      this._APIwithActionService.getDatabyID(this.forecastid, 'Forecsatinfo', 'Getbyid').subscribe((data) => {

        this.forecasttype = data.forecastType;
        this._APIwithActionService.getDatabyID(this.forecastid, 'Assumption', 'GetforecastpatientAssumption').subscribe((data) => {

          this.parameterlist = data;
          this._APIwithActionService.getDatabyID(this.forecastid, 'Assumption', 'GetforecastDynamiccontrol','entitytype=3').subscribe((data) => {
            this.controlArray = data
            this._APIwithActionService.getDatabyID(this.forecastid, 'Assumption', 'getdynamicheader', 'entitytype=3').subscribe((data2) => {
              this.HeaderArray = data2
            let ss = <FormArray>this.Patientass.controls["_patientAssumption"];
            for (let index = 0; index < this.parameterlist.length; index++) {
              ss.push(this._fb.group(this.parameterlist[index], { validator: this.comparecurrentpatient.bind(this) }))


            }
          })
          })


        })

        // 

        //    
      })
      for (let index = 0; index < this.controlArray.length; index++) {
        if (this.controlArray[index].type == "text") {
this.disableinput=true
        }
    }
  }
  }
  savepatientassumption() {
    let patientAssumption = <FormArray>this.Patientass.controls["_patientAssumption"];
    let patientAssumptionlist = new Array();

    let patientAssumptionvalue = new Array();
    if (this.forecasttype == "S") {
      patientAssumption.getRawValue().forEach(x => {

        patientAssumptionlist.push({
          ID: x.id,
          ForecastinfoID: x.forecastinfoID,
          SiteID: null,
          CategoryID: x.siteCategoryID

        })

        for (let index = 0; index < this.controlArray.length; index++) {
          if (this.controlArray[index].type == "number") {
            patientAssumptionvalue.push({

              Parameterid: this.controlArray[index].id,
              Parametervalue: x[this.controlArray[index].name],
              Forecastid: x.forecastinfoID,
              SiteID: null,
              CategoryID: x.siteCategoryID
            })

          }

        }

      });
    }
    else {
      patientAssumption.getRawValue().forEach(x => {

        patientAssumptionlist.push({
          ID: x.id,
          ForecastinfoID: x.forecastinfoID,
          SiteID: null,
          CategoryID: x.siteCategoryID

        })
        for (let index = 0; index < this.controlArray.length; index++) {
          if (this.controlArray[index].type == "number") {
            patientAssumptionvalue.push({

              Parameterid: this.controlArray[index].id,
              Parametervalue: x[this.controlArray[index].name],
              Forecastid:  x.forecastinfoID,
              SiteID:null,
              CategoryID:  x.siteCategoryID,
            })

          }

        }
      });
    }

    this._APIwithActionService.postAPI(patientAssumptionlist,"Assumption","SavepatientAssumption").subscribe((data)=>{

      this._APIwithActionService.postAPI(patientAssumptionvalue,'Assumption','savemmgeneralassumptionvalue').subscribe((data)=>{
      
      if(data["_body"] !=0)
      {
       // this._GlobalAPIService.SuccessMessage("Patient Assumption Saved Successfully");
      }
   this._router.navigate(["Forecast/Demographicproduct",this.forecastid,this.programid])
    //this.nextStep.emit('step5,N,'+this.forecastid);
  }
      )
    })
 
  }

  inputvalue(args,i,datatype)
  {
    let name=args.target.name;
    if (datatype==2)
    {
      if (args.target.value>100)
      {
      this._GlobalAPIService.FailureMessage("Percentage should not be greater than 100");
      (<FormArray>(this.Patientass.controls["_patientAssumption"])).controls[i].patchValue({
        [name]:0
      })
    }

  }
}
}
