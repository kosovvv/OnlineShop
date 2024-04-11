import { Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Product } from 'src/app/shared/models/products';
import { ShopService } from '../shop.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-add-review',
  templateUrl: './add-review.component.html',
  styleUrls: ['./add-review.component.scss']
})
export class AddReviewComponent implements OnInit {
  constructor(private fb: FormBuilder, private shopService: ShopService, private toastr: ToastrService) {}

  reviewForm!: FormGroup;
  @Input() product!: Product;
  @ViewChild("closeButton") closeButton!: ElementRef

  ngOnInit() {
    this.reviewForm = this.fb.group({
      score: [undefined, Validators.required],
      reviewedProduct: [this.product],
      description: ['', Validators.required],
    });
  }

  submitReview() {
    this.shopService.createReview(this.reviewForm.value).subscribe({
      next: () => {
        this.toastr.success("Successfully created review!")
        this.closeButton.nativeElement.click();
      }
    });
  }

  setScore($event: any) {
    this.reviewForm.get('score')?.patchValue($event);
  }
}
