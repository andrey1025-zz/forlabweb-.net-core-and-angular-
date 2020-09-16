import { Component, OnInit, ElementRef, ViewChild, Renderer, TemplateRef, Input, Output, EventEmitter } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { APIwithActionService } from "../shared/APIwithAction.service"
import { ManageDatatableComponent } from 'app/shared/ui/datatable/ManageDatatable.component';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { NotificationService } from 'app/shared/utils/notification.service';
import { Http } from '@angular/http';
import { ForecastAddComponent } from './ForecastAdd/ForecastAdd.component';
declare var $: any;

@Component({
  selector: 'app-conduct-forecast',
  templateUrl: './ConductForecast.component.html',
  styleUrls: ['./ConductForecast.component.css']
})
export class ConductForecastComponent implements OnInit {
  @ViewChild(ManageDatatableComponent) DataView: ManageDatatableComponent;
  id: number;
  rowindex: number;
  bsForecastAddModalRef: BsModalRef;
  checkedAll: boolean;
  constructor(
    public http: Http,
    private notificationService: NotificationService,
    private _router: Router,
    private _render: Renderer,
    private _APIwithActionService: APIwithActionService,
    private modalService: BsModalService
  ) {
  }

  ngAfterViewInit(): void {
    document.querySelector('body').addEventListener('click', (event) => {
      let target = <Element>event.target;// Cast EventTarget into an Element
      if (target.className.includes('Edit')) {
      }
      if (target.className.includes('del')) {
      }
    });
  }
  ngOnInit() {
    this.checkedAll = false;
    const router = this._router;
    const APIwithActionService = this._APIwithActionService;
    var aButtonsData = new Array();
    aButtonsData.push({
      text: '', className: 'btn-view',
      action: (e, dt, node, config) => {
        // TODO: redirect ForecastComparison Page
      }
    });
    aButtonsData.push({ text: '', className: 'btn-trash' });
    aButtonsData.push({
      text: 'New Forecast',
      className: 'btn-new',
      action: (e, dt, node, config) => {
        this.openFirstModal();
      }
    });
    aButtonsData.push({ text: '', className: 'btn-rect' });
    this.DataView.options = {
      dom: "Bfrtip",
      aaSorting: [],
      ajax: (data, callback, settings) => {
        this._APIwithActionService.getDatabyID(0, "Forecsatinfo", "getrecentforecast").subscribe((data) => {
          callback({
            aaData: data.slice(0, 100)
          })
        })
      },
      columns: [
        { data: 'checked', defaultContent: "" },
        { data: 'name' },
        { data: 'dateofforecast' },
        { data: 'duration' },
        { data: 'forecastvalue' },
        {
          render: (data, type, fullRow, meta) => {
            return `
            <a class='Edit icon-report' data-ins-id="${fullRow.id}"></a>
            <a class='Edit icon-edit' data-ins-id="${fullRow.id}"></a>
            <a class='del icon-trash' data-ins-id="${fullRow.id}"></a>
            `;
          }
        }
      ],
      columnDefs: [
        {
          targets: 0,
          orderable: false,
          className: 'select-checkbox'
        }
      ],
      select: {
        style: 'os',
        selector: 'td:first-child'
      },
      buttons: aButtonsData
    };

  }

  openFirstModal() {
    this.bsForecastAddModalRef = this.modalService.show(ForecastAddComponent, { class: 'modal-forecastadd', ignoreBackdropClick: false });
    this.bsForecastAddModalRef.content.event.subscribe(res => {
      console.log("res", res);
    })
  }

  onCheckedAllChange() {
    if (!this.checkedAll) {
      $.fn.dataTable.tables({ visible: true, api: true }).rows().select();
    } else {
      $.fn.dataTable.tables({ visible: true, api: true }).rows().deselect();
    }
    this.checkedAll = !this.checkedAll;
  }
}