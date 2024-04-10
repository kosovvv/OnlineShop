import { Product } from "./products";

export interface Review {
    id: number
    author: string
    score: number
    reviewedProduct: Product;
    description: string
    createdOn: Date;
    isVerified: boolean;
} 