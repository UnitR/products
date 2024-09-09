import { Divider } from '@mantine/core';
import React, { useEffect, useState } from 'react';

import { Product } from '../objects/product';
import ProductForm from './ProductForm';
import ProductsGrid from './ProductsGrid';

function Products() {
    const [products, setProducts] = useState<Product[]>([]);

    useEffect(() => {
        updateProducts()
    }, []);

    const updateProducts = () => {
        fetch('api/products')
            .then(response => response.json())
            .then(data => setProducts(data))
            .catch(error => console.error('Error fetching products:', error));
    }

    const onFormSubmit = (newProduct: Product) => {
        const request = fetch('api/products/new', {
            method: 'POST',
            body: JSON.stringify(newProduct),
            headers: {
                'Content-Type': 'application/json',
            },
        });
        request
            .then(response => {
                if (response.ok) {
                    updateProducts();
                }
            })
            .catch(error => console.error('Error fetching products:', error));
    }

    return (
        <>
            <ProductsGrid products={products}/>
            <Divider my="xs" label="Add a new product record" labelPosition="left"/>
            <ProductForm onFormSubmit={onFormSubmit}/>
        </>
    );
}

export default Products;
