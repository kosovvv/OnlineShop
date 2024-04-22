import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Review } from 'src/app/shared/models/review';
import { Product } from 'src/app/shared/models/products';
import { ReviewService } from 'src/app/core/services/review.service';

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

  constructor(private reviewService: ReviewService) {}

  ngOnInit(): void {
    this.getReviews(this.productId)
  }

  getReviews(productId: number | undefined) {
    if (productId) {
      this.reviewService.getReviewsByProduct(productId, true).subscribe({
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
