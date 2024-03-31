import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ShopService } from '../shop.service';
import { BreadcrumbService } from 'xng-breadcrumb';
import { Product } from 'src/app/shared/models/products';
import { BasketService } from 'src/app/basket/basket.service';
import { take } from 'rxjs';
import { Basket, BasketItem } from 'src/app/shared/models/basket';

@Component({
  selector: 'app-product-edit',
  templateUrl: './product-edit.component.html',
  styleUrls: ['./product-edit.component.scss']
})
export class ProductEditComponent implements OnInit {
  errors: string[] | null = null;

  productForm = this.fb.group({
    id: [0, Validators.required],
    name: ['', Validators.required],
    description: ['', [Validators.required,]],
    price: [0, [Validators.required]],
    pictureUrl: ['', [Validators.required]],
    productType: ['', [Validators.required]],
    productBrand: ['', [Validators.required]],
  })

  constructor(private fb: FormBuilder, private shopService: ShopService, 
    private router: Router, private toastr: ToastrService,
    private bcService: BreadcrumbService, private activatedRoute : ActivatedRoute, private basketService: BasketService) {
      this.bcService.set('@editProduct', ' ')
    }
  
  ngOnInit(): void {
    this.loadProduct();
  }

  loadProduct() {
    const id = this.activatedRoute.snapshot.paramMap.get('id');
    if (id) {
      this.shopService.getProduct(+id).subscribe({
        next: (product : Product) => {
          this.productForm.patchValue(product);
          console.log(product);
          this.bcService.set('@editProduct', `Edit ${product.name}`);
        },
        error: error => this.toastr.error(error)
      })
    }
  }

  onSubmit() {
    this.shopService.editProduct(this.productForm.value as Product).subscribe({
      next: (product : Product) => {
        this.basketService.basketSource$.pipe(take(1)).subscribe({
          next: basket => {
            if (basket) {
              const item = basket.items.find(x => x.id === product.id);
              if (item) {
                this.updateBasket(item, product);
                this.basketService.setBasket(basket);
              }
            }
           
          }
        })
        this.router.navigateByUrl(`/shop/details/${product.id}`);
      }
    })
    
  }
  updateBasket(item: BasketItem, product: Product) {
    item.productName = product.name;
    item.price = product.price;
    item.brand = product.productBrand;
    item.type = product.productType;
  }
}


