// import { type MRT_ColumnDef, MRT_Table, useMantineReactTable } from 'mantine-react-table';
import dayjs from 'dayjs';
import { DataTable } from 'mantine-datatable';
import React from 'react';
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
                    }, {
                        accessor: 'dateAdded',
                        title: 'Date Added',
                        render: ({dateAdded}) => dayjs(dateAdded).format('DD/MM/YYYY'),
                    }
                ]}
                records={products}
            />
        </div>
    );
}

    export default ProductsGrid;

