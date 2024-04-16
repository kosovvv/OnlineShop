import { Product } from "./products";

export interface Type {
    id: number;
    name: string;
    pictureUrl?: string;
    products?: Product[];
}