import { Component, OnInit, ElementRef, AfterViewInit, Renderer, ViewChild, TemplateRef } from '@angular/core';
import { Http } from '@angular/http';
import { Router, ActivatedRoute } from '@angular/router';
import { APIwithActionService } from '../../shared/APIwithAction.service';
import { FadeInTop } from "../../shared/animations/fade-in-top.decorator";
import { NotificationService } from "../../shared/utils/notification.service";
import * as XLSX from 'xlsx';
import * as FileSaver from 'file-saver';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { ManageDatatableComponent } from 'app/shared/ui/datatable/ManageDatatable.component';
import { ProductAddComponent } from '../ProductAdd/ProductAdd.component';
declare var $: any;

@FadeInTop()
@Component({
  selector: 'app-ProductList',
  templateUrl: './ProductList.component.html'
})
export class ProductListComponent implements AfterViewInit, OnInit {
  public options;
  @ViewChild(ManageDatatableComponent) DataView: ManageDatatableComponent
  @ViewChild(TemplateRef) proaddModal: any;
  id: string;
  bsProAddModalRef: BsModalRef;
  bsProEditModalRef: BsModalRef;
  bsDeleteModalRef: BsModalRef;

  constructor(public http: Http, private notificationService: NotificationService, private _router: Router, private _render: Renderer,
    private _APIwithActionServicee: APIwithActionService, private modalService: BsModalService) {

  }

  delete(ProductID) {

    this._APIwithActionServicee.deleteData(ProductID, 'Product', 'Del01').subscribe((data) => {
    }, error => alert(error))

  }
  ngAfterViewInit(): void {
    document.querySelector('body').addEventListener('click', (event) => {
      let target = <Element>event.target;// Cast EventTarget into an Element

      if (target.className.includes('Edit')) {
        // this._router.navigate(["/Managedata/ProductAdd", target.getAttribute('data-pro-id')]);

      }
      if (target.className.includes('del')) {
        // this.id = target.getAttribute('data-pro-id');
        // this.rowindex = target.parentElement.parentElement["rowIndex"];
        // this.smartModEg1();
        // this._router.navigate(['/Category/CategoryList']);  
      }
    });

  }
  addProduct() {
    this._router.navigate(['/Managedata/ProductAdd']);

  }
  smartModEg1() {
    this.notificationService.smartMessageBox({
      title: "Deletion",
      content: "Do you want to delete Product having id " + this.id,
      buttons: '[No][Yes]'
    }, (ButtonPressed) => {
      if (ButtonPressed === "Yes") {



        let table = document.querySelector('table');
        this._APIwithActionServicee.deleteData(this.id, 'Product', 'Del01').subscribe((data) => {
          if (data["_body"] != 0) {
            this.notificationService.smallBox({
              title: "Deletion",
              content: "<i class='fa fa-clock-o'></i> <i>Product  Deleted</i>",
              color: "#659265",
              iconSmall: "fa fa-check fa-2x fadeInRight animated",
              timeout: 4000
              // function:this.delete(SiteCategory)
            });

          }
          else {
            this.notificationService.smallBox({
              title: "Cancelation",
              content: "<i class='fa fa-clock-o'></i> <i>Product already used so you can't delete this Product</i>",
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

    const APIwithActionService = this._APIwithActionServicee
    let productlist = new Array();
    let productlist1 = new Array();
    const fileType = 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=UTF-8';
    const fileExtension = '.xlsx';


    var aButtonsData = [];
    aButtonsData.push({ text: '', className: 'btn-rect' });
    aButtonsData.push({
      text: '',
      className: 'btn-export',
      action: function (e, dt, node, config) {
        productlist.push({
          ProductName: "Product Name",
          ProductType: "Product Type",
          SerialNo: "Serial No",
          Specification: "Specification",
          BasicUnit: "Basic Unit",
          Minpackpersite: "Min Packs Per Site",
          RapidTestSpecification: "Rapid Test Specification",
          Price: "Price",
          PackingSize: "Packing Size",
          PriceAsDate: "Price As of Date"
        })
        console.log(productlist1)
        productlist1.forEach(element => {
          productlist.push({
            ProductName: element.productName,
            ProductType: element.productType,
            SerialNo: element.catalog,
            Specification: "",
            BasicUnit: element.basicUnit,
            Minpackpersite: element.minpacksize,
            RapidTestSpecification: "",
            Price: element.packcost,
            PackingSize: element.packsize,
            PriceAsDate: element.priceDate
          })
        });
        const ws: XLSX.WorkSheet = XLSX.utils.json_to_sheet(productlist, { skipHeader: true });//{header:["Testing Area","Instrument Name","Max Through Put","Per Test Control","Daily Control Test","Weekly Control Test","Monthly Control Test","Quarterly control Test"]}
        ws["A1"].fill = {
          type: 'pattern',
          pattern: 'darkVertical',
          fgColor: 'red'//{argb:'FFFF0000'}
        };
        ws["A1"].font = {
          bold: true
        }
        const wb: XLSX.WorkBook = { Sheets: { 'Product': ws }, SheetNames: ['Product'] };
        const excelBuffer: any = XLSX.write(wb, { bookType: 'xlsx', type: 'array', cellStyles: true });
        const data1: Blob = new Blob([excelBuffer], { type: fileType });
        FileSaver.saveAs(data1, "Product List" + fileExtension);
      }
    })
    aButtonsData.push({ text: '', extend: 'pdf', className: 'btn-pdf', filename: 'Product List' })
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
      text: 'New Product',
      className: 'btn-new',
      action: (e, dt, node, config) => {
        // router.navigate(["/Managedata/ProductAdd"])
        this.bsProAddModalRef = this.modalService.show(ProductAddComponent, { class: 'modal-proadd', ignoreBackdropClick: false });
        this.bsProAddModalRef.content.event.subscribe(res => {
          $.fn.dataTable.tables({ visible: true, api: true }).ajax.reload();
        })
      }
    })

    this.DataView.options = {
      dom: "Bfrtip",
      aaSorting: [],
      ajax: (data, callback, settings) => {
        this._APIwithActionServicee.getDataList('Product', 'GetAll').subscribe((data) => {
          productlist1 = data.aaData
          callback({
            aaData: data.aaData,
          })
        })
      },

      columns: [
        { data: 'productID' },
        { data: 'productName' },
        { data: 'productType' },
        //    { data: 'catalog' },
        { data: 'basicUnit' },
        { data: 'minpacksize' },
        { data: 'packcost' },
        { data: 'packsize' },
        {
          render: (data, type, fullRow, meta) => {
            return `
                      <a class='Edit icon-edit' data-pro-id="${fullRow.productID}"></a>
                      <a class='del icon-trash' data-pro-id="${fullRow.productID}"></a>
                    `;
          }
        }
      ],
      buttons: aButtonsData
    };
  }
  onProAddClose() {
    this.bsProAddModalRef.hide();
  }

}