export interface IProduct {
    productId: number
    productName: string
    description: string
    price: number
    stock: number
    categoryId: number
    category: Category
    productImage: string
    feedback: any
    CategoryName: string
  }
  
  export interface Category {
    categoriesId: number,
    categoryName: string
  }

  
      
  
  
  