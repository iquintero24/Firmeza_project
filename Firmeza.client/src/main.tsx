import React from 'react';
import ReactDOM from 'react-dom/client';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import App from './App.tsx';
import './index.css';

import { LoginPage } from './pages/LoginPage.tsx';
import { CataloguePage } from './pages/CataloguePage.tsx';
import { ProtectedRoute } from './components/ProtectedRoute.tsx';
import { RegisterPage } from './pages/RegisterPage.tsx';

// ⭐ IMPORTANTE: importar el provider global del carrito
import { CartProvider } from "./context/CartContext.tsx";


// ⭐ IMPORTA LA PÁGINA DEL CARRITO
import { CartPage } from "./pages/CartPage.tsx";

ReactDOM.createRoot(document.getElementById('root')!).render(
    <React.StrictMode>
        {/* 🔥 Ahora toda la app tiene acceso al carrito */}
        <CartProvider>
            <BrowserRouter>
                <Routes>
                    <Route path="/" element={<App />}>

                        {/* Rutas públicas */}
                        <Route index element={<LoginPage />} />
                        <Route path="/login" element={<LoginPage />} />
                        <Route path="/register" element={<RegisterPage />} />

                        {/* Rutas protegidas */}
                        <Route element={<ProtectedRoute />}>
                            <Route path="/catalogo" element={<CataloguePage />} />
                            <Route path="/carrito" element={<CartPage />} />
                            
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
