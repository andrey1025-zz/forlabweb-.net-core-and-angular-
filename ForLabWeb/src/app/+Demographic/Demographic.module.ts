
import { NgModule } from '@angular/core';

import { SmartadminModule } from '../shared/smartadmin.module';
import {SmartadminDatatableModule} from "../shared/ui/datatable/smartadmin-datatable.module";

import { routing } from './Demographic.routing';

import {DemographicComponent} from "./Demographic.component";


import {SmartadminWizardsModule} from "../shared/forms/wizards/smartadmin-wizards.module";
import {DemograhicListComponent} from "./DemographicList/DemographicList.component"
import {DemographicAddComponent} from "./DemographicAdd/DemographicAdd.component"
import { Globals } from '../shared/Globals';  
import { GlobalAPIService } from "../shared/GlobalAPI.service";
import { APIwithActionService } from "../shared/APIwithAction.service";
import {SmartadminEditorsModule} from "../shared/forms/editors/smartadmin-editors.module";
import { FormsModule,ReactiveFormsModule }   from '@angular/forms';
import { BsDatepickerModule } from 'ngx-bootstrap';
import { SitebysiteforecastComponent } from './sitebysiteforecast/sitebysiteforecast.component';
import { AggregrateforecastComponent } from './aggregrateforecast/aggregrateforecast.component';
import { PatientGroupRatioComponent } from './patient-group-ratio/patient-group-ratio.component';
import { PatientAssumptionComponent } from './patient-assumption/patient-assumption.component';
import { ProductassumptionComponent } from './productassumption/productassumption.component';
import { LineargrowthComponent } from './lineargrowth/lineargrowth.component';
import { ForecastchartComponent } from './forecastchart/forecastchart.component';
import { TestingprotocolComponent } from './testingprotocol/testingprotocol.component';
import { DemographicwizardComponent } from './demographicwizard/demographicwizard.component';
import { ProgramlistComponent } from './programlist/programlist.component';
import {NgxDatatableModule} from "@swimlane/ngx-datatable";
@NgModule({
  declarations: [
    DemographicComponent,
    DemograhicListComponent,
    DemographicAddComponent,
    SitebysiteforecastComponent,
    AggregrateforecastComponent,
    PatientGroupRatioComponent,
    PatientAssumptionComponent,
    ProductassumptionComponent,
    LineargrowthComponent,
    ForecastchartComponent,
    TestingprotocolComponent,
    DemographicwizardComponent,
    ProgramlistComponent
  ],
  imports: [


    SmartadminModule,
    routing,
    ReactiveFormsModule,
    FormsModule,
    SmartadminEditorsModule,
    SmartadminDatatableModule,
    SmartadminWizardsModule,
    BsDatepickerModule.forRoot(),
    NgxDatatableModule
   
  ],

  entryComponents: [DemographicComponent],
  providers:[GlobalAPIService,APIwithActionService,Globals]
})
export class DemograhicModule {

}

