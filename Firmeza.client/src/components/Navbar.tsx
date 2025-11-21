/**
 * @fileoverview Componente de Barra de Navegación (Navbar)
 * Recrea la estructura de Shadcn UI (NavigationMenu, Button)
 * en un único archivo React con estilos Tailwind CSS neutrales (gris, blanco, negro).
 */

import React, { useState } from 'react';
import { ShoppingCart, LogOut, User, Package, ListChecks, ChevronDown } from 'lucide-react';
import { useNavigate } from "react-router-dom";   // ⭐ IMPORTANTE

// ---- LIST ITEM ----
const ListItem = ({ title, children, href, ...props }) => {
    return (
        <li {...props}>
            <a
                href={href}
                className="block select-none space-y-1 rounded-md p-3 leading-none no-underline transition-colors hover:bg-gray-100 outline-none"
            >
                <div className="text-sm font-medium leading-none text-black">
                    {title}
                </div>
                <p className="line-clamp-2 text-sm leading-snug text-gray-500">
                    {children}
                </p>
            </a>
        </li>
    );
};

// ---- BUTTON ----
const Button = ({ children, variant = "default", className, onClick, ...props }) => {
    let baseStyles = "inline-flex items-center justify-center whitespace-nowrap rounded-md text-sm font-medium transition-colors h-10 px-4 py-2 shadow-sm";
    let variantStyles = "";

    switch (variant) {
        case 'outline':
            variantStyles = "border border-gray-300 bg-white hover:bg-gray-100 text-gray-900";
            break;
        case 'destructive':
            variantStyles = "bg-red-600 text-white hover:bg-red-700";
            break;
        default:
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

// ---- TRIGGER ----
const NavigationMenuTrigger = ({ children, content }) => {
    const [isOpen, setIsOpen] = useState(false);
    return (
        <div
            className="relative"
            onMouseEnter={() => setIsOpen(true)}
            onMouseLeave={() => setIsOpen(false)}
        >
            <button
                onClick={() => setIsOpen(!isOpen)}
                className="inline-flex items-center justify-center rounded-md px-4 py-2 text-sm font-medium transition-colors bg-white text-black hover:bg-gray-100 focus:bg-gray-100"
            >
                {children}
                <ChevronDown className={`ml-1 h-4 w-4 transition duration-200 ${isOpen ? 'rotate-180' : ''}`} />
            </button>
            {isOpen && (
                <div className="absolute left-1/2 transform -translate-x-1/2 mt-2 w-max rounded-md border border-gray-200 bg-white shadow-lg p-4 z-50">
                    {content}
                </div>
            )}
        </div>
    );
};

// ---- NAVBAR ----
interface NavbarProps {
    onLogout?: () => void;
}

export function Navbar({ onLogout }: NavbarProps) {

    const navigate = useNavigate();   // ⭐ NECESARIO PARA NAVEGAR

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
            description: "Gestiona tu información personal."
        },
        {
            title: "Historial de Pedidos",
            href: "/cuenta/pedidos",
            description: "Consulta tus pedidos previos."
        },
    ];

    return (
        <header className="w-full border-b border-gray-200 bg-white sticky top-0 z-40 shadow-sm">
            <div className="max-w-7xl mx-auto px-6 py-4 flex justify-between items-center h-16">

                <a href="/" className="text-2xl font-bold tracking-tight text-gray-900 hover:text-gray-700">
                    Firmeza Catálogo
                </a>

                <nav className="hidden lg:flex items-center gap-1">
                    <NavigationMenuTrigger content={
                        <ul className="grid w-[400px] gap-1 p-3">
                            {productosMenu.map(item => (
                                <ListItem key={item.title} title={item.title} href={item.href}>
                                    {item.description}
                                </ListItem>
                            ))}
                        </ul>
                    }>
                        <Package className="h-4 w-4 mr-1" /> Productos
                    </NavigationMenuTrigger>

                    <NavigationMenuTrigger content={
                        <ul className="grid w-[400px] gap-1 p-3">
                            {cuentaMenu.map(item => (
                                <ListItem key={item.title} title={item.title} href={item.href}>
                                    {item.description}
                                </ListItem>
                            ))}
                        </ul>
                    }>
                        <User className="h-4 w-4 mr-1" /> Cuenta
                    </NavigationMenuTrigger>

                    <a
                        href="/catalogo"
                        className="inline-flex items-center text-sm text-black hover:text-gray-700 px-4"
                    >
                        <ListChecks className="h-4 w-4 mr-1" /> Ver Catálogo
                    </a>
                </nav>

                <div className="flex items-center gap-3">
                    <Button variant="default" onClick={() => navigate("/carrito")}>
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

// ---- APP ----
export default function App() {

    const handleLogout = () => {
        alert("Sesión cerrada (Simulación)");
    };

    return (
        <div className="min-h-screen bg-gray-50 font-sans">
            <Navbar onLogout={handleLogout} />
        </div>
    );
}
