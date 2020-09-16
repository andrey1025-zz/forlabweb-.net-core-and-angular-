import { Component, Inject, OnInit, ElementRef, AfterViewInit, Renderer, ViewChild, Directive, TemplateRef } from '@angular/core';
import { Http, Headers } from '@angular/http';
// import {DataTables} from 'angular-datatables';
import { Router, ActivatedRoute, Route } from '@angular/router';
import { GlobalAPIService } from '../../shared/GlobalAPI.service';
import { FadeInTop } from "../../shared/animations/fade-in-top.decorator";
import { SiteCategory } from '../CategoryAdd/Category.model';
import { NotificationService } from "../../shared/utils/notification.service";
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { ManageDatatableComponent } from 'app/shared/ui/datatable/ManageDatatable.component';

import * as XLSX from 'xlsx';
import * as FileSaver from 'file-saver';

import { ModalDirective } from "ngx-bootstrap";
import { ScriptService } from '../Managedata.service';
declare let pdfMake: any;
declare var $: any;
@FadeInTop()
@Component({
  selector: 'app-CategoryList',
  templateUrl: './CategoryList.component.html'
})
export class CategoryListComponent implements AfterViewInit, OnInit {
  public catList: SiteCategory[];
  public options;
  @ViewChild(ManageDatatableComponent) DataView: ManageDatatableComponent
  @ViewChild(TemplateRef) cateaddModal: any;
  id: number;
  rowindex: number;
  bsCateAddModalRef: BsModalRef;

  constructor(public http: Http, private notificationService: NotificationService, private _router: Router, private _render: Renderer,
    private _GlobalAPIService: GlobalAPIService, private modalService: BsModalService) {
    this.getSiteCategories();
  }
  getSiteCategories() {
    this._GlobalAPIService.getDataList('SiteCategory').subscribe((data) => {
      this.catList = data
      console.log(this.catList)
    }
    ), err => {
      console.log(err);
    }

  }
  delete(employeeID): boolean {
    let res: boolean;
    let table = document.querySelector('table');
    this._GlobalAPIService.deleteData(employeeID, 'SiteCategory').subscribe((data) => {
      if (data["_body"] != 0) {
        this._render.setElementStyle(table.rows[this.rowindex], 'display', 'none')
        res = true;

      }
      else {
        res = false;
      }


    })
    return res
  }
  ngAfterViewInit(): void {
    document.querySelector('body').addEventListener('click', (event) => {
      let target = <Element>event.target;// Cast EventTarget into an Element

      if (target.className.includes('Edit')) {
        // this._router.navigate(["/Managedata/CategoryAdd", target.getAttribute('data-cat-id')]);

      }
      if (target.className.includes('del')) {
        // this.id = parseInt(target.getAttribute('data-cat-id'));
        // this.rowindex = target.parentElement.parentElement["rowIndex"];
        // this.smartModEg1();
        // this._router.navigate(['/Category/CategoryList']);  
      }
    });

  }
  smartModEg1() {
    this.notificationService.smartMessageBox({
      title: "Deletion",
      content: "Do you want to delete  SiteCategory having id " + this.id + " SiteCategory",
      buttons: '[No][Yes]'
    }, (ButtonPressed) => {
      if (ButtonPressed === "Yes") {
        let res: boolean;


        let table = document.querySelector('table');
        this._GlobalAPIService.deleteData(this.id, 'SiteCategory').subscribe((data) => {
          if (data["_body"] != 0) {
            this._render.setElementStyle(table.rows[this.rowindex], 'display', 'none')
            this.notificationService.smallBox({
              title: "Deletion",
              content: "<i class='fa fa-clock-o'></i> <i>SiteCategory Deleted</i>",
              color: "#659265",
              iconSmall: "fa fa-check fa-2x fadeInRight animated",
              timeout: 4000
              // function:this.delete(SiteCategory)
            });

          }
          else {
            this.notificationService.smallBox({
              title: "Cancelation",
              content: "<i class='fa fa-clock-o'></i> <i>Site Category already used so you can't delete this category</i>",
              color: "#C46A69",
              iconSmall: "fa fa-times fa-2x fadeInRight animated",
              timeout: 4000

            });
            res = false;
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


    var canUserCreateProjects = true;
    const router = this._router;
    // DataTables TableTools buttons options
    var aButtonsData = [];
    aButtonsData.push({ text: '', className: 'btn-rect' });
    aButtonsData.push({ text: '', extend: 'csv', className: 'btn-export', filename: 'Category List' });
    aButtonsData.push({ text: '', extend: 'pdf', className: 'btn-pdf', filename: 'Category List' });
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
      text: 'New Category',
      className: 'btn-new',
      action: (e, dt, node, config) => {
        // router.navigate(["/Managedata/CategoryAdd"])
        this.bsCateAddModalRef = this.modalService.show(this.cateaddModal, { class: 'modal-cateadd', ignoreBackdropClick: false });
      }
    })

    this.DataView.options =
    {
      dom: "Bfrtip",
      aaSorting: [],
      ajax: (data, callback, settings) => {

        this._GlobalAPIService.getDataList('SiteCategory')
          .subscribe((data) => {
            callback({
              aaData: data.aaData,

            })

          })
      },


      // },
      //  ajax:GlobalVariable.BASE_API_URL+"SiteCategory",
      columns: [
        { data: 'categoryID' },
        { data: 'categoryName' },

        {
          render: (data, type, fullRow, meta) => {
            return `
                      <a class='Edit icon-edit' data-cat-id="${fullRow.categoryID}"></a>
                      <a class='del icon-trash' data-cat-id="${fullRow.categoryID}"></a>
                      `;
          }
        }],
      buttons:
        aButtonsData
    };
  }

  onCateAddClose() {
    this.bsCateAddModalRef.hide();
  }
}