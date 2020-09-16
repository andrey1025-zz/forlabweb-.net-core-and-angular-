import {NgModule} from "@angular/core";
import {CommonModule} from "@angular/common";
import {FormsModule} from "@angular/forms";

//import {HeaderModule} from "./header/header.module";
import {FooterComponent} from "./footer/footer.component";

import {NavigationModule} from "./navigation/navigation.module";
import {RibbonModule} from "./ribbon/ribbon.module";
import {ShortcutComponent} from "./shortcut/shortcut.component";
import {ToggleActiveDirective} from "../utils/toggle-active.directive";
import {LayoutSwitcherComponent} from "./layout-switcher.component";
import { MainLayoutComponent } from './app-layouts/main-layout.component';
import { EmptyLayoutComponent } from './app-layouts/empty-layout.component';
import {RouterModule} from "@angular/router";
import { AuthLayoutComponent } from './app-layouts/auth-layout.component';
import {TooltipModule, BsDropdownModule} from "ngx-bootstrap";

import {UtilsModule} from "../utils/utils.module";

@NgModule({
  imports: [
    CommonModule,
    //HeaderModule,
    RibbonModule,
    NavigationModule,
    FormsModule,
    RouterModule,

    UtilsModule,


    TooltipModule,
    BsDropdownModule,
  ],
  declarations: [
    FooterComponent,
   // RibbonComponent,
    ShortcutComponent,
    LayoutSwitcherComponent,
    MainLayoutComponent,
    EmptyLayoutComponent,
    AuthLayoutComponent,
   
    
  ],
  exports:[
    //HeaderModule,
    NavigationModule,
    FooterComponent,
    RibbonModule,
    ShortcutComponent,
    LayoutSwitcherComponent,
  ]
})
export class SmartadminLayoutModule{

}
