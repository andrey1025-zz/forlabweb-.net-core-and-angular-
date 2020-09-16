import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { Http } from '@angular/http';
import { GlobalAPIService } from '../../shared/GlobalAPI.service';
import { FadeInTop } from "../../shared/animations/fade-in-top.decorator";
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { ManageDatatableComponent } from 'app/shared/ui/datatable/ManageDatatable.component';
import { ProductTypeAddComponent } from '../ProductTypeAdd/ProductTypeAdd.component';
import { DeleteModalComponent } from 'app/shared/ui/datatable/DeleteModal/DeleteModal.component';
declare var $: any;

@FadeInTop()
@Component({
  selector: 'app-ProductTypeList',
  templateUrl: './ProductTypeList.component.html'
})
export class ProductTypeListComponent implements AfterViewInit, OnInit {
  @ViewChild(ManageDatatableComponent) DataView: ManageDatatableComponent
  id: number;
  rowindex: number;
  bsProTypeAddModalRef: BsModalRef;
  bsProTypeEditModalRef: BsModalRef;
  bsProTypeDeleteModalRef: BsModalRef;

  constructor(public http: Http, private _GlobalAPIService: GlobalAPIService, private modalService: BsModalService) { }

  ngAfterViewInit(): void {
    document.querySelector('body').addEventListener('click', (event) => {
      let target = <Element>event.target;
      if (target.className.includes('product-type-edit')) {
        var initialState = {
          itemID: target.getAttribute('data-type-id')
        };
        this.bsProTypeEditModalRef = this.modalService.show(ProductTypeAddComponent, { class: 'modal-protypeadd', ignoreBackdropClick: false, initialState });
        this.bsProTypeEditModalRef.content.event.subscribe(res => {
          $.fn.dataTable.tables({ visible: true, api: true }).ajax.reload();
        })
      }
      if (target.className.includes('product-type-delete')) {
        this.id = parseInt(target.getAttribute('data-type-id'));
        this.bsProTypeDeleteModalRef = this.modalService.show(DeleteModalComponent, { class: 'modal-delete' });
        this.bsProTypeDeleteModalRef.content.event.subscribe(res => {
          if (res.type == "delete") {
            this._GlobalAPIService.deleteData(this.id, 'ProductType').subscribe((data) => {
              $.fn.dataTable.tables({ visible: true, api: true }).ajax.reload();
              if (data["_body"] != 0) {
                this._GlobalAPIService.SuccessMessage("Product Type Deleted Successfully");
              } else {
                this._GlobalAPIService.FailureMessage("Product Type already used so you can't delete this ProductType");
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
    aButtonsData.push({ text: '', extend: 'csv', className: 'btn-export', filename: 'Producttype List' });
    aButtonsData.push({ text: '', extend: 'pdf', className: 'btn-pdf', filename: 'Producttype List' });
    aButtonsData.push({ text: '', extend: 'print', className: 'btn-print' })
    if (localStorage.getItem("role") == "admin") {
      aButtonsData.push({
        text: 'Import',
        className: 'btn-import',
        action: function (e, dt, node, config) { }
      });
    }
    aButtonsData.push({
      text: 'New Product Type',
      className: 'btn-new',
      action: (e, dt, node, config) => {
        this.bsProTypeAddModalRef = this.modalService.show(ProductTypeAddComponent, { class: 'modal-protypeadd', ignoreBackdropClick: false });
        this.bsProTypeAddModalRef.content.event.subscribe(res => {
          $.fn.dataTable.tables({ visible: true, api: true }).ajax.reload();
        })
      }
    })

    this.DataView.options = {
      dom: "Bfrtip",
      aaSorting: [],
      ajax: (data, callback, settings) => {
        this._GlobalAPIService.getDataList('ProductType').subscribe((data) => {
          callback({
            aaData: data.aaData,
          })
        })
      },
      columns: [
        { data: 'typeID' },
        { data: 'typeName' },
        {
          render: (data, type, fullRow, meta) => {
            return `
                      <a class='product-type-edit icon-edit' data-type-id="${fullRow.typeID}"></a>
                      <a class='product-type-delete icon-trash' data-type-id="${fullRow.typeID}"></a>
                    `;
          }
        }
      ],
      buttons: aButtonsData
    };
  }
}