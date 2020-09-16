import { Component, OnInit ,ViewChild} from '@angular/core';
import { APIwithActionService } from '../../shared/APIwithAction.service';
import { ActivatedRoute } from '@angular/router';  
import {DatatableComponent} from "../../shared/ui/datatable/datatable.component";

@Component({
  selector: 'app-viewproductusage',
  templateUrl: './viewproductusage.component.html'
})
export class ViewproductusageComponent implements OnInit {

  typeid:string;
  areaid:string;
  @ViewChild(DatatableComponent) DataView: DatatableComponent

  constructor(private _APIwithActionService:APIwithActionService,private _avRoute:ActivatedRoute) { 



    if (this._avRoute.snapshot.params["typeid"]) {
      this.typeid = this._avRoute.snapshot.params["typeid"];
    }

    if (this._avRoute.snapshot.params["areaid"]) {
      this.areaid = this._avRoute.snapshot.params["areaid"];
    }

  }

  ngOnInit() {
    this.DataView.options=
    {
       dom: "Bfrtip",
     
       //  aaData:this.getdata
       
     //,
      ajax:(data, callback, settings) => {
        console.log('yy')
      this._APIwithActionService.getDataList('Report','GetProductUsagelist','param='+this.areaid+','+this.typeid)
          .subscribe((data) => {


            
          console.log('yy')
            callback({
              aaData: data
            
            })
          }
        
          )
        },
        columns: [
          { data: 'areaName' },
          { data: 'testName' },
          { data: 'instrumentName' },
          { data: 'productName' },
          { data: 'usagerate' }],
        buttons: [
          'copy', 'csv', 'excel', 'pdf', 'print'
          ]
    };  
  }

}
