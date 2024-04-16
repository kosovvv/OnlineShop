import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductDetailsComponent } from './product/product-details/product-details.component';
import { ShopComponent } from './shop.component';
import { ProductCreateComponent } from './product/product-create/product-create.component';
import { ProductEditComponent } from './product/product-edit/product-edit.component';
import { TypeManageComponent } from './type/type-manage/type-manage.component';
import { BrandManageComponent } from './brand/brand-manage/brand-manage.component';

const routes: Routes = [
  {path: '', component: ShopComponent},
  {path: 'create', component: ProductCreateComponent, data: {breadcrumb: {alias: 'createProduct'}}},
  {path: 'types', component: TypeManageComponent, data: {breadcrumb: {alias: 'types'}}},
  {path: 'brands', component: BrandManageComponent, data: {breadcrumb: {alias: 'brands'}}},
  {path: 'edit/:id', component: ProductEditComponent, data: {breadcrumb: {alias: 'editProduct'}}},
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
