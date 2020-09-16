import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { APIwithActionService } from '../../shared/APIwithAction.service';
import { GlobalAPIService } from '../../shared/GlobalAPI.service';
import {NotificationService} from '../../shared/utils/notification.service';
@Component({
  selector: 'app-forecast-factor',
  templateUrl: './forecast-factor.component.html',
  styleUrls: ['./forecast-factor.component.css']
})
export class ForecastFactorComponent implements OnInit {
  forecastfactor: FormGroup
  forecastid: number=0;
  data:string
  constructor(private notificationService:NotificationService,private _fb: FormBuilder, private _avRoute: ActivatedRoute,
    private _router: Router, private _APIwithActionService: APIwithActionService,
    private _GlobalAPIService: GlobalAPIService) {


    if (this._avRoute.snapshot.params["id"]) {
      this.forecastid = this._avRoute.snapshot.params["id"];


    }
    if(this.forecastid>0)
    {
      this._APIwithActionService.getDatabyID(this.forecastid,'Forecsatinfo','GetbyId').subscribe((resp) => {                  
        this.forecastfactor.patchValue({
        
          WastageRate: resp["westage"],
          Scaleup: resp["scaleup"],
       
          });
         
      })
    }
  }

  ngOnInit() {

    this.forecastfactor = this._fb.group({
      WastageRate: 0,
      Scaleup: 0
    })
  }
  Previousclick() {
    this.notificationService.smallBox({
      title: "Conformation!",
      content: "Would like to Cancel for this page? <p class='text-align-right'><a href='/#/Forecast/Importforecastdata/" + this.forecastid + "' class='btn btn-primary btn-sm'>Yes</a> <a href-void class='btn btn-danger btn-sm'>No</a></p>",
      color: "#296191",
      //timeout: 8000,
      icon: "fa fa-bell swing animated"
    });
   // this._router.navigate(["/Forecast/Importforecastdata", this.forecastid]);
  }
  save() {
    this.data=this.forecastfactor.controls["WastageRate"].value +","+this.forecastfactor.controls["Scaleup"].value;
    this._APIwithActionService.putAPI( this.forecastid ,this.data,"Forecsatinfo","Put02").subscribe((data1)=>{
      this._router.navigate(["/Forecast/forecastcalculationmethod", this.forecastid]);
      
    })

  }
}
