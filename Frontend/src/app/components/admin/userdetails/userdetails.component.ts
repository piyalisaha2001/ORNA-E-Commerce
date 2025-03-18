import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';

@Component({
  selector: 'app-userdetails',
  imports: [ CommonModule],
  templateUrl: './userdetails.component.html',
  styleUrl: './userdetails.component.css'
})
export class UserdetailsComponent {
  usersList: any [] = [];

  constructor(private http: HttpClient) {}

  getAlluser() {
    this.http.get("https://localhost:7183/api/User").subscribe((result:any) => {
      this.usersList = result;
      console.log(this.usersList);
      console.log("User details fetched successfully");
    })
  }
}
