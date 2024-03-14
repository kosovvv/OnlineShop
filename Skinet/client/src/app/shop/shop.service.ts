import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Product } from '../shared/models/products';
import { Pagination } from '../shared/models/pagination';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ShopService {

  baseUrl = 'https://localhost:5001/api/'

  constructor(private http:HttpClient) { }

  getProducts() : Observable<Pagination<Product[]>> {
    return this.http.get<Pagination<Product[]>>(this.baseUrl + 'products');
  }
}
