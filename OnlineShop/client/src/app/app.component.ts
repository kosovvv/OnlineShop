import { Component, OnInit } from '@angular/core';
import { BasketService } from './core/services/basket.service';
import { AccountService } from './core/services/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'client';
  
  constructor(private basketService: BasketService, private accountService: AccountService) {
    
  }


  ngOnInit(): void {
    this.loadBasket();
    this.loadCurrentUser();
  }

  loadCurrentUser() {
    const token = localStorage.getItem('token');
    this.accountService.loadCurrentUser(token).subscribe();
    
  }

  loadBasket() {
    const basketId = localStorage.getItem('basket_id');

    if (basketId) {
      this.basketService.getBasket(basketId);
    }
  }
  
}
