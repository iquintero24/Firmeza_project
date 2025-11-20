import { Navbar } from "@/components/Navbar";

export function AppLayout({children, onLogout,}: {
    children: React.ReactNode;
    onLogout?: () => void;
}) {
    return (
        <div className="min-h-screen bg-gray-50">
            <Navbar onLogout={onLogout} />
            <main className="max-w-7xl mx-auto px-6 py-10">{children}</main>
        </div>
    );
}
