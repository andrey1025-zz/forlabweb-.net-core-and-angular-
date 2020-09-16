
import { NgModule } from '@angular/core';

import { SmartadminModule } from '../shared/smartadmin.module';

import { routing } from './pendingapprovalentry.routing';

import { pendingapprovalentryComponent } from "./pendingapprovalentry.component";
// import { HighchartsChartModule } from 'highcharts-angular';
import { APIwithActionService } from "../shared/APIwithAction.service"
import { JqueryUiModule } from "../shared/ui/jquery-ui/jquery-ui.module";
import { GlobalAPIService } from "../shared/GlobalAPI.service";

import { SmartadminDatatableModule } from "../shared/ui/datatable/smartadmin-datatable.module";



@NgModule({
  declarations: [
    pendingapprovalentryComponent
  ],
  imports: [
    SmartadminModule,
    routing,
    // HighchartsChartModule,
    JqueryUiModule, SmartadminDatatableModule

  ],

  entryComponents: [pendingapprovalentryComponent],
  providers: [APIwithActionService, GlobalAPIService]
})
export class PendingapprovalModule {

}

