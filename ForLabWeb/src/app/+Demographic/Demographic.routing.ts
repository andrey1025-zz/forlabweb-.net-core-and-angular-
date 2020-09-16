

import { Routes, RouterModule } from "@angular/router";


import { DemograhicListComponent } from "./DemographicList/DemographicList.component";

import { DemographicComponent } from "./Demographic.component";

import {DemographicwizardComponent} from "./demographicwizard/demographicwizard.component";

import {ProgramlistComponent} from './programlist/programlist.component';
export const routes: Routes = [
  {
    path: '',
    component: DemographicComponent,
    children: [
      // {
      //   path: '',
      //   redirectTo: 'DemographicList/:id',
      //   pathMatch: 'full'
      // },
      {
        path: 'DemographicAdd/:id1',
        component: DemographicwizardComponent
      },
      {
        path: 'DemographicAdd',
        component: DemographicwizardComponent
      },
      {
        path: 'DemographicAdd/:id1/:id2',
        component: DemographicwizardComponent
      },
      {
        path: 'DemographicAdd/:id1/:id2/:F',
        component: DemographicwizardComponent
      },
      {
        path: 'DemographicList/:id',
        component: DemograhicListComponent
      },
      {
        path:'Programlist',
        component:ProgramlistComponent
      }
      // {
      //   path: 'Productassumption/:id',
      //   component: ProductassumptionComponent
      // },
      // {
      //   path:'Testingprotocol/:id1/:id2',
      //   component:TestingprotocolComponent
      // },
      // {
      //   path: 'forecastchart/:id',
      //   component: ForecastchartComponent
      // },
      // {
      //   path: 'lineargrowth/:id',
      //   component: LineargrowthComponent
      // },
      // {
      //   path: 'sitebysiteforecast/:id1/:id2',
      //   component: SitebysiteforecastComponent
      // },
      // {
      //   path: 'PatientGroupRatio/:id1/:id2',
      //   component: PatientGroupRatioComponent
      // },
      // {
      //   path: 'aggregrateforecast/:id1/:id2',
      //   component: AggregrateforecastComponent
      // },
     
      // {
      //   path: 'PatientAssumption/:id1/:id2',
      //   component: PatientAssumptionComponent
      // },

    ]
  }
];


export const routing = RouterModule.forChild(routes);

