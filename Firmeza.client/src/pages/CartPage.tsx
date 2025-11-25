import { useNavigate } from "react-router-dom";
import { AppLayout } from "@/layouts/AppLayout";

import { Button } from "@/components/ui/button";
import { Card, CardContent, CardFooter } from "@/components/ui/card";
import { Separator } from "@/components/ui/separator";

import { useCart } from "../context/CartContext";
import api from "../api/axiosInstance";

export function CartPage() {
    const navigate = useNavigate();
    const { cartItems, addItem, removeItem, deleteItem, total, clearCart } = useCart();

    // -------------------- FINALIZAR COMPRA --------------------
    const handleCheckout = async () => {
        try {
            const token = localStorage.getItem("jwt_token");
            if (!token) return alert("No hay sesión activa.");

            // Extraer datos del JWT
            const payload = JSON.parse(atob(token.split(".")[1]));
            const customerId = payload.customerId; // <<--- STRING CORRECTO

            if (!customerId) {
                alert("El token no contiene un usuario válido.");
                return;
            }

            const iva = +(total * 0.19).toFixed(2);
            const totalFinal = +(total + iva).toFixed(2);

            const sale = {
                customerId, // <<--- STRING, NO number
                saleDetails: cartItems.map((item) => ({
                    productId: item.productId,
                    quantity: item.quantity,
                    appliedUnitPrice: item.price,
                })),
                subtotal: total,
                iva,
                total: totalFinal,
            };

            const res = await api.post("/sales", sale, {
                headers: { Authorization: `Bearer ${token}` },
                responseType: "blob", // Para descargar PDF
            });

            // Descargar PDF
            const blob = new Blob([res.data], { type: "application/pdf" });
            const url = window.URL.createObjectURL(blob);

            const a = document.createElement("a");
            a.href = url;
            a.download = "factura.pdf";
            a.click();

            window.URL.revokeObjectURL(url);

            clearCart();
            navigate("/carrito");

        } catch (error) {
            console.error("Error al finalizar compra:", error);

            if (error?.response?.data) {
                const reader = new FileReader();
                reader.onload = () => alert(reader.result);
                reader.readAsText(error.response.data);
            } else {
                alert("No se pudo generar la venta.");
            }
        }
    };

    // -------------------- UI --------------------
    return (
        <AppLayout>
            <div className="max-w-3xl mx-auto mt-10">

                <h1 className="text-3xl font-bold mb-6">🛒 Mi Carrito</h1>

                {cartItems.length === 0 ? (
                    <p className="text-gray-500 text-lg text-center py-20">
                        Tu carrito está vacío.
                    </p>
                ) : (
                    <Card className="shadow-xl border rounded-2xl">
                        <CardContent className="p-6 space-y-6">

                            {cartItems.map((item) => (
                                <div key={item.productId}>
                                    <div className="flex justify-between items-center">

                                        {/* Info producto */}
                                        <div>
                                            <h3 className="font-bold text-lg">{item.name}</h3>
                                            <p className="text-gray-600">
                                                Precio:{" "}
                                                <span className="font-semibold">
                                                    ${item.price?.toLocaleString("es-CO")}
                                                </span>
                                            </p>
                                        </div>

                                        {/* Controles */}
                                        <div className="flex items-center gap-3">
                                            <Button
                                                variant="outline"
                                                size="icon"
                                                onClick={() => removeItem(item.productId)}
                                            >
                                                -
                                            </Button>

                                            <span className="text-xl font-bold">
                                                {item.quantity}
                                            </span>

                                            <Button
                                                size="icon"
                                                onClick={() =>
                                                    addItem(item.productId, 1, {
                                                        name: item.name,
                                                        price: item.price,
                                                    })
                                                }
                                            >
                                                +
                                            </Button>

                                            <Button
                                                variant="destructive"
                                                onClick={() => deleteItem(item.productId)}
                                            >
                                                Quitar
                                            </Button>
                                        </div>
                                    </div>

                                    <Separator className="my-4" />
                                </div>
                            ))}

                            {/* Totales */}
                            <div className="text-right space-y-2">
                                <p className="text-lg">
                                    Subtotal:{" "}
                                    <span className="font-semibold">
                                        ${total.toLocaleString("es-CO")}
                                    </span>
                                </p>
                                <p className="text-lg">
                                    IVA 19%:{" "}
                                    <span className="font-semibold">
                                        ${(total * 0.19).toLocaleString("es-CO")}
                                    </span>
                                </p>

                                <h2 className="text-2xl font-bold">
                                    Total:{" "}
                                    <span className="text-indigo-600">
                                        ${(total * 1.19).toLocaleString("es-CO")}
                                    </span>
                                </h2>
                            </div>
                        </CardContent>

                        <CardFooter className="flex justify-between p-6">
                            <Button variant="outline" onClick={() => navigate("/catalogo")}>
                                Seguir comprando
                            </Button>

                            <Button onClick={handleCheckout} className="bg-indigo-600 text-white">
                                Finalizar compra
                            </Button>
                        </CardFooter>
                    </Card>
                )}
            </div>
        </AppLayout>
    );
}
