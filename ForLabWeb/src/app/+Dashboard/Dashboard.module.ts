
import { NgModule } from '@angular/core';

import { SmartadminModule } from '../shared/smartadmin.module';

import { routing } from './Dashboard.routing';

import { DashboardComponent } from "./Dashboard.component";
// import { HighchartsChartModule } from 'highcharts-angular';
import { APIwithActionService } from "../shared/APIwithAction.service"
import { JqueryUiModule } from "../shared/ui/jquery-ui/jquery-ui.module";
import { GlobalAPIService } from "../shared/GlobalAPI.service";





@NgModule({
  declarations: [
    DashboardComponent
  ],
  imports: [
    SmartadminModule,
    routing,
    // HighchartsChartModule,
    JqueryUiModule

  ],

  entryComponents: [DashboardComponent],
  providers: [APIwithActionService, GlobalAPIService]
})
export class DashboardModule {

}

