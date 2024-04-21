import { Component, ElementRef, EventEmitter, Input, OnDestroy, OnInit, Output, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Product } from 'src/app/shared/models/products';
import { ToastrService } from 'ngx-toastr';
import { RatingComponent } from 'ngx-bootstrap/rating';
import { ReviewService } from 'src/app/core/services/review.service';

@Component({
  selector: 'app-add-review',
  templateUrl: './add-review.component.html',
  styleUrls: ['./add-review.component.scss']
})
export class AddReviewComponent  {
  constructor(private fb: FormBuilder, private reviewService: ReviewService, private toastr: ToastrService) {}


  reviewForm!: FormGroup;
  @Input() product!: Product;
  @Output() addReviewEvent = new EventEmitter<boolean>();
  @ViewChild("closeButton") closeButton!: ElementRef

  ngOnInit() {
    this.reviewForm = this.fb.group({
      score: [undefined, Validators.required],
      reviewedProduct: [this.product],
      description: ['', Validators.required],
    });
  }


  submitReview() {
    this.reviewService.createReview(this.reviewForm.value).subscribe({
      next: (review) => {
        this.toastr.success("Successfully created review!")
        this.closeButton.nativeElement.click();
        this.addReviewEvent.emit(false);
      }
    });
  }

  setScore($event: any) {
    this.reviewForm.get('score')?.patchValue($event);
  }
}
