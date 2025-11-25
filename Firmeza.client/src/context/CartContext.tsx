import React, { createContext, useContext, useState } from "react";

interface CartItem {
    productId: number;
    quantity: number;
    price?: number;   // Opcional pero útil
    name?: string;
}

interface CartContextType {
    cartItems: CartItem[];
    addItem: (productId: number, delta?: number, productInfo?: any) => void;
    removeItem: (productId: number) => void;
    deleteItem: (productId: number) => void;
    clearCart: () => void;
    getQuantity: (productId: number) => number;
    total: number;
}

const CartContext = createContext<CartContextType | null>(null);

export const CartProvider = ({ children }: { children: React.ReactNode }) => {
    const [cartItems, setCartItems] = useState<CartItem[]>([]);

    const addItem = (productId: number, delta = 1, productInfo?: any) => {
        setCartItems((prev) => {
            const existing = prev.find((x) => x.productId === productId);
            const currentQty = existing?.quantity ?? 0;

            const newQty = currentQty + delta;

            if (newQty <= 0)
                return prev.filter((x) => x.productId !== productId);

            // Si el item ya existe
            if (existing) {
                return prev.map((x) =>
                    x.productId === productId
                        ? { ...x, quantity: newQty }
                        : x
                );
            }

            // Si no existe, lo agrega con info opcional
            return [
                ...prev,
                {
                    productId,
                    quantity: newQty,
                    name: productInfo?.name,
                    price: productInfo?.price,
                },
            ];
        });
    };

    const deleteItem = (productId: number) => {
        setCartItems((prev) => prev.filter((x) => x.productId !== productId));
    };

    const removeItem = (productId: number) => {
        addItem(productId, -1);
    };

    const clearCart = () => setCartItems([]);

    const getQuantity = (productId: number) =>
        cartItems.find((x) => x.productId === productId)?.quantity || 0;

    const total = cartItems.reduce(
        (sum, item) => sum + (item.price ?? 0) * item.quantity,
        0
    );

    return (
        <CartContext.Provider
            value={{
                cartItems,
                addItem,
                removeItem,
                deleteItem,
                clearCart,
                getQuantity,
                total,
            }}
        >
            {children}
        </CartContext.Provider>
    );
};

export const useCart = () => {
    const ctx = useContext(CartContext);
    if (!ctx) throw new Error("useCart must be used inside CartProvider");
    return ctx;
};
