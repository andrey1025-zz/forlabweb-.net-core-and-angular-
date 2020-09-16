import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, NavigationEnd } from "@angular/router";

@Component({
  selector: 'sa-route-breadcrumbs',
  template: `
        <ol class="breadcrumb">
           <h1 *ngFor="let item of items">{{item}}</h1>
        </ol>
  `,
  styles: [`.breadcrumb h1:first-child { display: none; }
            .breadcrumb h1:last-child { color: #2B394E; letter-spacing: 0.1em; font-weight: bold; margin: 7px 0; opacity: 0.7; }
  `]
})
export class RouteBreadcrumbsComponent implements OnInit, OnDestroy {

  public items: Array<string> = [];
  private sub: any;

  constructor(private router: Router) { }

  ngOnInit() {
    this.extract(this.router.routerState.root)
    this.sub = this.router.events
      .filter(e => e instanceof NavigationEnd)
      .subscribe(v => {
        this.items = [];
        this.extract(this.router.routerState.root)
      });
  }

  ngOnDestroy(): void {
    this.sub.unsubscribe()
  }


  extract(route) {
    let pageTitle = route.data.value['pageTitle'];
    if (pageTitle && this.items.indexOf(pageTitle) == -1) {
      this.items.push(route.data.value['pageTitle'])
    }
    if (route.children) {
      route.children.forEach(it => {
        this.extract(it)
      })
    }
  }


}
