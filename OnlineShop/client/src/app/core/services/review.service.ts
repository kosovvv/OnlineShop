import { Injectable } from '@angular/core';
import { Observable, of, switchMap, tap } from 'rxjs';
import { Review } from '../../shared/models/review';
import { HttpClient } from '@angular/common/http';
import { AccountService } from './account.service';
import { enviroment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ReviewService {
  baseUrl = enviroment.apiUrl + 'review/'
  reviewCache = new Map<number, Review[]>();
  productReviewCache = new Map<number, boolean>();

  constructor(private http: HttpClient, private accountService: AccountService) { }

  createReview(data: any): Observable<Review> {
    return this.http.post<Review>(this.baseUrl, data).pipe(
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
  
    return this.http.get<Review[]>(this.baseUrl + productId).pipe(
      tap(reviews => {
        this.reviewCache.set(productId, reviews); 
      })
    );
  }

  editReview(id: number, data: any): Observable<Review> {
    return this.http.put<Review>(this.baseUrl + id, data).pipe(
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

  deleteReview(id: number): Observable<any> {
    return this.http.delete(this.baseUrl + id).pipe(
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

  isProductAlreadyReviewdByUser(productId: number): Observable<boolean> {
    if (this.productReviewCache.has(productId)) {
      return of(this.productReviewCache.get(productId)!);
    }

    return this.accountService.currentUser$.pipe(
      switchMap(user => {
        if (user) {
          return this.http.get<boolean>(this.baseUrl + `isReviewed/${productId}`).pipe(
            tap(result => {
              this.productReviewCache.set(productId, result);
            })
          );
        }
        this.productReviewCache.set(productId, true);
        return of(true);
      })
    );
  }
}
