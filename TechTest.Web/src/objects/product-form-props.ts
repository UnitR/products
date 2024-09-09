import { Product } from './product';

export interface ProductFormProps {
    onFormSubmit: (newProduct: Product) => void;
}
