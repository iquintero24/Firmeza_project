import { useCart } from "../context/CartContext";
import { Button } from "@/components/ui/button";
import { Card, CardContent } from "@/components/ui/card";
import { Separator } from "@/components/ui/separator";
import { AppLayout } from "@/layouts/AppLayout";
import { useNavigate } from "react-router-dom";

export function CartPage() {
    const { cart, addItem, removeItem, clearCart } = useCart();
    const navigate = useNavigate();

    const products = Object.values(cart || {});

    const subtotal = products.reduce(
        (sum, item) => sum + item.price * item.quantity,
        0
    );

    const iva = subtotal * 0.19;
    const total = subtotal + iva;

    const handleLogout = () => {
        localStorage.removeItem("jwt_token");
        navigate("/login", { replace: true });
    };

    return (
        <AppLayout onLogout={handleLogout}>
            <div className="max-w-4xl mx-auto p-6">
                <h1 className="text-4xl font-bold mb-6">🛒 Tu carrito</h1>

                {/* Carrito vacío */}
                {products.length === 0 && (
                    <Card className="p-10 text-center">
                        <p className="text-gray-500 text-xl">
                            Tu carrito está vacío.
                        </p>

                        <Button
                            className="mt-5"
                            onClick={() => navigate("/catalogue")}
                        >
                            Volver al catálogo
                        </Button>
                    </Card>
                )}

                {/* Lista de productos */}
                {products.length > 0 && (
                    <div className="space-y-5">
                        {products.map((item) => (
                            <Card key={item.id} className="p-4 flex items-center gap-4">
                                <img
                                    src={`https://placehold.co/120x120/e5e7eb/374151?text=${item.name}`}
                                    alt={item.name}
                                    className="w-28 h-28 rounded-lg object-cover border"
                                />

                                <CardContent className="flex-1 p-0">
                                    <h2 className="font-bold text-xl">{item.name}</h2>

                                    <p className="text-gray-600">
                                        Precio:{" "}
                                        <span className="font-bold text-indigo-600">
                                            ${item.price.toLocaleString("es-CO")}
                                        </span>
                                    </p>

                                    {/* Controles de cantidad */}
                                    <div className="flex items-center gap-3 mt-3">
                                        <Button
                                            size="icon"
                                            variant="outline"
                                            onClick={() => addItem(item.id, -1)}
                                        >
                                            -
                                        </Button>

                                        <span className="text-xl font-semibold">
                                            {item.quantity}
                                        </span>

                                        <Button
                                            size="icon"
                                            onClick={() => addItem(item.id, 1)}
                                        >
                                            +
                                        </Button>

                                        <Button
                                            variant="destructive"
                                            className="ml-auto"
                                            onClick={() => removeItem(item.id)}
                                        >
                                            Eliminar
                                        </Button>
                                    </div>
                                </CardContent>
                            </Card>
                        ))}

                        {/* Resumen */}
                        <Card className="p-6 mt-6">
                            <h2 className="text-2xl font-bold mb-4">Resumen</h2>

                            <div className="flex justify-between text-lg mb-2">
                                <span>Subtotal</span>
                                <span>${subtotal.toLocaleString("es-CO")}</span>
                            </div>

                            <div className="flex justify-between text-lg mb-2">
                                <span>IVA (19%)</span>
                                <span>${iva.toLocaleString("es-CO")}</span>
                            </div>

                            <Separator className="my-4" />

                            <div className="flex justify-between text-2xl font-bold mb-6">
                                <span>Total</span>
                                <span>${total.toLocaleString("es-CO")}</span>
                            </div>

                            <div className="flex gap-4">
                                <Button
                                    className="flex-1 text-lg py-6"
                                    onClick={() => navigate("/checkout")}
                                >
                                    Finalizar compra
                                </Button>

                                <Button
                                    variant="outline"
                                    className="flex-1 text-lg py-6"
                                    onClick={clearCart}
                                >
                                    Vaciar carrito
                                </Button>
                            </div>
                        </Card>
                    </div>
                )}
            </div>
        </AppLayout>
    );
}
    