import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IProduct } from '../model/product';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  
  apiURL = 'https://localhost:7183/api';
  constructor(private http: HttpClient) { }

  getAllProducts() :Observable<IProduct[]> {
    return this.http.get<any>(`${this.apiURL}/Product`);
  }

  deleteProduct( id:number) :Observable<any> {
    return this.http.delete(`${this.apiURL}/Product/${id}`);
  }

  editProduct( id: any, data:any) :Observable<any> {
    const headers = { 'Content-Type': 'application/json' };
    return this.http.put(`${this.apiURL}/Product/${id}`, data, {headers});
  }

  getProductById(productId: number): Observable<IProduct> {
    return this.http.get<IProduct>(`${this.apiURL}/Product/${productId}`);
  }
}

