import React from 'react';

import './App.css';
import ProductCharts from './components/ProductCharts';
import Products from './components/Products';
import { Route, Routes } from 'react-router-dom';


function App() {
    return (
        <div className="App">
            <main>
                <Routes>
                    <Route path="/" element={<Products />}/>
                    <Route path="/charts" element={<ProductCharts />}/>
                </Routes>
            </main>
        </div>
    );
}

export default App;
