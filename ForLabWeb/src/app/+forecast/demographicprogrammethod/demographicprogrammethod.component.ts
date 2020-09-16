import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { APIwithActionService } from '../../shared/APIwithAction.service';
import { GlobalAPIService } from '../../shared/GlobalAPI.service';
import { FormGroup, FormBuilder } from '@angular/forms';


@Component({
  selector: 'app-demographicprogrammethod',
  templateUrl: './demographicprogrammethod.component.html',
  styleUrls: ['./demographicprogrammethod.component.css']
})
export class DemographicprogrammethodComponent implements OnInit {
  parametername = new Array();
  ProgramList=new Array();
  forecastid:number=0;
  programid:string="0";
 // methodstatus:boolean=false;
  programname:string="";
  MMForecastParameterList = new Array();
  MMforcastparameter = new Array();
  parameterobject = new Object();
  method:string;
  programmethod:FormGroup;
  constructor(private _fb: FormBuilder,private _avRoute:ActivatedRoute,
    private _router: Router,private _APIwithActionService: APIwithActionService,
    private _GlobalAPIService:GlobalAPIService) { 

      if (this._avRoute.snapshot.params["id"]) {  
        this.forecastid = this._avRoute.snapshot.params["id"];  
       
    
    }  
    if (this._avRoute.snapshot.params["pid"]) {  
      this.programid = this._avRoute.snapshot.params["pid"];  
     
  
  }  
  this.programmethod = this._fb.group({
     
    method: '1'
  
               

})

  this.getprogramlist()
 

    }

  ngOnInit() {
 
    this.getprogramparameter()
  let postforecastparameter = new Array();
  if (this.programmethod.controls['method'].value == '1') {
      this.parametername[0] = "CurrentPatient";
      this.parametername[1] = "TargetPatient";
  }
  else {
      this.parametername[0] = "PopulationNumber";
      this.parametername[1] = "PrevalenceRate";
  }
  console.log(this.parametername)
  for (let index = 0; index < this.parametername.length; index++) {
      postforecastparameter.push(
          {
              id: 0,
              forecastMethod: parseInt(this.programmethod.controls['method'].value),
              forecastMethodname: name,
              variableName: this.parametername[index],
              variableDataType: 1,
              variableDataTypename: "Numeric",
              useOn: "OnAllSite",
              variableFormula: "",
              ProgramId: this.programid,
              VarCode: this.parametername[index][0] + this.programid,
              IsPrimaryOutput: false,
              VariableEffect: true,
              Forecastid:this.forecastid,
              isActive: "Yes"
          }

      )

  }
  this.MMforcastparameter.splice(0, this.MMforcastparameter.length)
  this.MMforcastparameter = postforecastparameter;
  postforecastparameter.forEach(x => {
      this.MMForecastParameterList.push(x);
  })
  this.parameterobject = {
      id: this.programid,
      ProgramName: this.programname,
      _mMForecastParameter: this.MMforcastparameter
  }



  }
  getprogramlist() {
    this._APIwithActionService.getDataList('MMProgram', 'Get').subscribe((resp) => {
        this.ProgramList = resp;
        this.programname=this.ProgramList.filter(x=>x.id===this.programid)["programName"]
    })
}
  changeforecsattype(value:any,name:any)
  {
    let postforecastparameter = new Array();
    if (value == 1) {
        this.parametername[0] = "CurrentPatient";
        this.parametername[1] = "TargetPatient";
    }
    else {
        this.parametername[0] = "PopulationNumber";
        this.parametername[1] = "PrevalenceRate";
    }
    console.log(this.parametername)
    for (let index = 0; index < this.parametername.length; index++) {
        postforecastparameter.push(
            {
                id: 0,
                forecastMethod: parseInt(value),
                forecastMethodname: name,
                variableName: this.parametername[index],
                variableDataType: 1,
                variableDataTypename: "Numeric",
                useOn: "OnAllSite",
                variableFormula: "",
                ProgramId: this.programid,
                VarCode: this.parametername[index][0] + this.programid,
                IsPrimaryOutput: false,
                VariableEffect: true,
                Forecastid:this.forecastid,
                isActive: "Yes"
            }

        )

    }
    this.MMforcastparameter.splice(0, this.MMforcastparameter.length)
    this.MMforcastparameter = postforecastparameter;
    postforecastparameter.forEach(x => {
        this.MMForecastParameterList.push(x);
    })
    this.parameterobject = {
        id: this.programid,
        ProgramName: this.programname,
        _mMForecastParameter: this.MMforcastparameter
    }

  

  }
  saveforecastparameter()
  {
    if (this.parametername.length != 0) {
      this._APIwithActionService.postAPI(this.parameterobject, 'MMProgram', 'Saveforecastparameter').subscribe(
          (data) => {
              this.parameterobject = "";
              this.parametername.splice(0, this.parametername.length);
              // this._router.navigate(["/Forecast/Demographicgroup", this.programid,this.forecastid]);
          }
      )
      
  }
  this._router.navigate(["/Forecast/Demographicgroup", this.forecastid,this.programid]);
  }
  getprogramparameter() {
    this._APIwithActionService.getDataList('MMProgram', 'Getforecastparameter').subscribe((resp) => {
        this.MMForecastParameterList = resp;
        this.MMforcastparameter = this.MMForecastParameterList.filter(x => x.programId === parseInt(this.programid))
        if (this.MMforcastparameter.length > 0) {
           // this.methodstatus = true;
console.log( this.MMforcastparameter[0]["forecastMethod"] )
            this.programmethod.patchValue({     
              method: parseInt(this.MMforcastparameter[0]["forecastMethod"]).toString()  
          })
          this.programmethod.get('method').disable();
          //  this.method=this.MMforcastparameter[0]["ForecastMethod"]
        }
        else {
           // this.methodstatus = false;
        }
        //console.log(this.methodstatus);
    })
}
}
