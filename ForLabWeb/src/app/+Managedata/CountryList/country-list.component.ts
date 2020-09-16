import { Component, OnInit, ViewChild, TemplateRef, Renderer, AfterViewInit } from '@angular/core';
import { APIwithActionService } from '../../shared/APIwithAction.service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { ManageDatatableComponent } from 'app/shared/ui/datatable/ManageDatatable.component';
import { NotificationService } from "../../shared/utils/notification.service";
import { Router, ActivatedRoute, Route } from '@angular/router';
import { CountryAddComponent } from '../CountryAdd/country-add.component';
import { FadeInTop } from 'app/shared/animations/fade-in-top.decorator';
declare var $: any;
@FadeInTop()
@Component({
  selector: 'app-country-list',
  templateUrl: './country-list.component.html',

})
export class CountryListComponent implements AfterViewInit,OnInit {
 
  @ViewChild(ManageDatatableComponent) DataView: ManageDatatableComponent


  id: string;
  bsCountryAddModalRef: BsModalRef;
  bsCountryEditModalRef: BsModalRef;
  bsDeleteModalRef: BsModalRef;
// bsCateAddModalRef: BsModalRef;

  constructor(private notificationService: NotificationService, private _router: Router, private _render: Renderer,
    private _APIwithActionService: APIwithActionService, private modalService: BsModalService) { }

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
      text: 'New Country',
      className: 'btn-new',
      action: (e, dt, node, config) => {
        // router.navigate(["/Managedata/ProductAdd"])
        this.bsCountryAddModalRef = this.modalService.show(CountryAddComponent, { class: 'modal-proadd', ignoreBackdropClick: false });
        this.bsCountryAddModalRef.content.event.subscribe(res => {
          $.fn.dataTable.tables({ visible: true, api: true }).ajax.reload();
        }) 
      }
    })
    // aButtonsData.push({
    //   text: 'New Country',
    //   className: 'btn-new',
    //   action: (e, dt, node, config) => {
    //     // router.navigate(["/Managedata/CategoryAdd"])
    //     this.bsCateAddModalRef = this.modalService.show(this.cuntryaddModal, { class: 'modal-cateadd', ignoreBackdropClick: false });
    //   }
    // })

    this.DataView.options =
    {
      dom: "Bfrtip",
      aaSorting: [],
      ajax: (data, callback, settings) => {
        this._APIwithActionService.getDataList('Site', 'Getcountrylist').subscribe((data) => {


          callback({
            aaData: data,

          })

        })
      },


      // },
      //  ajax:GlobalVariable.BASE_API_URL+"SiteCategory",
      columns: [
        { data: 'id' },
        { data: 'name' },
        { data: 'region' },
        {
          render: (data, type, fullRow, meta) => {
            return `
                      <a class='countryEdit icon-edit' data-cat-id="${fullRow.id}"></a>
                   
                      `;
          }
        }],
      buttons:
        aButtonsData
    };
  }
  // oncuntryAddClose() {
  //   this.bsCountryAddModalRef.hide();
  // }

  ngAfterViewInit(): void {
    document.querySelector('body').addEventListener('click', (event) => {
      let target = <Element>event.target;// Cast EventTarget into an Element
      if (target.className.includes('countryEdit')) {
        var initialState = {
          itemID1: target.getAttribute('data-cat-id')
        };
        this.bsCountryEditModalRef = this.modalService.show(CountryAddComponent, { class: 'modal-proadd', ignoreBackdropClick: false, initialState });
        this.bsCountryEditModalRef.content.event.subscribe(res => {
          $.fn.dataTable.tables({ visible: true, api: true }).ajax.reload();
        })
      }
    
    });
  }
}
