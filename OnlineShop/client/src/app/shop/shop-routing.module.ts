import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductDetailsComponent } from './product-details/product-details.component';
import { ShopComponent } from './shop.component';
import { ProductCreateComponent } from './product-create/product-create.component';

const routes: Routes = [
  {path: '', component: ShopComponent},
  {path: 'create', component: ProductCreateComponent, data: {breadcrumb: {alias: 'createProduct'}}},
  {path: ':id', component: ProductDetailsComponent, data: {breadcrumb: {alias: 'productDetails'}}},
]

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [
    RouterModule
  ]
})
export class ShopRoutingModule { }
