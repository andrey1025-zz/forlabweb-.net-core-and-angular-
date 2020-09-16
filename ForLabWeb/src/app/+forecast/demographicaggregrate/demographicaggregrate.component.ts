import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { APIwithActionService } from '../../shared/APIwithAction.service';
import { GlobalAPIService } from '../../shared/GlobalAPI.service';
import { FormGroup, FormBuilder, FormArray } from '@angular/forms';
import {NotificationService} from '../../shared/utils/notification.service';
@Component({
  selector: 'app-demographicaggregrate',
  templateUrl: './demographicaggregrate.component.html',
  styleUrls: ['./demographicaggregrate.component.css']
})
export class DemographicaggregrateComponent implements OnInit {
  demographicaggregrate:FormGroup
  forecastid:number
  programid:number
  Forecastmethod: number = 0;
  Show: boolean = false;
  Editdata=new Array();
  outsideRange:boolean=false;
  selectedRow: Number;
  constructor(private notificationService:NotificationService,private _fb: FormBuilder, private _avRoute: ActivatedRoute,
    private _router: Router, private _APIwithActionService: APIwithActionService,
    private _GlobalAPIService: GlobalAPIService) { 
    if (this._avRoute.snapshot.params["id"]) {  
      this.forecastid = this._avRoute.snapshot.params["id"];  
     
  
  }  



  if (this.forecastid > 0) {
    this._APIwithActionService.getDatabyID(this.forecastid, 'MMProgram', 'Getforecastparameterbyprogramid').subscribe((resp) => {

      this.Forecastmethod = resp[0]["forecastMethod"];
      console.log(this.Forecastmethod);
      if (this.Forecastmethod == 1) {
        this.Show = true
      }
      else {
        this.Show = false
      }
      console.log(this.Show);
    })
  }
  if (this.forecastid > 0) {
    this._APIwithActionService.getDatabyID(this.forecastid, 'Forecsatinfo', 'Getcategoryinfobyforecastid').subscribe((data) => {
      if(data.length>0)
      {
      for (let boxIndex = 0; boxIndex < data.length; boxIndex++) {
        this.addpatientnumber();
        (<FormGroup>(
          (<FormArray>this.demographicaggregrate.controls["_patientnumber"]).controls[
          boxIndex
          ]
        )).patchValue({

          id: data[boxIndex].id,
          ForecastinfoID: data[boxIndex].forecastinfoID,
          SiteCategoryId: data[boxIndex].siteCategoryId,
          SiteCategoryName: data[boxIndex].siteCategoryName,
          Currentpatient: data[boxIndex].currentPatient,
          Targetpatient: data[boxIndex].targetPatient,
          PopulationNumber: data[boxIndex].populationNumber,
          PrevalenceRate: data[boxIndex].prevalenceRate,

        });


      }
      this.demographicaggregrate.patchValue({
        methodtype:data[0].methodtype
      })
    //  this.showdiv(data[0].methodtype);
      this.demographicaggregrate.get('methodtype').disable();
    }
    })
   
  }
}
Previousclick()
{
  this.notificationService.smallBox({
    title: "Conformation!",
    content: "Would like to Cancel for this page? <p class='text-align-right'><a href='/#/Forecast/Importforecastdata/" + this.forecastid + "' class='btn btn-primary btn-sm'>Yes</a> <a href-void class='btn btn-danger btn-sm'>No</a></p>",
    color: "#296191",
    //timeout: 8000,
    icon: "fa fa-bell swing animated"
  });
  
}

  ngOnInit() {
    this.demographicaggregrate = this._fb.group({
 
      methodtype:'catregions',
    
      _patientnumber: this._fb.array([])

    })
  }
  addpatientnumber() {
    (<FormArray>this.demographicaggregrate.controls["_patientnumber"]).push(
      this.initpatientnumber()
    );
  }
  initpatientnumber() {
    let patientnumber: FormGroup = this._fb.group({
      id: 0,
      ForecastinfoID: 0,
      SiteCategoryId: 0,
      SiteCategoryName: [{ value: '', disabled: true }],
      Currentpatient: 0,
      Targetpatient: 0,
      PopulationNumber: 0,
      PrevalenceRate: 0
    }
      // , {
      //     validator: this.comparecurrentpatient.bind(this),

      //   }


    );
  
    return patientnumber;
  }
  comparecurrentpatient(group: FormGroup) {

    if (group.value.Currentpatient > group.value.Targetpatient) {

      this.outsideRange = true;



    }
    else {
      this.outsideRange = false;
    }
    return this.outsideRange

  }

  savesitebycategoryinfo() {

    let patientnumber = <FormArray>this.demographicaggregrate.controls["_patientnumber"]
    let sitesarray = <FormArray>this.demographicaggregrate.controls["sitecategorylist"]
    let patientnumberusage = new Array();
    let categorysite = new Array();



    if (patientnumber.getRawValue().length > 0) {
      for (let index = 0; index < patientnumber.getRawValue().length; index++) {
        if ((Number)(patientnumber.getRawValue()[index].Currentpatient) > (Number)(patientnumber.getRawValue()[index].Targetpatient)) {
          this._GlobalAPIService.FailureMessage(patientnumber.getRawValue()[index].SiteCategoryName + " Should have Current Patient less than target Patient")

          return;
        }
        else {
          patientnumberusage.push({
            ForecastinfoID: patientnumber.getRawValue()[index].ForecastinfoID,
            SiteCategoryId:patientnumber.getRawValue()[index].SiteCategoryId,
            SiteCategoryName: patientnumber.getRawValue()[index].SiteCategoryName,
            Currentpatient: patientnumber.getRawValue()[index].Currentpatient,
            Targetpatient: patientnumber.getRawValue()[index].Targetpatient,
            PopulationNumber:patientnumber.getRawValue()[index].PopulationNumber,
            PrevalenceRate:patientnumber.getRawValue()[index].PrevalenceRate,
            methodtype:this.demographicaggregrate.controls["methodtype"].value
          })
            
            
          
        }
      }
    }
    else {

      this._GlobalAPIService.FailureMessage("Please Select atleast one category")
      return;
    }
    // patientnumber.getRawValue().forEach(x => {

    //   patientnumberusage.push(x)

    // });


    sitesarray.getRawValue().forEach(x => {

      categorysite.push(x)

    });
    let newobject = new Object();


    newobject = {
      patientnumberusage,
      categorysite
    }

    this._APIwithActionService.postAPI(newobject, 'Forecsatinfo', 'saveforecastcategoryinfo').subscribe((data) => {
      if (data["_body"] != 0) {

        //this._GlobalAPIService.SuccessMessage("Aggregrate Forecast saved successfully");
      }
      // if (this.Delsiteids != "0") {
      //   this._APIwithActionService.deleteData(this.Delsiteids, 'Forecsatinfo', 'delforecastcategorysiteinfo').subscribe((data) => {

          

      //   })
      // }
      // if (this.Delidpatientno != "0") {
      //   this._APIwithActionService.deleteData(this.Delidpatientno, 'Forecsatinfo', 'delforecastcategoryinfo').subscribe((data) => {
      //     this.Delsiteids = "0";
      //     this.Delidpatientno = "0";
      //   })
      // }
      // // this._router.navigate(["Demographic/PatientGroupRatio", this.programid, this.forecastid]);
      // this.nextStep.emit('step2,N,' + this.forecastid);
    })
  }
}
