/**
 * Created by griga on 7/11/16.
 */


import { Routes, RouterModule } from '@angular/router';
import { MainLayoutComponent } from "./shared/layout/app-layouts/main-layout.component";
import { AuthLayoutComponent } from "./shared/layout/app-layouts/auth-layout.component";
import { ModuleWithProviders } from "@angular/core";
import { AuthGuard } from "./guards/auth-guard.service"


export const routes: Routes = [
  { path: '', component: AuthLayoutComponent, loadChildren: 'app/+auth/auth.module#AuthModule' },
  {
    path: '',
    component: MainLayoutComponent,
    data: { pageTitle: 'Home' },
    children: [
      {
        path: '', redirectTo: 'Dashboard', pathMatch: 'full',


      },
      { path: 'home', loadChildren: 'app/+home/home.module#HomeModule', data: { pageTitle: 'Home' } },
      { path: 'Dashboard', loadChildren: 'app/+Dashboard/Dashboard.module#DashboardModule', data: { pageTitle: 'Dashboard' } },
      { path: 'Managedata', loadChildren: 'app/+Managedata/Managedata.module#managedataModule', data: { pageTitle: 'Manage Data' } },
      { path: 'ConstructMorbidity', loadChildren: 'app/+ConstructMorbidity/ConstructMorbidity.module#ConstructMorbidityModule', data: { pageTitle: 'Construct Morbidity' } },
      { path: 'ConductForecast', loadChildren: 'app/+ConductForecast/ConductForecast.module#ConductForecastModule', data: { pageTitle: 'Conduct Forecast' } },

      // {path: 'dashboard', loadChildren: 'app/+dashboard/dashboard.module#DashboardModule',data:{pageTitle: 'Dashboard'}},
      // {path: 'smartadmin', loadChildren: 'app/+smartadmin-intel/smartadmin-intel.module#SmartadminIntelModule',data:{pageTitle: 'Smartadmin'}},
      // {path: 'app-views', loadChildren: 'app/+app-views/app-views.module#AppViewsModule',data:{pageTitle: 'App Views'}},
      // {path: 'calendar', loadChildren: 'app/+calendar/calendar.module#CalendarModule',data:{pageTitle: 'Calendar'}},
      // {path: 'e-commerce', loadChildren: 'app/+e-commerce/e-commerce.module#ECommerceModule',data:{pageTitle: 'E-commerce'}},
      // {path: 'forms', loadChildren: 'app/+forms/forms-showcase.module#FormsShowcaseModule',data:{pageTitle: 'Forms'}},
      // {path: 'graphs', loadChildren: 'app/+graphs/graphs-showcase.module#GraphsShowcaseModule',data:{pageTitle: 'Graphs'}},
      // {path: 'maps', loadChildren: 'app/+maps/maps.module#MapsModule',data:{pageTitle: 'Maps'}},
      { path: 'miscellaneous', loadChildren: 'app/+miscellaneous/miscellaneous.module#MiscellaneousModule', data: { pageTitle: 'Miscellaneous' } },
      // {path: 'outlook', loadChildren: 'app/+outlook/outlook.module#OutlookModule',data:{pageTitle: 'Outlook'}},
      // {path: 'tables', loadChildren: 'app/+tables/tables.module#TablesModule',data:{pageTitle: 'Tables'}},
      // {path: 'ui', loadChildren: 'app/+ui-elements/ui-elements.module#UiElementsModule',data:{pageTitle: 'Ui'}},
      // {path: 'widgets', loadChildren: 'app/+widgets/widgets-showcase.module#WidgetsShowcaseModule',data:{pageTitle: 'Widgets'}},
      // {path: 'Category', loadChildren: 'app/+Category/Category.module#CategoryModule',data:{pageTitle: 'SiteCategory'}},
      // {path: 'Region', loadChildren: 'app/+Region/Region.module#RegionModule',data:{pageTitle: 'Region'}},
      // {path: 'TestingArea', loadChildren: 'app/+TestingArea/TestingArea.module#TestingAreaModule',data:{pageTitle: 'Testing Area'}},
      // {path: 'Instrument', loadChildren: 'app/+Instrument/Instrument.module#InstrumentModule',data:{pageTitle: 'Instrument'}},
      // {path: 'Product', loadChildren: 'app/+Product/Product.module#ProductModule',data:{pageTitle: 'Product'}},
      // {path: 'ProductType', loadChildren: 'app/+ProductType/ProductType.module#ProductTypeModule',data:{pageTitle: 'ProductType'}},
      // {path: 'Test', loadChildren: 'app/+Test/Test.module#TestModule',data:{pageTitle: 'Test'}},

      { path: 'SearchProduct', loadChildren: 'app/+SearchProduct/SearchProduct.module#SearchProductModule', data: { pageTitle: 'SearchProduct' } },
      { path: 'SearchSite', loadChildren: 'app/+SearchSite/SearchSite.module#SearchSiteModule', data: { pageTitle: 'SearchSite' } },

      // {path: 'Site', loadChildren: 'app/+Site/Site.module#SiteModule',data:{pageTitle: 'Site'}},
      { path: 'Demographic', loadChildren: 'app/+Demographic/Demographic.module#DemograhicModule', data: { pageTitle: 'Demographic' } },
      { path: 'Consumption', loadChildren: 'app/+Consumption/Consumption.module#ConsumptionModule', data: { pageTitle: 'Consumption' } },
      { path: 'ServiceStatistic', loadChildren: 'app/+ServiceStatistic/ServiceStatistic.module#ServiceModule', data: { pageTitle: 'Service Statistic' } },
      { path: 'Demographicsettings', loadChildren: 'app/+Demographicsettings/Demographicsettings.module#DemographicsettingModule', data: { pageTitle: 'Construct Morbidity' } },
      { path: 'ImportData', loadChildren: 'app/+ImportData/ImportData.module#ImportDataModule', data: { pageTitle: 'Import Data' } },
      { path: 'Report', loadChildren: 'app/+Report/Report.module#ReportModule', data: { pageTitle: 'Report' } },
      //{path: 'Verifylink', loadChildren: 'app/+Verifylink/Verifylink.module.ts#verifylink1Module',data:{pageTitle: 'Verify link'}},
      { path: 'GlobalAdmin', loadChildren: 'app/+adminuser/adminuser.module#adminuserModule', data: { pageTitle: 'Admin User' } },
      { path: 'CopyDefaultData', loadChildren: 'app/+copydefaultdata/copydefaultdata.module#copydefaultdataModule', data: { pageTitle: 'Import Default Data' } },
      { path: 'cmspage', loadChildren: 'app/+cmspagenew/cmspagenew.module#cmspagenewModule', data: { pageTitle: 'CMS Page' } },
      { path: 'Forecast', loadChildren: 'app/+forecast/forecast.module#ForecastModule', data: { pageTitle: 'Conduct Forecast' } },
      { path: 'Approvemasterdata', loadChildren: 'app/+pendingapprovalentry/pendingapprovalentry.module#PendingapprovalModule', data: { pageTitle: 'CMS Page' } },
    ],


    canActivate: [AuthGuard]
  },



]



export const routing: ModuleWithProviders = RouterModule.forRoot(routes, { useHash: true });
