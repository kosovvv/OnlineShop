import { CdkStep, CdkStepper } from '@angular/cdk/stepper';
import { Component, Input } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { BasketService } from 'src/app/core/services/basket.service';
import { PaymentService } from 'src/app/core/services/payment.service';

@Component({
  selector: 'app-checkout-review',
  templateUrl: './checkout-review.component.html',
  styleUrls: ['./checkout-review.component.scss']
})
export class CheckoutReviewComponent {

  @Input() appStepper?:CdkStepper

  constructor(private paymentService:PaymentService, private toastr: ToastrService) {}

  createPaymentIntent() {
    this.paymentService.createPaymentIntent().subscribe({
      next: () => this.appStepper?.next(),
      error: error => this.toastr.error(error.message)
    })
  }
}
