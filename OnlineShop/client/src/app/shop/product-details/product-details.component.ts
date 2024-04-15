import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap, Router } from '@angular/router';
import { of, switchMap, take } from 'rxjs';
import { BasketService } from 'src/app/basket/basket.service';
import { BreadcrumbService } from 'xng-breadcrumb';
import { ShopService } from '../shop.service';
import { Product } from 'src/app/shared/models/products';
import { AccountService } from 'src/app/account/account.service';
import { ToastrService } from 'ngx-toastr';
import { Review } from 'src/app/shared/models/review';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.scss']
})
export class ProductDetailsComponent implements OnInit {
  product!: Product;
  quantity = 1;
  quantityInBasket = 0;
  isReviewButtonActive = false;


  constructor(private shopService: ShopService, private activatedRoute: ActivatedRoute, private router : Router,
    private bcService: BreadcrumbService, private basketService: BasketService,
     public accountService: AccountService, private toastr: ToastrService) {
      this.bcService.set('@productDetails', ' ')
    }

  ngOnInit(): void {
    this.loadProduct();
  }

  updateProduct($event: Review) {
    if (this.product.reviewsCount == 1) {
      this.product.averageScore = $event.score;
    } else {
      let sumRating = this.product.reviewsCount * this.product.averageScore;
      sumRating += $event.score;
      this.product.reviewsCount += 1;
      this.product.averageScore = sumRating / this.product.reviewsCount;
    }
}

  loadProduct() {
    this.activatedRoute.paramMap.pipe(
      switchMap((params: ParamMap) => {
        const id = params.get('id');
        if (id !== null && id !== undefined) {
          return this.shopService.getProduct(+id);
        } else {
          return of(null);
        }
      })
    ).subscribe({
      next: product => {
        if (product !== null) {
          this.product = product;
          console.log(product);
          this.checkIfIsUserReviewedProduct();
          this.bcService.set('@productDetails', product.name);
          this.basketService.basketSource$.pipe(take(1)).subscribe({
            next: basket => {
              if (this.product && this.product.id) {
                const item = basket?.items.find(x => x.id === this.product?.id);
                if (item) {
                  this.quantity = item.quantity;
                  this.quantityInBasket = item.quantity;
                }
              }
            }
          });
        } else {
          this.toastr.error("No 'id' parameter found in the URL");
        }
      },
      error: error => console.log(error)
    });
  }
  
  checkIfIsUserReviewedProduct() {
    if (this.product) {
      this.shopService.isProductAlreadyReviewdByUser(this.product.id).subscribe({
        next: (isReviewed) => {
          console.log(isReviewed)
          this.isReviewButtonActive = !isReviewed
        }
      })
    }
  }

  updateButton($event : boolean) {
    this.isReviewButtonActive = $event;
  }

  incrementQuantity() {
    this.quantity++;
  }

  decrementQuantity() {
    this.quantity--;
  }

  updateBasket() {
    if (this.product) {
      if (this.quantity > this.quantityInBasket) {
        const itemsToAdd = this.quantity - this.quantityInBasket;
        this.quantityInBasket += itemsToAdd;
        this.basketService.addItemToBasket(this.product, itemsToAdd);
      } else {
        const itemsToRemove = this.quantityInBasket - this.quantity;
        this.quantityInBasket -= itemsToRemove;
        this.basketService.removeItemFromBasket(this.product.id, itemsToRemove);
      }
    }
  }

  deleteItem(id : number) {
    this.shopService.deleteProduct(id).subscribe({
      next: (response) => {
        this.router.navigateByUrl('/shop')
        this.toastr.success("Product deleted successfully.")
        this.basketService.basketSource$.pipe(take(1)).subscribe({
          next: (basket) => {
            if (basket) {
              basket.items = basket.items.filter(x => x.id !== id);
              this.basketService.setBasket(basket);
            }
          }
        })
      },
      error: (error) => {
        this.toastr.error("Error deleting product.")
      }
    });
  }

  get buttonText() {
    return this.quantityInBasket === 0 ? 'Add to basket' : 'Update basket';
  }

}
