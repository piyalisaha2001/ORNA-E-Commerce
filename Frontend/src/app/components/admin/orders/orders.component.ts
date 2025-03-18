import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { FormsModule, NgForm } from '@angular/forms';

interface Order {
  OrderId: number;
  OrderDate: Date;
  UserId: string;
  TotalAmount: number;
}

@Component({
  selector: 'app-orders',
  imports: [ CommonModule ],
  templateUrl: './orders.component.html',
  styleUrl: './orders.component.css'
})
export class OrdersComponent {

  orderList: any [] = [];

  constructor(private http: HttpClient) {}

  getOrders() {
    this.http.get("https://localhost:7183/api/Order").subscribe((result:any) => {
      this.orderList = result;
      console.log(this.orderList);
      console.log("Order details fetched successfully");
    })
  }
  
}
