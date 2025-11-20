import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import api from "../api/axiosInstance";

// UI imports
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import {
    Card,
    CardHeader,
    CardContent,
    CardFooter
} from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { Alert, AlertTitle, AlertDescription } from "@/components/ui/alert";
import { Skeleton } from "@/components/ui/skeleton";

import { AppLayout } from "@/layouts/AppLayout";

// Interfaces
interface Product {
    id: number;
    name: string;
    unitPrice: number;
    stock: number;
    imageUrl: string;
    category: "Material" | "Otro";
}

export function CataloguePage() {
    const navigate = useNavigate();

    const [products, setProducts] = useState<Product[]>([]);
    const [searchTerm, setSearchTerm] = useState("");
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState("");

    const [cartItems, setCartItems] = useState<
        { productId: number; quantity: number }[]
    >([]);

    // ------------------- Logout --------------------
    const handleLogout = () => {
        localStorage.removeItem("jwt_token");
        navigate("/login", { replace: true });
    };

    // ------------------- Cart logic --------------------
    const handleAddToCart = (productId: number, change: number) => {
        setCartItems((prev) => {
            const existing = prev.find((x) => x.productId === productId);
            const product = products.find((p) => p.id === productId);
            if (!product) return prev;

            const max = product.stock;

            if (existing) {
                const newQty = existing.quantity + change;

                if (newQty <= 0)
                    return prev.filter((x) => x.productId !== productId);

                if (newQty > max)
                    return prev.map((x) =>
                        x.productId === productId ? { ...x, quantity: max } : x
                    );

                return prev.map((x) =>
                    x.productId === productId
                        ? { ...x, quantity: newQty }
                        : x
                );
            }

            if (change > 0) {
                return [...prev, { productId, quantity: 1 }];
            }

            return prev;
        });
    };

    const getCartQuantity = (id: number) =>
        cartItems.find((x) => x.productId === id)?.quantity || 0;

    // ------------------- Fetch products --------------------
    const fetchProducts = async () => {
        setIsLoading(true);
        setError("");
        try {
            const token = localStorage.getItem("jwt_token");
            if (!token) return;

            const res = await api.get("/products", {
                headers: { Authorization: `Bearer ${token}` },
            });

            setProducts(
                res.data.map((item: any) => ({
                    id: item.id,
                    name: item.name,
                    unitPrice: item.unitPrice,
                    stock: item.stock,
                    imageUrl:
                        item.imageUrl ||
                        `https://placehold.co/600x400/e5e7eb/374151?text=${item.name}`,
                    category: item.name.toLowerCase().includes("arepa")
                        ? "Otro"
                        : "Material",
                }))
            );
        } catch (err: any) {
            setError("No se pudieron cargar los productos.");
        } finally {
            setIsLoading(false);
        }
    };

    useEffect(() => {
        fetchProducts();
    }, []);

    const filteredProducts = products.filter((p) =>
        p.name.toLowerCase().includes(searchTerm.toLowerCase())
    );

    // ------------------- UI --------------------
    return (
        <AppLayout onLogout={handleLogout}>
            {/* Search section */}
            <div className="mb-10">
                <Input
                    placeholder="Buscar productos..."
                    value={searchTerm}
                    onChange={(e) => setSearchTerm(e.target.value)}
                    disabled={isLoading}
                    className="text-lg"
                />
            </div>

            {/* Error message */}
            {error && (
                <Alert variant="destructive" className="mb-6">
                    <AlertTitle>Error</AlertTitle>
                    <AlertDescription>
                        {error}
                        <Button className="mt-4" onClick={fetchProducts}>
                            Reintentar
                        </Button>
                    </AlertDescription>
                </Alert>
            )}

            {/* Loading skeletons */}
            {isLoading && (
                <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-6">
                    {Array.from({ length: 8 }).map((_, i) => (
                        <Skeleton key={i} className="h-80 rounded-xl" />
                    ))}
                </div>
            )}

            {/* Products */}
            {!isLoading && (
                <>
                    {filteredProducts.length > 0 ? (
                        <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-8">
                            {filteredProducts.map((product) => {
                                const qty = getCartQuantity(product.id);

                                return (
                                    <Card
                                        key={product.id}
                                        className="overflow-hidden border hover:shadow-xl transition rounded-2xl"
                                    >
                                        <CardHeader className="p-0">
                                            <img
                                                src={product.imageUrl}
                                                className="w-full h-52 object-cover"
                                                alt={product.name}
                                            />
                                        </CardHeader>

                                        <CardContent className="p-5 space-y-3">
                                            <Badge variant="secondary" className="text-xs">
                                                {product.category}
                                            </Badge>

                                            <h3 className="font-bold text-xl">{product.name}</h3>

                                            <p className="text-2xl font-bold text-indigo-600">
                                                ${product.unitPrice.toLocaleString("es-CO")}
                                            </p>

                                            <p className="text-sm text-gray-500">
                                                Stock disponible: {product.stock}
                                            </p>
                                        </CardContent>

                                        <CardFooter className="p-5 flex justify-between items-center">
                                            <Button
                                                variant="outline"
                                                size="icon"
                                                onClick={() => handleAddToCart(product.id, -1)}
                                                disabled={qty === 0}
                                            >
                                                -
                                            </Button>

                                            <span className="text-xl font-bold">{qty}</span>

                                            <Button
                                                size="icon"
                                                onClick={() => handleAddToCart(product.id, 1)}
                                                disabled={qty >= product.stock}
                                            >
                                                +
                                            </Button>
                                        </CardFooter>
                                    </Card>
                                );
                            })}
                        </div>
                    ) : (
                        <p className="text-center text-gray-500 text-xl py-20">
                            No se encontraron productos.
                        </p>
                    )}
                </>
            )}
        </AppLayout>
    );
}
