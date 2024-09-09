import { BarChart, LineChart } from '@mantine/charts';
import { Divider } from '@mantine/core';
import { useEffect, useState } from 'react';

import { CategoryStock } from '../objects/category-stock';
import { DateProductAdded } from '../objects/date-product-added';

function ProductCharts() {
    const [categoryStocks, setCategoryStocks] = useState<CategoryStock[]>([]);
    const [dateProductAdded, setDateProductAdded] = useState<DateProductAdded[]>([]);

    useEffect(() => {
        fetch('api/products/charts/quantity')
            .then(response => response.json())
            .then(data => setCategoryStocks(data))
            .catch(error => console.error(error));
    }, []);

    useEffect(() => {
        fetch('api/products/charts/timeframe')
            .then(response => response.json())
            .then(data => setDateProductAdded(data))
            .catch(error => console.error(error));
    }, []);

    return (
        <div style={{padding: '20px'}}>
            <BarChart
                h={300}
                data={categoryStocks}
                dataKey="categoryName"
                tickLine="y"
                series={[
                    {name: 'totalStock', color: 'purple', label: 'Stock'},
                ]}
            />
            <Divider my='lg' variant='solid' />
            <LineChart
                h={300}
                data={dateProductAdded}
                dataKey="timeframe"
                withLegend
                series={[
                    {name: 'productCount', color: 'indigo.6', label: 'Total products added'},
                ]}
                type="gradient"
                gradientStops={[
                    {offset: 0, color: 'red.6'},
                    {offset: 20, color: 'orange.6'},
                    {offset: 40, color: 'yellow.5'},
                    {offset: 70, color: 'lime.5'},
                    {offset: 80, color: 'cyan.5'},
                    {offset: 100, color: 'blue.5'},
                ]}
                strokeWidth={5}
                curveType="natural"
            />
        </div>
    );
}

export default ProductCharts;
