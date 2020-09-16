import { Component, ViewChild, TemplateRef, Renderer } from '@angular/core';
import { APIwithActionService } from "../shared/APIwithAction.service";
import { ManageDatatableComponent } from 'app/shared/ui/datatable/ManageDatatable.component';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { Http } from '@angular/http';
import { NotificationService } from 'app/shared/utils/notification.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-conduct-morbidity',
  templateUrl: './ConstructMorbidity.component.html',
  styleUrls: ['./ConstructMorbidity.component.css']
})
export class ConstructMorbidityComponent {
  @ViewChild(ManageDatatableComponent) DataView: ManageDatatableComponent;
  @ViewChild(TemplateRef) programaddModal: any;
  id: number;
  rowindex: number;
  bsProgramAddModalRef: BsModalRef;
  ProgramList = new Array();

  constructor(public http: Http, private notificationService: NotificationService,
    private _router: Router, private _render: Renderer, private _APIwithActionService: APIwithActionService, private modalService: BsModalService) {
  }


  ngAfterViewInit(): void {
    document.querySelector('body').addEventListener('click', (event) => {
      let target = <Element>event.target;// Cast EventTarget into an Element
      if (target.className.includes('Edit')) {
        // this._router.navigate(["/Managedata/InstrumentAdd", target.getAttribute('data-ins-id')]);
      }
      if (target.className.includes('del')) {
        // this.id = parseInt(target.getAttribute('data-ins-id'));
        // this.rowindex = target.parentElement.parentElement["rowIndex"];
      }
    });
  }
  ngOnInit() {
    const router = this._router;
    this.getprogramlist();
    var aButtonsData = new Array();

    aButtonsData.push({
      text: 'New Program',
      className: 'btn-new',
      action: (e, dt, node, config) => {
        this.bsProgramAddModalRef = this.modalService.show(this.programaddModal, { class: 'modal-programadd', ignoreBackdropClick: false });
      }
    });
    aButtonsData.push({ text: '', className: 'btn-rect' });

    this.DataView.options = {
      dom: "Bfrtip",
      aaSorting: [],
      ajax: (data, callback, settings) => {
        this._APIwithActionService.getDataList('MMProgram', 'Getprogramlist').subscribe((data) => {
          callback({
            aaData: data.slice(0, 100)
          })
        })
      },
      columns: [
        { data: 'programName' },
        { data: 'forecastmethod' },
        { data: 'totalGrp' },
        { data: 'totalforecast' },
        {
          render: (data, type, fullRow, meta) => {
            return `
            <a class='Edit icon-edit' data-ins-id="${fullRow.id}"></a>
            <a class='del icon-trash' data-ins-id="${fullRow.id}"></a>
            `;
          }
        }
      ],
      columnDefs: [
        {
          targets: 0,
          orderable: false
        }
      ],
      select: {
        style: 'os',
        selector: 'td:first-child'
      },
      buttons: aButtonsData
    };
  }

  onProgramAddClose() {
    this.bsProgramAddModalRef.hide();
  }
  getprogramlist() {
    this._APIwithActionService.getDataList('MMProgram', 'Getprogramlist').subscribe((resp) => {
      this.ProgramList = resp;
    })
  }

}