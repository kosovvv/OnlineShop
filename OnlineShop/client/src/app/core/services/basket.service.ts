
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs';
import { Basket, BasketItem, BasketTotals } from '../../shared/models/basket';
import { DeliveryMethod } from '../../shared/models/deliveryMethod';
import { enviroment } from 'src/environments/environment';
import { Product } from '../../shared/models/products';

@Injectable({
  providedIn: 'root'
})
export class BasketService {
  
  baseUrl = enviroment.apiUrl + 'basket/'

  basketSource = new BehaviorSubject<Basket | null>(null);
  private basketTotalSource = new BehaviorSubject<BasketTotals | null>(null);

  basketSource$ = this.basketSource.asObservable();
  basketTotalSource$ = this.basketTotalSource.asObservable();

  constructor(private http: HttpClient) { }


  getBasket(id: string) {
    return this.http.get<Basket>(this.baseUrl + '?id=' + id).subscribe({
      next: basket => {
        this.basketSource.next(basket);
        this.calculateTotals();
      }
    })
  }

  setBasket(basket: Basket) {
    return this.http.post<Basket>(this.baseUrl, basket).subscribe({
      next: basket => {
        this.basketSource.next(basket);
        this.calculateTotals();
      }
    })
  }

  getCurrentBasketValue() {
    return this.basketSource.value;
  }

  setShippingPrice(deliveryMethod: DeliveryMethod) {
    const basket = this.getCurrentBasketValue();
    if (basket) {
      basket.shippingPrice = deliveryMethod.price;
      basket.deliveryMethodId = deliveryMethod.id;
      this.setBasket(basket);
    }
  }


  addItemToBasket(item: Product | BasketItem, quantity = 1) {
    if (this.isProduct(item)) item = this.mapProductItemToBasketItem(item);
    const basket = this.getCurrentBasketValue() ?? this.createBasket();
    basket.items = this.addOrUpdateItem(basket.items, item, quantity);
    this.setBasket(basket);
  }

  removeItemFromBasket(id: number, quantity = 1) {
    const basket = this.getCurrentBasketValue();
    if (!basket) return;
    const item = basket.items.find(x => x.id === id);
    if (item) {
      item.quantity -= quantity;
      if (item.quantity === 0) {
        basket.items = basket.items.filter(x => x.id !== id);
      }
      if (basket.items.length > 0) this.setBasket(basket);
      else this.deleteBasket(basket);
    }
  }

  deleteBasket(basket: Basket) {
    return this.http.delete(this.baseUrl + '?id=' + basket.id).subscribe({
      next: () => {
        this.deleteLocalBasket();
      }
    })
  }

  private deleteLocalBasket() : void {
    this.basketSource.next(null);
    this.basketTotalSource.next(null);
    localStorage.removeItem('basket_id');
  }

  private addOrUpdateItem(items: BasketItem[], itemToAdd: BasketItem, quantity: number): BasketItem[] {
    const item = items.find(x => x.id === itemToAdd.id);
    if (item) item.quantity += quantity;
    else {
      itemToAdd.quantity = quantity;
      items.push(itemToAdd);
    }
    return items;
  }

  private createBasket(): Basket {
    const basket = new Basket();
    localStorage.setItem('basket_id', basket.id);
    return basket;
  }

  private mapProductItemToBasketItem(item: Product): BasketItem {
    return {
      id: item.id,
      productName: item.name,
      price: item.price,
      quantity: 0,
      pictureUrl: item.pictureUrl,
      brand: item.productBrand,
      type: item.productType
    }
  }

  private calculateTotals() {
    const basket = this.getCurrentBasketValue();
    if (!basket) return;
    const subtotal = basket.items.reduce((a, b) => (b.price * b.quantity) + a, 0);
    const total = subtotal + basket.shippingPrice;
    this.basketTotalSource.next({shippingPrice: basket.shippingPrice, subtotal, total});
  }

  private isProduct(item: Product | BasketItem): item is Product {
    return (item as Product).productBrand !== undefined;
  }
}
