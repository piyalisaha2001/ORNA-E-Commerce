// import { CommonModule } from '@angular/common';
// import { Component, OnInit } from '@angular/core';
// import { FormsModule, ReactiveFormsModule } from '@angular/forms';
// import { ActivatedRoute, Router } from '@angular/router';
// import { CartService } from '../../../service/cart.service';
// import { HttpClient, HttpErrorResponse } from '@angular/common/http';

// @Component({
//   selector: 'app-checkout',
//   imports: [FormsModule, ReactiveFormsModule, CommonModule, HttpClient],
//   templateUrl: './checkout.component.html',
//   styleUrl: './checkout.component.css'
// })

// export class CheckoutComponent implements OnInit {
//  paymentDetails: any = {
//   "paymentMethod": "",
//   "amount": 0,
//   "cardNo": 0,
//   "expiryDate": 0,
//   "upiId": "",
//   "totalAmount": 0
//  };


//   constructor(private cartService: CartService, private route: ActivatedRoute, private router: Router, private http: HttpClient) {}

//   getPaymentDetails() {
//     const url = `${this.apiUrl}?username=${username}`;
//     this.paymentDetails.amount = this.amount;
//   }
  
//   ngOnInit(): void {

    
//   }
// }


// 2nd part 

import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Checkout } from '../../../model/cart';
import { CartService } from '../../../service/cart.service';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-checkout',
  imports: [ ReactiveFormsModule, CommonModule, HttpClientModule],
  standalone: true,
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.css']
})
export class CheckoutComponent implements OnInit {
  checkoutForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private cartService: CartService,
    private router: Router
  ) {
    this.checkoutForm = this.fb.group({
      paymentMethod: ['', Validators.required],
      amount: ['', [Validators.required, Validators.min(1)]],
      cardNo: [''],
      expiryDate: [''],
      upiId: ['']
    });
  }

  ngOnInit(): void {
    this.calculateTotalAmount();
  }

  calculateTotalAmount() {
    const username = localStorage.getItem('username'); // Retrieve the username from local storage
    if (username) {
      this.cartService.getCart(username).subscribe({
        next: (cart) => {
          const totalAmount = cart.items.reduce((acc: number, item: any) => acc + (item.productPrice * item.quantity), 0);
          this.checkoutForm.patchValue({ amount: totalAmount });
        },
        error: (error) => {
          console.error('Error loading cart:', error);
          alert('Error loading cart. Please try again later.');
        }
      });
    } else {
      console.error('Username not found in local storage.');
      alert('Please log in again.');
      this.router.navigate(['/login']);
    }
  }

  onSubmit() {
    if (this.checkoutForm.valid) {
      const checkoutData: Checkout = this.checkoutForm.value;
      const username = localStorage.getItem('username'); // Retrieve the username from local storage
      if (username) {
        this.cartService.checkout(checkoutData, username).subscribe({
          next: (response) => {
            console.log('Checkout successful:', response);
            alert('Payment successful!');
            this.router.navigate(['/paymentstatus']);
          },
          error: (error) => {
            console.error('Error during checkout:', error);
            alert('Payment failed. Please try again.');
          }
        });
      } else {
        console.error('Username not found in local storage.');
        alert('Please log in again.');
        this.router.navigate(['/login']);
      }
    } else {
      alert('Please fill in all required fields.');
    }
  }
}
