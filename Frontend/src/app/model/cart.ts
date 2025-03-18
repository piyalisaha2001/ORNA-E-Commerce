export interface CartItem {
    productId: number;
    productName: string;
    productImageUrl: string;
    productPrice: number;
    quantity: number;
  }
  
  export interface Cart {
    cartId: number;
    userId: number;
    items: CartItem[];
  }

  export interface Checkout {
    username: string;
    paymentMethod: string;
    amount: number;
    cardNo: string;
    expiryDate: string;
    upiId: string;
    }
 