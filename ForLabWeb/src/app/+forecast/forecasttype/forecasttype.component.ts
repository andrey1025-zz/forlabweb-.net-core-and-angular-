import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { APIwithActionService } from '../../shared/APIwithAction.service';
import { GlobalAPIService } from '../../shared/GlobalAPI.service';
import {NotificationService} from '../../shared/utils/notification.service';
@Component({
  selector: 'app-forecasttype',
  templateUrl: './forecasttype.component.html',
  styleUrls: ['./forecasttype.component.css']
})
export class ForecasttypeComponent implements OnInit {

  forecastid:number;
  methodology:string;
  programid:number;
  exist:number=0;
  isHighlight1:boolean=false;
  isHighlight2:boolean=false;
  
  constructor(private notificationService:NotificationService, private _avRoute: ActivatedRoute,
    private _router: Router, private _APIwithActionService: APIwithActionService,
    private _GlobalAPIService: GlobalAPIService) {
      if (this._avRoute.snapshot.params["id"]) {  
        this.forecastid = this._avRoute.snapshot.params["id"];  
       
    
    }  
    this._APIwithActionService.getDatabyID(this.forecastid,'Forecsatinfo','GetbyId').subscribe((resp) => {                  
      console.log(resp)
    
this.methodology=resp["methodology"];
this.programid=resp["programId"];


switch ( resp["forecastType"]) {
  case "S":
    this.isHighlight1 = true
    this.isHighlight2 = false
   
    break;
  case "C":
    this.isHighlight1 = false
    this.isHighlight2 = true
  
  
    break;
 
}
    })


this._APIwithActionService.getDatabyID(this.forecastid,'Forecsatinfo','Isdataimported').subscribe((resp) => {                  
  this.exist=resp
  // console.log(resp)

   })
     }

  ngOnInit() {
  }
  updateforecast(type: string) {

   
    this._APIwithActionService.putAPI( this.forecastid,type,"Forecsatinfo","Put01").subscribe((data)=>{
     
      this._router.navigate(["/Forecast/Importforecastdata", this.forecastid]);
      
    })
    
  }
  Previousclick()
  {
    switch (this.methodology) {
      case "CONSUMPTION":
        this.notificationService.smallBox({
          title: "Conformation!",
          content: "Would like to Cancel for this page? <p class='text-align-right'><a href='/#/Forecast/Forecastmethodology/" + this.forecastid + "' class='btn btn-primary btn-sm'>Yes</a> <a href-void class='btn btn-danger btn-sm'>No</a></p>",
          color: "#296191",
          //timeout: 8000,
          icon: "fa fa-bell swing animated"
        });


        this._router.navigate(["/Forecast/Forecastmethodology", this.forecastid]);
        break;
        case "SERVICE STATSTICS":
          this.notificationService.smallBox({
            title: "Conformation!",
            content: "Would like to Cancel for this page? <p class='text-align-right'><a href='/#/Forecast/Forecastmethodology/" + this.forecastid + "' class='btn btn-primary btn-sm'>Yes</a> <a href-void class='btn btn-danger btn-sm'>No</a></p>",
            color: "#296191",
            //timeout: 8000,
            icon: "fa fa-bell swing animated"
          });
          break;
          case "MORBIDITY":
            this.notificationService.smallBox({
              title: "Conformation!",
              content: "Would like to Cancel for this page? <p class='text-align-right'><a href='/#/Forecast/Demographitraetmentcyscle/" + this.forecastid + "' class='btn btn-primary btn-sm'>Yes</a> <a href-void class='btn btn-danger btn-sm'>No</a></p>",
              color: "#296191",
              //timeout: 8000,
              icon: "fa fa-bell swing animated"
            });
          //  this._router.navigate(["/Forecast/Demographitraetmentcyscle", this.forecastid,this.programid]);
            break;
    }

    
  }
  Saveforecastproductusage()
  {
    this._router.navigate(["/Forecast/Importforecastdata", this.forecastid]);
  }
}
