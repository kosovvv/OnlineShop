import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { enviroment } from 'src/environments/environment';
import { DeliveryMethod } from '../../shared/models/deliveryMethod';
import { map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DeliveryMethodService {

  baseUrl = enviroment.apiUrl + 'deliveryMethods/';

  constructor(private http: HttpClient) { }
  getDeliveryMethods() {
    return this.http.get<DeliveryMethod[]>(this.baseUrl).pipe(
      map((dm : DeliveryMethod[]) => {
        return dm.sort((a, b) => b.price - a.price);
      })
    )
  }
}
