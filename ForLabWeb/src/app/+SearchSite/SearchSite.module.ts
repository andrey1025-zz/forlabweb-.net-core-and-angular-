import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { searchsiteRouting } from './SearchSite.routing';
import {SearchSiteComponent} from "./SearchSite.component";
import {SmartadminModule} from "../shared/smartadmin.module";
import { FormsModule,ReactiveFormsModule }   from '@angular/forms';
import { GlobalAPIService } from "../shared/GlobalAPI.service";
import { APIwithActionService } from "../shared/APIwithAction.service";
import { HttpModule } from '@angular/http';
@NgModule({
  imports: [
    CommonModule,
    searchsiteRouting,
    SmartadminModule,
    ReactiveFormsModule,
    HttpModule
  ],
  declarations: [SearchSiteComponent],
  providers:[GlobalAPIService,APIwithActionService]
})
export class SearchSiteModule { }
