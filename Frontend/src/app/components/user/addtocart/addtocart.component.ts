import { Component, OnInit } from '@angular/core';
import { CartService } from '../../../service/cart.service'; // Adjust the import according to your project structure
import { Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-addtocart',
  templateUrl: './addtocart.component.html',
  styleUrls: ['./addtocart.component.css'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule]
})
export class AddtocartComponent implements OnInit {
  quantity: number = 1;
  cart: any; // Adjust the type according to your project structure

  constructor(private cartService: CartService, private router: Router) {}

  ngOnInit(): void {
    this.loadCart();
  }

  loadCart() {
    const username = localStorage.getItem('username'); // Retrieve the username from local storage
    if (username) {
      this.cartService.getCart(username).subscribe({
        next: (cart) => {
          this.cart = cart;
          console.log('Cart loaded:', cart);
        },
        error: (error: HttpErrorResponse) => {
          console.error('Error loading cart:', error);
          if (error.status === 404) {
            alert('Cart not found. Please check the username or try again later.');
          } else {
            alert('Error loading cart. Please try again later.');
          }
        }
      });
    } else {
      console.error('Username not found in local storage.');
      alert('Please log in again.');
      this.router.navigate(['/login']);
    }
  }

  addToCart(id: number) {
    const username = localStorage.getItem('username'); // Retrieve the username from local storage
    if (username) {
      this.cartService.addItemToCart(username, id, this.quantity).subscribe({
        next: (response) => {
          this.cart = response;
          console.log('Item added to cart:', response);
        },
        error: (error: HttpErrorResponse) => {
          console.error('Error adding item to cart:', error);
          alert('Error adding item to cart. Please try again later.');
        }
      });
    } else {
      console.error('Username not found in local storage.');
      alert('Please log in again.');
      this.router.navigate(['/login']);
    }
  }

  increaseQuantity(cartItem: any) {
    if (cartItem.quantity < 5) {
      cartItem.quantity++;
      this.updateCartItem(cartItem);
    }
  }

  decreaseQuantity(cartItem: any) {
    if (cartItem.quantity > 1) {
      cartItem.quantity--;
      this.updateCartItem(cartItem);
    }
  }
  
  
  updateCartItem(cartItem: any) {
    const username = localStorage.getItem('username'); // Retrieve the username from local storage
    if (username) {
      this.cartService.updateCartItem(username, cartItem.productId, cartItem.quantity).subscribe({
        next: (response: any) => {
          console.log('Cart item updated:', response);
        },
        error: (error: HttpErrorResponse) => {
          console.error('Error updating cart item:', error);
          alert('You have updated the quantity.');
        }
      });
    } else {
      console.error('Username not found in local storage.');
      alert('Please log in again.');
      this.router.navigate(['/login']);
    }
  }
  

  removeItem(productId: number) {
    const username = localStorage.getItem('username'); // Retrieve the username from local storage
    if (username) {
      this.cartService.removeItemFromCart(username, productId).subscribe({
        next: (response: any) => {
          console.log('Item removed from cart:', response);
          this.loadCart(); // Reload the cart after removing the item
        },
        error: (error: HttpErrorResponse) => {
          console.error('Error removing item from cart:', error);
          alert('Error removing item from cart. Please try again later.');
        }
      });
    } else {
      console.error('Username not found in local storage.');
      alert('Please log in again.');
      this.router.navigate(['/login']);
    }
  }

  getTotalPrice(): number {
    return this.cart?.items?.reduce((acc: number, item: any) => acc + (item.productPrice * item.quantity), 0) || 0;
  }

  navigateToCheckout() {
    this.router.navigate(['/checkout']);
  }
}


