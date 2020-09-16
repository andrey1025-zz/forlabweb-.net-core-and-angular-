
import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { APIwithActionService } from '../../shared/APIwithAction.service';
import { GlobalAPIService } from '../../shared/GlobalAPI.service';
import { ModalDirective } from "ngx-bootstrap";
@Component({
  selector: 'app-demographicprogramgroup',
  templateUrl: './demographicprogramgroup.component.html',
  styleUrls: ['./demographicprogramgroup.component.css']
})
export class DemographicprogramgroupComponent implements OnInit {
  patientgrouplist = new Array();
  patientgroup = new Array();
  selectedpatientgroup = new Array();
  forecastid: number = 0;
  programid: string = "0";
  model: any;
  @ViewChild('lgModal4') public lgModal4: ModalDirective;
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
      groupName: '',

    }
    this.getpatientgroup();
  }

  ngOnInit() {
  }
  getpatientgroup() {
    this._APIwithActionService.getDatabyID(this.forecastid, 'MMProgram', 'Getpatientgroupforforecast', "id=" + this.programid).subscribe((resp) => {
      this.patientgroup = resp;

      // if (this.programid != 0) {
      //     this.patientgroup = this.patientgrouplist.filter(x => x.programId === this.programid)
      // }
    })
  }
  addnewgroup() {
    let newgroup = new Object();
    let index ;
    if (this.model.groupName == "") {
      this._GlobalAPIService.FailureMessage("Please Fill group Name")
      return;
    }
    if (this.model.groupName != "") {
      index = this.patientgroup.findIndex(x => x.groupName === this.model.groupName)
      if (index >= 0) {
        this._GlobalAPIService.FailureMessage("Duplicate group Name")
        this.model.groupName="";
        return
      }
    }

    newgroup = {
      Id: 0,
      groupName: this.model.groupName,
      programId: parseInt(this.programid),
      forecastid: this.forecastid,
      isActive: true,
      type: ''

    }
    console.log(this.patientgroup)
    this.patientgroup.push(newgroup);
    this.selectedpatientgroup.push(newgroup)
    this.lgModal4.hide();
    // this._APIwithActionService.postAPI(newgroup, 'MMProgram', 'savegroup').subscribe((data) => {
    //     if (data["_body"] != 0) {
    //         // this._APIwithActionService.postAPI(this.suggestedvariable, "MMProgram", "savesuggustion").subscribe((data1) => {


    //         // })
    //         this._GlobalAPIService.SuccessMessage("Group saved successfully");

    //         this.getpatientgroup();

    //     }
    //     else {
    //         this._GlobalAPIService.FailureMessage("Group Name must not be Duplicate");
    //     }
    // })
  }
  onselectgroup(data: any, isChecked: boolean) {
    if (isChecked) {

      // index = selectedsiteList.length;
      let index = this.selectedpatientgroup.findIndex(x => x.groupName === data.groupName)
      if (index < 0) {
        data.forecastid = this.forecastid
        this.selectedpatientgroup.push(data)
      }
    }
    else {
      let index = this.selectedpatientgroup.findIndex(x => x.groupName === data.groupName)

      if (index >= 0) {
        this.selectedpatientgroup.splice(index, 1)
      }
    }
  }
  save() {
    console.log(this.selectedpatientgroup)
    this._APIwithActionService.postAPI(this.selectedpatientgroup, "MMProgram", "saveforecastmmgroup").subscribe((data) => {
      this._router.navigate(["/Forecast/Demographicpatientassumption", this.forecastid,this.programid]);  
    })

  }
}
