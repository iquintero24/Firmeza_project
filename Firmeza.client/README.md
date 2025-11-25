# 🛒 Client App – Frontend
Aplicación cliente construida en **React + TypeScript**, enfocada en la gestión de clientes, ventas y productos. Proporciona una interfaz moderna, segura y fácil de usar para interactuar con la API del sistema.

## 📌 Características principales
- Gestión de clientes y productos desde una interfaz intuitiva.
- Actualización dinámica de precios, cantidades y totales.
- Carrito funcional con recálculo automático.
- Manejo completo de sesión de usuario y autenticación JWT.
- Componentes reutilizables con TailwindCSS + Lucide Icons.
- Integración total con la API (axios instance configurada).
- Arquitectura limpia: pages, components, hooks y services.

## 🏗️ Tecnologías utilizadas
| Área | Tecnología |
|------|------------|
| Lenguaje | TypeScript |
| Framework | React |
| Enrutamiento | React Router DOM |
| UI | TailwindCSS + Lucide React |
| Estado / Data fetching | Hooks + Context |
| HTTP Client | Axios |
| Build | Vite |

## 📂 Estructura del proyecto
client/
│── src/
│   ├── api/
│   │   └── axiosInstance.ts
│   ├── components/
│   │   ├── Navbar.tsx
│   │   └── LoginForm.tsx
│   ├── pages/
│   │   ├── LoginPage.tsx
│   │   └── EditSale.tsx
│   ├── hooks/
│   ├── context/
│   ├── types/
│   ├── App.tsx
│   └── main.tsx
│── public/
│── package.json
│── tsconfig.json
│── README.md

## ⚙️ Instalación y ejecución local
Requisitos:
- Node.js 18+

Instalar dependencias:
npm install

Configurar .env:
VITE_API_URL=http://localhost:5000/api

Ejecutar:
npm run dev

Build:
npm run build

## 🐳 Docker
Construir:
docker build -t client-app .

Ejecutar:
docker run -p 5173:80 client-app

## 🔐 Autenticación
Se usa JWT desde localStorage.authToken.

## 🔗 Integración API
Todas las llamadas pasan por src/api/axiosInstance.ts.

## 🎨 Estilos
TailwindCSS + lucide-react.
