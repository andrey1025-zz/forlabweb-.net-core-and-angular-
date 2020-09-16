import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, FormArray } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { APIwithActionService } from '../../shared/APIwithAction.service';
import { GlobalAPIService } from '../../shared/GlobalAPI.service';
import {NotificationService} from '../../shared/utils/notification.service';
@Component({
  selector: 'app-forecastselectedinstrument',
  templateUrl: './forecastselectedinstrument.component.html',
  styleUrls: ['./forecastselectedinstrument.component.css']
})
export class ForecastselectedinstrumentComponent implements OnInit {
  selectedins:FormGroup;
  forecastid:number=0;
  instrumentlist=new Array();
  _siteins = new Array();
  constructor(private notificationService:NotificationService,private _fb: FormBuilder,private _avRoute:ActivatedRoute,
    private _router: Router,private _APIwithActionService: APIwithActionService,
    private _GlobalAPIService:GlobalAPIService) {
      if (this._avRoute.snapshot.params["id"]) {  
        this.forecastid = this._avRoute.snapshot.params["id"];  
       
    
    }  

      this._APIwithActionService.getDatabyID(this.forecastid, 'Instrument', 'getAllforecastinstrumentlist').subscribe((data) => {
      
  
      this.instrumentlist=data

 
      for (let boxIndex = 0; boxIndex < this.instrumentlist.length; boxIndex++) {
        this.addsiteinstrument();
        (<FormGroup>(
            (<FormArray>this.selectedins.controls["_instrument"]).controls[
            boxIndex
            ]
        )).patchValue({
         
            instrumentID: this.instrumentlist[boxIndex].insID,
            instrumentName: this.instrumentlist[boxIndex].instrumentName,           
           areaName: this.instrumentlist[boxIndex].areaName,
            forecastid: this.instrumentlist[boxIndex].forecastID,
            quantity: this.instrumentlist[boxIndex].quantity,
            TestRunPercentage: this.instrumentlist[boxIndex].testRunPercentage,
            TestingAreaID: this.instrumentlist[boxIndex].testingAreaID

        });

    }
      
      
      })

     }

  ngOnInit() {


    this.selectedins = this._fb.group({
   
    
      _instrument: this._fb.array([])
  })

  }
  initsiteinstrument() {
    let siteinstrument: FormGroup = this._fb.group({
      
        instrumentID: 0,
        instrumentName: [{ value: '' }],
        TestingAreaID:0,
        areaName: [{ value: ''}],
        forecastid: 0,
        quantity: 0,
        TestRunPercentage: [{ value: 0 }],
    });
    return siteinstrument;
}
addsiteinstrument() {
    (<FormArray>this.selectedins.controls["_instrument"]).push(
        this.initsiteinstrument()
    );
}
Previousclick()
{
  this.notificationService.smallBox({
    title: "Conformation!",
    content: "Would like to Cancel for this page? <p class='text-align-right'><a href='/#/Forecast/ForecastIns/" + this.forecastid + "' class='btn btn-primary btn-sm'>Yes</a> <a href-void class='btn btn-danger btn-sm'>No</a></p>",
    color: "#296191",
    //timeout: 8000,
    icon: "fa fa-bell swing animated"
  });
 // this
 
}
Saveforecastinstrument()
{   
  let insobject= new Object();
  this._siteins=[];
  let _siteins1 = <FormArray>this.selectedins.controls["_instrument"]
  _siteins1.getRawValue().forEach(x => {

     this._siteins.push({
        InsID:x.instrumentID,
        forecastID:x.forecastid,
        UserId:parseInt(localStorage.getItem("userid")),
        Quantity:parseInt(x.quantity),
        TestRunPercentage:parseFloat(x.TestRunPercentage),
       
      })
  });

  let diffareaid = new Array();
  for (let index = 0; index < _siteins1.getRawValue().length; index++) {
      const element = _siteins1.getRawValue()[index];
      let j=diffareaid.findIndex(x => x.areaid === element.TestingAreaID)
      if ( j>= 0) {
          diffareaid[j].testrunpercentage = parseFloat(diffareaid[j].testrunpercentage) + parseFloat(element.TestRunPercentage)
         

      }
      else {
          diffareaid.push({
              areaid: element.TestingAreaID,
                        testrunpercentage:element.TestRunPercentage
          })
        //  this.totalrunpercentage = element.TestRunPercentage
      }

  }
  for (let index = 0; index < diffareaid.length; index++) {
      const element = diffareaid[index];
      if (element.testrunpercentage > 100 || element.testrunpercentage < 100) {
          this._GlobalAPIService.FailureMessage("Test Run Percentage should be equal 100 for same area")

          return;
      }
      
  }
 
  for (let index = 0; index < this._siteins.length; index++) {
   // const element = diffareaid[index];
    if (this._siteins[index].Quantity ==0) {
        this._GlobalAPIService.FailureMessage("Quantity Should be greater than zero");

        return;
    }
    
}


  
insobject={
  ForecastIns:this._siteins
}
console.log(insobject)
  this._APIwithActionService.postAPI( insobject,"Instrument","Updateforecastinstrument").subscribe((data)=>{
    this._router.navigate(["/Forecast/ForecastProductusage", this.forecastid]);
  });
}
}
