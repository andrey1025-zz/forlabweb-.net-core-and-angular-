import { Component, Inject, OnInit, ElementRef, AfterViewInit, Renderer, ViewChild, Directive, TemplateRef } from '@angular/core';
import { Http, Headers } from '@angular/http';
// import {DataTables} from 'angular-datatables';
import { Router, ActivatedRoute, Route } from '@angular/router';
import { APIwithActionService } from '../../shared/APIwithAction.service';
import { FadeInTop } from "../../shared/animations/fade-in-top.decorator";
import { SiteCategory } from '../Category.model';
import { NotificationService } from "../../shared/utils/notification.service";
import { GlobalVariable } from '../../shared/globalclass'
import * as XLSX from 'xlsx';
import * as FileSaver from 'file-saver';
import { BsModalService, BsModalRef } from 'ngx-bootstrap';
import { ManageDatatableComponent } from 'app/shared/ui/datatable/ManageDatatable.component';

declare var $: any;
declare let pdfMake: any;

@FadeInTop()
@Component({
  selector: 'app-SiteList',
  templateUrl: './SiteList.component.html'
})
export class SiteListComponent implements AfterViewInit, OnInit {

  public options;
  @ViewChild(ManageDatatableComponent) DataView: ManageDatatableComponent
  @ViewChild(TemplateRef) stepOneModal: any;
  @ViewChild(TemplateRef) stepTwoModal: any;

  id: string;
  rowIndex: number;
  modalRef1: BsModalRef;
  modalRef2: BsModalRef;

  constructor(
    public http: Http,
    private notificationService: NotificationService,
    private _router: Router,
    private _render: Renderer,
    private _APIwithActionService: APIwithActionService,
    private modalService: BsModalService
  ) {

  }
  delete(SiteID) {

    let table = document.querySelector('table');
    this._APIwithActionService.deleteData(SiteID, 'Site', 'Del01').subscribe((data) => {
      this._render.setElementStyle(table.rows[this.rowIndex], 'display', 'none')
    }, error => alert(error))

  }
  ngAfterViewInit(): void {
    document.querySelector('body').addEventListener('click', (event) => {
      let target = <Element>event.target;// Cast EventTarget into an Element

      if (target.className.includes('Edit')) {
        // this._router.navigate(["/Managedata/SiteAdd", target.getAttribute('data-site-id')]);

      }
      if (target.className.includes('del')) {
        // this.id = target.getAttribute('data-site-id');
        // this.rowIndex = target.parentElement.parentElement["rowIndex"];
        // this.smartModEg1();
      }
    });

  }
  smartModEg1() {
    this.notificationService.smartMessageBox({
      title: "Deletion",
      content: "Do you want to delete " + this.id + " Site",
      buttons: '[No][Yes]'
    }, (ButtonPressed) => {
      if (ButtonPressed === "Yes") {
        this.delete(this.id);
        this.notificationService.smallBox({
          title: "Deletion",
          content: "<i class='fa fa-clock-o'></i> <i>Site Deleted</i>",
          color: "#659265",
          iconSmall: "fa fa-check fa-2x fadeInRight animated",
          timeout: 4000
          // function:this.delete(SiteCategory)
        });

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
    const router = this._router;
    let siteList = new Array();
    let siteList1 = new Array();
    const fileType = 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=UTF-8';
    const fileExtension = '.xlsx';

    var aButtonsData = [];
    aButtonsData.push({ text: '', className: 'btn-rect' });
    aButtonsData.push({
      text: '',
      className: 'btn-export',
      action: function (e, dt, node, config) {
        siteList1.push({
          Region: "Region",
          SiteCategory: "Site Category",
          SiteName: "Site Name",
          WorkingDays: "Working Days"
        })
        siteList.forEach(element => {
          siteList1.push({
            Region: element.regionName,
            SiteCategory: element.categoryName,
            SiteName: element.siteName,
            WorkingDays: element.workingDays,
          })
        });
        const ws: XLSX.WorkSheet = XLSX.utils.json_to_sheet(siteList1, { skipHeader: true });//{header:["Testing Area","Instrument Name","Max Through Put","Per Test Control","Daily Control Test","Weekly Control Test","Monthly Control Test","Quarterly control Test"]}
        const wb: XLSX.WorkBook = { Sheets: { 'Site': ws }, SheetNames: ['Site'] };
        const excelBuffer: any = XLSX.write(wb, { bookType: 'xlsx', type: 'array', cellStyles: true });
        const data1: Blob = new Blob([excelBuffer], { type: fileType });
        FileSaver.saveAs(data1, "Site List" + fileExtension);
      }
    })

    aButtonsData.push({ text: '', extend: 'pdf', className: 'btn-pdf', filename: 'Site List' });
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
      text: 'New Site',
      className: 'btn-new',
      action: (e, dt, node, config) => {
        this.openFirstModal();
      }
    })

    this.DataView.options = {
      dom: "Bfrtip",
      aaSorting: [],
      ajax: (data, callback, settings) => {
        this._APIwithActionService.getDatabyID(localStorage.getItem("countryid"), 'Site', 'GetAll')
          .subscribe((data) => {
            siteList = data.aaData
            callback({
              aaData: data.aaData,
            })
          })
      },

      columns: [
        { data: 'siteID' },
        { data: 'siteName' },
        { data: 'countryName' },
        { data: 'regionName' },
        { data: 'categoryName' },
        { data: 'workingDays' },
        // { data: 'getLastOpenDate' },
        // { data: 'getLastClosedDate' },
        {
          render: (data, type, fullRow, meta) => {
            return `
                      <a class='Edit icon-edit' data-site-id="${fullRow.siteID}"></a>
                      <a class='del icon-trash' data-site-id="${fullRow.siteID}"></a>
                        `;
          }
        }
      ],
      buttons: aButtonsData
    };
  }

  openFirstModal() {
    if (this.modalRef2) {
      this.closeSecondModal();
    }
    this.modalRef1 = this.modalService.show(this.stepOneModal, { class: 'modal-siteadd', ignoreBackdropClick: false });
  }

  closeFirstModal() {
    if (!this.modalRef1) {
      return;
    }

    this.modalRef1.hide();
    this.modalRef1 = null;
  }

  openSecondModal(template: TemplateRef<any>) {
    this.closeFirstModal();
    this.modalRef2 = this.modalService.show(template, { class: 'modal-siteadd', ignoreBackdropClick: false });
  }

  closeSecondModal() {
    if (!this.modalRef2) {
      return;
    }

    this.modalRef2.hide();
    this.modalRef2 = null;
  }
}
