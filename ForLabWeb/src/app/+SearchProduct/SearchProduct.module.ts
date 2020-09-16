import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { searchproductRouting } from './SearchProduct.routing';
import {SearchProductComponent} from "./SerachProduct.component";
import {SmartadminModule} from "../shared/smartadmin.module";
import { FormsModule,ReactiveFormsModule }   from '@angular/forms';
import { GlobalAPIService } from "../shared/GlobalAPI.service";
import { APIwithActionService } from "../shared/APIwithAction.service";
import { HttpModule } from '@angular/http';
@NgModule({
  imports: [
    CommonModule,
    searchproductRouting,
    SmartadminModule,
    ReactiveFormsModule,
    HttpModule
  ],
  declarations: [SearchProductComponent],
  providers:[GlobalAPIService,APIwithActionService]
})
export class SearchProductModule { }
