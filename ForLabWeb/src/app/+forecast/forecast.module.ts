
import { NgModule } from '@angular/core';
import { SmartadminModule } from '../shared/smartadmin.module';
import { SmartadminDatatableModule } from "../shared/ui/datatable/smartadmin-datatable.module";
import { routing } from './forecast.routing';
import { ForecastComponent } from "./forecast.component";

import { SmartadminEditorsModule } from "../shared/forms/editors/smartadmin-editors.module";

import { JqueryUiModule } from "../shared/ui/jquery-ui/jquery-ui.module";
import { ReactiveFormsModule } from '@angular/forms';
//import { BsDatepickerModule } from 'ngx-bootstrap';
import { APIwithActionService } from '../shared/APIwithAction.service';
import { GlobalAPIService } from '../shared/GlobalAPI.service';

import { NgxLoadingModule } from 'ngx-loading';
import { RecentForecastComponent } from './recent-forecast/recent-forecast.component';
import { ForecasttestComponent } from './forecasttest/forecasttest.component';
import { DefineForecastComponent } from './define-forecast/define-forecast.component';
import { BsDatepickerModule } from 'ngx-bootstrap';
import { ForecastinstrumentComponent } from './forecastinstrument/forecastinstrument.component';
import { ForecastselectedinstrumentComponent } from './forecastselectedinstrument/forecastselectedinstrument.component';
import { ForecastproductusagesComponent } from './forecastproductusages/forecastproductusages.component';
import { ForecastmethodologyComponent } from './forecastmethodology/forecastmethodology.component';
import { ForecasttypeComponent } from './forecasttype/forecasttype.component';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { ImportforecastdateComponent } from './importforecastdate/importforecastdate.component';
import { ForecastFactorComponent } from './forecast-factor/forecast-factor.component';
import { ForecastlistComponent } from './forecastlist/forecastlist.component';
import { ForecastcalculationmethodComponent } from './forecastcalculationmethod/forecastcalculationmethod.component';
import { ForecastchartnewComponent } from './forecastchartnew/forecastchartnew.component';
import { DemographicprogramlistComponent } from './demographicprogramlist/demographicprogramlist.component';
import { DemographicprogrammethodComponent } from './demographicprogrammethod/demographicprogrammethod.component';
import { DemographicprogramgroupComponent } from './demographicprogramgroup/demographicprogramgroup.component';
import { DemographicpatientassumptionComponent } from './demographicpatientassumption/demographicpatientassumption.component';
import { DemographictestassumptionComponent } from './demographictestassumption/demographictestassumption.component';
import { DemographicproductassumptionComponent } from './demographicproductassumption/demographicproductassumption.component';
import { DemographictreatmentcycleComponent } from './demographictreatmentcycle/demographictreatmentcycle.component';
import { DemographicpatientgroupratioComponent } from './demographicpatientgroupratio/demographicpatientgroupratio.component';
// import { HighchartsChartModule } from 'highcharts-angular';
import { DemographictestingprotocolComponent } from './demographictestingprotocol/demographictestingprotocol.component';
import { ForecastchartserviceComponent } from './forecastchartservice/forecastchartservice.component';
import { DemographicsitebysiteComponent } from './demographicsitebysite/demographicsitebysite.component';
import { DemographicaggregrateComponent } from './demographicaggregrate/demographicaggregrate.component';
import { PatientassumptionComponent1 } from './patientassumption/patientassumption.component';
import { ProassumptionComponent } from './proassumption/proassumption.component';
import { DemographiclineargrowthComponent } from './demographiclineargrowth/demographiclineargrowth.component';
import { DemographicchartComponent } from './demographicchart/demographicchart.component';
@NgModule({
  declarations: [
    ForecastComponent,
    RecentForecastComponent,
    ForecasttestComponent,
    DefineForecastComponent,
    ForecastinstrumentComponent,
    ForecastselectedinstrumentComponent,
    ForecastproductusagesComponent,
    ForecastmethodologyComponent,
    ForecasttypeComponent,
    ImportforecastdateComponent,
    ForecastFactorComponent,
    ForecastlistComponent,
    ForecastcalculationmethodComponent,
    ForecastchartnewComponent,
    DemographicprogramlistComponent,
    DemographicprogrammethodComponent,
    DemographicprogramgroupComponent,
    DemographicpatientassumptionComponent,
    DemographictestassumptionComponent,
    DemographicproductassumptionComponent,
    DemographictreatmentcycleComponent,
    DemographicpatientgroupratioComponent,
    DemographictestingprotocolComponent,
    ForecastchartserviceComponent,
    DemographicsitebysiteComponent,
    DemographicaggregrateComponent,
    PatientassumptionComponent1,
    ProassumptionComponent,
    DemographiclineargrowthComponent,
    DemographicchartComponent,


  ],
  imports: [
    SmartadminModule,
    routing,
    SmartadminEditorsModule,
    SmartadminDatatableModule,
    JqueryUiModule,
    ReactiveFormsModule,
    // HighchartsChartModule,
    BsDatepickerModule.forRoot(),
    NgxLoadingModule.forRoot({}),
    TabsModule.forRoot()

  ],

  entryComponents: [ForecastComponent],
  providers: [APIwithActionService, GlobalAPIService]

})
export class ForecastModule {

}

