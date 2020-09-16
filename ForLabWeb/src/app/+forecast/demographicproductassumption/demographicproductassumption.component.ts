import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { APIwithActionService } from '../../shared/APIwithAction.service';
import { GlobalAPIService } from '../../shared/GlobalAPI.service';
import { ModalDirective } from "ngx-bootstrap";

@Component({
  selector: 'app-demographicproductassumption',
  templateUrl: './demographicproductassumption.component.html',
  styleUrls: ['./demographicproductassumption.component.css']
})
export class DemographicproductassumptionComponent implements OnInit {
  @ViewChild('lgModal4') public lgModal4: ModalDirective;
  forecastid: number = 0;
  programid: string = "0";
  Assumptionarray = new Array();
  updatedassumption = new Array();
  selectedassumption= new Array();
  model: any;
  constructor(private _avRoute: ActivatedRoute,
    private _router: Router, private _APIwithActionService: APIwithActionService,
    private _GlobalAPIService: GlobalAPIService) {


    if (this._avRoute.snapshot.params["id"]) {
      this.forecastid = this._avRoute.snapshot.params["id"];


    }
    if (this._avRoute.snapshot.params["pid"]) {
      this.programid = this._avRoute.snapshot.params["pid"];


    }
    this.model = {
      variableName: '',
      effect:0
    }
    this.getassumption();
  }

  ngOnInit() {
  }
  addgeneralassumption()
  {
    let index ;
    if (this.model.variableName == "") {
      this._GlobalAPIService.FailureMessage("Please Fill variable name")
      return;
    }
    if (this.model.variableName != "") {
      index = this.updatedassumption.findIndex(x => x.variableName === this.model.variableName.replace(/\s/g, ""))
      if (index >= 0) {
        this._GlobalAPIService.FailureMessage("Duplicate variable name")
        this.model.variableName="";
        return
      }
    }
    if (this.model.effect ==0) {
      this._GlobalAPIService.FailureMessage("Please Select effect of variable ")
      return
    }
    let effect = "INCREASES";
let effectid=true;
    if (this.model.effect ==2) {
      effect = "DECREASES";
      effectid=false;
      // this._GlobalAPIService.FailureMessage("Please Select effect of variable ")
      // return
    }
    else
        {
          effect = "INCREASES";
          effectid=true;
        }

    this.updatedassumption.push({
      variableName: this.model.variableName.replace(/\s/g, ""),
      variableDataType: 1,
      useOn: "OnAllSite",
     // variableFormula: element.variableFormula,
      programId: this.programid,
      varCode: "xx",
      assumptionType: 2,
      variableEffect: effect,
      isActive:true,
      entity_type_id: 4,
      userId: localStorage.getItem("userid"),
      forecastid: this.forecastid
    })
    this.selectedassumption.push({
      variableName: this.model.variableName.replace(/\s/g, ""),
      variableDataType: 1,
      useOn: "OnAllSite",
      variableFormula: "",
      programId: this.programid,
      varCode: "xx",
      assumptionType: 2,
      variableEffect: effectid,
      isActive:true,
      entity_type_id: 4,
      userId: localStorage.getItem("userid"),
      forecastid: this.forecastid
    })
    this.model.variableName="";
    this.model.effect =0;
this.lgModal4.hide();
  }
  getassumption() {
    let effect = "INCREASES";
    this._APIwithActionService.getDatabyID(this.forecastid, 'MMProgram', 'GetDemographicMMGeneralAssumptions', "param=" + this.programid + ",2").subscribe((resp) => {
      this.Assumptionarray = resp;
      this.Assumptionarray.forEach(element => {
        if (element.variableEffect == false) {
          effect = "DECREASES";
        }
        else
        {
          effect = "INCREASES";
        }
        this.updatedassumption.push({
          variableName: element.variableName,
          variableDataType: element.variableDataType,
          useOn: element.useOn,
          variableFormula: element.variableFormula,
          programId: element.programId,
          varCode: element.varCode,
          assumptionType: element.assumptionType,
          variableEffect: effect,
          isActive: element.isActive,
          entity_type_id: element.entity_type_id,
          userId: element.userId,
          forecastid: element.forecastid
        })


        this.selectedassumption.push({
          variableName: element.variableName,
          variableDataType: element.variableDataType,
          useOn: element.useOn,
          variableFormula: element.variableFormula,
          programId: element.programId,
          varCode: element.varCode,
          assumptionType: element.assumptionType,
          variableEffect: element.variableEffect,
          isActive: element.isActive,
          entity_type_id: element.entity_type_id,
          userId: element.userId,
          forecastid: element.forecastid
        })
      });
      // if (this.programid != 0) {
      //     this.patientgroup = this.patientgrouplist.filter(x => x.programId === this.programid)
      // }
    })
  }
  onselectassumption(data: any, isChecked: boolean)
  {
    if (isChecked) {
      let effectid=true;
      if (data.variableEffect == "DECREASES") {
      
        effectid=false;
        // this._GlobalAPIService.FailureMessage("Please Select effect of variable ")
        // return
      }
      // index = selectedsiteList.length;
      let index = this.selectedassumption.findIndex(x => x.variableName === data.variableName)
      if (index < 0) {
        data.forecastid = this.forecastid

        this.selectedassumption.push({
          variableName: data.variableName.replace(/\s/g, ""),
          variableDataType: 1,
          useOn: "OnAllSite",
        variableFormula: "",
          programId: this.programid,
          varCode: "xx",
          assumptionType: 2,
          variableEffect: effectid,
          isActive:true,
          entity_type_id: 4,
          userId: localStorage.getItem("userid"),
          forecastid: this.forecastid
        })
        //this.selectedassumption.push(data)
      }
    }
    else {
      let index = this.selectedassumption.findIndex(x => x.variableName === data.variableName)

      if (index >= 0) {
        this.selectedassumption[index].isActive=false
       // this.selectedassumption.splice(index, 1)
      }
    }
  }
  save()
  {

    console.log( this.selectedassumption);
    this._APIwithActionService.postAPI(this.selectedassumption, "MMProgram", "saveDemographicMMGeneralAssumptions").subscribe((data) => {
     
      this._router.navigate(["/Forecast/Demographitraetmentcyscle", this.forecastid,this.programid]);  
      
    })
  }

}
