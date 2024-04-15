import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Product } from '../shared/models/products';
import { Pagination } from '../shared/models/pagination';
import { Observable, map, of, tap } from 'rxjs';
import { Brand } from '../shared/models/brand';
import { Type } from '../shared/models/type';
import { ShopParams } from '../shared/models/shopParams';
import { Review } from '../shared/models/review';

@Injectable({
  providedIn: 'root'
})

export class ShopService {

  baseUrl = 'https://localhost:5001/api/'
  //cache
  products: Product[] = [];
  brands: Brand[] = [];
  types: Type[] = [];
  pagination?: Pagination<Product[]>;
  shopParams = new ShopParams();
  productCache = new Map<string,Pagination<Product[]>>();
  reviewCache = new Map<number, Review[]>();

  constructor(private http:HttpClient) { }


  getProducts(useCache = true): Observable<Pagination<Product[]>> {
    if (!useCache) {
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
    return this.http.post<Product>(this.baseUrl + 'products/create', data)
  }

  createReview(data: any): Observable<Review> {
    return this.http.post<Review>(this.baseUrl + 'review/create', data).pipe(
      tap(review => {
        const productId = data.reviewedProduct.id;
        if (productId) {
          const reviews = this.reviewCache.get(productId)!;
          reviews.push(review);
          this.reviewCache.set(productId, reviews);
        }
      })
    );
  }
  
  getReviewsByProduct(productId: number, useCache = true): Observable<Review[]> {
    if (useCache && this.reviewCache.has(productId)) {
      return of(this.reviewCache.get(productId)!); 
    }
  
    return this.http.get<Review[]>(this.baseUrl + `review/${productId}`).pipe(
      tap(reviews => {
        this.reviewCache.set(productId, reviews); 
      })
    );
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
  
  isProductAlreadyReviewdByUser(productId : number) {
    console.log("asdsa")
    return this.http.get<boolean>(this.baseUrl + `review/isReviewed/${productId}`)
  }

  editReview(id: number, data: any): Observable<Review> {
    return this.http.put<Review>(this.baseUrl + `review/${id}`, data).pipe(
      tap(updatedReview => {
        this.reviewCache.forEach((reviews, productId) => {
          const index = reviews.findIndex(review => review.id === id);
          if (index !== -1) {
            reviews[index] = updatedReview;
            this.reviewCache.set(productId, reviews);
          }
        });
      })
    );
  }
  

  deleteProduct(id : number) {
    return this.http.delete(this.baseUrl + 'products/' + id).pipe();
  }

  deleteReview(id: number): Observable<any> {
    return this.http.delete(this.baseUrl + `review/${id}`).pipe(
      tap(() => {
        this.reviewCache.forEach((reviews, productId) => {
          const index = reviews.findIndex(review => review.id === id);
          if (index !== -1) {
            reviews.splice(index, 1);
            this.reviewCache.set(productId, reviews);
          }
        });
      })
    );
  }
  

  getBrands() {
    if (this.brands.length > 0) {
      return of(this.brands);
    }
    return this.http.get<Brand[]>(this.baseUrl + 'brands').pipe(
      tap(brands => {
        this.brands = brands;
      })
    );
  }
  
  getTypes() {
    if (this.types.length > 0) {
      return of(this.types);
    }
    return this.http.get<Type[]>(this.baseUrl + 'types').pipe(
      tap(types => {
        this.types = types;
      })
    );
  }
  

  uploadImage(data: FormData) {
    return this.http.post<FormData>(this.baseUrl + 'images/upload', data)
  }
}
