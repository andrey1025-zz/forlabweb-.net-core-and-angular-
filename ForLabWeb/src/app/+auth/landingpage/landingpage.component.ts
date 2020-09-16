import { Component, OnInit,ViewEncapsulation } from '@angular/core';
import * as Highcharts from 'highcharts';
import { GlobalAPIService } from "../../shared/GlobalAPI.service";
import { Pipe, PipeTransform } from '@angular/core';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { Transform } from 'stream';
@Component({
  selector: 'app-landingpage',
  templateUrl: './landingpage.component.html',
  styleUrls: ['./landingpage.component.css'],
  encapsulation: ViewEncapsulation.None
})
@Pipe({ name: 'noSanitize' })
export class LandingpageComponent implements OnInit {
  public data = {
     "Nigeria":3,"Uganda":1,"Cameroon":1,"Ethiopia":3,"Congo, the Democratic Republic of the":3,"Kenya":1,"Tanzania, United Republic of":1,"Myanmar":1,"Viet Nam":1
, "Zimbabwe":3,"Mozambique":3,"Lesotho":1,"South Africa":1,"Botswana":1,"Côte d'Ivoire":3
  };
 public headertext:any;
 public homefunctionality:any
 public homebenefits:any;
 public aboutusheadertext:any;
 public faq:any;

 public resource:any;
  constructor(private _GlobalAPIService:GlobalAPIService,private domSanitizer:DomSanitizer) { 

    this._GlobalAPIService.getDataList('CMS').subscribe((data)=>{
      this.headertext= data['homeheader']
      this.aboutusheadertext=data['aboutusheader']
      this.homefunctionality=this.domSanitizer.bypassSecurityTrustHtml(data['homefunctionality'])
   this.homebenefits=this.domSanitizer.bypassSecurityTrustHtml(data['homebenefits'])
      this.faq=this.domSanitizer.bypassSecurityTrustHtml(data['faq'])
      this.resource=this.domSanitizer.bypassSecurityTrustHtml(data['resource'])
    })
  }
 
  ngOnInit() {



 
    
}

}
