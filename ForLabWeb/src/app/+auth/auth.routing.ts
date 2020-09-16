
import { Routes, RouterModule } from "@angular/router";
import { AuthComponent } from "./auth.component";
import { VerifylinkComponent } from "./verifylink/verifylink.component";
import { ForgotComponent } from "./+forgot/forgot.component";
import { ResetpasswordComponent } from "./resetpassword/resetpassword.component";
import { LandingpageComponent } from "./landingpage/landingpage.component";
import { LandingpagenewComponent } from "./landingpagenew/landingpagenew.component";
export const routes: Routes = [
  {
    path: '',
    component: AuthComponent,
    children: [
      {
        path: '',
        redirectTo: 'landingpage',
        pathMatch: 'full',

      },

      {
        path: 'landingpage',
        component: LandingpagenewComponent
      },
      {
        path: 'landing',
        component: LandingpageComponent
      },
      {
        path: 'forgot-password',
        component: ForgotComponent
      },
      {
        path: 'verifylink/:id',
        component: VerifylinkComponent
      },
      {
        path: 'resetpassword/:id',
        component: ResetpasswordComponent
      },
    ]
  }
];

export const routing = RouterModule.forChild(routes);
