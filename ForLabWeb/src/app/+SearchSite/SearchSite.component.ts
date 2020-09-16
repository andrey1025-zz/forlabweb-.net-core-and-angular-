import { Component, OnInit, Renderer,Pipe,PipeTransform } from '@angular/core';
import { FadeInTop } from "../shared/animations/fade-in-top.decorator";
import { FormBuilder, FormGroup, FormControl, FormArray } from "@angular/forms";
import { ProductType } from "../shared/GlobalInterface";
import { GlobalAPIService } from "../shared/GlobalAPI.service";
import { APIwithActionService } from "../shared/APIwithAction.service";
import { Router, ActivatedRoute } from '@angular/router';  

@FadeInTop()
@Component({
  selector: 'sa-searchsite',
  templateUrl: './Searchsite.component.html',
})
export class SearchSiteComponent implements OnInit {

  buttonstatus = true;
  Searchproform: FormGroup;
  RegionList=new Array();
  SiteList: any[];
  div: any;
  Producttypeid: string = "";
  constructor(private _fb: FormBuilder,private _router:Router, private _GlobalAPIService: GlobalAPIService, private _APIwithActionService: APIwithActionService, private _render: Renderer) {

    this.Searchproform = this._fb.group({
        SerachSite: [''],
      SerachType: [''],
      Regions: this._fb.array([])
    });

    //this.getproducttype();

  }
  initForm() {
    let allCategories: FormArray = new FormArray([]);
    for (let i = 0; i < this.RegionList.length; i++) {
      let fg = new FormGroup({});
      fg.addControl(this.RegionList[i].typeName, new FormControl(false))
      allCategories.push(fg)
    }
  }
  getregions() {
    this._GlobalAPIService.getDataList('Region').subscribe((data) => {
      this.RegionList = data.aaData
      //const controls = this.ProductTypeList.map(c => new FormControl(false));
      //  controls[0].setValue(true); // Set the first checkbox to true (checked)

      this.addnewcontrols()



    }
    ), err => {
      console.log(err);
    }

  }
  inittype() {
    // Initialize add Box form field
    let boxForm: FormGroup = this._fb.group({
      name: [""],
      caption: [""]
    });
    return boxForm;
  }
  addtype() {
    (<FormArray>this.Searchproform.controls["Regions"]).push(
      this.inittype()
    );
    // get box length for box name like box 1,box 2 in sidebar boxes combo list

  }

  addnewcontrols() {
    ;
    for (let boxIndex = 0; boxIndex < this.RegionList.length; boxIndex++) {
      this.addtype();

      (<FormGroup>(
        (<FormArray>this.Searchproform.controls["Regions"]).controls[
        boxIndex
        ]
      )).patchValue({
        caption: this.RegionList[boxIndex].typeName
      });


    }
  }

  ngOnInit() {

    this.div = document.querySelector(".SiteList")


    this._render.setElementStyle(this.div, 'display', 'none')
  }
  addCheckboxes(args) {
    if (args.target.checked == true) {

      this.getregions();
      this.buttonstatus = false;
    }

  }
  removeCheckboxes(args) {
    this._render.setElementStyle(this.div, 'display', 'none')
    const array1 = <FormArray>this.Searchproform.controls["Regions"]

    while (array1.length !== 0) {
      array1.removeAt(0)
    }
    this.RegionList = []
    this.buttonstatus = true;

  }
  DeseleectAll() {
    const array1 = <FormArray>this.Searchproform.controls["Regions"]

    while (array1.length !== 0) {
      array1.removeAt(0)
    }
    this.Producttypeid="";
    this.RegionList = []
    this.getregions();
  }
  seleectAll() {
    for (let index = 0; index < this.RegionList.length; index++) {
      (<FormGroup>(
        (<FormArray>this.Searchproform.controls["Regions"]).controls[
        index
        ]
      )).patchValue({
        name: this.RegionList[index].regionID
      });
      this.Producttypeid += "," + this.RegionList[index].regionID
    }
  }
  chkchange(typeID, index) {
    if ((<FormGroup>(
      (<FormArray>this.Searchproform.controls["Regions"]).controls[
      index
      ]
    )).controls.name.value == true) {
      this.Producttypeid += "," + typeID
    }
    else
    {
      // let index=this.Producttypeid.indexOf(","+typeID);
      // let newstr="/"+this.Producttypeid.slice(index,index+2)+"/"
      
      // this.Producttypeid.replace(newstr,"");
    
    }
  }
  Searchproduct() {
    let producttypeids: string;
    producttypeids = ""
    // const array = (<FormArray>this.Searchproform.controls["productType"])
    // for (let index = 0; index < array.length; index++) {
    //  if ((<FormGroup>(
    //   (<FormArray>this.Searchproform.controls["productType"]).controls[
    //   index
    //   ]
    // )).controls.name.value !="")
    // {
    //   producttypeids += "," + (<FormGroup>(
    //     (<FormArray>this.Searchproform.controls["productType"]).controls[
    //     index
    //     ]
    //   )).controls.name.value
    // }
    // }
    console.log(producttypeids)
   
    if (this.Producttypeid == "") {
      this._APIwithActionService.getDatabyID(this.Searchproform.controls["SerachSite"].value, 'Site', 'GetSitebykeyword').subscribe((data) => {
        this.SiteList = data
        this._render.setElementStyle(this.div, 'display', 'block')
        //const controls = this.ProductTypeList.map(c => new FormControl(false));
        //  controls[0].setValue(true); // Set the first checkbox to true (checked)




      }
      ), err => {
        console.log(err);
      }

    }
    else {
      this._APIwithActionService.getDatabyID(this.Searchproform.controls["SerachSite"].value, 'Site', 'GetSitebykeywordtypes', 'type=' + this.Producttypeid).subscribe((data) => {
        this.SiteList = data
        this._render.setElementStyle(this.div, 'display', 'block')
        //const controls = this.ProductTypeList.map(c => new FormControl(false));
        //  controls[0].setValue(true); // Set the first checkbox to true (checked)




      }
      ), err => {
        console.log(err);
      }

    }
  }
  editProduct(id)
  {
    this._router.navigate(["/Product/ProductAdd",id]);
  }
}
