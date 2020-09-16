import { Injectable, EventEmitter } from '@angular/core';
import { Http, Response, Headers, RequestOptions, RequestMethod, } from '@angular/http';
import { HttpHeaders, HttpClient } from "@angular/common/http"
import { Observable } from 'rxjs/Observable';
import { GlobalVariable } from './globalclass';
import { NotificationService } from "./utils/notification.service";
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/toPromise';


@Injectable(
) export class APIwithActionService {
  constructor(private http1: HttpClient, private http: Http, private _notificationservice: NotificationService) { }



  GetDataUsage = new EventEmitter<any>();
  Getpatientnumber = new EventEmitter<any>();
  Getsitesfordatausage2 = new EventEmitter<any>();


  postAPI1(postdata, posttype: string, actionname: string) {
    var body = JSON.stringify(postdata);
    //  console.log(body);
    var token = localStorage.getItem("jwt");
    var userid = localStorage.getItem("userid")
    var headerOptions = new Headers({ 'Content-Type': 'application/json' });
    var requestOptions = new RequestOptions({ method: RequestMethod.Post });
    return this.http.post(GlobalVariable.BASE_API_URL + posttype + '/' + actionname, body, requestOptions).map(x => x);
  }
  postAPI(postdata, posttype: string, actionname: string) {
    console.log(postdata);
    var body = JSON.stringify(postdata);
    var token = localStorage.getItem("jwt");
    var userid = localStorage.getItem("userid")
    var role = localStorage.getItem("role")
    var headerOptions = new Headers({ "Authorization": "Bearer " + token, 'userid': userid, 'role': role, 'Content-Type': 'application/json' });
    var requestOptions = new RequestOptions({ method: RequestMethod.Post, headers: headerOptions });
    return this.http.post(GlobalVariable.BASE_API_URL + posttype + '/' + actionname, body, requestOptions).map(x => x);
  }
  putAPI(id, putdata, puttype: string, actionname: string) {
    var body = JSON.stringify(putdata);
    var token = localStorage.getItem("jwt");
    var userid = localStorage.getItem("userid")
    var role = localStorage.getItem("role")
    var headerOptions = new Headers({ "Authorization": "Bearer " + token, 'userid': userid, 'role': role, 'Content-Type': 'application/json' });
    var requestOptions = new RequestOptions({ method: RequestMethod.Put, headers: headerOptions });
    return this.http.put(GlobalVariable.BASE_API_URL + puttype + '/' + actionname + '/' + id,
      body,
      requestOptions).map(res => res || {});
  }
  getDatabyID(id, gettype: string, actionname: string, parameter: string = '') {
    var path = '';
    var token = localStorage.getItem("jwt");
    var userid = localStorage.getItem("userid");
    var countryid = localStorage.getItem("countryid");
    var role = localStorage.getItem("role")
    var headerOptions = new Headers({ "Authorization": "Bearer " + token, 'userid': userid, 'role': role, 'countryid': countryid, 'Content-Type': 'application/json' });
    var requestOptions = new RequestOptions({ method: RequestMethod.Get, headers: headerOptions });

    if (parameter != '') {
      path = GlobalVariable.BASE_API_URL + gettype + '/' + actionname + '/' + id + '?' + parameter
    }
    else {
      path = GlobalVariable.BASE_API_URL + gettype + '/' + actionname + '/' + id
    }
    return this.http.get(path, requestOptions)
      .map((response: Response) =>

        response.json()


        // console.log(response.json()) 
      )
      .catch(this.errorHandler);


  }
  getDataList(type: string, actionname: string, param: string = "") {
    var token = localStorage.getItem("jwt");
    var userid = localStorage.getItem("userid");
    var countryid = localStorage.getItem("countryid");
    var role = localStorage.getItem("role")
    var headerOptions = new Headers({ "Authorization": "Bearer " + token, 'userid': userid, 'role': role, 'countryid': countryid, 'Content-Type': 'application/json' });
    var requestOptions = new RequestOptions({ method: RequestMethod.Get, headers: headerOptions });
    var path = '';
    if (param != '') {
      path = GlobalVariable.BASE_API_URL + type + '/' + actionname + '?' + param
    }
    else {
      path = GlobalVariable.BASE_API_URL + type + '/' + actionname
    }
    return this.http.get(path, requestOptions)
      .map((response: Response) => response.json())
    // .catch(this.errorHandler);  

  }
  deleteData(id: any, deltype: string, actionname: string, parameter: string = '') {
    var path = '';
    var token = localStorage.getItem("jwt");
    var userid = localStorage.getItem("userid")

    var headerOptions = new Headers({ "Authorization": "Bearer " + token, 'userid': userid, 'Content-Type': 'application/json' });
    var requestOptions = new RequestOptions({ method: RequestMethod.Delete, headers: headerOptions });

    if (parameter != '') {
      path = GlobalVariable.BASE_API_URL + deltype + '/' + actionname + '/' + id + '?' + parameter
    }
    else {
      path = GlobalVariable.BASE_API_URL + deltype + '/' + actionname + '/' + id
    }
    return this.http.delete(path, requestOptions).map((response: Response) => response);
  }
  errorHandler(error: Response) {
    console.log(error);
    return Observable.throw(error);
  }

  SuccessMessage(getcontent: string) {

    this._notificationservice.smallBox({
      title: "Success",
      content: getcontent,
      color: "#659265",
      iconSmall: "fa fa-check fa-2x fadeInRight animated",
      timeout: 4000
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
}