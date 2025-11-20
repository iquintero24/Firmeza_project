import {
    NavigationMenu,
    NavigationMenuItem,
    NavigationMenuList,
    NavigationMenuTrigger,
    NavigationMenuContent,
    NavigationMenuLink,
} from "@/components/ui/navigation-menu";

import { Button } from "@/components/ui/button";

interface Props {
    onLogout?: () => void;
}

export function Navbar({ onLogout }: Props) {
    return (
        <header className="border-b bg-white sticky top-0 z-40">
            <div className="max-w-7xl mx-auto px-6 py-4 flex justify-between items-center">

                {/* LOGO */}
                <h1 className="text-3xl font-extrabold tracking-tight text-indigo-600">
                    Firmeza Catálogo
                </h1>

                {/* MENU DE NAVEGACIÓN */}
                <NavigationMenu>
                    <NavigationMenuList className="flex gap-2">

                        {/* MENU PRODUCTOS */}
                        <NavigationMenuItem>
                            <NavigationMenuTrigger>Productos</NavigationMenuTrigger>
                            <NavigationMenuContent className="p-4">
                                <div className="grid grid-cols-1 gap-3 w-64">
                                    <NavigationMenuLink className="block hover:bg-accent rounded-md p-2 cursor-pointer">
                                        Materiales
                                    </NavigationMenuLink>
                                    <NavigationMenuLink className="block hover:bg-accent rounded-md p-2 cursor-pointer">
                                        Otros productos
                                    </NavigationMenuLink>
                                </div>
                            </NavigationMenuContent>
                        </NavigationMenuItem>

                        {/* MENU CUENTA */}
                        <NavigationMenuItem>
                            <NavigationMenuTrigger>Cuenta</NavigationMenuTrigger>
                            <NavigationMenuContent className="p-4">
                                <div className="grid w-64 gap-3">
                                    <NavigationMenuLink className="block hover:bg-accent rounded-md p-2 cursor-pointer">
                                        Perfil
                                    </NavigationMenuLink>
                                    <NavigationMenuLink className="block hover:bg-accent rounded-md p-2 cursor-pointer">
                                        Historial de pedidos
                                    </NavigationMenuLink>
                                </div>
                            </NavigationMenuContent>
                        </NavigationMenuItem>

                    </NavigationMenuList>
                </NavigationMenu>

                {/* BOTONES DERECHA */}
                <div className="flex items-center gap-3">
                    <Button variant="outline">Carrito</Button>
                    {onLogout && (
                        <Button variant="destructive" onClick={onLogout}>
                            Salir
                        </Button>
                    )}
                </div>
            </div>
        </header>
    );
}
