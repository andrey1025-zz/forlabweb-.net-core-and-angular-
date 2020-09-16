

import {NgModule} from "@angular/core";
import {CommonModule} from "@angular/common";
import {I18nModule} from "../../i18n/i18n.module";
import {BigBreadcrumbsComponent} from "./big-breadcrumbs.component";
import {MinifyMenuComponent} from "./minify-menu.component";
import {NavigationComponent} from "./navigation.component";
import {SmartMenuDirective} from "./smart-menu.directive";
import {UserModule} from "../../user/user.module";
import {RouterModule} from "@angular/router";
import {APIwithActionService} from "../../APIwithAction.service";
import { HttpModule } from '@angular/http';
// import {ChatModule} from "../../chat/chat.module";

@NgModule({
  imports: [
    CommonModule,
    RouterModule,
    I18nModule,
    UserModule,
    HttpModule
    // ChatModule
  ],
  declarations: [
    BigBreadcrumbsComponent,
    MinifyMenuComponent,
    NavigationComponent,
    SmartMenuDirective,
  ],
  exports: [
    BigBreadcrumbsComponent,
    MinifyMenuComponent,
    NavigationComponent,
    SmartMenuDirective,
  ],providers:[APIwithActionService]
})
export class NavigationModule{}
