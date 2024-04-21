import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of, tap } from 'rxjs';
import { Type } from '../../shared/models/type';

@Injectable({
  providedIn: 'root'
})
export class TypeService {
  types: Type[] = [];
  baseUrl = 'https://localhost:5001/api/'
  constructor(private http: HttpClient) { }

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
  

  createType(type: any) {
    return this.http.post<Type>(this.baseUrl + 'types', type).pipe(
      tap(type => {
        this.types.push(type)
      })
    )
  }

  editType(typeId: number, type: Type) {
    return this.http.put<Type>(this.baseUrl + `types/${typeId}`, type).pipe(
      tap(type => {
        const typeToEdit = this.types.find(x => x.id == type.id);
        if (typeToEdit) {
          typeToEdit.name = type.name;
          typeToEdit.pictureUrl = type.pictureUrl
        }
      })
    )
  }
  deleteType(typeId: number) {
    return this.http.delete<boolean>(this.baseUrl + `types/${typeId}`).pipe(
      tap(isDeleted => {
        if (isDeleted) {
          const index = this.types.findIndex(x => x.id == typeId)
          this.types.splice(index, 1);
        }
      })
    )
  }
}
