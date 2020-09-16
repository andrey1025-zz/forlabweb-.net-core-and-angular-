import { Component, OnInit ,ViewChild} from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { GlobalVariable } from '../../shared/globalclass';
import {DatatableComponent} from "../../shared/ui/datatable/datatable.component";
@Component({
  selector: 'app-viewreportlist',
  templateUrl: './viewreportlist.component.html',

})
export class ViewreportlistComponent implements OnInit {
  no: number;
  logic: string;
  show:string;
  public options; 
  @ViewChild(DatatableComponent) DataView: DatatableComponent
  @ViewChild('aa') aa: DatatableComponent
  constructor(private _avRoute: ActivatedRoute) {

    if (this._avRoute.snapshot.params["no"]) {
      this.no = this._avRoute.snapshot.params["no"];
    }
    if (this._avRoute.snapshot.params["logic"]) {
      this.logic = this._avRoute.snapshot.params["logic"];
    }

    if (this._avRoute.snapshot.params["show"]) {
      this.show = this._avRoute.snapshot.params["show"];
    }
 
  }

  ngOnInit() {
let aa:DatatableComponent =this.aa as DatatableComponent;


    this.DataView.options=
    {
       dom: "Bfrtip",
      
      ajax:GlobalVariable.BASE_API_URL+"Report/Getregionlist/" +this.no +"?logic="+this.logic+","+localStorage.getItem("countryid"),
        columns: [
            { data: 'shortName' },
            { data: 'regionName' },
            { data: 'noofSites',visiable:false },
          
        ] , 
        buttons: [
          'copy', 'csv', 'excel', 'pdf', 'print'
          ]
    };   
    if(this.show=="false")
    {
    aa.options.columns.splice(2,aa.options.columns.length);
    }
  }

}
