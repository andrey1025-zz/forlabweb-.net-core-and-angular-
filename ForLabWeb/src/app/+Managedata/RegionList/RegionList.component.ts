import { Component, Inject, OnInit, ElementRef, AfterViewInit, Renderer, ViewChild, Directive, TemplateRef } from '@angular/core';
import { Http, Headers } from '@angular/http';
// import {DataTables} from 'angular-datatables';
import { Router, ActivatedRoute, Route } from '@angular/router';
import { GlobalAPIService } from '../../shared/GlobalAPI.service';
import { APIwithActionService } from '../../shared/APIwithAction.service';
import { FadeInTop } from "../../shared/animations/fade-in-top.decorator";
import { NotificationService } from "../../shared/utils/notification.service";
import { GlobalVariable } from '../../shared/globalclass'
import * as XLSX from 'xlsx';
import * as FileSaver from 'file-saver';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { ManageDatatableComponent } from 'app/shared/ui/datatable/ManageDatatable.component';
declare var $: any;
declare let pdfMake: any;

@FadeInTop()
@Component({
  selector: 'app-RegionList',
  templateUrl: './RegionList.component.html'
})
export class RegionlistComponent implements AfterViewInit, OnInit {

  public options;
  @ViewChild(ManageDatatableComponent) DataView: ManageDatatableComponent
  @ViewChild(TemplateRef) regionaddModal: any;
  id: number;
  rowindex: number;
  bsRegionAddModalRef: BsModalRef;

  constructor(public http: Http, private notificationService: NotificationService, private _router: Router, private _render: Renderer,
    private _GlobalAPIService: GlobalAPIService, private _APIwithActionService: APIwithActionService, private modalService: BsModalService) {

  }
  delete(RegionID) {

    let table = document.querySelector('table');
    this._GlobalAPIService.deleteData(RegionID, 'Region').subscribe((data) => {
      this._render.setElementStyle(table.rows[this.rowindex], 'display', 'none')
    }, error => alert(error))

  }
  ngAfterViewInit(): void {
    document.querySelector('body').addEventListener('click', (event) => {
      let target = <Element>event.target;// Cast EventTarget into an Element

      if (target.className.includes('Edit')) {
        // this._router.navigate(["/Managedata/RegionAdd", target.getAttribute('data-reg-id')]);

      }
      if (target.className.includes('del')) {
        // this.id = parseInt(target.getAttribute('data-reg-id'));
        // this.rowindex = target.parentElement.parentElement["rowIndex"];
        // this.smartModEg1();
      }
    });
  }
  smartModEg1() {
    this.notificationService.smartMessageBox({
      title: "Deletion",
      content: "Do you want to delete Region having id " + this.id,
      buttons: '[No][Yes]'
    }, (ButtonPressed) => {
      if (ButtonPressed === "Yes") {
        let table = document.querySelector('table');
        this._GlobalAPIService.deleteData(this.id, 'Region').subscribe((data) => {
          if (data["_body"] != 0) {
            this._render.setElementStyle(table.rows[this.rowindex], 'display', 'none')
            this.notificationService.smallBox({
              title: "Deletion",
              content: "<i class='fa fa-clock-o'></i> <i>Region Deleted</i>",
              color: "#659265",
              iconSmall: "fa fa-check fa-2x fadeInRight animated",
              timeout: 4000
            });

          }
          else {
            this.notificationService.smallBox({
              title: "Cancelation",
              content: "<i class='fa fa-clock-o'></i> <i>Region already used so you could not delete this Region</i>",
              color: "#C46A69",
              iconSmall: "fa fa-times fa-2x fadeInRight animated",
              timeout: 4000
            });
          }
        })
      }
      if (ButtonPressed === "No") {
        this.notificationService.smallBox({
          title: "Cancelation",
          content: "<i class='fa fa-clock-o'></i> <i>Deletion Cancelled</i>",
          color: "#C46A69",
          iconSmall: "fa fa-times fa-2x fadeInRight animated",
          timeout: 4000
        });
      }
    });
  }

  ngOnInit() {
    const router = this._router
    let regionlist = new Array();
    let regionlist1 = new Array();
    const fileType = 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=UTF-8';
    const fileExtension = '.xlsx';

    var aButtonsData = [];
    aButtonsData.push({ text: '', className: 'btn-rect' });
    aButtonsData.push({
      text: '',
      className: 'btn-export',
      action: function (e, dt, node, config) {
        regionlist1.push({
          RegionName: "Region",
          ShortName: "Short Name",
        })
        regionlist.forEach(element => {
          regionlist1.push({
            RegionName: element.regionName,
            ShortName: element.shortName,
          })
        });
        const ws: XLSX.WorkSheet = XLSX.utils.json_to_sheet(regionlist1, { skipHeader: true });//{header:["Testing Area","Instrument Name","Max Through Put","Per Test Control","Daily Control Test","Weekly Control Test","Monthly Control Test","Quarterly control Test"]}
        const wb: XLSX.WorkBook = { Sheets: { 'Region': ws }, SheetNames: ['Region'] };
        const excelBuffer: any = XLSX.write(wb, { bookType: 'xlsx', type: 'array', cellStyles: true });
        const data1: Blob = new Blob([excelBuffer], { type: fileType });
        FileSaver.saveAs(data1, "Region List" + fileExtension);
      }
    })
    aButtonsData.push({ text: '', extend: 'pdf', className: 'btn-pdf', filename: 'Region List' });
    aButtonsData.push({ text: '', extend: 'print', className: 'btn-print' })
    if (localStorage.getItem("role") == "admin") {
      aButtonsData.push({
        text: 'Import',
        className: 'btn-import',
        action: function (e, dt, node, config) {
          // router.navigate(["/ImportData"])
        }
      });
    }
    aButtonsData.push({
      text: 'New Region',
      className: 'btn-new',
      action: (e, dt, node, config) => {
        // router.navigate(["/Managedata/RegionAdd"])
        this.bsRegionAddModalRef = this.modalService.show(this.regionaddModal, { class: 'modal-regionadd', ignoreBackdropClick: false });
      }
    })
    this.DataView.options =
    {
      dom: "Bfrtip",
      aaSorting: [],
      ajax: (data, callback, settings) => {

        this._APIwithActionService.getDatabyID(localStorage.getItem("countryid"), 'Site', 'GetregionbyCountryID')
          .subscribe((data) => {
            regionlist = data
            callback({
              aaData: data,
            })
          })
      },

      columns: [
        { data: 'regionID' },
        { data: 'regionName' },
        { data: 'countryName' },
        {
          render: (data, type, fullRow, meta) => {
            return `
                      <a class='Edit icon-edit' data-reg-id="${fullRow.regionID}"></a>
                      <a class='del icon-trash' data-reg-id="${fullRow.regionID}"></a>
                    `;
          }
        }
      ],
      buttons: aButtonsData
    };

  }

  onRegionAddClose() {
    this.bsRegionAddModalRef.hide();
  }

}