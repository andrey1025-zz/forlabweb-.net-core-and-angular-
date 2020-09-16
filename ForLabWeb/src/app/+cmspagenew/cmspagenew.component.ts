import { Component, OnInit, Renderer, TemplateRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import * as Highcharts from 'highcharts';
import { APIwithActionService } from "../shared/APIwithAction.service"
import { GlobalAPIService } from "../shared/GlobalAPI.service";
import { HttpClient, HttpRequest, HttpEventType, HttpResponse, HttpHeaders } from '@angular/common/http'
@Component({
  selector: 'app-cmspagenew',
  templateUrl: './cmspagenew.component.html',
  styleUrls: ['./cmspagenew.component.css']
})
export class CmspagenewComponent implements OnInit {
  cmspagenew: FormGroup
  file: File;
  formData = new FormData();
  constructor(private http: HttpClient, private _fb: FormBuilder, private _rd: Renderer, private _GlobalAPIService: GlobalAPIService, private _avRoute: ActivatedRoute, private _APIwithActionService: APIwithActionService) { }

  ngOnInit() {
    this.cmspagenew = this._fb.group({

      hometitle: [''],
      Homedet: [''],
      featuretitle: [''],
      featureImgtitle1: [''],
      featureImgdetail1: [''],
      featureImgtitle2: [''],
      featureImgdetail2: [''],
      featureImgtitle3: [''],
      featureImgdetail3: [''],
      featureImgtitle4: [''],
      featureImgdetail4: [''],
      featureImgtitle5: [''],
      featureImgdetail5: [''],
      videotitle: [''],
      videourl1: [''],
      videotitle1: [''],
      videourl2: [''],
      videotitle2: [''],
      videourl3: [''],
      videotitle3: [''],
      videourl4: [''],
      videotitle4: [''],
      faqq1: [''],
      faqa1: [''],
      faqq2: [''],
      faqa2: [''],
      faqq3: [''],
      faqa3: [''],
      faqq4: [''],
      faqa4: [''],
      faqq5: [''],
      faqa5: [''],
      faqq6: [''],
      faqa6: [''],
      AT1: [''],
      ATS1: [''],
      AT2: [''],
      ATS2: [''],
      AT3: [''],
      ATS3: [''],
      Contemail: [''],
      Contmobile: [''],
      contactaddress: [''],
      aturl1: [''],
      aturl2: [''],
      aturl3: [''],
      filename3: [''],
      filename2: [''],
      filename1: [''],
      doctitle3: [''],
      doctitle2: [''],
      doctitle1: ['']
    })


    this._APIwithActionService.getDataList('CMS', 'Get').subscribe((data) => {
      console.log(data);
      this.cmspagenew.patchValue({
        hometitle: data['hometitle'],
        Homedet: data['homedet'],


        videourl1: data['videourl1'],
        videotitle1: data['videotitle1'],
        videourl2: data['videourl2'],
        videotitle2: data['videotitle2'],
        videourl3: data['videourl3'],
        videotitle3: data['videotitle3'],
        videourl4: data['videourl4'],
        videotitle4: data['videotitle4'],
        faqq1: data['faqq1'],
        faqa1: data['faqa1'],
        faqq2: data['faqq2'],
        faqa2: data['faqa2'],
        faqq3: data['faqq3'],
        faqa3: data['faqa3'],
        faqq4: data['faqq4'],
        faqa4: data['faqa4'],
        faqq5: data['faqq5'],
        faqa5: data['faqa5'],
        faqq6: data['faqq6'],
        faqa6: data['faqa6'],
        AT1: data['aT1'],
        ATS1: data['atS1'],
        AT2: data['aT2'],
        ATS2: data['atS2'],
        AT3: data['aT3'],
        ATS3: data['atS3'],
        Contemail: data['contemail'],
        Contmobile: data['contmobile'],
        contactaddress: data['contactaddress'],
        aturl1: data['aturl1'],
        aturl2: data['aturl2'],
        aturl3: data['aturl3']
      })


    })
  }
  save() {

    this._APIwithActionService.putAPI(1, this.cmspagenew.value, "CMS", "Put").subscribe((data) => {

    })

  }
  incomingfile(event, filenumber) {

   
    this.file = event.target.files[0];
    let filedata = new Object();
    console.log(this.file);
    if (filenumber == "1") {
      this.cmspagenew.patchValue({
        filename1: this.file.name
      })
      if (this.cmspagenew.controls["doctitle1"].value == "") {
        this._GlobalAPIService.FailureMessage("Please fill title first then upload file");
        return;
      }
    
    }
    this.formData.append(this.file.name, this.file);

    filedata = {
      title: this.cmspagenew.controls["doctitle1"].value,
      files: this.formData
    }
    console.log(filedata);
    var token = localStorage.getItem("jwt");
    const uploadReq = new HttpRequest('PUT', `http://localhost:53234/api/Cloud/PUT/` + this.cmspagenew.controls["doctitle1"].value, this.formData, {
      headers: new HttpHeaders({ "Authorization": "Bearer " + token, 'userid': localStorage.getItem("userid"), 'countryid': localStorage.getItem("countryid") }),
      reportProgress: true,
    });

    this.http.request(uploadReq).subscribe(event => {
      // if (this.stringarr == 'S') {
      //   this._router.navigate(["/Forecast/Demographicsitebysite", this.forecastid]);
      // }
      // else {
      //   this._router.navigate(["/Forecast/DemographicAggregrate", this.forecastid]);
      // }
      // this._APIwithActionService.Getpatientnumber.emit(event["body"])

      // this.lgModal3.hide();
    })
  }
}
