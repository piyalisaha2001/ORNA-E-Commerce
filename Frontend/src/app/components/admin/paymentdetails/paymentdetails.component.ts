import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';

@Component({
  selector: 'app-paymentdetails',
  imports: [CommonModule],
  templateUrl: './paymentdetails.component.html',
  styleUrl: './paymentdetails.component.css'
})
export class PaymentdetailsComponent {
  PaymentsList: any [] = [];

  constructor(private http: HttpClient) {}

  getPaymentdetails() {
    this.http.get("https://localhost:7183/api/Payment").subscribe((result:any) => {
      this.PaymentsList = result;
      console.log(this.PaymentsList);
      console.log("Payment details fetched successfully");
    })
  }
}
