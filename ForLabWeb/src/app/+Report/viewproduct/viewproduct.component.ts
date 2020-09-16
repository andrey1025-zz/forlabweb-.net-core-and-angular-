import { Component, OnInit ,ViewChild} from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { GlobalVariable } from '../../shared/globalclass';
import {DatatableComponent} from "../../shared/ui/datatable/datatable.component";

@Component({
  selector: 'app-viewproduct',
  templateUrl: './viewproduct.component.html'
})
export class ViewproductComponent implements OnInit {
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
      
      ajax:GlobalVariable.BASE_API_URL+"Report/Getproductlist/" +this.id,
        columns: [
            { data: 'productType' },
            { data: 'product' },
            { data: 'catalog' },
            { data: 'basicUnit' },
            { data: 'minimumpacksize' },
         
          
        ] , 
        buttons: [
          'copy', 'csv', 'excel', 'pdf', 'print'
          ]
    };   
  }

}
