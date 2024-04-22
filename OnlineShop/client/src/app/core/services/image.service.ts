import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { enviroment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ImageService {

  constructor(private http: HttpClient) { }
  
  baseUrl = enviroment.apiUrl + 'images/'
  uploadImage(data: FormData) {
    return this.http.post<FormData>(this.baseUrl, data)
  }
}
