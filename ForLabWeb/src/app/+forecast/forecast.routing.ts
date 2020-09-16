import {Routes, RouterModule} from "@angular/router";

import {ForecastComponent} from "./forecast.component";

import { RecentForecastComponent } from "./recent-forecast/recent-forecast.component";
import {ForecasttestComponent} from "./forecasttest/forecasttest.component";
import { DefineForecastComponent } from "./define-forecast/define-forecast.component";
import { ForecastinstrumentComponent } from "./forecastinstrument/forecastinstrument.component";
import { ForecastselectedinstrumentComponent } from "./forecastselectedinstrument/forecastselectedinstrument.component";
import { ForecastproductusagesComponent } from "./forecastproductusages/forecastproductusages.component";
import { ForecastmethodologyComponent } from "./forecastmethodology/forecastmethodology.component";
import { ForecasttypeComponent } from "./forecasttype/forecasttype.component";
import { ImportforecastdateComponent } from "./importforecastdate/importforecastdate.component";
import { ForecastFactorComponent } from "./forecast-factor/forecast-factor.component";
import { ForecastcalculationmethodComponent } from "./forecastcalculationmethod/forecastcalculationmethod.component";
import { ForecastlistComponent } from "./forecastlist/forecastlist.component";
import { ForecastchartnewComponent } from "./forecastchartnew/forecastchartnew.component";
import { DemographicprogramlistComponent } from "./demographicprogramlist/demographicprogramlist.component";
import { DemographicprogrammethodComponent } from "./demographicprogrammethod/demographicprogrammethod.component";
import { DemographicprogramgroupComponent } from "./demographicprogramgroup/demographicprogramgroup.component";
import { DemographicpatientassumptionComponent } from "./demographicpatientassumption/demographicpatientassumption.component";
import { DemographictestassumptionComponent } from "./demographictestassumption/demographictestassumption.component";
import { DemographicproductassumptionComponent } from "./demographicproductassumption/demographicproductassumption.component";
import { DemographictreatmentcycleComponent } from "./demographictreatmentcycle/demographictreatmentcycle.component";
import { DemographicpatientgroupratioComponent } from "./demographicpatientgroupratio/demographicpatientgroupratio.component";
import { DemographictestingprotocolComponent } from "./demographictestingprotocol/demographictestingprotocol.component";
import { ForecastchartserviceComponent } from "./forecastchartservice/forecastchartservice.component";
import { DemographicsitebysiteComponent } from "./demographicsitebysite/demographicsitebysite.component";
import { DemographicaggregrateComponent } from "./demographicaggregrate/demographicaggregrate.component";
import { PatientAssumptionComponent } from "app/+Demographic/patient-assumption/patient-assumption.component";
import { PatientassumptionComponent1 } from "./patientassumption/patientassumption.component";
import { ProassumptionComponent } from "./proassumption/proassumption.component";
import { DemographiclineargrowthComponent } from "./demographiclineargrowth/demographiclineargrowth.component";
import { DemographicchartComponent } from "./demographicchart/demographicchart.component";

export const routes: Routes = [
  {
    path: '',
    component: ForecastComponent,
    children: [
    
      {
        path: 'RecentForecast',
        component: RecentForecastComponent
      },
      {
        path: 'Defineforecast',
        component: DefineForecastComponent
      },
      {
        path: 'Defineforecast/:id',
        component: DefineForecastComponent
      },
      {
        path: 'ForecastTest/:id',
        component: ForecasttestComponent
      },
      {
        path: 'ForecastIns/:id',
        component: ForecastinstrumentComponent
      }
      ,
      {
        path: 'ForecastSelectedIns/:id',
        component: ForecastselectedinstrumentComponent
      }
      ,
      {
        path: 'ForecastProductusage/:id',
        component: ForecastproductusagesComponent
      }
      ,
      {
        path: 'Forecastmethodology/:id',
        component: ForecastmethodologyComponent
      },
      {
        path: 'Forecasttype/:id',
        component: ForecasttypeComponent
      }
      ,
      {
        path: 'Importforecastdata/:id',
        component: ImportforecastdateComponent
      }
      ,
      {
        path: 'forecastFactor/:id',
        component: ForecastFactorComponent
      }
      ,
      {
        path: 'forecastcalculationmethod/:id',
        component: ForecastcalculationmethodComponent
      }
      ,
      {
        path: 'ForecastChartnew/:id',
        component: ForecastchartnewComponent
      },
      {
        path: 'ForecastChartservice/:id',
        component:ForecastchartserviceComponent
      }
      ,
      {
        path: 'forecastlist',
        component: ForecastlistComponent
      }
      ,
      {
        path: 'DemographicProgramlist/:id',
        component: DemographicprogramlistComponent
      }
      ,
      {
        path: 'Demographicprogrammethodlist/:id/:pid',
        component: DemographicprogrammethodComponent
      }
      ,
      {
        path: 'Demographicgroup/:id/:pid',
        component: DemographicprogramgroupComponent
      }
      ,
      {
        path: 'Demographicpatientassumption/:id/:pid',
        component: DemographicpatientassumptionComponent
      }
      ,
      {
        path: 'Demographictestassumption/:id/:pid',
        component: DemographictestassumptionComponent
      }
      ,
      {
        path: 'Demographicproductassumption/:id/:pid',
        component: DemographicproductassumptionComponent
      }
      ,
      {
        path: 'Demographicsitebysite/:id',
        component: DemographicsitebysiteComponent
      }
      ,
      {
        path: 'Demographitraetmentcyscle/:id/:pid',
        component: DemographictreatmentcycleComponent
      }
      ,
      {
        path: 'DemographicAggregrate/:id',
        component: DemographicaggregrateComponent
      }
      ,
      {
        path: 'Demographipatientgroup/:id',
        component: DemographicpatientgroupratioComponent
      }
      ,
      {
        path: 'Demographitestingprotocol/:id/:pid',
        component: DemographictestingprotocolComponent
      }
      ,
      {
        path: 'Demographicpatient/:id/:pid',
        component: PatientassumptionComponent1
      }
      ,
      {
        path: 'Demographicproduct/:id/:pid',
        component: ProassumptionComponent
      }
      ,
      {
        path: 'Demographiclineargrowth/:id/:pid',
        component: DemographiclineargrowthComponent
      }
      ,
      {
        path: 'Demographicchart/:id/:pid',
        component: DemographicchartComponent
      }
    ]
  }
];


export const routing = RouterModule.forChild(routes);
