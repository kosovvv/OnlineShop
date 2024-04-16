import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ImageService {

  constructor(private http: HttpClient) { }
  
  baseUrl = 'https://localhost:5001/api/'
  uploadImage(data: FormData) {
    return this.http.post<FormData>(this.baseUrl + 'images', data)
  }
}
