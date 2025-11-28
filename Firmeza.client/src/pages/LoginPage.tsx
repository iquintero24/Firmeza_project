import { LoginForm } from "../components/LoginForm"; // 👈 RUTA RELATIVA: Solución más estable.

export function LoginPage() {
    return (
        // Contenedor principal: 
        // 1. flex justify-center items-center: Centrado perfecto (Vertical y Horizontal)
        // 2. min-h-screen: Ocupa el 100% de la altura de la pantalla (ESENCIAL)
        // 3. bg-gray-100: Mantiene tu fondo gris claro
        <div className="flex justify-center items-center min-h-screen min-w-screen bg-gray-100 p-4">
            {/* Contenedor del Formulario (El 'Card' blanco) */}
            <div className="
                bg-white shadow-2xl rounded-xl p-10 
                w-full 
                max-w-md  /* 👈 CARD MUCHO MÁS ANCHO (896px) */
                transition-all duration-300
            ">
                <h2 className="text-3xl font-bold text-center mb-10 text-gray-800">
                    Iniciar Sesión
                </h2>

                {/* Contenedor para limitar el ancho interno del formulario (mejor UX) */}
                <div className="mx-auto max-w-sm">
                    <LoginForm />
                </div>

                <p className="text-center text-sm text-gray-500 pt-6 border-t mt-6 border-gray-100">
                    ¿No tienes cuenta? <a href="/register" className="text-indigo-600 hover:text-indigo-500 font-semibold transition duration-150">Regístrate aquí</a>
                </p>
            </div>
        </div>
    );
}