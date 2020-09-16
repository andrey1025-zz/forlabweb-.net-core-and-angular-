import { Component, OnInit } from '@angular/core';
import { Router } from "@angular/router";
import { NotificationService } from "../../utils/notification.service";

declare var $: any;

@Component({
  selector: 'sa-logout',
  template: `
        <div id="logout" (click)="showPopup()" class="btn-header transparent pull-right">
          <span> <a (click)="logout()" title="Sign Out" data-action="userLogout" 
            data-logout-msg="You can improve your security further after logging out by closing this opened browser"> 
          <i class="fa fa-power-off"></i></a> </span>
        </div>
  `,
  styles: [`
        .btn-header.pull-right { margin: 0; }
        .btn-header>:first-child>a { background: none; margin: 27px 0 0 0; padding: 0; height: 24px; min-width: 24px; color: #F35A48; border: none; cursor: pointer !important; }
        .btn-header a .fa-power-off { font-size: 24px; }
  `]
})
export class LogoutComponent implements OnInit {

  constructor(private router: Router,
    private notificationService: NotificationService) { }

  showPopup() {
    this.notificationService.smartMessageBox({
      title: "<i class='fa fa-power-off txt-color-orangeDark'></i> Logout <span class='txt-color-orangeDark'><strong>" + $('#show-shortcut').text() + "</strong></span> ?",
      content: "You can improve your security further after logging out by closing this opened browser",
      buttons: '[No][Yes]'

    }, (ButtonPressed) => {
      if (ButtonPressed == "Yes") {
        this.logout()
      }
    });
  }

  logout() {
    localStorage.removeItem("jwt");
    localStorage.removeItem("username");
    localStorage.setItem("userid", "0");
    this.router.navigate(['/'])
  }

  ngOnInit() {

  }



}
