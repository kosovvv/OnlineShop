import { Component } from '@angular/core';
import { AccountService } from 'src/app/shared/services/account.service';
import { BasketService } from 'src/app/shared/services/basket.service';
import { BasketItem } from 'src/app/shared/models/basket';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.scss']
})
export class NavBarComponent {

  constructor(public basketService: BasketService, public accountService: AccountService) {}

  getCount(items: BasketItem[]) {
    let count = 0;

    items.forEach(element => {
      count += element.quantity;
    });

    return count;
  }

}
