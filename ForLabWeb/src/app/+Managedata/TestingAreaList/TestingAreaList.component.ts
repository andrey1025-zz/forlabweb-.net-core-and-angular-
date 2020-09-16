import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { Http } from '@angular/http';
import { GlobalAPIService } from '../../shared/GlobalAPI.service';
import { FadeInTop } from "../../shared/animations/fade-in-top.decorator";
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { ManageDatatableComponent } from 'app/shared/ui/datatable/ManageDatatable.component';
import { TestingAreaAddComponent } from '../TestingAreaAdd/TestingAreaAdd.component';
import { DeleteModalComponent } from 'app/shared/ui/datatable/DeleteModal/DeleteModal.component';
declare var $: any;

@FadeInTop()
@Component({
  selector: 'app-TestingAreaList',
  templateUrl: './TestingAreaList.component.html'
})
export class TestingAreaListComponent implements AfterViewInit, OnInit {
  @ViewChild(ManageDatatableComponent) DataView: ManageDatatableComponent
  id: number;
  rowindex: number;
  bsTestAreaAddModalRef: BsModalRef;
  bsTestAreaEditModalRef: BsModalRef;
  bsTestAreaDeleteModalRef: BsModalRef;

  constructor(public http: Http, private _GlobalAPIService: GlobalAPIService, private modalService: BsModalService) { }

  ngAfterViewInit(): void {
    document.querySelector('body').addEventListener('click', (event) => {
      let target = <Element>event.target;
      if (target.className.includes('test-area-edit')) {
        var initialState = {
          itemID: target.getAttribute('data-testarea-id')
        };
        this.bsTestAreaEditModalRef = this.modalService.show(TestingAreaAddComponent, { class: 'modal-testareaadd', ignoreBackdropClick: false, initialState });
        this.bsTestAreaEditModalRef.content.event.subscribe(res => {
          $.fn.dataTable.tables({ visible: true, api: true }).ajax.reload();
        })
      }
      if (target.className.includes('test-area-delete')) {
        this.id = parseInt(target.getAttribute('data-testarea-id'));
        this.bsTestAreaDeleteModalRef = this.modalService.show(DeleteModalComponent, { class: 'modal-delete' });
        this.bsTestAreaDeleteModalRef.content.event.subscribe(res => {
          if (res.type == "delete") {
            this._GlobalAPIService.deleteData(this.id, 'TestArea').subscribe((data) => {
              $.fn.dataTable.tables({ visible: true, api: true }).ajax.reload();
              if (data["_body"] != 0) {
                this._GlobalAPIService.SuccessMessage("Testing Area Deleted Successfully");
              }
              else {
                this._GlobalAPIService.FailureMessage("Testing Area already used so you could not delete this Testing Area");
              }
            });
          }
        })
      }
    });
  }

  ngOnInit() {
    var aButtonsData = [];
    aButtonsData.push({ text: '', className: 'btn-rect' });
    aButtonsData.push({ text: '', extend: 'csv', className: 'btn-export', filename: 'TestingArea List' });
    aButtonsData.push({ text: '', extend: 'pdf', className: 'btn-pdf', filename: 'TestingArea List' });
    aButtonsData.push({ text: '', extend: 'print', className: 'btn-print' })
    if (localStorage.getItem("role") == "admin") {
      aButtonsData.push({
        text: 'Import',
        className: 'btn-import',
        action: function (e, dt, node, config) {
        }
      });
    }
    aButtonsData.push({
      text: 'New Testing Area',
      className: 'btn-new',
      action: (e, dt, node, config) => {
        this.bsTestAreaAddModalRef = this.modalService.show(TestingAreaAddComponent, { class: 'modal-testareaadd', ignoreBackdropClick: false });
        this.bsTestAreaAddModalRef.content.event.subscribe(res => {
          $.fn.dataTable.tables({ visible: true, api: true }).ajax.reload();
        })
      }
    })
    this.DataView.options = {
      dom: "Bfrtip",
      aaSorting: [],
      ajax: (data, callback, settings) => {
        this._GlobalAPIService.getDataList('TestArea').subscribe((data) => {
          callback({
            aaData: data.aaData,
          })
        })
      },
      columns: [
        { data: 'testingAreaID' },
        { data: 'areaName' },
        {
          render: (data, type, fullRow, meta) => {
            return `
                      <a class='test-area-edit icon-edit' data-testarea-id="${fullRow.testingAreaID}"></a>
                      <a class='test-area-delete icon-trash' data-testarea-id="${fullRow.testingAreaID}"></a>
                      `;
          }
        }
      ]
      ,
      buttons: aButtonsData
    };
  }
}