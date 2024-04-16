import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Product } from '../shared/models/products';
import { Brand } from '../shared/models/brand';
import { Type } from '../shared/models/type';
import { ShopParams } from '../shared/models/shopParams';
import { TypeService } from '../shared/services/type-service';
import { BrandService } from '../shared/services/brand.service';
import { ProductService } from '../shared/services/product.service';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss']
})
export class ShopComponent implements OnInit {
  @ViewChild("search") searchTerm?:ElementRef
  products: Product[] = [];
  brands: Brand[] = [];
  types: Type[] = [];
  
  shopParams: ShopParams
  totalCount = 0;

  sortOptions = [
    {name: "Alphabetical", value: "name"},
    {name: "Price: Low to high", value: "priceAsc"},
    {name: "Price: High to low", value: "priceDesc"},
  ]


  constructor(private productService: ProductService, private typeService: TypeService, private brandService: BrandService) {
    this.shopParams = productService.getShopParams();
  }

  ngOnInit(): void {
    this.getProducts();
    this.getBrands(true);
    this.getTypes(true);
  }

  getProducts() {
    this.productService.getProducts().subscribe({
      next: response => {
        this.products = response.data,
        this.totalCount = response.count;
      },
      error: error => console.log(error)
    })
  }

  getBrands(onlyWithProducts = false) {
    this.brandService.getBrands().subscribe({
      next: (brands) => {
        if (onlyWithProducts) {
          brands = brands.filter(x => x.products?.length! > 0)
        }
        this.brands = [{id: 0, name: 'All'}, ...brands]
      },
      error: error => console.log(error)
    })
  }

  getTypes(onlyWithProducts = false) {
    this.typeService.getTypes().subscribe({
      next: (types) => {
        if (onlyWithProducts) {
          types = types.filter(x => x.products?.length! > 0)
        }
        this.types = [{id: 0, name: 'All'}, ...types]
      },
      error: error => console.log(error)
    })
  }

  onBrandSelected(brandId: number) {
    const params = this.productService.getShopParams();
    params.brandId = brandId;
    params.pageNumber = 1;
    this.productService.setShopParams(params);
    this.shopParams = params;
    this.getProducts();
  }
  onTypeSelected(typeId: number) {
    const params = this.productService.getShopParams();
    params.typeId = typeId;
    params.pageNumber = 1;
    this.productService.setShopParams(params);
    this.shopParams = params;
    this.getProducts();
  }

  onSortSelected(event: any) {
    const params = this.productService.getShopParams();
    params.sort = event.target.value;
    this.productService.setShopParams(params);
    this.shopParams = params;
    this.getProducts();
  }

  onPageChanged(event:any) {
    const params = this.productService.getShopParams();
    if (params.pageNumber != event) {
      params.pageNumber = event;
      this.productService.setShopParams(params);
      this.shopParams = params;
      this.getProducts();
    }
    
  }

  onSearch() {
    const params = this.productService.getShopParams();
    params.search = this.searchTerm?.nativeElement.value
    params.pageNumber = 1;
    this.productService.setShopParams(params);
    this.shopParams = params;
    this.getProducts();
  }

  onReset() {
    if (this.searchTerm) {
      this.searchTerm.nativeElement.value = '';
    }
    const params = new ShopParams();
    this.productService.setShopParams(params);
    this.shopParams = params;
    this.getProducts();
  }
}
