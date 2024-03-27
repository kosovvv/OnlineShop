import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, delay, finalize } from 'rxjs';
import { BusyService } from '../services/busy.service';

@Injectable()
export class LoadingInterceptor implements HttpInterceptor {

  constructor(private busySerice:BusyService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    if (!request.url.includes('emailExists') || request.method === "POST" && request.url.includes('orders') 
    || request.method === 'DELETE') {
     next.handle(request);
    }


    this.busySerice.busy();
    return next.handle(request).pipe(
      delay(1000),
      finalize(() => this.busySerice.idle())
    );
  }
}
