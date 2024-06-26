import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { enviroment } from 'src/environments/environment';
import { DeliveryMethod } from '../../shared/models/deliveryMethod';
import { map } from 'rxjs';
import { Order, OrderToCreate } from '../../shared/models/order';

@Injectable({
  providedIn: 'root'
})
export class CheckoutService {

  baseUrl = enviroment.apiUrl + 'order/';

  constructor(private http: HttpClient) { }

  createOrder(order : OrderToCreate) {
    return this.http.post<Order>(this.baseUrl, order)
  }
}
