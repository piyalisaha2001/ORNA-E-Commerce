import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { ProductService } from '../../service/product.service';

@Component({
  selector: 'app-add-product',
  templateUrl: './add-product.component.html',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, HttpClientModule, RouterLink],
  styleUrls: ['./add-product.component.css']
})
export class AddProductComponent implements OnInit {
  addProductForm: FormGroup;
  id: number = 0;

  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router, private productService: ProductService) {
    this.addProductForm = this.fb.group({
      productName: ['', [Validators.required]],
      description: ['', [Validators.required]],
      price: ['', [Validators.required, Validators.min(0)]],
      stock: ['', [Validators.required, Validators.min(0)]],
      categoryId: ['', [Validators.required, Validators.min(0)]],
      productImage: ['', [Validators.required]],
      categoryName: ['', [Validators.required]]
    });
  }

  ngOnInit(): void {
    console.log('AddProductComponent initialized.');
  }

  onSubmit() {
    console.log('Form Submitted');
    if (this.addProductForm.valid) {
      console.log('Form is valid');
      const productData = {
        productName: this.addProductForm.value.productName,
        description: this.addProductForm.value.description,
        price: this.addProductForm.value.price,
        stock: this.addProductForm.value.stock,
        categoryId: this.addProductForm.value.categoryId,
        productImage: this.addProductForm.value.productImage,
        categoryName: this.addProductForm.value.categoryName
      };

      const token = localStorage.getItem('token'); // Retrieve the token from local storage
      const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

      this.http.post('https://localhost:7183/api/Product', productData, { headers }).subscribe(
        (response: any) => {
          console.log(response);
          if (response && response.result) {
            alert('Product Added Successfully');
            this.router.navigate(['/products']);
          } else {
            alert('Product Added Successfully');
          }
        },
        (error: HttpErrorResponse) => {
          console.error('Product addition failed', error);
          if (error.status === 401) {
            alert('Unauthorized: Please log in again.');
            this.router.navigate(['/login']);
          } else {
            alert('Product Addition Failed');
          }
        }
      );
    } else {
      console.log('Form is invalid');
    }
  }

  onReset() {
    this.addProductForm.reset();
  }

  //delete

  deleteProduct(id: number) {
    this.productService.deleteProduct(id).subscribe((data:any)=>{
      console.log()
      alert('Product Deleted Successfully');
    })
  }

  //edit

  onEdit(id:any,data: any){
    this.editProduct(id,data);
  }

  editProduct(id: number,data:any) {
    this.productService.editProduct(id,data).subscribe((data:any)=>{
      console.log()
      alert('Product Edited Successfully');
    })
  }
}