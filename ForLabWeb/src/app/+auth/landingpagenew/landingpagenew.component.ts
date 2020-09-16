import { Component, OnInit, TemplateRef } from '@angular/core';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { GlobalAPIService } from "../../shared/GlobalAPI.service";
import { APIwithActionService } from "../../shared/APIwithAction.service";
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BsModalService, BsModalRef } from 'ngx-bootstrap';
import * as Highcharts from 'highcharts/highmaps';
import * as map from "@highcharts/map-collection/custom/world.geo.json";

@Component({
  selector: 'app-landingpagenew',
  templateUrl: './landingpagenew.component.html',
  styleUrls: ['./landingpagenew.component.css']
})
export class LandingpagenewComponent implements OnInit {
  bsLoginModalRef: BsModalRef;
  bsRegisterModalRef: BsModalRef;
  bsForgotpwdModalRef: BsModalRef;
  menucol: string = "collapse";
  faq1: string = "";
  messageForm: FormGroup;
  faq2: string = "";
  faq3: string = "";
  faq4: string = "";
  faq5: string = "";
  faq6: string = "";
  url: string = "";
  cms = new Array();
  countrylist = new Array();
  total_trainedarea = 0;
  total_usedarea = 0;
  public data = {
    "Nigeria": 3, "Uganda": 1, "Cameroon": 1, "Ethiopia": 3, "Congo, the Democratic Republic of the": 3, "Kenya": 1, "Tanzania, United Republic of": 1, "Myanmar": 1, "Viet Nam": 1
    , "Zimbabwe": 3, "Mozambique": 3, "Lesotho": 1, "South Africa": 1, "Botswana": 1, "Côte d'Ivoire": 3
  };
  downloads = ["", "", "", "", "", "", "", "", "", "", "",];

  constructor(private _fb: FormBuilder, private modalService: BsModalService, private _APIwithActionService: APIwithActionService, private _GlobalAPIService: GlobalAPIService, public domSanitizer: DomSanitizer) {

    this._APIwithActionService.getDataList('CMS', 'Get').subscribe((data) => {
      this.cms = data
    })
  }

  ngOnInit() {
    this.messageForm = this._fb.group({
      Name: ['', Validators.required],
      Email: ['', Validators.required],
      Message: ['', Validators.required]
    });
    this.onDisplayMap();
  }

  scroll(el: HTMLElement) {
    el.scrollIntoView({ behavior: 'smooth' });
  }

  collapsedclas() {
    if (this.menucol == "collapse") {
      this.menucol = "";
    } else {
      this.menucol = "collapse";
    }
  }
  clickfaq1() {
    if (this.faq1 == "cd-faq__content--visible") {
      this.faq1 = "";
    } else {
      this.faq1 = "cd-faq__content--visible";
    }

  }
  clickfaq2() {
    if (this.faq2 == "cd-faq__content--visible") {
      this.faq2 = "";
    } else {
      this.faq2 = "cd-faq__content--visible";
    }

  }
  clickfaq3() {
    if (this.faq3 == "cd-faq__content--visible") {
      this.faq3 = "";
    } else {
      this.faq3 = "cd-faq__content--visible";
    }

  }
  clickfaq4() {
    if (this.faq4 == "cd-faq__content--visible") {
      this.faq4 = "";
    } else {
      this.faq4 = "cd-faq__content--visible";
    }

  }
  clickfaq5() {
    if (this.faq5 == "cd-faq__content--visible") {
      this.faq5 = "";
    } else {
      this.faq5 = "cd-faq__content--visible";
    }

  }
  clickfaq6() {
    if (this.faq6 == "cd-faq__content--visible") {
      this.faq6 = "";
    } else {
      this.faq6 = "cd-faq__content--visible";
    }

  }
  sendmessage() {
    this._APIwithActionService.postAPI(this.messageForm.value, 'CMS', 'sendmail').subscribe((data) => {
    })
  }

  onLogin(event, template: TemplateRef<any>) {
    event.preventDefault();
    this.bsLoginModalRef = this.modalService.show(template, { class: 'modal-login', ignoreBackdropClick: false });
  }

  onLoginClose() {
    this.bsLoginModalRef.hide();
  }

  openForgotpwd(template: TemplateRef<any>) {
    this.bsLoginModalRef.hide();
    this.bsForgotpwdModalRef = this.modalService.show(template, { class: 'modal-forgotpwd', ignoreBackdropClick: false });
  }

  onRegister(event, template: TemplateRef<any>) {
    event.preventDefault();
    this.bsRegisterModalRef = this.modalService.show(template, { class: 'modal-register', ignoreBackdropClick: false });
  }

  onRegisterClose() {
    this.bsRegisterModalRef.hide()
  }

  onForgotpwdClose() {
    this.bsForgotpwdModalRef.hide()
  }

  openLogin(template: TemplateRef<any>) {
    this.bsForgotpwdModalRef.hide();
    this.bsLoginModalRef = this.modalService.show(template, { class: 'modal-login', ignoreBackdropClick: false });
  }

