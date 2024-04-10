import { Component, EventEmitter, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-add-review',
  templateUrl: './add-review.component.html',
  styleUrls: ['./add-review.component.scss']
})
export class AddReviewComponent {
  constructor(private fb:FormBuilder) {}
  @Output('onReviewSubmitted') onReviewSubmitted = new EventEmitter<FormGroup>();

  reviewForm = this.fb.group({
    score: [undefined, Validators.required],
    description: ['', Validators.required],
  })

  onReviewSubmit() {
    this.onReviewSubmitted.emit(this.reviewForm);
  }

  setScore($event : any) {
    this.reviewForm.get('score')?.patchValue($event);
  }
}
