import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ShopService } from '../shop.service';
import { Review } from 'src/app/shared/models/review';
import { Product } from 'src/app/shared/models/products';

@Component({
  selector: 'app-product-reviews',
  templateUrl: './product-reviews.component.html',
  styleUrls: ['./product-reviews.component.scss']
})
export class ProductReviewsComponent implements OnInit {

  @Input() productId? : number;
  @Output() reviewDeleted = new EventEmitter<boolean>();
  @Output() reviewEdited = new EventEmitter<Review>();
  reviews?: Review[]

  constructor(private shopService: ShopService) {}

  ngOnInit(): void {
    this.getReviews(this.productId)
  }

  getReviews(productId: number | undefined) {
    if (productId) {
      this.shopService.getReviewsByProduct(productId).subscribe({
        next: (reviews) => {
          this.reviews = reviews;
        } 
      })
    }
  }
  reviewDeletedEvent($event : boolean) {
    this.reviewDeleted.emit($event);
  }
  reviewEditedEvent($event : Review) {
    this.reviewEdited.emit($event);
  }
}
