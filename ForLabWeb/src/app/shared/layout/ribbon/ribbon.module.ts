import {CommonModule} from "@angular/common";
import {FormsModule} from "@angular/forms";
import {NgModule} from "@angular/core";

import {PopoverModule} from "ngx-popover";

import {CollapseMenuComponent} from "../header/collapse-menu/collapse-menu.component"

import {FullScreenComponent} from "../header/full-screen/full-screen.component";

import {ActivitiesComponent} from "../header/activities/activities.component";
import {ActivitiesMessageComponent} from "../header/activities/activities-message/activities-message.component";
import {ActivitiesNotificationComponent} from "../header/activities/activities-notification/activities-notification.component";
import {ActivitiesTaskComponent} from "../header/activities/activities-task/activities-task.component";
 import {RibbonComponent} from "./ribbon.component";

 import {RouteBreadcrumbsComponent} from "../ribbon/route-breadcrumbs.component"
import {UtilsModule} from "../../utils/utils.module";
import {I18nModule} from "../../i18n/i18n.module";
import {UserModule} from "../../user/user.module";
import {VoiceControlModule} from "../../voice-control/voice-control.module";
import {BsDropdownModule} from "ngx-bootstrap";
import {TooltipModule} from "ngx-bootstrap";

@NgModule({
  imports: [
    CommonModule,
    TooltipModule,
    FormsModule,

    VoiceControlModule,

    BsDropdownModule,

    UtilsModule, I18nModule, UserModule, PopoverModule,
  ],
  declarations: [
    ActivitiesMessageComponent,
    ActivitiesNotificationComponent,
    ActivitiesTaskComponent,
    RouteBreadcrumbsComponent,
   // RecentProjectsComponent,
    FullScreenComponent,
    CollapseMenuComponent,
    RibbonComponent,
     ActivitiesComponent,
   
    // HeaderComponent,
  ],
  exports: [
    RibbonComponent
  ]
})
export class RibbonModule{}
