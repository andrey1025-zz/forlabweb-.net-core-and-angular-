
import { NgModule } from '@angular/core';
import { SmartadminModule } from '../shared/smartadmin.module';
import { SmartadminDatatableModule } from "../shared/ui/datatable/smartadmin-datatable.module";
import { routing } from './Consumption.routing';
import { ConsumptionComponent } from "./Consumption.component";
import { ConsumptionListComponent } from "./ConsumptionList/ConsumptionList.component"
import { SmartadminEditorsModule } from "../shared/forms/editors/smartadmin-editors.module";
import { ConsumptionAddComponent } from './consumption-add/consumption-add.component';
import { JqueryUiModule } from "../shared/ui/jquery-ui/jquery-ui.module";
import { ReactiveFormsModule } from '@angular/forms';
import { BsDatepickerModule } from 'ngx-bootstrap';
import { APIwithActionService } from '../shared/APIwithAction.service';
import { GlobalAPIService } from '../shared/GlobalAPI.service';
import { TestComponent } from './test/test.component';
import { NgxLoadingModule } from 'ngx-loading';
import { Consumptiondatausage1Component } from './consumptiondatausage1/consumptiondatausage1.component';
import { Consumptiondatausage2Component } from './consumptiondatausage2/consumptiondatausage2.component';
import { Consumptiondatausage3Component } from './consumptiondatausage3/consumptiondatausage3.component';
import { ConsumptionwizardComponent } from './consumptionwizard/consumptionwizard.component';
//import {GlobalModule} from '../shared/GlobalModule.module';
@NgModule({
  declarations: [
    ConsumptionComponent,
    ConsumptionListComponent,
    ConsumptionAddComponent,
    TestComponent,
    Consumptiondatausage1Component,
    Consumptiondatausage2Component,
    Consumptiondatausage3Component,
    ConsumptionwizardComponent,
    // conductForecastComponent
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

  entryComponents: [ConsumptionComponent],
  providers: [APIwithActionService, GlobalAPIService]

})
export class ConsumptionModule {

}

