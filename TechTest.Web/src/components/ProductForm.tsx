import { Container, NumberInput, Select, ComboboxItem, TextInput, Group, Button } from '@mantine/core';
import { useForm } from '@mantine/form';
import React, { useEffect, useState } from 'react';
import { Category } from '../objects/category';
import { Product } from '../objects/product';
import { ProductFormProps } from '../objects/product-form-props';


function ProductForm({onFormSubmit}: ProductFormProps) {

    const [categories, setCategories] = useState<Category[]>([]);
    const [selectItems, setSelectItems] = useState<ComboboxItem[]>([]);
    const [submittedValues, setSubmittedValues] = useState<typeof form.values | null>(null);
    const [selectedCategory, setSelectedCategory] = useState<ComboboxItem | null>(null);

    const form = useForm<Product>({
        mode: 'uncontrolled',
    });

    const getCategories = async () => {
        await fetch('api/categories')
            .then(response => response.json())
            .then(data => setCategories(data))
            .catch(error => console.error('Error fetching categories:', error));
    }

    useEffect(() => {
        getCategories();
    }, []);

    useEffect(() => {
        setSelectItems(categories.map(c => (
            {value: c.categoryId.toString(), label: c.name}
        )));
    }, [categories]);

    useEffect(() => {
        if (submittedValues) {
            onFormSubmit({
                ...submittedValues,
                categoryId: parseInt(selectedCategory?.value ?? '0'),
            });
            form.reset();
        }
    }, [submittedValues]);

    return (
        <Container size='xs'>
            <form onSubmit={form.onSubmit(setSubmittedValues)}>
                <TextInput
                    label="Name"
                    key={form.key('name')}
                    {...form.getInputProps('name')}
                />
                <TextInput
                    label="SKU"
                    key={form.key('sku')}
                    {...form.getInputProps('sku')}
                />
                <Select
                    label="Category"
                    key={form.key('category')}
                    data={selectItems}
                    {...form.getInputProps('category')}
                    value={selectedCategory ? selectedCategory.value : '0'}
                    onChange={(value) => setSelectedCategory(selectItems.find(i => i.value === value) ?? null)}
                />
                <NumberInput
                    label="Price"
                    key={form.key('price')}
                    {...form.getInputProps('price')}
                />
                <NumberInput
                    label="Stock"
                    key={form.key('quantity')}
                    {...form.getInputProps('quantity')}
                />

                <Group mt="md">
                    <Button type="submit">Submit</Button>
                </Group>
            </form>
        </Container>
    );
}

export default ProductForm;
