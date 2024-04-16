import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Order } from '../models/order';
import { enviroment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class OrderService {

  baseUrl = enviroment.apiUrl;

  constructor(private http: HttpClient) { }

  getOrderByIdForUser(id : number) {
    return this.http.get<Order>(this.baseUrl + 'order/' + id);
  }

  getAllOrdersForUser() {
    return this.http.get<Order[]>(this.baseUrl + 'order');
  }
}
