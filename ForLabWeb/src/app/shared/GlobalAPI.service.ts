import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions, RequestMethod } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { GlobalVariable } from './globalclass';
import { NotificationService } from "./utils/notification.service";
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/toPromise';

@Injectable() export class GlobalAPIService {
  constructor(private http: Http, private _notificationservice: NotificationService) { }

  token = localStorage.getItem("jwt");
  userid = localStorage.getItem("userid");
  countryid = localStorage.getItem("countryid");
  role = localStorage.getItem("role");
  postAPI(postdata, posttype: string) {

    var body = JSON.stringify(postdata);
    var headerOptions = new Headers({ "Authorization": "Bearer " + this.token, 'userid': this.userid, 'role': this.role, 'Content-Type': 'application/json' });
    var requestOptions = new RequestOptions({ method: RequestMethod.Post, headers: headerOptions });
    return this.http.post(GlobalVariable.BASE_API_URL + posttype, body, requestOptions).map(x => x);
  }
  putAPI(id, putdata, puttype: string) {
    var body = JSON.stringify(putdata);
    var headerOptions = new Headers({ "Authorization": "Bearer " + this.token, 'userid': this.userid, 'role': this.role, 'Content-Type': 'application/json' });
    var requestOptions = new RequestOptions({ method: RequestMethod.Put, headers: headerOptions });
    return this.http.put(GlobalVariable.BASE_API_URL + puttype + '/' + id,
      body,
      requestOptions).map(res => res);
  }
  getDatabyID(id, gettype: string) {
    var headerOptions = new Headers({ "Authorization": "Bearer " + this.token, 'userid': this.userid, 'role': this.role, 'Content-Type': 'application/json' });
    var requestOptions = new RequestOptions({ method: RequestMethod.Get, headers: headerOptions });

    return this.http.get(GlobalVariable.BASE_API_URL + gettype + '/' + id, requestOptions)
      .map((response: Response) =>
        response.json()

        // console.log(response.json()) 
      )
      .catch(this.errorHandler);


  }
  getDataList(type: string) {

    var headerOptions = new Headers({ "Authorization": "Bearer " + this.token, 'userid': this.userid, 'role': this.role, 'countryid': this.countryid, 'Content-Type': 'application/json' });
    var requestOptions = new RequestOptions({ method: RequestMethod.Get, headers: headerOptions });

    return this.http.get(GlobalVariable.BASE_API_URL + type, requestOptions)
      .map((response: Response) => response.json())
    // .catch(this.errorHandler);  

  }
  deleteData(id: number, deltype: string) {
    var headerOptions = new Headers({ "Authorization": "Bearer " + this.token, 'userid': this.userid, 'role': this.role, 'Content-Type': 'application/json' });
    var requestOptions = new RequestOptions({ method: RequestMethod.Get, headers: headerOptions });

    return this.http.delete(GlobalVariable.BASE_API_URL + deltype + '/' + id, requestOptions).map((response: Response) => response);
  }
  errorHandler(error: Response) {
    console.log(error);
    return Observable.throw(error);
  }

  SuccessMessage(getcontent: string) {

    this._notificationservice.smallBox({
      title: "Success",
      content: getcontent,
      color: "#20BDEF",
      iconSmall: "fa fa-check fa-2x fadeInRight animated",
      timeout: 5000,
      sound_off: true
    });
  }
  FailureMessage(getcontent: string) {
    this._notificationservice.smallBox({
      title: "Error",
      content: getcontent,
      color: "#C46A69",
      iconSmall: "fa fa-times fa-2x fadeInRight animated",
      timeout: 4000

    });
  }



  GetAdjustedVolume(reported: number, dos: number, period: string, workingdays: number) {

    let y: number = 0;
    if (period == "Monthly")
      y = workingdays;
    if (period == "Bimonthly")
      y = workingdays * 2;
    if (period == "Quarterly")
      y = workingdays * 3;
    if (period == "Yearly")
      y = workingdays * 12;

    return Math.round(((reported * y) / (y - dos)));
  }
}