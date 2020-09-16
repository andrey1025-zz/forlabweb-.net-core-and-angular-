import { Component, OnInit, Renderer ,TemplateRef} from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import * as Highcharts from 'highcharts';
import { APIwithActionService } from "../shared/APIwithAction.service"
import { GlobalAPIService } from "../shared/GlobalAPI.service";

import {UploadAdapter} from './UploadAdapter'; 


@Component({
  selector: 'app-cmspage',
  templateUrl: './cmspage.component.html'
})

export class cmspageComponent implements OnInit {
  cmspage: FormGroup

  public ckconfig:any;
  constructor(private _fb: FormBuilder, private _rd: Renderer, private _GlobalAPIService: GlobalAPIService, private _avRoute: ActivatedRoute, private _APIwithActionService: APIwithActionService) { }

  ngOnInit() {

    // $script("https://cdn.ckeditor.com/4.5.11/full/ckeditor.js", ()=> {
    //   const CKEDITOR = window['CKEDITOR'];

    //   CKEDITOR.replace('ckeditorshowcase');
    // });
    this.cmspage = this._fb.group({
      homeheader: [''],
      homefunctionality:[''],
      homebenefits:[''],
      aboutusheader:[''],
      faq:[''],
      resource:['']
    })
    this._GlobalAPIService.getDataList('CMS').subscribe((data)=>{
      this.cmspage.patchValue({
        homeheader: data['homeheader'],
        homefunctionality: data['homefunctionality'],
        homebenefits: data['homebenefits'],
        aboutusheader:data['aboutusheader'],
        faq:data['faq'],
        resource:data['reource']
      })
  
     
    })
    this.ckconfig = {
      // include any other configuration you want
      extraPlugins: [ this.TheUploadAdapterPlugin ]
    };
  }
  save(){

    this._GlobalAPIService.putAPI(1,this.cmspage.value,"CMS").subscribe((data)=>{

    })

  }
  TheUploadAdapterPlugin(editor) {
   
    editor.plugins.get('FileRepository').createUploadAdapter = (loader) => {
      return new UploadAdapter(loader, 'assets/img');
    };
  }
}