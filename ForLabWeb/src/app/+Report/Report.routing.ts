

import { Routes, RouterModule } from "@angular/router";

import { ReportComponent } from "./Report.component";
import { RegionreportComponent} from "./regionlist/regionlist.component";
import { ViewreportlistComponent} from "./viewreportlist/viewreportlist.component";
import { FiltersiteComponent} from "./filtersite/filtersite.component";
import { ViewsitelistComponent} from "./viewsitelist/viewsitelist.component"
import { FilterinsComponent } from './filterins/filterins.component';
import { ViewinstrumentComponent } from './viewinstrument/viewinstrument.component';
import { FilterproductComponent } from './filterproduct/filterproduct.component';
import { ViewproductComponent } from './viewproduct/viewproduct.component';
import { FiltersiteinsComponent } from './filtersiteins/filtersiteins.component';
import { ViewsiteinstrumentComponent } from './viewsiteinstrument/viewsiteinstrument.component';
import { FilterproductpriceComponent } from './filterproductprice/filterproductprice.component';
import { ViewproductpriceComponent } from './viewproductprice/viewproductprice.component';
import { FilterproductusageComponent } from './filterproductusage/filterproductusage.component';
import { ViewproductusageComponent } from './viewproductusage/viewproductusage.component';
import { FiltertestComponent } from './filtertest/filtertest.component';
import { ViewtestComponent } from './viewtest/viewtest.component';
import { FilterforecastsummaryComponent } from './filterforecastsummary/filterforecastsummary.component';
import { ViewfilterforecastsummaryComponent } from './viewfilterforecastsummary/viewfilterforecastsummary.component';
import { FilterforecastcomparisionComponent } from './filterforecastcomparision/filterforecastcomparision.component';
import { ViewforecastcomparisionComponent } from './viewforecastcomparision/viewforecastcomparision.component';
import { ViewpatientsummaryComponent} from "./viewpatientsummary/viewpatientsummary.component";
export const routes: Routes = [
  {
    path: '',
    component: ReportComponent,
    children: [
     
   {
     path:'regionreport',
     component:RegionreportComponent
   },
   {
     path:'Filtersite',
     component:FiltersiteComponent
   },
   {
    path:'Filterproduct',
    component:FilterproductComponent
  },
  {
    path:'Filterproductprice',
    component:FilterproductpriceComponent
  },
  {
    path:'Filterproductusge',
    component:FilterproductusageComponent
  },
  {
    path:'Filtersiteins',
    component:FiltersiteinsComponent
  },
  {
    path:'Filterins',
    component:FilterinsComponent
  },
  {
    path:'filtertest',
    component:FiltertestComponent
  },
  {
    path:'viewtest/:areaid',
    component:ViewtestComponent
  },
  {
    path:'filterforecastsummary',
    component:FilterforecastsummaryComponent
  },
  {
    path:'viewforecastsummary/:id/:method',
    component:ViewfilterforecastsummaryComponent
  },
  {
    path:'filterforecastcomparision/:type',
    component:FilterforecastcomparisionComponent
  },
  {
    path:'viewforecastcomparision/:id',
    component:ViewforecastcomparisionComponent
  },

  {
    path:'viewforecastpatientsummary/:id',
    component:ViewpatientsummaryComponent
  },
  
  
  {
    path:'viewsiteins/:regid/:catid/:areaid',
    component:ViewsiteinstrumentComponent
  },
  {
    path:'viewproductusage/:areaid/:typeid',
    component:ViewproductusageComponent
  },
 // viewproductprice
  {
  
    path:'viewins/:areaid/:show',
    component:ViewinstrumentComponent
    },
{
  
path:'viewreport/:no/:logic/:show',
component:ViewreportlistComponent
},
{
path:"Viewsite/:regid/:catid/:show",
component:ViewsitelistComponent
},
{
  
  path:'viewproduct/:id',
  component:ViewproductComponent
  },

{
  
  path:'viewproductprice/:id',
  component:ViewproductpriceComponent
  },
    ]
  }
];


export const routing = RouterModule.forChild(routes);

