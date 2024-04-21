import { Injectable } from '@angular/core';
import { Product } from '../../shared/models/products';
import { Pagination } from '../../shared/models/pagination';
import { ShopParams } from '../../shared/models/shopParams';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, of, tap } from 'rxjs';
import { AccountService } from './account.service';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  baseUrl = 'https://localhost:5001/api/'
  //cache
  products: Product[] = [];
  pagination?: Pagination<Product[]>;
  shopParams = new ShopParams();
  productCache = new Map<string,Pagination<Product[]>>();

  constructor(private http:HttpClient, private accountService: AccountService) { }


  getProducts(useCache = true): Observable<Pagination<Product[]>> {
    if (useCache) {
      this.productCache = new Map();
    }
  
    if (this.productCache.size > 0 && useCache) {
      if (this.productCache.has(Object.values(this.shopParams).join('-'))) {
        this.pagination = this.productCache.get(Object.values(this.shopParams).join('-'));
  
        if (this.pagination) {
          return of(this.pagination);
        }
      }
    }
  
    let params = new HttpParams();
    params = params.append("sort", this.shopParams.sort);
    params = params.append("pageIndex", this.shopParams.pageNumber);
    params = params.append("pageSize", this.shopParams.pageSize);
  
    if (this.shopParams.brandId > 0) {
      params = params.append("brandId", this.shopParams.brandId);
    }
    if (this.shopParams.typeId > 0) {
      params = params.append("typeId", this.shopParams.typeId);
    }
    if (this.shopParams.search) {
      params = params.append("search", this.shopParams.search);
    }    
  
    return this.http.get<Pagination<Product[]>>(this.baseUrl + 'products', { params }).pipe(
      tap(response => {
        this.productCache.set(Object.values(this.shopParams).join('-'), response);
        this.pagination = response;
      })
    );
  }
  


  setShopParams(params: ShopParams) {
    this.shopParams = params;
  }

  getShopParams() {
    return this.shopParams;
  }

  createProduct(data: any) {
    return this.http.post<Product>(this.baseUrl + 'products/', data)
  }

  getProduct(id :number) { 
    const product = [...this.productCache.values()]
    .reduce((acc, paginatedResult) => {
      return {...acc, ...paginatedResult.data.find(x => x.id === id)}
    }, {} as Product)


    if (Object.keys(product).length !== 0) {
      return of(product);  
    }

    return this.http.get<Product>(this.baseUrl + 'products/' + id);
  }

  editProduct(product: Product) {
    return this.http.put<Product>(this.baseUrl + 'products/' + product.id, product).pipe(
      tap(response => {
        const cache = this.productCache.get(Object.values(this.shopParams).join('-'));
        if (cache) {
          const index = cache.data.findIndex(x => x.id === product.id);
          if (index !== -1) {
            cache.data[index] = { ...cache.data[index], ...product };
            this.productCache.set(Object.values(this.shopParams).join('-'), cache); 
          }
        }
      })
    );
  }


  deleteProduct(id : number) {
    return this.http.delete(this.baseUrl + 'products/' + id).pipe();
  }
}
