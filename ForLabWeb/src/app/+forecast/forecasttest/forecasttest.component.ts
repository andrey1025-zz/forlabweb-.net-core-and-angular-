import { Component, OnInit } from '@angular/core';
import { APIwithActionService } from '../../shared/APIwithAction.service';
import { FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { GlobalAPIService } from 'app/shared/GlobalAPI.service';
import { DomSanitizer } from '@angular/platform-browser';
import {NotificationService} from '../../shared/utils/notification.service';
@Component({
  selector: 'app-forecasttest',
  templateUrl: './forecasttest.component.html',
  styleUrls: ['./forecasttest.component.css']
})
export class ForecasttestComponent implements OnInit {
  testAreaList = new Array();
  forecastid: number;
  selectedtest = new Array();
  Html: string = "";
  public divhtml: any;
  constructor(private notificationService:NotificationService, private _fb: FormBuilder, private _avRoute: ActivatedRoute,
    private _router: Router, private _APIwithActionService: APIwithActionService,
    private _GlobalAPIService: GlobalAPIService, private domSanitizer: DomSanitizer) {


    if (this._avRoute.snapshot.params["id"]) {
      this.forecastid = this._avRoute.snapshot.params["id"];


    }

  }
  previousclick()
  {
 

      this.notificationService.smallBox({
        title: "Conformation!",
        content: "Would like to Cancel for this page? <p class='text-align-right'><a href='/#/Forecast/Defineforecast/" + this.forecastid + "' class='btn btn-primary btn-sm'>Yes</a> <a href-void class='btn btn-danger btn-sm'>No</a></p>",
        color: "#296191",
        //timeout: 8000,
        icon: "fa fa-bell swing animated"
      });
      // [routerLink]="['/Forecast/Defineforecast',forecastid]"
  
  }
 
  ngOnInit() {

    let i: number = 0;
    this._APIwithActionService.getDatabyID(this.forecastid, "Test", "getAlltestbytestingarea").subscribe((data) => {
      this.testAreaList = data;
      // for (let index = 0; index < this.testAreaList.length; index++) {
      //   if (index == 0) {
      //     this.Html += '<div class="row"><div class="col-md-4" >' +
      //     '<h3 style="color: gray;"><b>' + this.testAreaList[index].testareaname + ' </b></h3>';
      //   for (let index1 = 0; index1 < this.testAreaList[index].tests.length; index1++) {

      //     this.Html += '<div> <label><input type="checkbox" class="checkbox style-3" [checked]=' + this.testAreaList[index].tests[index1].type + '=="A"' +
      //       '(change)="onChange(' + this.testAreaList[index].tests[index1].testID + ', $event.target.checked)"   /><span>' + this.testAreaList[index].tests[index1].testName + '</span>' +
      //       '<br></label> </div>';
      //   }
      //   this.Html += '</div>';
      //   }
      //   else {
      //     if (index% 2 == 0) {
      //       this.Html += '<div class="row" style="margin-Left:5%">'
      //     }
      //     this.Html += ' <div class="col-md-4" >' +
      //       '<h3 style="color: gray;"><b>' + this.testAreaList[index].testareaname + ' </b></h3>';
      //     for (let index1 = 0; index1 < this.testAreaList[index].tests.length; index1++) {

      //       this.Html += '<div> <label><input type="checkbox" class="checkbox style-3" [checked]=' + this.testAreaList[index].tests[index1].type + '=="A"' +
      //         '(change)="onChange(' + this.testAreaList[index].tests[index1].testID + ', $event.target.checked)"   /><span>' + this.testAreaList[index].tests[index1].testName + '</span>' +
      //         '<br></label> </div>';

      //     }
      //     this.Html += '</div>';
        
      //   }
      //   if (index % 2 == 0 && index !=0) {
      //     this.Html += '</div>'
      //   }
      //   i++;
      // }
      // this.divhtml = this.domSanitizer.bypassSecurityTrustHtml(this.Html)
      // console.log(this.Html)
    })



  }
  onChange(testid, ischecked) {

    let index: number
    let regionids: string = "";
    if (ischecked) {

      this.selectedtest.push({
        testid: testid,
        forecastID: this.forecastid,
        userId: localStorage.getItem("userid")
      })


    } else {
      let index = this.selectedtest.findIndex(x => x.testid == testid)
      this.selectedtest.splice(index);

    }

  }
  Saveforecasttest() {
    console.log(this.selectedtest);
    this._APIwithActionService.postAPI(this.selectedtest, "Test", "Saveforecasttest").subscribe((data) => {
      this._router.navigate(["/Forecast/ForecastIns", this.forecastid]);
    });
  }
}
