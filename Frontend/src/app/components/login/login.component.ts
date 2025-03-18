import { Component, inject } from '@angular/core';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Router, RouterLink } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { JwtHelperService } from '@auth0/angular-jwt';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  imports: [FormsModule, ReactiveFormsModule, HttpClientModule, RouterLink, CommonModule],
  host: { 'skiphydration': 'true' }
})
export class LoginComponent {
  loginObj = { username: '', password: '' };
  http = inject(HttpClient);
  router = inject(Router);

  constructor(private jwthelper: JwtHelperService) {}
  onLogin() {
    console.log("Login attempt");
    this.http.post('https://localhost:7183/api/Authentication/login', this.loginObj).subscribe(
      (res: any) => {
        console.log(res);
        if (res && res.token) { // Check if the response contains a token
          alert('Login Successful');
          localStorage.setItem('token', res.token); // Store the token in local storage
          localStorage.setItem('username', this.loginObj.username); // Store the username in local storage
          localStorage.setItem('id', res.userId); // Store the user id in local storage

          if (this.loginObj.username === 'Asaha' && this.loginObj.password === 'Asaha@123') {
            console.log("Admin Login");
            this.router.navigate(['/adminpanel']); // Navigate to admin panel
          } else {
            console.log("User Login");
            this.router.navigate(['/home']); // Navigate to home page
          }
        } else {
          alert('Login Failed');
        }
      },
      
      (error) => {
        console.error('Login failed', error);
        alert('Login Failed');
      }
    );
  }
  // isUserAuthenticated() {
  //   const token = localStorage.getItem('jwtToken');

  //   if(token && !this.jwthelper.isTokenExpired(token)) {
  //     console.log("User is authenticated");
  //     return true;
  //   } else {
  //     console.log("User is not authenticated");
  //     this.router.navigate(['/login']);
  //     return false
  //   }
  // }
}