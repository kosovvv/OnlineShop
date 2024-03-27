import { Component, OnInit } from '@angular/core';
import { Order } from 'src/app/shared/models/order';
import { OrderService } from '../order.service';
import { ActivatedRoute, Router } from '@angular/router';
import { BreadcrumbService } from 'xng-breadcrumb';

@Component({
  selector: 'app-order-detailed',
  templateUrl: './order-detailed.component.html',
  styleUrls: ['./order-detailed.component.scss']
})
export class OrderDetailedComponent implements OnInit {
  order!: Order;

  constructor(private ordersService: OrderService, private activatedRoute: ActivatedRoute, private bcService: BreadcrumbService) {}

  ngOnInit(): void {
    const id = this.activatedRoute.snapshot.params['id'];

    if (id) {
      this.loadDetails(id); 
    }
  }

  loadDetails(id : number) {
    this.ordersService.getOrderByIdForUser(id).subscribe({
      next: order => {
        this.order = order
        this.bcService.set('@OrderDetailed', `Order# ${order.id} - ${order.status}`);
      }
    })
  }
}
