import { createContext, useContext, useState } from "react";

// ==================
// 🛒 Tipos
// ==================
export interface CartItem {
    productId: number;
    name: string;     
    price: number;    
    quantity: number;
}

interface CartContextType {
    cartItems: CartItem[];
    addItem: (productId: number, delta?: number, info?: { name: string; price: number }) => void;
    removeItem: (productId: number) => void;
    deleteItem: (productId: number) => void;
    clearCart: () => void;
    getQuantity: (productId: number) => number;
    total: number;
}

// ==================
// 🧠 Contexto
// ==================
const CartContext = createContext<CartContextType | null>(null);

// ==================
// 🛍 Provider
// ==================
export const CartProvider = ({ children }: { children: React.ReactNode }) => {
    const [cartItems, setCartItems] = useState<CartItem[]>([]);

    // -------------------------
    // ➕ Agregar Items
    // -------------------------
    const addItem = (
        productId: number,
        delta: number = 1,
        info?: { name: string; price: number }
    ) => {
        setCartItems((prev) => {
            const existing = prev.find((x) => x.productId === productId);

            const newQty = (existing?.quantity ?? 0) + delta;

            if (newQty <= 0) {
                return prev.filter((x) => x.productId !== productId);
            }

            // Si ya existe → solo cambia cantidad
            if (existing) {
                return prev.map((x) =>
                    x.productId === productId
                        ? { ...x, quantity: newQty }
                        : x
                );
            }

            // Si no existe, se agrega (info ES OBLIGATORIA)
            if (!info) {
                console.error("addItem requires product info for new items");
                return prev;
            }

            return [
                ...prev,
                {
                    productId,
                    quantity: newQty,
                    name: info.name,
                    price: info.price,
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
        cartItems.find((x) => x.productId === productId)?.quantity ?? 0;

    const total = cartItems.reduce(
        (sum, item) => sum + item.price * item.quantity,
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

// ==================
// 🎣 Hook
// ==================
export const useCart = () => {
    const ctx = useContext(CartContext);
    if (!ctx) throw new Error("useCart must be used inside CartProvider");
    return ctx;
};
