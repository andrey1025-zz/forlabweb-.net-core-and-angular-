
import { NgModule } from '@angular/core';
import { NgxLoadingModule } from 'ngx-loading';

import { SmartadminModule } from '../shared/smartadmin.module';
import { SmartadminDatatableModule } from "../shared/ui/datatable/smartadmin-datatable.module";
import { routing } from './ConductForecast.routing';
import { SmartadminEditorsModule } from "../shared/forms/editors/smartadmin-editors.module";

import { JqueryUiModule } from "../shared/ui/jquery-ui/jquery-ui.module";
import { ReactiveFormsModule } from '@angular/forms';
import { BsDatepickerModule, ModalModule } from 'ngx-bootstrap';
import { APIwithActionService } from '../shared/APIwithAction.service';
import { GlobalAPIService } from '../shared/GlobalAPI.service';

import { ConductForecastComponent } from './ConductForecast.component';
import { ForecastAddComponent } from './ForecastAdd/ForecastAdd.component';
import { ForecastTestAddComponent } from './ForecastTestAdd/ForecastTestAdd.component';
import { ForecastComparisonComponent } from './ForecastComparison/ForecastComparison.component';

@NgModule({
  declarations: [
    ConductForecastComponent,
    ForecastAddComponent,
    ForecastTestAddComponent,
    ForecastComparisonComponent
  ],
  entryComponents: [
    ForecastAddComponent,
    ForecastTestAddComponent
  ],
  imports: [
    routing,
    SmartadminModule,
    SmartadminEditorsModule,
    SmartadminDatatableModule,
    JqueryUiModule,
    ReactiveFormsModule,
    BsDatepickerModule.forRoot(),
    ModalModule.forRoot(),
    NgxLoadingModule.forRoot({})
  ],
  exports: [
    ConductForecastComponent,
  ],
  providers: [APIwithActionService, GlobalAPIService]

})
export class ConductForecastModule {

}