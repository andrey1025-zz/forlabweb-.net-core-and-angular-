import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { routing } from "./Managedata.routing";
import { managedatacomponent } from './Managedata.component';
//import {InstrumentModule} from "../+Instrument/Instrument.module";
import { InstrumentListComponent } from "./InstrumentList/InstrumentList.component";
import { TestingAreaAddComponent } from "./TestingAreaAdd/TestingAreaAdd.component";
import { TestingAreaListComponent } from "./TestingAreaList/TestingAreaList.component";
import { InstrumentAddComponent } from "./InstrumentAdd/InstrumentAdd.component";

import { TestAddComponent } from "./TestAdd/TestAdd.component";
import { TestListComponent } from "./TestList/TestList.component";
import { ProductTypeAddComponent } from "./ProductTypeAdd/ProductTypeAdd.component";
import { ProductTypeListComponent } from "./ProductTypeList/ProductTypeList.component";
import { ProductAddComponent } from "./ProductAdd/ProductAdd.component";
import { ProductListComponent } from "./ProductList/ProductList.component";
import { RegionAddComponent } from "./RegionAdd/RegionAdd.component";
import { RegionlistComponent } from "./RegionList/RegionList.component";

import { SiteAddStepOneComponent } from "./SiteList/SiteAddStepOne/SiteAddStepOne.component";
import { SiteAddStepTwoComponent } from "./SiteList/SiteAddStepTwo/SiteAddStepTwo.component";
import { SiteListComponent } from "./SiteList/SiteList.component";
import { AddSitecategoryComponent } from "./CategoryAdd/CategoryAdd.component";
import { CategoryListComponent } from "./CategoryList/CategoryList.component";
import { FormsModule } from '@angular/forms';
import { APIwithActionService } from "../shared/APIwithAction.service";
import { GlobalAPIService } from "../shared/GlobalAPI.service";
import { SmartadminModule } from '../shared/smartadmin.module';
import { JqueryUiModule } from "../shared/ui/jquery-ui/jquery-ui.module";
import { SharedequalModule } from '../shared/Equalvalidateshared.module';
import { ReactiveFormsModule } from '@angular/forms';
import { SmartadminDatatableModule } from "../shared/ui/datatable/smartadmin-datatable.module";
import { BsDatepickerModule, ModalModule } from 'ngx-bootstrap';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { DeleteModalComponent } from 'app/shared/ui/datatable/DeleteModal/DeleteModal.component';

@NgModule({
  imports: [
    CommonModule,
    SharedequalModule,
    routing,
    FormsModule,
    SmartadminModule,
    JqueryUiModule,
    SmartadminDatatableModule,
    ReactiveFormsModule,
    BsDatepickerModule.forRoot(),
    ModalModule.forRoot(),
    TabsModule.forRoot()


  ],
  declarations: [
    managedatacomponent,
    InstrumentListComponent,
    InstrumentAddComponent,
    TestingAreaAddComponent,
    TestingAreaListComponent,
    TestAddComponent,
    TestListComponent, ProductTypeAddComponent, ProductTypeListComponent,
    ProductAddComponent, ProductListComponent, RegionAddComponent, RegionlistComponent,
    SiteListComponent, AddSitecategoryComponent, CategoryListComponent,
    SiteAddStepOneComponent, SiteAddStepTwoComponent
  ],
  entryComponents: [
    managedatacomponent,
    InstrumentAddComponent,
    DeleteModalComponent,
    TestingAreaAddComponent,
    ProductTypeAddComponent,
    ProductAddComponent
  ],
  providers: [APIwithActionService, GlobalAPIService]
})
export class managedataModule {

}