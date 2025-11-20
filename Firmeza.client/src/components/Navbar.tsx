/**
 * @fileoverview Componente de Barra de Navegación (Navbar)
 * Recrea la estructura de Shadcn UI (NavigationMenu, Button)
 * en un único archivo React con estilos Tailwind CSS neutrales (gris, blanco, negro).
 *
 * NOTA: Los componentes de Shadcn como 'NavigationMenu' se simulan
 * usando divs y clases de Tailwind, ya que no se pueden importar
 * dependencias externas en este entorno de archivo único.
 */

import React, { useState } from 'react';
import { ShoppingCart, LogOut, User, Package, ListChecks, ChevronDown } from 'lucide-react';

/**
 * Componente funcional ListItem (simula Link de NavigationMenu)
 * @param {string} title - Título principal.
 * @param {React.ReactNode} children - Descripción.
 * @param {string} href - URL de destino.
 * @returns {JSX.Element} Un elemento de lista con formato de enlace.
 */
const ListItem = ({ title, children, href, ...props }) => {
    return (
        <li {...props}>
            <a
                href={href}
                className="block select-none space-y-1 rounded-md p-3 leading-none no-underline transition-colors hover:bg-gray-100 outline-none"
            >
                <div className="text-sm font-medium leading-none text-gray-900">
                    {title}
                </div>
                <p className="line-clamp-2 text-sm leading-snug text-gray-500">
                    {children}
                </p>
            </a>
        </li>
    );
};

// --- SIMULACIÓN DE COMPONENTES SHADCN ---

/**
 * Simulación de NavigationMenuLink y Button (estilos base)
 */
const NavigationMenuLink = ({ children, className, ...props }) => (
    <a {...props} className={`inline-flex h-10 w-max items-center justify-center rounded-md px-4 py-2 text-sm font-medium transition-colors focus:bg-gray-100 hover:bg-gray-100 text-gray-700 ${className}`}>
        {children}
    </a>
);

const Button = ({ children, variant = "default", className, onClick, ...props }) => {
    let baseStyles = "inline-flex items-center justify-center whitespace-nowrap rounded-md text-sm font-medium transition-colors h-10 px-4 py-2 shadow-sm";
    let variantStyles = "";

    switch (variant) {
        case 'outline':
            variantStyles = "border border-gray-300 bg-white hover:bg-gray-100 text-gray-900";
            break;
        case 'destructive':
            // Se mantiene el rojo por convención de seguridad para 'Salir' (Logout)
            variantStyles = "bg-red-600 text-white hover:bg-red-700";
            break;
        case 'default':
        default:
            // Estilo neutral oscuro
            variantStyles = "bg-gray-900 text-white hover:bg-gray-800";
            break;
    }

    return (
        <button
            className={`${baseStyles} ${variantStyles} ${className}`}
            onClick={onClick}
            {...props}
        >
            {children}
        </button>
    );
};

/**
 * Componente NavigationTrigger (con estado para simular la apertura)
 */
const NavigationMenuTrigger = ({ children, content }) => {
    // Nota: Aunque se eliminó la directiva "use client", el uso de useState
    // implica que este es un componente del lado del cliente.
    const [isOpen, setIsOpen] = useState(false);

    const toggleMenu = () => setIsOpen(!isOpen);

    return (
        <div
            className="relative"
            onMouseEnter={() => setIsOpen(true)}
            onMouseLeave={() => setIsOpen(false)}
        >
            <button
                onClick={toggleMenu}
                className="inline-flex items-center justify-center rounded-md px-4 py-2 text-sm font-medium transition-colors hover:bg-gray-100 focus:bg-gray-100 text-gray-900 focus:outline-none"
            >
                {children}
                <ChevronDown className={`ml-1 h-4 w-4 transition duration-200 ${isOpen ? 'rotate-180' : ''}`} />
            </button>
            {isOpen && (
                <div className="absolute left-1/2 transform -translate-x-1/2 mt-2 w-max rounded-md border border-gray-200 bg-white shadow-lg p-4 z-50 animate-in fade-in-0 slide-in-from-top-1">
                    {content}
                </div>
            )}
        </div>
    );
};


// --- COMPONENTE NAVBAR PRINCIPAL ---

interface NavbarProps {
    onLogout?: () => void;
}

