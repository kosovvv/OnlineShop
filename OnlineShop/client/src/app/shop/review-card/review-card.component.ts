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

  constructor(private shopService: ShopService, private toastr: ToastrService) {}

 deleteReview(reviewId : number) {
  this.shopService.deleteReview(reviewId).subscribe({
    next: () => {
      this.toastr.success('Successfully deleted review');
    }
  });
 }
}
