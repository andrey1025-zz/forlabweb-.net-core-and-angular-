import { Component, Inject, OnInit, ElementRef, AfterViewInit, Renderer, ViewChild, Directive, TemplateRef } from '@angular/core';
import { Http, Headers } from '@angular/http';
import { GlobalAPIService } from '../../shared/GlobalAPI.service';
import { APIwithActionService } from '../../shared/APIwithAction.service';
import { FadeInTop } from "../../shared/animations/fade-in-top.decorator";
import { NotificationService } from "../../shared/utils/notification.service";
import * as XLSX from 'xlsx';
import * as FileSaver from 'file-saver';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { ManageDatatableComponent } from 'app/shared/ui/datatable/ManageDatatable.component';
import { InstrumentAddComponent } from 'app/+Managedata/InstrumentAdd/InstrumentAdd.component'
import { DeleteModalComponent } from 'app/shared/ui/datatable/DeleteModal/DeleteModal.component';
declare var $: any;

@FadeInTop()
@Component({
  selector: 'app-InsList',
  templateUrl: './InstrumentList.component.html'
})
export class InstrumentListComponent implements AfterViewInit, OnInit {
  @ViewChild(ManageDatatableComponent) DataView: ManageDatatableComponent;
  id: number;
  bsInsAddModalRef: BsModalRef;
  bsInsEditModalRef: BsModalRef;
  bsDeleteModalRef: BsModalRef;
  aButtonsData: any[];

  constructor(public http: Http, private notificationService: NotificationService,
    private _GlobalAPIService: GlobalAPIService, private _render: Renderer, private _APIwithActionService: APIwithActionService, private modalService: BsModalService) {
  }
  ngAfterViewInit(): void {
    document.querySelector('body').addEventListener('click', (event) => {
      let target = <Element>event.target;// Cast EventTarget into an Element
      if (target.className.includes('ins-edit')) {
        var initialState = {
          itemID: target.getAttribute('data-ins-id')
        };
        this.bsInsEditModalRef = this.modalService.show(InstrumentAddComponent, { class: 'modal-insadd', ignoreBackdropClick: false, initialState });
        this.bsInsEditModalRef.content.event.subscribe(res => {
          $.fn.dataTable.tables({ visible: true, api: true }).ajax.reload();
        })
      }
      if (target.className.includes('ins-delete')) {
        this.id = parseInt(target.getAttribute('data-ins-id'));
        this.bsDeleteModalRef = this.modalService.show(DeleteModalComponent, { class: 'modal-delete' });
        this.bsDeleteModalRef.content.event.subscribe(res => {
          if (res.type == "delete") {
            this._APIwithActionService.deleteData(this.id, 'Instrument', 'Del01').subscribe((data) => {
              $.fn.dataTable.tables({ visible: true, api: true }).ajax.reload();
              if (data["_body"] != 0) {
                this._GlobalAPIService.SuccessMessage("Instrument Deleted Successfully");
              }
              else {
                this._GlobalAPIService.FailureMessage("Instrument already used so you can't delete this Instrument");
              }
            });
          }
        })
      }
    });
  }

  ngOnInit() {
    this.fetchTableData();
  }

  fetchTableData() {
    const APIwithActionService = this._APIwithActionService
    let insliat = new Array();
    const fileType = 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=UTF-8';
    const fileExtension = '.xlsx';
    this.aButtonsData = new Array();
    this.aButtonsData.push({ text: '', className: 'btn-rect' });
    this.aButtonsData.push(
      {
        className: 'btn-export',
        action: function (e, dt, node, config) {
          APIwithActionService.getDataList('Instrument', 'GetAll').subscribe((data) => {
            insliat.push({
              TestingArea: "Testing Area",
              InstrumentName: "Instrument Name",
              maxThroughPut: "Max Through Put",
              PerTestControl: "Per Test Control",
              DailyControlTest: "Daily Control Test",
              WeeklyControlTest: "Weekly Control Test",
              MonthlyControlTest: "Monthly Control Test",
              QuarterlycontrolTest: "Quarterly control Test"
            })
            data.aaData.forEach(element => {
              insliat.push({
                TestingArea: element.testingArea,
                InstrumentName: element.instrumentName,
                maxThroughPut: element.maxThroughPut,
                PerTestControl: element.maxTestBeforeCtrlTest,
                DailyControlTest: element.dailyCtrlTest,
                WeeklyControlTest: element.weeklyCtrlTest,
                MonthlyControlTest: element.monthlyCtrlTest,
                QuarterlycontrolTest: element.quarterlyCtrlTest
              })
            });
            const ws: XLSX.WorkSheet = XLSX.utils.json_to_sheet(insliat, { skipHeader: true });//{header:["Testing Area","Instrument Name","Max Through Put","Per Test Control","Daily Control Test","Weekly Control Test","Monthly Control Test","Quarterly control Test"]}
            const wb: XLSX.WorkBook = { Sheets: { 'Instrument': ws }, SheetNames: ['Instrument'] };
            const excelBuffer: any = XLSX.write(wb, { bookType: 'xlsx', type: 'array' });
            const data1: Blob = new Blob([excelBuffer], { type: fileType });
            FileSaver.saveAs(data1, "Instrument List" + fileExtension);
          })
        }
      }
    )
    this.aButtonsData.push({ text: null, extend: 'pdf', className: 'btn-pdf', filename: 'Instrument List' });
    this.aButtonsData.push({ text: null, extend: 'print', className: 'btn-print' })
    if (localStorage.getItem("role") == "admin") {
      this.aButtonsData.push({
        text: 'Import',
        className: 'btn-import',
        action: function (e, dt, node, config) {
        }
      });
    }
    this.aButtonsData.push({
      text: 'New Instrument',
      className: 'btn-new',
      action: (e, dt, node, config) => {
        this.bsInsAddModalRef = this.modalService.show(InstrumentAddComponent, { class: 'modal-insadd', ignoreBackdropClick: false });
        this.bsInsAddModalRef.content.event.subscribe(res => {
          $.fn.dataTable.tables({ visible: true, api: true }).ajax.reload();
        })
      }
    });
    this.DataView.options = {
      dom: "Bfrtip",
      aaSorting: [],
      ajax: (data, callback, settings) => {
        this._APIwithActionService.getDataList('Instrument', 'GetAll').subscribe((data) => {
          callback({
            aaData: data.aaData,
          })
        })
      },
      columns: [
        { data: 'instrumentID' },
        { data: 'instrumentName' },
        { data: 'testingArea' },
        { data: 'maxThroughPut' },
        { data: 'dailyCtrlTest' },
        { data: 'maxTestBeforeCtrlTest' },
        { data: 'weeklyCtrlTest' },
        { data: 'monthlyCtrlTest' },
        { data: 'quarterlyCtrlTest' },
        {
          render: (data, type, fullRow, meta) => {
            return `
                <a class='ins-edit icon-edit' data-ins-id="${fullRow.instrumentID}"></a>
                <a class='ins-delete icon-trash' data-ins-id="${fullRow.instrumentID}"></a>
                `;
          }
        }
      ],
      buttons: this.aButtonsData
    };
  }
}
