
import { NgModule } from '@angular/core';

import { SmartadminModule } from '../shared/smartadmin.module';
import {SmartadminDatatableModule} from "../shared/ui/datatable/smartadmin-datatable.module";

import { routing } from './Report.routing';

import {DemographicComponent} from "./Demographic.component";


import {SmartadminWizardsModule} from "../shared/forms/wizards/smartadmin-wizards.module";


import { GlobalAPIService } from "../shared/GlobalAPI.service";
import { APIwithActionService } from "../shared/APIwithAction.service";
import {SmartadminEditorsModule} from "../shared/forms/editors/smartadmin-editors.module";
import { FormsModule,ReactiveFormsModule }   from '@angular/forms';
import { BsDatepickerModule } from 'ngx-bootstrap';
import { ReportComponent } from './Report.component';
import { RegionreportComponent } from './regionlist/regionlist.component';
import { ViewreportlistComponent } from './viewreportlist/viewreportlist.component';
import { FiltersiteComponent } from './filtersite/filtersite.component';
import { ViewsitelistComponent } from './viewsitelist/viewsitelist.component';
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
import { ViewpatientsummaryComponent } from './viewpatientsummary/viewpatientsummary.component';

@NgModule({
  declarations: [
    ReportComponent,
    RegionreportComponent,
    ViewreportlistComponent,
    FiltersiteComponent,
    ViewsitelistComponent,
    FilterinsComponent,
    ViewinstrumentComponent,
    FilterproductComponent,
    ViewproductComponent,
    FiltersiteinsComponent,
    ViewsiteinstrumentComponent,
    FilterproductpriceComponent,
    ViewproductpriceComponent,
    FilterproductusageComponent,
    ViewproductusageComponent,
    FiltertestComponent,
    ViewtestComponent,
    FilterforecastsummaryComponent,
    ViewfilterforecastsummaryComponent,
    FilterforecastcomparisionComponent,
    ViewforecastcomparisionComponent,
    ViewpatientsummaryComponent,
  
  ],
  imports: [


    SmartadminModule,
    routing,
    ReactiveFormsModule,
    FormsModule,
    SmartadminEditorsModule,
    SmartadminDatatableModule,
    SmartadminWizardsModule,
    BsDatepickerModule.forRoot()
  ],

  entryComponents: [ReportComponent],
  providers:[GlobalAPIService,APIwithActionService]
})
export class ReportModule {

}

