
import { Routes, RouterModule } from "@angular/router";
import { TestingAreaAddComponent } from "./TestingAreaAdd/TestingAreaAdd.component";
import { TestingAreaListComponent } from "./TestingAreaList/TestingAreaList.component";
import { InstrumentAddComponent } from "./InstrumentAdd/InstrumentAdd.component";
import { InstrumentListComponent } from "./InstrumentList/InstrumentList.component";
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
import { managedatacomponent } from "./Managedata.component";
export const routes: Routes = [
  {
    path: '',
    component: managedatacomponent,


  },
  {
    path: 'ManagedataList/:tab',
    component: managedatacomponent,

  },
  // {
  //     path: 'InstrumentList',
  //     component: InstrumentListComponent
  //   },
  // {
  //   path: 'InstrumentAdd/:id',
  //   component: InstrumentAddComponent
  // },
  // {
  //   path: 'InstrumentAdd',
  //   component: InstrumentAddComponent
  // },
  // {
  //   path: 'TestingAreaAdd',
  //   component: TestingAreaAddComponent
  // }, {
  //   path: 'TestingAreaAdd/:id',
  //   component: TestingAreaAddComponent
  // },
  //  {
  //   path: 'TestingAreaList',
  //   component: TestingAreaListComponent
  // },
  // {
  //   path: 'TestAdd',
  //   component: TestAddComponent
  // }, {
  //   path: 'TestAdd/:id',
  //   component: TestAddComponent
  // },
  //  {
  //   path: 'TestList',
  //   component: TestListComponent
  // },
  // {
  //   path: 'ProducttypeAdd',
  //   component: ProductTypeAddComponent
  // }, {
  //   path: 'ProducttypeAdd/:id',
  //   component: ProductTypeAddComponent
  // },
  //   {
  //   path: 'Producttypelist',
  //   component: ProductTypeListComponent
  // },

  // {
  //   path: 'ProductAdd',
  //   component: ProductAddComponent
  // }, {
  //   path: 'ProductAdd/:id',
  //   component: ProductAddComponent
  // },
  //   {
  //   path: 'Productlist',
  //   component: ProductListComponent
  // },


  // {
  //   path: 'RegionAdd',
  //   component: RegionAddComponent
  // }, {
  //   path: 'RegionAdd/:id',
  //   component: RegionAddComponent
  // },
  //   {
  //   path: 'RegionListlist',
  //   component: RegionlistComponent
  // },


  // {
  //   path: 'CategoryAdd',
  //   component: AddSitecategoryComponent
  // }, {
  //   path: 'CategoryAdd/:id',
  //   component: AddSitecategoryComponent
  // },
  //  {
  //   path: 'CategoryList',
  //   component: CategoryListComponent
  // },

  // {
  //   path: 'SiteAdd',
  //   component: SiteAddStepOneComponent
  // }, {
  //   path: 'SiteAdd/:id',
  //   component: SiteAddStepOneComponent
  // },

  // {
  //   path: 'SiteTwo',
  //   component: SiteAddStepTwoComponent
  // }, {
  //   path: 'SiteTwo/:id',
  //   component: SiteAddStepTwoComponent
  // },
  //  {
  //   path: 'SiteList',
  //   component: SiteListComponent
  // },

];


export const routing = RouterModule.forChild(routes);