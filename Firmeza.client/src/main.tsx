import React from 'react';
import ReactDOM from 'react-dom/client';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import App from './App.tsx';
import './index.css';

import { LoginPage } from './pages/LoginPage.tsx';
import { CataloguePage } from './pages/CataloguePage.tsx';
import { ProtectedRoute } from './components/ProtectedRoute.tsx';
import { RegisterPage } from './pages/RegisterPage.tsx';

// Provider carrito
import { CartProvider } from "./context/CartContext.tsx";

// Páginas
import { CartPage } from "./pages/CartPage.tsx";
import SalesHistoryPage from "./pages/SalesHistoryPage.tsx"; // 👈 IMPORTACIÓN CORRECTA

ReactDOM.createRoot(document.getElementById('root')!).render(
    <React.StrictMode>
        <CartProvider>
            <BrowserRouter>
                <Routes>
                    <Route path="/" element={<App />}>

                        {/* Públicas */}
                        <Route index element={<LoginPage />} />
                        <Route path="/login" element={<LoginPage />} />
                        <Route path="/register" element={<RegisterPage />} />

                        {/* Protegidas */}
                        <Route element={<ProtectedRoute />}>
                            <Route path="/catalogo" element={<CataloguePage />} />
                            <Route path="/carrito" element={<CartPage />} />

                            {/* 👇 NUEVA RUTA DEL HISTORIAL */}
                            <Route path="/cuenta/pedidos" element={<SalesHistoryPage />} />
                        </Route>

                        {/* 404 */}
                        <Route
                            path="*"
                            element={
                                <div className="flex justify-center items-center min-h-screen bg-gray-100">
                                    <h1 className="text-4xl font-extrabold text-red-600">
                                        404 - Página no encontrada
                                    </h1>
                                </div>
                            }
                        />
                    </Route>
                </Routes>
            </BrowserRouter>
        </CartProvider>
    </React.StrictMode>
);
