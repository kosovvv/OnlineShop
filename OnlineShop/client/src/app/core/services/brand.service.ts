import { Injectable } from '@angular/core';
import { Brand } from '../../shared/models/brand';
import { HttpClient } from '@angular/common/http';
import { of, tap } from 'rxjs';
import { enviroment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class BrandService {

  baseUrl = enviroment.apiUrl + 'brands/'
  brands: Brand[] = [];

  constructor(private http: HttpClient) { }

  getBrands() {
    if (this.brands.length > 0) {
      return of(this.brands);
    }
    
    return this.http.get<Brand[]>(this.baseUrl).pipe(
      tap(brands => {
        this.brands = brands;
      })
    );
  }
  
  createBrand(brand: any) {
    return this.http.post<Brand>(this.baseUrl, brand).pipe(
      tap(brand => {
        console.log(brand);
        this.brands.push(brand)
      })
    )
  }

  editBrand(brandId: number, brand: Brand) {
    return this.http.put<Brand>(this.baseUrl + brandId, brand).pipe(
      tap(brand => {
        const brandToEdit = this.brands.find(x => x.id == brand.id);
        if (brandToEdit) {
          brandToEdit.name = brand.name;
          brandToEdit.pictureUrl = brand.pictureUrl
          brandToEdit.products = brand.products;
        }
      })
    )
  }
  deleteBrand(brandId: number) {
    return this.http.delete<boolean>(this.baseUrl + brandId).pipe(
      tap(isDeleted => {
        if (isDeleted) {
          const index = this.brands.findIndex(x => x.id == brandId)
          this.brands.splice(index, 1);
        }
      })
    )
  }
}
