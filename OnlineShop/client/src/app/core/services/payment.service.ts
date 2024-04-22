import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Basket } from "src/app/shared/models/basket";
import { BasketService } from "./basket.service";
import { map } from "rxjs";
import { enviroment } from "src/environments/environment";

@Injectable({
    providedIn: 'root'
})

export class PaymentService {

    baseUrl = enviroment.apiUrl + 'payments/'

    constructor(private http: HttpClient, private basketService: BasketService) { }

    createPaymentIntent() {
        return this.http.post<Basket>(this.baseUrl + this.basketService.getCurrentBasketValue()?.id, {})
            .pipe(
                map(basket => {
                    this.basketService.basketSource.next(basket);
                })
            )
    }
}