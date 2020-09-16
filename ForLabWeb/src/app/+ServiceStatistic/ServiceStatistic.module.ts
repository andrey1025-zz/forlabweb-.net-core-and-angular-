import { NgModule } from '@angular/core';
import { SmartadminModule } from '../shared/smartadmin.module';
import { SmartadminDatatableModule } from "../shared/ui/datatable/smartadmin-datatable.module";
import { routing } from './ServiceStatistic.routing';

import { SmartadminEditorsModule } from "../shared/forms/editors/smartadmin-editors.module";
import { NgxLoadingModule } from 'ngx-loading';
import { JqueryUiModule } from "../shared/ui/jquery-ui/jquery-ui.module";
import { ReactiveFormsModule } from '@angular/forms';
import { BsDatepickerModule } from 'ngx-bootstrap';
import { APIwithActionService } from '../shared/APIwithAction.service';
import { GlobalAPIService } from '../shared/GlobalAPI.service';
import { ServiceComponent } from "./ServiceStatistic.component";



import { ServiceListComponent } from "./ServiceStatisticList/ServiceStatisticList.component"

import { ServicestatisticaddComponent } from './servicestatisticadd/servicestatisticadd.component';
import { Servicestatisticdatausage1Component } from './servicestatisticdatausage1/servicestatisticdatausage1.component';
import { Servicestatisticdatausage2Component } from './servicestatisticdatausage2/servicestatisticdatausage2.component';
import { Servicestatisticdatausage3Component } from './servicestatisticdatausage3/servicestatisticdatausage3.component';
import { ServicewizardComponent } from './servicewizard/servicewizard.component';
//import {GlobalModule} from '../shared/GlobalModule.module';
@NgModule({
  declarations: [
    ServiceComponent,
    ServiceListComponent,
    ServicestatisticaddComponent,
    Servicestatisticdatausage1Component,
    Servicestatisticdatausage2Component,
    Servicestatisticdatausage3Component,
    ServicewizardComponent,
    //  conductForecastComponent

  ],
  imports: [

    SmartadminModule,
    routing,
    SmartadminEditorsModule,
    SmartadminDatatableModule,
    JqueryUiModule,
    ReactiveFormsModule,
    BsDatepickerModule.forRoot(),
    NgxLoadingModule.forRoot({}),
    // sharedModule
  ],

  entryComponents: [ServiceComponent],
  providers: [APIwithActionService, GlobalAPIService]
})
export class ServiceModule {

}

