import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ShopComponent } from './shop.component';
import { ProductItemComponent } from './product-item/product-item.component';
import { SharedModule } from '../shared/shared.module';
import { ProductDetailsComponent } from './product-details/product-details.component';
import { RouterModule } from '@angular/router';
import { ShopRoutingModule } from './shop-routing.module';
import { ProductCreateComponent } from './product-create/product-create.component';
import { ProductEditComponent } from './product-edit/product-edit.component';
import { AddReviewComponent } from './add-review/add-review.component';
import { ProductReviewsComponent } from './product-reviews/product-reviews.component';
import { ReviewCardComponent } from './review-card/review-card.component';



@NgModule({
  declarations: [
    ShopComponent,
    ProductItemComponent,
    ProductDetailsComponent,
    ProductCreateComponent,
    ProductEditComponent,
    AddReviewComponent,
    ProductReviewsComponent,
    ReviewCardComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    ShopRoutingModule
  ],
})
export class ShopModule { }