  onDisplayMap() {
    let obj = new Object();
    let data1 = new Array();
    var traning_area = 'url(assets/img/Landingimage/map-pin-training.png)';
    var traning_color = '#EFBF20';
    var used_area = 'url(assets/img/Landingimage/map-pin-used.png)';
    var used_color = '#20BDEF';
    this._APIwithActionService.getDataList('Country', 'Countrylistusedortraine').subscribe((data) => {

      this.countrylist = data;
      for (let index = 0; index < this.countrylist.length; index++) {

        if (this.countrylist[index]["z"] == 1) {
          this.total_trainedarea+=1;
        }    
        else {
          this.total_usedarea+=1;
        }  


        obj =
        {
          name: this.countrylist[index]["name"],
          lat: this.countrylist[index]["lat"],
          lon: this.countrylist[index]["lon"],
          z: this.countrylist[index]["z"],
          color: this.countrylist[index]["z"] == 1 ? traning_color : used_color,
          marker: {
            symbol: this.countrylist[index]["z"] == 1 ? traning_area : used_area,
          }
        }
        data1.push(obj);

      }
console.log(data1)
      Highcharts.mapChart('world-map', {
        chart: {
          map: map,
          backgroundColor: "#F4F5F6",
          events: {
            load: function () {
              this.series[0].data = this.series[0].data.map((el) => {
                el.color = "#2B394E"
                return el
              })

              this.update({
                series: [{
                  data: this.series[0].data
                }]
              })
            }
          }
        },
        title: {
          text: false,
        },
        legend: false,
        credits: {
          enabled: false
        },
        tooltip: {
          // headerFormat: null,
          // borderWidth: 0,
          // borderRadius: 15,
          // pointFormat: '<span style="background-color:yellow">{point.name}</span>'
          shared: false,
          useHTML: true,
          borderWidth: 0,
          shadow: false,
          backgroundColor: 'rgba(255,255,255,0)',
          formatter: function () {
            return '<div class="myTooltip" style="background-color:' + this.color + ';">' + this.key + '</div>';
          }

        },
        series: [{
          showInLegend: false,
          borderColor: "#2B394E"

        }, {
          minSize: 12,
          maxSize: 24,
          type: 'mapbubble',
          data: data1,
          // [{
          //   name: "Nigeria",
          //   lat: 1.373333,
          //   lon: 32.290275,
          //   z: 1,
          //   color: traning_color,
          //   marker: {
          //     symbol: traning_area
          //   }
          // }, {
          //   name: "Uganda",
          //   lat: 9.081999,
          //   lon: 8.675277,
          //   z: 3,
          //   color: used_color,
          //   marker: {
          //     symbol: used_area
          //   }
          // }, {
          //   name: "Cameroon",
          //   lat: 7.369722,
          //   lon: 12.354722,
          //   z: 1,
          //   color: traning_color,
          //   marker: {
          //     symbol: traning_area
          //   }
          // }, {
          //   name: "Ethiopia",
          //   lat: 9.145,
          //   lon: 40.489673,
          //   z: 3,
          //   color: used_color,
          //   marker: {
          //     symbol: used_area
          //   }
          // }, {
          //   name: "Congo, the Democratic Republic of the",
          //   lat: -4.038333,
          //   lon: 21.758664,
          //   z: 3,
          //   color: used_color,
          //   marker: {
          //     symbol: used_area
          //   }
          // }, {
          //   name: "Kenya",
          //   lat: -0.023559,
          //   lon: 37.906193,
          //   z: 1,
          //   color: traning_color,
          //   marker: {
          //     symbol: traning_area
          //   }
          // }, {
          //   name: "Tanzania, United Republic of",
          //   lat: -6.369028,
          //   lon: 34.888822,
          //   z: 1,
          //   color: traning_color,
          //   marker: {
          //     symbol: traning_area
          //   }
          // }, {
          //   name: "Myanmar",
          //   lat: 21.913965,
          //   lon: 95.956223,
          //   z: 1,
          //   color: used_color,
          //   marker: {
          //     symbol: used_area
          //   }
          // }, {
          //   name: "Vietnam",
          //   lat: 14.058324,
          //   lon: 108.277199,
          //   z: 1,
          //   color: used_color,
          //   marker: {
          //     symbol: used_area
          //   }
          // }, {
          //   name: "Zimbabwe",
          //   lat: -19.015438,
          //   lon: 29.154857,
          //   z: 3,
          //   color: traning_color,
          //   marker: {
          //     symbol: traning_area
          //   }
          // }, {
          //   name: "Mozambique",
          //   lat: -18.665695,
          //   lon: 35.529562,
          //   z: 3,
          //   color: used_color,
          //   marker: {
          //     symbol: used_area
          //   }
          // }, {
          //   name: "South Africa",
          //   lat: -30.559482,
          //   lon: 22.937506,
          //   z: 3,
          //   color: traning_color,
          //   marker: {
          //     symbol: traning_area
          //   }
          // }],
        }]
      });
    })
  }
}
