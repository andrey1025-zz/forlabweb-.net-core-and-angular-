import { Component, OnInit } from '@angular/core';
import { UserService } from "../user.service";
import { LayoutService } from "../../layout/layout.service";

@Component({

  selector: 'sa-login-info',
  templateUrl: './login-info.component.html',
  styles: [`
          .user-info { width: 150px; height: 36px; border: none; box-shadow: none; margin:17px 64px auto}
          .user-info a { width: 100%; display: inline-block; }
          .user-info a img { width: 36px; height: 36px; border-radius: 18px; margin: 0 12px 0 0; border: none; }
          .user-info a span { font-size: 14px; letter-spacing: 0.1em; color: #2B394E; }
  `]
})
export class LoginInfoComponent implements OnInit {

  user: any;
  name: any;
  constructor(
    private userService: UserService,
    private layoutService: LayoutService) {
  }

  ngOnInit() {
    this.name = localStorage.getItem("username")
    this.userService.getLoginInfo().subscribe(user => {
      this.user = user
    })

  }

  toggleShortcut() {
    this.layoutService.onShortcutToggle()
  }

}
