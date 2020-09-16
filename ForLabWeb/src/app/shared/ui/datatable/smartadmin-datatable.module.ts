import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DatatableComponent } from './datatable.component';
import { ManageDatatableComponent } from './ManageDatatable.component';
import { ModalDatatableComponent } from './ModalDatatable.component';
import { DeleteModalComponent } from './DeleteModal/DeleteModal.component';

// require('smartadmin-plugins/bower_components/datatables.net-colreorder-bs/css/colReorder.bootstrap.min.css');

@NgModule({
  imports: [
    CommonModule
  ],
  declarations: [DatatableComponent, ManageDatatableComponent, ModalDatatableComponent, DeleteModalComponent],
  exports: [DatatableComponent, ManageDatatableComponent, ModalDatatableComponent],
})
export class SmartadminDatatableModule { }