export function Navbar({ onLogout }: NavbarProps) {
    const productosMenu = [
        {
            title: "Materiales Estructurales",
            href: "/productos/materiales",
            description: "Catálogo completo de aceros, concretos y maderas para proyectos grandes."
        },
        {
            title: "Herramientas y Equipo",
            href: "/productos/herramientas",
            description: "Desde maquinaria pesada hasta herramientas manuales de precisión."
        },
        {
            title: "Ferretería General",
            href: "/productos/ferreteria",
            description: "Clavos, tornillos, tuercas y elementos de fijación esenciales."
        },
    ];

    const cuentaMenu = [
        {
            title: "Mi Perfil",
            href: "/cuenta/perfil",
            description: "Gestiona tu información personal y preferencias de la cuenta."
        },
        {
            title: "Historial de Pedidos",
            href: "/cuenta/pedidos",
            description: "Revisa y rastrea tus compras anteriores y estado de las entregas."
        },
        {
            title: "Soporte y Ayuda",
            href: "/soporte",
            description: "Preguntas frecuentes, contacto y centro de ayuda al cliente."
        },
    ];

    return (
        // Ocupa todo el ancho, fondo blanco y borde inferior (estilo neutral)
        <header className="w-full border-b border-gray-200 bg-white sticky top-0 z-40 shadow-sm">

            {/* Contenedor centralizado para el contenido de la barra */}
            <div className="max-w-7xl mx-auto px-6 py-4 flex justify-between items-center h-16">

                {/* LOGO - Todo neutral: texto gris oscuro */}
                <a href="/" className="text-2xl font-bold tracking-tight text-gray-900 hover:text-gray-700 transition-colors">
                    Firmeza Catálogo
                </a>

                {/* MENU DE NAVEGACIÓN (Simulación de NavigationMenu) */}
                <nav className="hidden lg:flex items-center gap-1">

                    {/* MENU PRODUCTOS */}
                    <NavigationMenuTrigger content={
                        <ul className="grid w-[400px] gap-1 p-3">
                            {productosMenu.map((item) => (
                                <ListItem
                                    key={item.title}
                                    title={item.title}
                                    href={item.href}
                                >
                                    {item.description}
                                </ListItem>
                            ))}
                        </ul>
                    }>
                        <Package className="h-4 w-4 mr-1" /> Productos
                    </NavigationMenuTrigger>

                    {/* MENU CUENTA */}
                    <NavigationMenuTrigger content={
                        <ul className="grid w-[400px] gap-1 p-3">
                            {cuentaMenu.map((item) => (
                                <ListItem
                                    key={item.title}
                                    title={item.title}
                                    href={item.href}
                                >
                                    {item.description}
                                </ListItem>
                            ))}
                        </ul>
                    }>
                        <User className="h-4 w-4 mr-1" /> Cuenta
                    </NavigationMenuTrigger>

                    {/* ENLACE SIMPLE (simula NavigationMenuLink) */}
                    <NavigationMenuLink href="/catalogo">
                        <ListChecks className="h-4 w-4 mr-1" /> Ver Catálogo
                    </NavigationMenuLink>

                </nav>

                {/* BOTONES DERECHA */}
                <div className="flex items-center gap-3">
                    <Button variant="outline">
                        <ShoppingCart className="h-4 w-4 mr-2" /> Carrito
                    </Button>
                    {onLogout && (
                        <Button variant="destructive" onClick={onLogout}>
                            <LogOut className="h-4 w-4 mr-2" /> Salir
                        </Button>
                    )}
                </div>
            </div>
        </header>
    );
}

// El componente App es necesario para el renderizado del archivo único.
export default function App() {
    // Función de ejemplo para el logout
    const handleLogout = () => {
        console.log("Cerrando sesión...");
        // Usamos un modal o componente UI en lugar de alert()
        // Para esta simulación, solo logeamos.
        // En una app real, pondrías un modal.
        alert("Sesión cerrada (Simulación)");
    };

    return (
        <div className="min-h-screen bg-gray-50 font-sans">
            <Navbar onLogout={handleLogout} />
            <main className="max-w-7xl mx-auto p-8">
                <h2 className="text-3xl font-semibold mb-6 text-gray-900">Contenido de la Aplicación</h2>
                <p className="text-gray-600">
                    Esta es la simulación de la página de inicio. La barra de navegación superior está estilizada en tonos neutrales (gris, blanco, negro) y ocupa todo el ancho de la pantalla.
                </p>
                <div className="mt-8 grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                    {Array.from({ length: 6 }).map((_, i) => (
                        <div key={i} className="bg-white p-6 rounded-lg shadow-md border border-gray-100">
                            <h3 className="text-xl font-bold mb-2 text-gray-800">Producto de Ejemplo {i + 1}</h3>
                            <p className="text-gray-500">Detalles del producto o material.</p>
                            <Button className="mt-4 w-full">Comprar</Button>
                        </div>
                    ))}
                </div>
            </main>
        </div>
    );
}