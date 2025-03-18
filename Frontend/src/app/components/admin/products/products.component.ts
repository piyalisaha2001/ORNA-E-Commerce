// import { Component, ElementRef, inject, OnInit, signal, ViewChild, viewChild } from '@angular/core';
// import { IProduct } from '../../../model/product';
// import { ProductService } from '../../../service/product.service';
// import { CommonModule } from '@angular/common';
// import { RouterLink } from '@angular/router';

// @Component({
//   selector: 'app-products',
//   imports: [CommonModule, RouterLink],
//   templateUrl: './products.component.html',
//   styleUrl: './products.component.css'
// })
// export class ProductsComponent implements OnInit {
  
//   @ViewChild('newModal') modal: ElementRef | undefined; //providing a datatype

//   productSrv = inject(ProductService);
//   productList: any[] = [];


  
//   ngOnInit(): void {
//     this.loadProducts();
//   }

//   loadProducts() {
//       this.productSrv.getAllProducts().subscribe((res:IProduct[]) => {
//         this.productList = res;

//         console.log(res);
//   })
// }

//   openModal() {
//     if(this.modal) {
//       this.modal.nativeElement.style.display = 'block';
//     }
//   }

//   closeModal() {
//     if(this.modal) {
//       this.modal.nativeElement.style.display = 'none';
//     }
//   }

//   //delete

//   deleteProduct(productId: number) {
//     this.productSrv.deleteProduct(productId).subscribe(res => {
//       this.loadProducts();
//       console.log(productId);
//     })
//   }
  

//   //edit
//   onEdit(product: IProduct) {
//     // Implement the logic to edit the product
//     console.log('Editing product:', product);
//     // You can navigate to the edit product page or open a modal with the product details
//   }
// }

import { Component, ElementRef, inject, OnInit, ViewChild } from '@angular/core';
import { IProduct } from '../../../model/product';
import { ProductService } from '../../../service/product.service';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-products',
  imports: [CommonModule, RouterLink, FormsModule],
  templateUrl: './products.component.html',
  styleUrl: './products.component.css'
})
export class ProductsComponent implements OnInit {
  
  @ViewChild('editModal') editModal: ElementRef | undefined;
  productSrv = inject(ProductService);
  productList: any[] = [];
  selectedProduct: IProduct | undefined;

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts() {
    this.productSrv.getAllProducts().subscribe((res: IProduct[]) => {
      this.productList = res;
      console.log(res);
    });
  }

  openEditModal(productId: number) {
    this.productSrv.getProductById(productId).subscribe((product: IProduct) => {
      this.selectedProduct = product;
      if (this.editModal) {
        this.editModal.nativeElement.style.display = 'block';
      }
    });
  }

  closeEditModal() {
    if (this.editModal) {
      this.editModal.nativeElement.style.display = 'none';
    }
  }

  saveProduct() {
    if (this.selectedProduct) {
      console.log('Updating product:', this.selectedProduct);
      this.productSrv.editProduct(this.selectedProduct.productId, this.selectedProduct).subscribe(
        res => {
          console.log('Product updated successfully:', res);
          this.loadProducts();
          this.closeEditModal();
        },
        error => {
          console.error('Error updating product:', error);
          // Handle the error (e.g., show a notification to the user)
        }
      );
    }
  }

  deleteProduct(productId: number) {
    this.productSrv.deleteProduct(productId).subscribe(res => {
      this.loadProducts();
      console.log(productId);
    });
  }

  onEdit(product: IProduct) {
    this.openEditModal(product.productId);
  }
}