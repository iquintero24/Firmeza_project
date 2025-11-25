import React from 'react';
import { Navigate, Outlet } from 'react-router-dom';

/**
 * Componente que verifica si el usuario tiene un token JWT válido.
 * Si no lo tiene, redirige a la página de inicio de sesión.
 */
export function ProtectedRoute() {
    // 1. Obtener el token del almacenamiento local
    const token = localStorage.getItem('jwt_token');

    // 2. Comprobar si el token existe
    if (!token) {
        // Si no hay token, redirigir al usuario a /login
        // 'replace' evita que la página protegida quede en el historial
        return <Navigate to="/login" replace />;
    }

    // 3. Si el token existe, renderizar las rutas hijas (el contenido protegido, ej. CataloguePage)
    return <Outlet />;
}