import { Component, OnInit ,ViewChild} from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { GlobalVariable } from '../../shared/globalclass';
import {DatatableComponent} from "../../shared/ui/datatable/datatable.component";

@Component({
  selector: 'app-viewproductprice',
  templateUrl: './viewproductprice.component.html'
})
export class ViewproductpriceComponent implements OnInit {
  @ViewChild(DatatableComponent) DataView: DatatableComponent
id:number;
  constructor(private _avRoute: ActivatedRoute) {

    if (this._avRoute.snapshot.params["id"]) {
      this.id = this._avRoute.snapshot.params["id"];
    } }

  ngOnInit() {
    
    this.DataView.options=
    {
       dom: "Bfrtip",
      
      ajax:GlobalVariable.BASE_API_URL+"Report/Getproductpricelist/" +this.id,
        columns: [
            { data: 'productType' },
            { data: 'product' },
            { data: 'fromdate' },
            { data: 'packsize' },
            { data: 'packcost' },
         
          
        ] , 
        buttons: [
          'copy', 'csv', 'excel', 'pdf', 'print'
          ]
    };   
  }
}
