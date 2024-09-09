// import { type MRT_ColumnDef, MRT_Table, useMantineReactTable } from 'mantine-react-table';
import { DataTable } from 'mantine-datatable';
import React, { useMemo } from 'react';
import { Product } from '../objects/product';
import { ProductsGridProps } from '../objects/products-grid-props';

function ProductsGrid({products}: ProductsGridProps) {
    return (
        <div>
            <h1>Products</h1>
            <DataTable
                withTableBorder
                withColumnBorders
                striped
                highlightOnHover
                columns={[
                    {
                        accessor: 'productId',
                        title: '#',
                        textAlign: 'right',
                    }, {
                        accessor: 'name',
                        title: 'Name',
                    }, {
                        accessor: 'price',
                        title: 'Price',
                    }, {
                        accessor: 'sku',
                        title: 'SKU',
                    }, {
                        accessor: 'category.name',
                        title: 'Category',
                    }, {
                        accessor: 'quantity',
                        title: 'Stock',
                    },
                ]}
                records={products}
            />
        </div>
    );
}

    export default ProductsGrid;

