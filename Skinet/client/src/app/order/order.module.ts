import { NgModule } from '@angular/core';
import { OrderRoutingModule } from './order-routing.module';
import { OrdersComponent } from './orders/orders.component';
import { OrderDetailedComponent } from './order-detailed/order-detailed.component';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';

@NgModule({
  declarations: [
    OrdersComponent,
    OrderDetailedComponent
  ],
  imports: [
    OrderRoutingModule,
    CommonModule
  ],
  exports: [
    RouterModule
  ]
})
export class OrderModule { }
