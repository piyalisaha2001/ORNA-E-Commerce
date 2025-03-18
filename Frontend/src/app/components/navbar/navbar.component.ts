import { Component} from '@angular/core';
// import { Router } from '@angular/router';

@Component({
  selector: 'app-navbar',
  imports: [],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent {

  //  dropdownOpen = false;
   
  //  constructor(private router: Router) {}
 
  // toggleDropdown() {
  //   this.dropdownOpen = !this.dropdownOpen;
  // }
 
  // navigateToLogin(userType: string, event: Event) {
  //   event.stopPropagation(); // Prevents closing dropdown immediately
  //   this.router.navigate(['/login'], { queryParams: { type: userType } });
  //   this.dropdownOpen = false;
  // }
 
  // @HostListener('document:click', ['$event'])
  // closeDropdownOnClickOutside(event: Event) {
  //   const clickedInside = (event.target as HTMLElement).closest('.account-dropdown');
  //   if (!clickedInside) {
  //     this.dropdownOpen = false;
  //   }
  // }
}
