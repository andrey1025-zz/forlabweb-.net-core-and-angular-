import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { APIwithActionService } from '../../shared/APIwithAction.service';
import { GlobalAPIService } from '../../shared/GlobalAPI.service';
import {NotificationService} from '../../shared/utils/notification.service';
@Component({
  selector: 'app-forecastinstrument',
  templateUrl: './forecastinstrument.component.html',
  styleUrls: ['./forecastinstrument.component.css']
})
export class ForecastinstrumentComponent implements OnInit {
  testAreaList=new Array();
  forecastid:number;
  selectedIns= new Array();
  constructor(private notificationService:NotificationService,private _fb: FormBuilder,private _avRoute:ActivatedRoute,
    private _router: Router,private _APIwithActionService: APIwithActionService,
    private _GlobalAPIService:GlobalAPIService) {


      if (this._avRoute.snapshot.params["id"]) {  
        this.forecastid = this._avRoute.snapshot.params["id"];  
       
    
    }  

     }

  ngOnInit() {


    this._APIwithActionService.getDatabyID( this.forecastid ,"Forecsatinfo","getallinstrumentbyforecasttest").subscribe((data)=>{
      this.testAreaList=data;
  console.log(data)
     })
  }
  onChange(InsID ,ischecked)
  {
    

    if (ischecked) {

     this.selectedIns.push({
      InsID:InsID,
       forecastID: this.forecastid ,
       userId:localStorage.getItem("userid")
     })


    } else {
      let index = this.selectedIns.findIndex(x => x.InsID == InsID)
      this.selectedIns.splice(index);

    }

  }
  Saveforecasttest()
  {
    console.log(this.selectedIns);
this._APIwithActionService.postAPI(this.selectedIns,"Instrument","saveforecastIns").subscribe((data)=>{
  this._router.navigate(["/Forecast/ForecastSelectedIns", this.forecastid]);
});
  }
  Previousclick()
  {
    this.notificationService.smallBox({
      title: "Conformation!",
      content: "Would like to Cancel for this page? <p class='text-align-right'><a href='/#/Forecast/ForecastTest/" + this.forecastid + "' class='btn btn-primary btn-sm'>Yes</a> <a href-void class='btn btn-danger btn-sm'>No</a></p>",
      color: "#296191",
      //timeout: 8000,
      icon: "fa fa-bell swing animated"
    });
   // this._router.navigate(["/Forecast/ForecastTest", this.forecastid]);
  }
}
