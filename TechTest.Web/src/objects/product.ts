import { Category } from './category';

export interface Product {
    productId: number;
    name: string;
    price: number;
    sku: string;
    categoryId: number;
    category: Category;
    quantity: number;
}
