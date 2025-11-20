import React from 'react';
import ReactDOM from 'react-dom/client';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import App from './App.tsx';
import './index.css';

// Importa las páginas y componentes necesarios
import { LoginPage } from './pages/LoginPage.tsx';
import { CataloguePage } from './pages/CataloguePage.tsx'; // Importación correcta
import { ProtectedRoute } from './components/ProtectedRoute.tsx'; // Importación correcta
import { RegisterPage } from './pages/RegisterPage.tsx';

ReactDOM.createRoot(document.getElementById('root')!).render(
    <React.StrictMode>
        <BrowserRouter>
            <Routes>
                {/* Ruta Padre: Usa el componente App como Layout */}
                <Route path="/" element={<App />}>

                    {/* Rutas Públicas (Accesibles sin Login) */}
                    <Route index element={<LoginPage />} />
                    <Route path="/login" element={<LoginPage />} />
                    <Route path="/register" element={<RegisterPage />} />
                    

                    {/* GRUPO DE RUTAS PROTEGIDAS */}
                    <Route element={<ProtectedRoute />}>
                        {/* Rutas que requieren Login (JWT Token) */}
                        <Route path="/catalogo" element={<CataloguePage />} />
                        {/* Aquí irían otras rutas como /perfil, /carrito, etc. */}
                    </Route>

                    {/* Rutas de Error 404 */}
                    <Route path="*" element={
                        <div className="flex justify-center items-center min-h-screen bg-gray-100">
                            <h1 className="text-4xl font-extrabold text-red-600">
                                404 - Página no encontrada
                            </h1>
                        </div>
                    } />
                </Route>
            </Routes>
        </BrowserRouter>
    </React.StrictMode>,
)