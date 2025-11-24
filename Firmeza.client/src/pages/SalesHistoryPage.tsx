import { useEffect, useState } from "react";
import api from "../api/axiosInstance";
import { AppLayout } from "@/layouts/AppLayout";

interface Sale {
    id: number;
    saleDate: string;
    receiptNumber: string;
    customerName: string;
    total: number;
    pdfUrl: string | null;
}

export default function SalesHistoryPage() {

    const [sales, setSales] = useState<Sale[]>([]);
    const [loading, setLoading] = useState(true);

    const user = JSON.parse(localStorage.getItem("user")!);
    const customerId = user?.customerId;

    const handleLogout = () => {
        localStorage.removeItem("jwt_token");
        navigate("/login", { replace: true });
    };

    useEffect(() => {
        const fetchSales = async () => {
            try {
                const res = await api.get(`/sales/customer/${customerId}`);
                setSales(res.data);
            } catch (err) {
                console.error(err);
            } finally {
                setLoading(false);
            }
        };

        fetchSales();
    }, []);

    return (
        <AppLayout onLogout={handleLogout}>
            <div className="max-w-2xl mx-auto mt-8">
                <h1 className="text-3xl font-bold mb-6">Historial de compras</h1>

                {loading ? (
                    <p className="text-center mt-10">Cargando historial...</p>
                ) : sales.length === 0 ? (
                    <p className="text-gray-500">No tienes compras registradas</p>
                ) : (
                    <div className="space-y-4">
                        {sales.map((sale) => (
                            <div key={sale.id} className="p-4 border rounded-lg shadow bg-white">
                                <div className="flex justify-between">
                                    <div>
                                        <p className="font-semibold">
                                            Recibo: {sale.receiptNumber}
                                        </p>

                                        <p className="text-sm text-gray-500">
                                            Fecha:{" "}
                                            {new Date(sale.saleDate).toLocaleString()}
                                        </p>

                                        <p className="font-medium mt-1">
                                            Total: $
                                            {sale.total.toLocaleString("es-CO")}
                                        </p>
                                    </div>

                                    {sale.pdfUrl ? (
                                        <a
                                            href={sale.pdfUrl}
                                            target="_blank"
                                            className="text-blue-600 underline"
                                        >
                                            Ver PDF
                                        </a>
                                    ) : (
                                        <span className="text-gray-400 text-sm">
                                            PDF no disponible
                                        </span>
                                    )}
                                </div>
                            </div>
                        ))}
                    </div>
                )}
            </div>
        </AppLayout>
    );
}
