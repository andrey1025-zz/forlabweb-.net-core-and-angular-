

import { Component, OnInit, ChangeDetectorRef, ViewChild, EventEmitter } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormArray, FormControl } from '@angular/forms';
import { APIwithActionService } from 'app/shared/APIwithAction.service';
import { BsModalRef } from "ngx-bootstrap";
import { GlobalAPIService } from '../../shared/GlobalAPI.service';
@Component({
  selector: 'app-country-add',
  templateUrl: './country-add.component.html',
  styleUrls: ['./country-add.component.css']
})
export class CountryAddComponent implements OnInit {
  GlobalList = new Array();
  CountryForm: FormGroup;
  submitBtnName = "Add Country";
  currentTestArea = 1;
  Countryhistoricaldatalist = new Array();
  diseaseList = new Array();
  Historicaldata = new Array();
  YearList = new Array();
  itemID1: any;
  errorMessage: any;
  public event: EventEmitter<any> = new EventEmitter();
  constructor(private _GlobalAPIService: GlobalAPIService, public bsModalRef: BsModalRef, private _fb: FormBuilder, private _APIwithActionService: APIwithActionService) {
    this._APIwithActionService.getDataList('User', 'Getallglobalregions').subscribe((data) => {
      this.GlobalList = data
    });


    this._APIwithActionService.getDataList('Country', 'GetMastDiseaseslist').subscribe((data) => {
      this.diseaseList = data
    });
  }

  ngOnInit() {
    this.CountryForm = this._fb.group({
      ID: 0,
      Name: ['', Validators.compose([Validators.required, Validators.maxLength(150)])],
      RegionId: ['', [Validators.required]],

      Period: ['0', [Validators.required]],

    });
    if (this.itemID1 > 0) {
      this.submitBtnName = "Update Country";
      this._APIwithActionService.getDatabyID(this.itemID1, 'Country', 'getcountrydatabyid').subscribe((resp) => {
       console.log(resp);
        this.CountryForm.setValue({
          ID: resp["id"],
          Name: resp["name"],
          RegionId: resp["regionid"],
          Period: resp["period"]==null?'0':resp["period"]
        });
        this.currentTestArea = resp["regionid"]
        this.Historicaldata=resp["CountryDiseasedetail"];
      }, error => this.errorMessage = error);
    }
  }


  // inithistoricaldata() {
  //   let historicaldata: FormGroup = this._fb.group({
  //     id: 0,
  //     Year: '',
  //     Countryid:0,
  //     Diseaseid:0,
  //     Disease: '',
  //     Population: 0,
  //     Incidence: 0,
  //     Incidence1000population: 0,
  //     Incidence1000kpopulation: 0,
  //     Prevalencerate: 0,
  //     Prevalencerate1000: 0,
  //     Prevalencerate1000k: 0,
  //   });
  //   return historicaldata;
  // }
  // addproductprice() {
  //   (<FormArray>this.CountryForm.controls["_histroicalpatientdata"]).push(
  //     this.inithistoricaldata()
  //   );
  // }

  // addhistoricaldata()
  // {
  //   let Year=   this.CountryForm.controls["Year"].value
  //   let Diseaseid=  parseFloat( this.CountryForm.controls["Diseaseid"].value)
  //   let Population=  this.CountryForm.controls["Population"].value
  //   let Incidence=  this.CountryForm.controls["Incidence"].value
  //   let Prevalencerate=  this.CountryForm.controls["Prevalencerate"].value
  //      if (Year != "" && Diseaseid != 0 && Population !=0) {
  //          let index = this.Countryhistoricaldatalist.indexOf(this.Countryhistoricaldatalist.find(x => x.year === Year && x.diseaseid === Diseaseid))
  //          if (index >= 0) {
  //          this._GlobalAPIService.FailureMessage('This Diseases already added for '+Year);
  //          }
  //          else {
  //              this.Countryhistoricaldatalist.push({

  //                  id: 0,
  //                  Year: Year,
  //                  Countryid:0,
  //                  Diseaseid:Diseaseid,

  //                  Disease: this.diseaseList.find(x=>x.id === Diseaseid)["Name"],
  //                  Population: Population,
  //                  Incidence: Incidence,
  //                  Incidence1000population: this.CountryForm.controls["Incidence1000population"].value,
  //                  Incidence1000kpopulation: this.CountryForm.controls["Incidence1000kpopulation"].value,
  //                  Prevalencerate: Prevalencerate,
  //                  Prevalencerate1000: this.CountryForm.controls["Prevalencerate1000"].value,
  //                  Prevalencerate1000k: this.CountryForm.controls["Prevalencerate1000kss"].value,

  //              })
  //              let boxIndex = this.Countryhistoricaldatalist.length - 1;
  //              this.addhistoricaldata();
  //              (<FormGroup>(
  //                  (<FormArray>this.CountryForm.controls["_histroicalpatientdata"]).controls[
  //                  boxIndex
  //                  ]
  //              )).patchValue({


  //               id:this.Countryhistoricaldatalist[boxIndex].id,
  //               Year:this.Countryhistoricaldatalist[boxIndex].Year,
  //               Countryid:this.Countryhistoricaldatalist[boxIndex].Countryid,
  //               Diseaseid:this.Countryhistoricaldatalist[boxIndex].Diseaseid,
  //               Disease: this.Countryhistoricaldatalist[boxIndex].Disease,
  //               Population: this.Countryhistoricaldatalist[boxIndex].Population,
  //               Incidence: this.Countryhistoricaldatalist[boxIndex].Incidence,
  //               Incidence1000population: this.Countryhistoricaldatalist[boxIndex].Incidence1000population,
  //               Incidence1000kpopulation: this.Countryhistoricaldatalist[boxIndex].Incidence1000kpopulation,
  //               Prevalencerate: this.Countryhistoricaldatalist[boxIndex].Prevalencerate,
  //               Prevalencerate1000: this.Countryhistoricaldatalist[boxIndex].Prevalencerate1000,
  //               Prevalencerate1000k: this.Countryhistoricaldatalist[boxIndex].Prevalencerate1000k,


  //              });
  //          }
  //      }
  //      this.CountryForm.patchValue({
  //          Price: '',
  //          PackSize:'',
  //          AsOfDate:null  
  //      })
  // }
  handleSelectTestArea(ID) {
    this.CountryForm.patchValue({
      RegionId: ID
    })
    this.currentTestArea = ID;
  }
  save() {
   // this._GlobalAPIService.FailureMessage("Please select Period");
    if(this.CountryForm.controls["Period"].value=="0")
    {
      this._GlobalAPIService.FailureMessage("Please select Period");
      return;
    }
    this._APIwithActionService.postAPI(this.CountryForm.value, 'Country','Savecountry').subscribe((data) => {
      if (data["_body"] != "0") {
          this._GlobalAPIService.SuccessMessage("Country Saved Successfully");
          this.event.emit(this.CountryForm.value);
          this.bsModalRef.hide();
      } else {
          this._GlobalAPIService.FailureMessage("Something Went wrong");
      }
  }, error => this.errorMessage = error)

  }
  onCloseModal() {
    this.bsModalRef.hide();
  }
}
