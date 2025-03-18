import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Cart, Checkout } from '../model/cart';
 

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private apiUrl = 'https://localhost:7183/api/Cart'; // Adjust the API URL according to your project

  constructor(private http: HttpClient) {}

  addItemToCart(username: string, productId: number, quantity: number): Observable<Cart> {
    const url = `${this.apiUrl}/AddItem/${username}?productId=${productId}&quantity=${quantity}`;
    const token = localStorage.getItem('token'); // Retrieve the token from local storage
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.http.post<Cart>(url, null, { headers });
  }

  getCart(username: string): Observable<Cart> {
    const url = `${this.apiUrl}/${username}`;
    const token = localStorage.getItem('token'); // Retrieve the token from local storage
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.http.get<Cart>(url, { headers });
  }

  updateCartItem(username: string, productId: number, quantity: number): Observable<Cart> {
    const url = `${this.apiUrl}/UpdateItem/${username}?productId=${productId}&quantity=${quantity}`;
    const token = localStorage.getItem('token'); // Retrieve the token from local storage
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.http.put<Cart>(url, null, { headers });
  }

  removeItemFromCart(username: string, productId: number): Observable<Cart> {
    const url = `${this.apiUrl}/RemoveItem?username=${username}&productId=${productId}`;
    const token = localStorage.getItem('token'); // Retrieve the token from local storage
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.http.delete<Cart>(url, { headers });
  }

  checkout(checkoutData: Checkout, username: string): Observable<any> {
    const url = `${this.apiUrl}/Checkout?username=${username}`;
    const token = localStorage.getItem('token'); // Retrieve the token from local storage
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.http.post<any>(url, checkoutData, { headers });
  }
  }
