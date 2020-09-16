import { Component, OnInit, Renderer, Output, EventEmitter } from '@angular/core';

import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { Site } from '../RegionAdd/Region.model'
import { GlobalAPIService } from '../../shared/GlobalAPI.service';
import { APIwithActionService } from '../../shared/APIwithAction.service';

import { NotificationService } from "../../shared/utils/notification.service";


@Component({
  selector: 'app-RegionAdd',
  templateUrl: './RegionAdd.component.html',
  styleUrls: ['./RegionAdd.component.css']
})

export class RegionAddComponent implements OnInit {
  @Output() close = new EventEmitter()
  public SiteList: Site[];
  buttonstatus = true;
  Selectedsiteid: number = 0;
  regionForm: FormGroup;
  title: string = "Create";
  id: number;
  errorMessage: any;
  countryList = new Array();

  countryId: any
  disbool: boolean


  constructor(private notificationService: NotificationService, private _fb: FormBuilder, private _avRoute: ActivatedRoute, private _APIwithActionService: APIwithActionService,
    private _GlobalAPIService: GlobalAPIService, private _router: Router, private _render: Renderer) {
    if (this._avRoute.snapshot.params["id"]) {
      this.id = this._avRoute.snapshot.params["id"];
    }


    if (localStorage.getItem("role") == "admin") {
      this.countryId = ''
      this.disbool = false
    }
    else {
      this.countryId = localStorage.getItem("countryid")
      this.disbool = true
    }
  }
  Opensiteadd() {
    this._router.navigate(["/Managedata/SiteAdd"]);
  }
  deletesite() {
    this.notificationService.smartMessageBox({
      title: "Deletion",
      content: "Do you want to delete " + this.id + " Site",
      buttons: '[No][Yes]'
    }, (ButtonPressed) => {
      if (ButtonPressed === "Yes") {



        let table = document.querySelector('table');
        this._APIwithActionService.deleteData(this.Selectedsiteid, 'Site', 'Del01').subscribe((data) => {
          if (data["_body"] != 0) {
            //  this._render.setElementStyle(table.rows[this.rowindex],'display','none')      
            this.notificationService.smallBox({
              title: "Deletion",
              content: "<i class='fa fa-clock-o'></i> <i>Site Deleted</i>",
              color: "#659265",
              iconSmall: "fa fa-check fa-2x fadeInRight animated",
              timeout: 4000
              // function:this.delete(SiteCategory)
            });

          }
          else {
            this.notificationService.smallBox({
              title: "Cancelation",
              content: "<i class='fa fa-clock-o'></i> <i>Site already used so you could not delete this Site</i>",
              color: "#C46A69",
              iconSmall: "fa fa-times fa-2x fadeInRight animated",
              timeout: 4000

            });

          }


        })



      }
      if (ButtonPressed === "No") {
        this.notificationService.smallBox({
          title: "Cancelation",
          content: "<i class='fa fa-clock-o'></i> <i>Deletion Cancelled</i>",
          color: "#C46A69",
          iconSmall: "fa fa-times fa-2x fadeInRight animated",
          timeout: 4000

        });
      }

    });
  }
  ngOnInit() {

    this.regionForm = this._fb.group({
      RegionID: 0,
      regionName: ['', Validators.compose([Validators.required, Validators.maxLength(64)])],
      countryId: [{ value: this.countryId, disabled: this.disbool }, [Validators.required]],

    })
    this._APIwithActionService.getDataList('Site', 'Getcountrylist').subscribe((data) => {

      this.countryId = data;

      console.log(this.countryId);
      console.log(this.countryId);
      let div = document.querySelector(".Sitelist")
      if (this.id > 0) {
        this.title = "Edit";

        this._render.setElementStyle(div, 'display', 'block')

        this._APIwithActionService.getDatabyID(this.id, 'Site', 'GetSitebyReg').subscribe((data) => {
          this.SiteList = data
          console.log(this.SiteList)
        }
        ), err => {
          console.log(err);
        }

        this._GlobalAPIService.getDatabyID(this.id, 'Region')
          .subscribe((resp) => {
            this.regionForm.setValue({
              RegionID: resp["regionID"],
              regionName: resp["regionName"],
              countryId: resp["countryId"]
              // formControlName2: myValue2 (can be omitted)
            });
            if (localStorage.getItem("role") != "admin") {
              this.regionForm.get('countryId').disable();
            }


          }

            , error => this.errorMessage = error);
      }
      else {

        this._render.setElementStyle(div, 'display', 'none')
      }
    })
    //  this.getSiteCategories();
  }
  editSite(siteid) {
    this.buttonstatus = false;
    this.Selectedsiteid = siteid;

  }
  editsitedetail() {
    if (this.Selectedsiteid != 0) {
      this._router.navigate(["/Managedata/SiteAdd", this.Selectedsiteid])
    }
  }
  save() {
    if (!this.regionForm.valid) {
      // this._GlobalAPIService.FailureMessage("Date is not valid");
      return;
    }
    if (this.title == "Create") {
      this._GlobalAPIService.postAPI(this.regionForm.getRawValue(), 'Region')
        .subscribe((data) => {
          if (data["_body"] == "Success") {

            this._GlobalAPIService.SuccessMessage("Region Saved Successfully");
            this._router.navigate(['/Managedata/ManagedataList', 5]);
          }
          else {
            this._GlobalAPIService.FailureMessage("Region already exists");
          }

          ///alert(data);
          //   
        }, error => this.errorMessage = error)
    }
    else if (this.title == "Edit") {
      this._GlobalAPIService.putAPI(this.id, this.regionForm.getRawValue(), 'Region')
        .subscribe((data) => {
          if (data["_body"] == "Success") {
            this._GlobalAPIService.SuccessMessage("Region Updated Successfully");
            this._router.navigate(['/Managedata/ManagedataList', 5]);
          }
          else {
            this._GlobalAPIService.FailureMessage("Region already exists");
          }
        }, error => this.errorMessage = error)
    }
  }

  clearctrl() {
    // this._router.navigate(['/Managedata/ManagedataList', 5]);
    this.close.emit(true);
  }


}  