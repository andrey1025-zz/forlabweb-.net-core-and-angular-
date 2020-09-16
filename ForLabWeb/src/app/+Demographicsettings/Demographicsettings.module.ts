import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DemographicsettingRouting } from './Demographicsettings.routing';
import {Demographicsettingcomponent} from "./Demographicsettings.component";
import {SmartadminModule} from "../shared/smartadmin.module";
import { FormsModule,ReactiveFormsModule }   from '@angular/forms';
import { GlobalAPIService } from "../shared/GlobalAPI.service";
import { APIwithActionService } from "../shared/APIwithAction.service";
import { HttpModule } from '@angular/http';
import {SmartadminValidationModule} from "../shared/forms/validation/smartadmin-validation.module";
import {JqueryUiModule} from "../shared/ui/jquery-ui/jquery-ui.module";
import {TooltipModule} from "ngx-bootstrap/tooltip";
import { NgxLoadingModule } from 'ngx-loading';
@NgModule({
  imports: [
    CommonModule,
    DemographicsettingRouting,
    SmartadminModule,
    SmartadminValidationModule,
    ReactiveFormsModule,
    HttpModule,
    JqueryUiModule,
    TooltipModule.forRoot(),
    NgxLoadingModule.forRoot({})

  ],
  declarations: [Demographicsettingcomponent],
  providers:[GlobalAPIService,APIwithActionService]
})
export class DemographicsettingModule { }
