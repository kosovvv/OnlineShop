import { Product } from "./products";

export interface Brand {
    id: number;
    name: string;
    pictureUrl?:string;
    products?: Product[];
}