import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterLink,Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';

@Component({
  selector: 'app-home',
  imports: [RouterLink, CommonModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent  {
  constructor(private router: Router, private jwthelper: JwtHelperService) {}

  navigateToProducts() {
    this.router.navigate(['/products']);
  }

  logout(): void {
    localStorage.removeItem('jwtToken');
    this.router.navigate(['/login']);
  }
}




