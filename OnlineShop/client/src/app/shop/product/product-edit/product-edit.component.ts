import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, ParamMap, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { BreadcrumbService } from 'xng-breadcrumb';
import { Product } from 'src/app/shared/models/products';
import { BasketService } from 'src/app/shared/services/basket.service';
import { forkJoin, of, switchMap, take } from 'rxjs';
import { Basket, BasketItem } from 'src/app/shared/models/basket';
import { Type } from 'src/app/shared/models/type';
import { Brand } from 'src/app/shared/models/brand';
import { TypeService } from 'src/app/shared/services/type-service';
import { BrandService } from 'src/app/shared/services/brand.service';
import { ProductService } from 'src/app/shared/services/product.service';

@Component({
  selector: 'app-product-edit',
  templateUrl: './product-edit.component.html',
  styleUrls: ['./product-edit.component.scss']
})
export class ProductEditComponent implements OnInit {
  errors: string[] | null = null;
  types: Type[] = [];
  brands : Brand[] = [];

  productForm = this.fb.group({
    id: [0, Validators.required],
    name: ['', Validators.required],
    description: ['', [Validators.required,]],
    price: [0, [Validators.required]],
    pictureUrl: ['', [Validators.required]],
    productType: ['', [Validators.required]],
    productBrand: ['', [Validators.required]],
  })

  constructor(private fb: FormBuilder,private typeService: TypeService,private productService: ProductService, 
    private router: Router, private toastr: ToastrService,
    private bcService: BreadcrumbService, private activatedRoute : ActivatedRoute, private basketService: BasketService,
  private brandService: BrandService) {
      this.bcService.set('@editProduct', ' ')
    }
  
  ngOnInit(): void {
    this.loadProduct();
    this.getProductsAndTypes();
  }

  getProductsAndTypes() {
    forkJoin([
      this.typeService.getTypes(),
      this.brandService.getBrands()
    ]).subscribe({
      next: ([types, brands]) => {
        this.types = types;
        this.brands = brands;
      },
      error: (error) => {
        this.toastr.error(error);
      }
    });
  }

  loadProduct() {
    this.activatedRoute.paramMap.pipe(
      switchMap((params: ParamMap) => {
        const id = params.get('id');
        console.log(id);
        if (id !== null && id !== undefined) {
          return this.productService.getProduct(+id);
        } else {
          return of(null);
        }
      })
    ).subscribe({
      next: (product: Product | null) => {
        console.log(product);
        if (product) {
          console.log("in the next")
          this.productForm.patchValue(product, {emitEvent: false});
          this.bcService.set('@editProduct', `Edit ${product.name}`); 
          console.log("in the next")
        }
      },
      error: error => this.toastr.error(error)
    })

  }

  

  onSubmit() {
    this.productService.editProduct(this.productForm.value as Product).subscribe({
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
        this.router.navigateByUrl(`/shop/${product.id}`);
      }
    })
    
  }
  updateBasket(item: BasketItem, product: Product) {
    item.productName = product.name;
    item.price = product.price;
    item.brand = product.productBrand;
    item.type = product.productType;
  }

  getDefaultProductTypeValue(): any {
    return this.productForm.controls['productType'].value;
  }
  getDefaultProdtyBrandValue(): any {
    return this.productForm.controls['productBrand'].value;
  }
}


