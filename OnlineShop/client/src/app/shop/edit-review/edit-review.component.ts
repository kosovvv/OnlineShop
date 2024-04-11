import { Component, ElementRef, EventEmitter, Input, Output, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ShopService } from '../shop.service';
import { Review } from 'src/app/shared/models/review';

@Component({
  selector: 'app-edit-review',
  templateUrl: './edit-review.component.html',
  styleUrls: ['./edit-review.component.scss']
})
export class EditReviewComponent {
  
  @Input() review!: Review
  @Output() onReviewEdit = new EventEmitter<Review>();
  reviewForm!: FormGroup;
  @ViewChild("closeButton") closeButton!: ElementRef

  constructor(private fb: FormBuilder, private shopService: ShopService, private toastr: ToastrService) {}

  ngOnInit() {
    this.reviewForm = this.fb.group({
      score: [undefined, Validators.required],
      description: ['', Validators.required],
    });
    this.loadScoreAndDescription();
  }


  editReview() {
    this.shopService.editReview(this.review.id,this.reviewForm.value).subscribe({
      next: (updatedReview) => {
        this.toastr.success("Successfully edited review!")
        this.closeButton.nativeElement.click();
        this.onReviewEdit.emit(updatedReview);
      }
    })
  }

  loadScoreAndDescription() {
    this.reviewForm.get('score')?.patchValue(this.review.score);
    this.reviewForm.get('description')?.patchValue(this.review.description);
  }

  setScore($event: any) {
    this.reviewForm.get('score')?.patchValue($event);
  }

}
