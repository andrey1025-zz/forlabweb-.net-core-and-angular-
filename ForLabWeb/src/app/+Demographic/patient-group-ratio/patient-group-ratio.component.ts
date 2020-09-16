import { Component, OnInit, Output, EventEmitter, Input,AfterViewInit } from '@angular/core';
import { APIwithActionService } from '../../shared/APIwithAction.service';
import { GlobalAPIService } from '../../shared/GlobalAPI.service';
import { Router, ActivatedRoute } from '@angular/router';

import { FormBuilder, FormGroup, FormControl, FormArray, Validators } from "@angular/forms";
//import { DemographicwizardComponent } from '../demographicwizard/demographicwizard.component';

@Component({
  selector: 'app-patient-group-ratio',
  templateUrl: './patient-group-ratio.component.html'

})
export class PatientGroupRatioComponent implements OnInit,AfterViewInit {
  forecastid: number;
  programid: number;
  Patientgroup: FormGroup;
  totalpercentage: number = 0;
  oldtoltalpercentage:number=0;
  totaltargetpatient: number = 0;
  forecasttype: string;
  @Input() RecforecastID:number;
  @Output()
  nextStep = new EventEmitter<string>();

  constructor(private _fb: FormBuilder, private _avRoute: ActivatedRoute,
    private _router: Router, private _APIwithActionService: APIwithActionService,
    private _GlobalAPIService: GlobalAPIService) {



  }

 ngAfterViewInit()
 {
   console.log('after view')
 }
  calculatepatientratio(searchValue: any, index: any) {

   if (searchValue<=100)
   {
    console.log(this.totalpercentage);
    (<FormGroup>(
      (<FormArray>this.Patientgroup.controls["_patientgroup"]).controls[
      index
      ]
    )).patchValue({
      PatientRatio: (parseFloat(searchValue) * this.totaltargetpatient) / 100
    })
  }
else{
  this._GlobalAPIService.FailureMessage("Percentage Should not be greater than 100");
  (<FormGroup>(
    (<FormArray>this.Patientgroup.controls["_patientgroup"]).controls[
    index
    ]
  )).patchValue({
    PatientRatio: 0,
    PatientPercentage:0
  })
}

  }
  ngOnInit() {
   
    if (this._avRoute.snapshot.params["id1"]) {
      this.programid = this._avRoute.snapshot.params["id1"];
      console.log('PG2')
    }
    if (this._avRoute.snapshot.params["id2"]) {
      this.forecastid = this._avRoute.snapshot.params["id2"];
      console.log('PG3')
    }
    if (this.RecforecastID>0)
    {
          this.forecastid = this.RecforecastID;
       
    }
 
    if (this.forecastid > 0) {
      this._APIwithActionService.getDatabyID(this.forecastid, 'Forecsatinfo', 'getforecasttype').subscribe((data) => {

        this.forecasttype = data.forecasttype;
      this._APIwithActionService.getDatabyID(this.forecastid, 'Forecsatinfo', 'Getpatientgroupbyforecastid', "programid=" + this.programid).subscribe((data) => {
        for (let boxIndex = 0; boxIndex < data.length; boxIndex++) {
         this.oldtoltalpercentage=this.oldtoltalpercentage+parseFloat(data[boxIndex].patientPercentage);
          this.addsitecategory();
          (<FormGroup>(
            (<FormArray>this.Patientgroup.controls["_patientgroup"]).controls[
            boxIndex
            ]
          )).patchValue({

            ID: data[boxIndex].id,
            ForecastInfoID: data[boxIndex].forecastinfoID,
            PatientGroupName: data[boxIndex].patientGroupName,
            PatientPercentage: data[boxIndex].patientPercentage,
            PatientRatio: data[boxIndex].patientRatio,
            GroupID: data[boxIndex].groupID
          });


        }
      })
      this._APIwithActionService.getDatabyID(this.forecastid, 'Forecsatinfo', 'Gettotaltargetpatient', "programid=" + this.programid).subscribe((data) => {

        this.totaltargetpatient = data;
      })
   
      })
      
    }
    this.Patientgroup = this._fb.group({
      _patientgroup: this._fb.array([]),

    })
    this.Patientgroup.controls._patientgroup.valueChanges.subscribe((change) => {
      const calculateAmount = (patientgroup: any[]): number => {
        return patientgroup.reduce((acc, current) => {
  
          this.totalpercentage = acc + parseFloat(current.PatientPercentage || 0);
          return acc + parseFloat(current.PatientPercentage || 0);
        }, 0);
      }
      calculateAmount(this.Patientgroup.controls._patientgroup.value);
  
    });
  }
  initsitecategory() {
    let patientgp: FormGroup = this._fb.group({
      ID: 0,
      GroupID: 0,
      ForecastInfoID: 0,
      PatientGroupName: [{ value: '', disabled: true }],
      PatientPercentage: 0,
      PatientRatio: [{ value: 0, disabled: true }],

    });
    return patientgp;
  }
  addsitecategory() {
    (<FormArray>this.Patientgroup.controls["_patientgroup"]).push(
      this.initsitecategory()
    );
  }
  delremovepatientgroup(index: any) {
    let groupid = (<FormGroup>(<FormArray>this.Patientgroup.controls["_patientgroup"])
      .controls[index]
    ).controls["GroupID"].value
    this._APIwithActionService.getDatabyID(this.forecastid, 'Forecsatinfo', 'getgroupexistintestingprotocol', "groupid=" + groupid).subscribe((data) => {
      if (data > 0) {
        this._GlobalAPIService.FailureMessage("You can't delete " + (<FormGroup>(<FormArray>this.Patientgroup.controls["_patientgroup"])
          .controls[index]
        ).controls["PatientGroupName"].value + " it is already use in testing protocol")
        return
      }
      else {
        this._APIwithActionService.deleteData(this.forecastid, 'Forecsatinfo', 'delpatientgroup', "groupid=" + groupid).subscribe((data) => {
          this._GlobalAPIService.SuccessMessage("Patient Group Deleted Successfully");
          (<FormArray>this.Patientgroup.controls["_patientgroup"]).removeAt(index);
        })
      }


    })
  }
  savepatientgroup() {
    let patientgpnumber = <FormArray>this.Patientgroup.controls["_patientgroup"]

    let patientgroupusage = new Array();

    patientgpnumber.getRawValue().forEach(x => {

      patientgroupusage.push(x)

    });
    console.log(patientgroupusage);
    if (this.totalpercentage > 100 || this.totalpercentage<100) {
      this._GlobalAPIService.FailureMessage("Ratio of Group should be equal to 100")
      return;
    }
    else {
      this._APIwithActionService.postAPI(patientgroupusage, 'Forecsatinfo', 'savepatientgroup').subscribe((data) => {
        if (data["_body"] != "0") {
          if(       this.oldtoltalpercentage==0)
          {
          this._GlobalAPIService.SuccessMessage("Patient Group Saved Successfully");
          }
      
        }
      })

    }
   this.nextStep.emit('step3,N,'+this.forecastid);
  }
  Previousclick()
  {
    if( this.forecasttype =="S")
    {
      this.nextStep.emit('step2S,P,'+this.forecastid)
    }
    else
    {
      this.nextStep.emit('step2A,P,'+this.forecastid)
     // this._router.navigate(["Demographic/aggregrateforecast",this.programid,this.forecastid])
    }
  }

} 
