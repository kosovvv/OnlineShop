import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Review } from 'src/app/shared/models/review';
import { ShopService } from '../shop.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-review-card',
  templateUrl: './review-card.component.html',
  styleUrls: ['./review-card.component.scss']
})
export class ReviewCardComponent {
 @Input() reviews : Review[] = [];
 @Output() reviewDeletedEvent = new EventEmitter<boolean>();
 @Output() reviewEdittedEvent = new EventEmitter<Review>();

  constructor(private shopService: ShopService, private toastr: ToastrService) {}

 deleteReview(reviewId : number) {
  this.shopService.deleteReview(reviewId).subscribe({
    next: () => {
      this.toastr.success('Successfully deleted review');
      this.reviewDeletedEvent.emit(true);
    }
  });
 }

 onReviewEdit($event : Review) {
    this.reviewEdittedEvent.emit($event);
 }
}
