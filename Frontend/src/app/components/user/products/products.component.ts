import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { CartService } from '../../../service/cart.service';


interface Product {
  productId: number;
  productName: string;
  description: string;
  price: number;
  productImage: string;
  category: { categoryName: string } | null;
}

interface Category {
  name: string;
  imageUrl: string;
}

@Component({
  selector: 'app-products',
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.css']
})
export class ProductUserComponent {
  userProducts: Product[] = [];
  categories: Category[] = [
    { name: 'rings', imageUrl: 'https://assets.ajio.com/medias/sys_master/root/20230519/MLln/6466d27a42f9e729d79a8d5a/-1117Wx1400H-466167376-silver-MODEL4.jpg' },
    { name: 'necklaces', imageUrl: 'https://emcgtdx36wv.exactdn.com/storage/2023/08/KNA936-6333.png?lossy=0&webp=90&avif=90&ssl=1' },
    { name: 'earrings', imageUrl: 'https://www.caratlane.com/blog/wp-content/uploads/2022/10/Wedding-n-Anniversary-20222408-1.jpg' },
    { name: 'bracelets', imageUrl: 'https://zariin.com/cdn/shop/files/BraceletofProtection-HamsaHand_2.jpg?v=1740732551' }
  ];
  selectedCategory: string | null = null;
  filteredProducts: Product[] = [];
  showFilter: boolean = false;
  minPrice: number | null = null;
  maxPrice: number | null = null;

  constructor(private router: Router, private route: ActivatedRoute, private http: HttpClient, private cartService: CartService) {}

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.selectedCategory = params.get('category');
      this.applyFilters();
    });
    this.getUserProducts();
  }

  getUserProducts() {
    this.http.get<Product[]>("https://localhost:7183/api/Product").subscribe({
      next:
        (result) => {
          this.userProducts = result;
          console.log(this.userProducts);
          console.log("User Products fetched successfully");
          this.applyFilters(); // Apply filters after fetching products
        },
      error:
        (error) => {
          console.error("Error fetching user products:", error);
        }
    });
  }

  navigateToProducts() {
    this.router.navigate(['/addtocart']);
  }

  applyFilters() {
    let productsToFilter = this.selectedCategory
      ? this.userProducts.filter(p => p.category?.categoryName === this.selectedCategory)
      : [...this.userProducts];

    if (typeof this.minPrice === 'number' && this.minPrice >= 0) {
      productsToFilter = productsToFilter.filter(p => p.price >= this.minPrice!);
    }

    if (typeof this.maxPrice === 'number' && this.maxPrice > 0) {
      productsToFilter = productsToFilter.filter(p => p.price <= this.maxPrice!);
    }

    this.filteredProducts = productsToFilter;
  }

  navigateToCategory(category: string) {
    const headers = new HttpHeaders().set('Authorization', 'Bearer YOUR_ACCESS_TOKEN'); // Add your authorization token here
    this.http.get<Product[]>("https://localhost:7183/api/Product/ProductsByCategory/" + category, { headers }).subscribe({
      next:
        (result) => {
          this.userProducts = result;
          console.log(this.userProducts);
          console.log("User Products fetched successfully");
          this.applyFilters(); // Apply filters after fetching products
        },
      error:
        (error) => {
          console.error("Error fetching user products:", error);
        }
    });
  }

  addToCart(productId: number) {
    const username = localStorage.getItem('username'); // Retrieve the username from local storage
    if (username) {
      this.cartService.addItemToCart(username, productId, 1).subscribe(
        (response) => {
          console.log('Item added to cart:', response);
          alert('Item added to cart successfully');
        },
        (error) => {
          console.error('Error adding item to cart:', error);
          alert('Error adding item to cart. Please try again later.');
        }
      );
    } else {
      console.error('Username not found in local storage.');
      alert('Please log in again.');
      this.router.navigate(['/login']);
    }
  }

  logout(): void {
    localStorage.removeItem('jwtToken');
    this.router.navigate(['/login']);
  }

}
