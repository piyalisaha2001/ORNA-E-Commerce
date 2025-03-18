import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { SignUpComponent } from './components/sign-up/sign-up.component';
import { HomeComponent } from './components/home/home.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { AddtocartComponent } from './components/user/addtocart/addtocart.component';
import { CheckoutComponent } from './components/user/checkout/checkout.component';
import { AboutusComponent } from './components/aboutus/aboutus.component';
import { OrdersComponent } from './components/admin/orders/orders.component';
import { ProductsComponent } from './components/admin/products/products.component';
import { AdminpanelComponent } from './components/admin/adminpanel/adminpanel.component';
import { AddProductComponent } from './components/add-product/add-product.component';
import { ProductUserComponent } from './components/user/products/products.component';
import { UserdetailsComponent } from './components/admin/userdetails/userdetails.component';
import { PaymentdetailsComponent } from './components/admin/paymentdetails/paymentdetails.component';
import { PaymentstatusComponent } from './components/user/paymentstatus/paymentstatus.component';





export const routes: Routes = [
    { path: 'login', component: LoginComponent },
    { path: 'register', component: SignUpComponent },
    { path: '', redirectTo: '/login', pathMatch: 'full' }, // Redirect to home by default
    { path: 'home', component: HomeComponent },
    { path: 'navbar', component: NavbarComponent },
    { path: 'products', component: ProductUserComponent },
    { path: 'products/:category', component: ProductsComponent}, //show filtered products
    // { path: '**',redirectTo:'products'}, //Redirect to all products by default
    { path: 'addtocart', component: AddtocartComponent},
    { path: 'checkout', component: CheckoutComponent},
    { path: 'aboutus', component: AboutusComponent},
    { path: 'orders', component: OrdersComponent},
    { path: 'product', component: ProductsComponent},
    { path: 'adminpanel', component: AdminpanelComponent},
    {path: 'addproduct', component: AddProductComponent},
    { path: 'userdetails', component: UserdetailsComponent},
    { path: 'paymentdetails', component: PaymentdetailsComponent},
    { path: 'paymentstatus', component: PaymentstatusComponent},
    { path: '**', redirectTo: 'home' }, // Redirect to home by default
    
   
    
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule { }