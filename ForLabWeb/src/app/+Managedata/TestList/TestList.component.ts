import { Component, OnInit, ElementRef, AfterViewInit, Renderer, ViewChild, TemplateRef } from '@angular/core';
import { Http } from '@angular/http';
// import {DataTables} from 'angular-datatables';
import { Router, ActivatedRoute } from '@angular/router';
import { APIwithActionService } from '../../shared/APIwithAction.service';


import { NotificationService } from "../../shared/utils/notification.service";
import { GlobalVariable } from '../../shared/globalclass'
import * as XLSX from 'xlsx';
import * as FileSaver from 'file-saver';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { ManageDatatableComponent } from 'app/shared/ui/datatable/ManageDatatable.component';
declare var $: any;
declare let pdfMake: any;

@Component({
  selector: 'app-TestList',
  templateUrl: './TestList.component.html',
  styles: [".addbutton { background-color: red!important; color: white; }"]
})
export class TestListComponent implements AfterViewInit, OnInit {

  public options;
  @ViewChild(ManageDatatableComponent) DataView: ManageDatatableComponent
  @ViewChild(TemplateRef) testaddModal: any;
  id: number;
  rowindex: number;
  bsTestAddModalRef: BsModalRef;

  constructor(public http: Http, private notificationService: NotificationService, private _router: Router, private _render: Renderer,
    private _APIwithActionService: APIwithActionService, private modalService: BsModalService) {

  }
  delete(TestID) {

    let table = document.querySelector('table');
    this._APIwithActionService.deleteData(TestID, 'Test', 'Del01').subscribe((data) => {
      this._render.setElementStyle(table.rows[this.rowindex], 'display', 'none')
    }, error => alert(error))

  }
  ngAfterViewInit(): void {
    document.querySelector('body').addEventListener('click', (event) => {
      let target = <Element>event.target;// Cast EventTarget into an Element

      if (target.className.includes('Edit')) {
        // this._router.navigate(["/Managedata/TestAdd", target.getAttribute('data-test-id')]);

      }
      if (target.className.includes('del')) {
        // this.id = parseInt(target.getAttribute('data-test-id'));
        // this.rowindex = target.parentElement.parentElement["rowIndex"];
        // this.smartModEg1();

      }
    });

  }
  smartModEg1() {
    this.notificationService.smartMessageBox({
      title: "Deletion",
      content: "Do you want to delete Test having id " + this.id,
      buttons: '[No][Yes]'
    }, (ButtonPressed) => {
      if (ButtonPressed === "Yes") {

        let table = document.querySelector('table');
        this._APIwithActionService.deleteData(this.id, 'Test', 'Del01').subscribe((data) => {
          if (data["_body"] != 0) {
            this._render.setElementStyle(table.rows[this.rowindex], 'display', 'none')
            this.notificationService.smallBox({
              title: "Deletion",
              content: "<i class='fa fa-clock-o'></i> <i>/Test Deleted</i>",
              color: "#659265",
              iconSmall: "fa fa-check fa-2x fadeInRight animated",
              timeout: 4000
              // function:this.delete(SiteCategory)
            });

          }
          else {
            this.notificationService.smallBox({
              title: "Cancelation",
              content: "<i class='fa fa-clock-o'></i> <i>Test already used so you can't delete this Test</i>",
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
    const APIwithActionService = this._APIwithActionService
    let testlist = new Array();
    const fileType = 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=UTF-8';
    const fileExtension = '.xlsx';


    var aButtonsData = [];
    aButtonsData.push({ text: '', className: 'btn-rect' });
    aButtonsData.push(
      {
        text: '',
        className: 'btn-export',
        action: function (e, dt, node, config) {
          APIwithActionService.getDataList('Test', 'GetAll').subscribe((data) => {
            console.log(data.aaData);
            testlist.push({
              testName: "Test Name",
              AreaName: "Area Name"
            })
            data.aaData.forEach(element => {
              testlist.push({
                testName: element.testName,
                AreaName: element.testingArea
              })
            });
            const ws: XLSX.WorkSheet = XLSX.utils.json_to_sheet(testlist, { skipHeader: true });//{header:["Testing Area","Instrument Name","Max Through Put","Per Test Control","Daily Control Test","Weekly Control Test","Monthly Control Test","Quarterly control Test"]}
            const wb: XLSX.WorkBook = { Sheets: { 'Test': ws }, SheetNames: ['Test'] };
            const excelBuffer: any = XLSX.write(wb, { bookType: 'xlsx', type: 'array' });
            const data1: Blob = new Blob([excelBuffer], { type: fileType });
            FileSaver.saveAs(data1, "Test List" + fileExtension);
          })
        }
      }
    )
    aButtonsData.push({ text: '', extend: 'pdf', className: 'btn-pdf', filename: 'Test List' });
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
    aButtonsData.push(
      {
        text: 'New Test',
        className: 'btn-new',
        action: (e, dt, node, config) => {
          // router.navigate(["/Managedata/TestAdd"])
          this.bsTestAddModalRef = this.modalService.show(this.testaddModal, { class: 'modal-testadd', ignoreBackdropClick: false });
        }
      }
    )
    this.DataView.options =
    {
      dom: "Bfrtip",
      aaSorting: [],
      ajax: (data, callback, settings) => {

        this._APIwithActionService.getDataList('Test', 'GetAll')
          .subscribe((data) => {
            callback({
              aaData: data.aaData,

            })

          })
      },

      //    ajax:GlobalVariable.BASE_API_URL+"Test/"+"GetAll",
      columns: [
        { data: 'testID' },
        { data: 'testName' },
        { data: 'testingArea' },
        {
          render: (data, type, fullRow, meta) => {
            return `
                  <a class='Edit icon-edit' data-test-id="${fullRow.testID}"></a>
                  <a class='del icon-trash' data-test-id="${fullRow.testID}"></a>
                  `;
          }
        }
      ],
      buttons: aButtonsData
    };

  }

  onTestAddClose() {
    this.bsTestAddModalRef.hide();
  }

}