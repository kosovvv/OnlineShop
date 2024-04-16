import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ShopComponent } from './shop.component';
import { ProductItemComponent } from './product/product-item/product-item.component';
import { SharedModule } from '../shared/shared.module';
import { ProductDetailsComponent } from './product/product-details/product-details.component';
import { ShopRoutingModule } from './shop-routing.module';
import { ProductCreateComponent } from './product/product-create/product-create.component';
import { ProductEditComponent } from './product/product-edit/product-edit.component';
import { AddReviewComponent } from './review/add-review/add-review.component';
import { ProductReviewsComponent } from './review/product-reviews/product-reviews.component';
import { ReviewCardComponent } from './review/review-card/review-card.component';
import { EditReviewComponent } from './review/edit-review/edit-review.component';
import { BrandManageComponent } from './brand/brand-manage/brand-manage.component';
import { TypeManageComponent } from './type/type-manage/type-manage.component';
import { TypeCreateComponent } from './type/type-create/type-create.component';
import { TypeEditComponent } from './type/type-edit/type-edit.component';
import { BrandCreateComponent } from './brand/brand-create/brand-create.component';
import { BrandEditComponent } from './brand/brand-edit/brand-edit.component';



@NgModule({
  declarations: [
    ShopComponent,
    ProductItemComponent,
    ProductDetailsComponent,
    ProductCreateComponent,
    ProductEditComponent,
    AddReviewComponent,
    ProductReviewsComponent,
    ReviewCardComponent,
    EditReviewComponent,
    BrandManageComponent,
    TypeManageComponent,
    TypeCreateComponent,
    TypeEditComponent,
    BrandCreateComponent,
    BrandEditComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    ShopRoutingModule
  ],
})
export class ShopModule { }
