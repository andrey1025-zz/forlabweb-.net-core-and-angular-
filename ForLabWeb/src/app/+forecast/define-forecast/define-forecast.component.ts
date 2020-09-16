import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { APIwithActionService } from '../../shared/APIwithAction.service';
import { GlobalAPIService } from '../../shared/GlobalAPI.service';
import { formatDate } from 'ngx-bootstrap/chronos';
@Component({
  selector: 'app-define-forecast',
  templateUrl: './define-forecast.component.html',
  styleUrls: ['./define-forecast.component.css']
})
export class DefineForecastComponent implements OnInit {
  forecastid: number=0;
  defineforecast:FormGroup;
  Foracstinfoobj: Object;
  date:Date;
  minDate: Date;
   Id:number;
   displayaattr: string = "none";
   validtxtscope: boolean = true;
  constructor(private _fb: FormBuilder,private _avRoute:ActivatedRoute,
    private _router: Router,private _APIwithActionService: APIwithActionService,
    private _GlobalAPIService:GlobalAPIService) {
      if (this._avRoute.snapshot.params["id"]) {  
        this.forecastid = this._avRoute.snapshot.params["id"];  
       
    
    }  

    if (this.forecastid > 0) {  
         


      this._APIwithActionService.getDatabyID(this.forecastid,'Forecsatinfo','GetbyId').subscribe((resp) => {                  
            console.log(resp)
              this.defineforecast.patchValue({
        
                ForecastID: resp["forecastNo"],
                forecastdate: new Date(resp["forecastDate"]),
                startdate: new Date(resp["startDate"]),
                Period:resp["period"],
                scopeofforecast: resp["scopeOfTheForecast"] == "NATIONAL" || resp["scopeOfTheForecast"] == "GLOBAL" ? resp["scopeOfTheForecast"] : 'CUSTOM'
                });
                if (resp["scopeOfTheForecast"] == "NATIONAL" || resp["scopeOfTheForecast"] == "GLOBAL") {
                  this.displayaattr = "none";
                }
                else {
                  this.displayaattr = "block";
                  this.defineforecast.patchValue({
                    txtscope: resp["scopeOfTheForecast"]
                  })
                }
              })
    }   
     }
     txtscopevalidator(group: FormGroup) {
      if (group.value.scopeofforecast == 'CUSTOM' && group.value.txtscope == "") {
        this.validtxtscope = false
      }
      else {
        this.validtxtscope = true
      }
    }
    displaytextscope(args) {
      if (args.target.value == "CUSTOM") {
        this.displayaattr = "block";
      }
      else {
        this.displayaattr = "none";
        this.defineforecast.patchValue({
          txtscope: ''
        })
      }
    }
    onValueChange(args:Date)
    {

     // let dateString = '1968-11-16T00:00:00' 
let newDate = new Date(args);
      console.log(newDate);
      this.minDate =args
      this.minDate.setDate(this.minDate.getDate() +1);


      this.defineforecast.get('forecastdate').enable();
     // this.minDate = formatDate(this.defineforecast.controls['startdate'].value,"DD/MMM/YYYY");
    }
  ngOnInit() {

    this.defineforecast = this._fb.group({
      ForecastID: ['',Validators.compose([Validators.required,Validators.maxLength(32)])],
      forecastdate:[{value:null, disabled:true}, [Validators.required]],
      startdate: [null, [Validators.required]],
      txtscope:[''],
      Period: 'Monthly',
      scopeofforecast: ['', [Validators.required]] }, {
        validator: this.txtscopevalidator.bind(this),

      })
                 
 
  }
  save() {
    this.date =  new Date(); 
    
      let forecasttype = ""
         
           this.Foracstinfoobj = {
             ForecastID:this.forecastid,
             ForecastNo:  this.defineforecast.controls['ForecastID'].value,
             Methodology:"",
             DataUsage: "",
             scopeOfTheForecast:this.defineforecast.controls['scopeofforecast'].value,
           
             Status: "OPEN",
             StartDate: formatDate(this.defineforecast.controls['startdate'].value,"DD/MMM/YYYY"),
             Period: this.defineforecast.controls['Period'].value,
             ForecastDate:formatDate(this.defineforecast.controls['forecastdate'].value,"DD/MMM/YYYY"),
           
             SlowMovingPeriod: this.defineforecast.controls['Period'].value,
             ForecastType: "S",
             Method: "",
             Extension: 4,
             LastUpdated: this.date ,
             Countryid: localStorage.getItem("countryid")
           }
        
           console.log(this.Foracstinfoobj)
           this._APIwithActionService.postAPI(this.Foracstinfoobj,'Forecsatinfo','saveforecastinfo')               
           .subscribe((data) => {  
               if (data["_body"] !=0)
               {
              
                 this.Id=  data["_body"]   
                
                                                     
                   this._GlobalAPIService.SuccessMessage("Forecast Info Saved Successfully");
                 
                  
                     this._router.navigate(["/Forecast/ForecastTest", this.Id]);
             
               }
               else
               {
                   this._GlobalAPIService.FailureMessage("Duplicate ForecastID");
                   
               
               }
               
             
                })
   
   
         }
}
