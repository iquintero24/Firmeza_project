import { zodResolver } from "@hookform/resolvers/zod"
import { useForm } from "react-hook-form"
import * as z from "zod"
// NOTA: Asumo que los componentes Button, Form, FormControl, etc., están disponibles en tu entorno (shadcn/ui)
import { Button } from "@/components/ui/button"
import {
    Form,
    FormControl,
    FormField,
    FormItem,
    FormLabel,
    FormMessage,
} from "@/components/ui/form"
import { Input } from "@/components/ui/input"
import api from '../api/axiosInstance'; // 👈 Importación corregida
import { useNavigate } from 'react-router-dom';
import { useState } from 'react';
import { Loader2 } from 'lucide-react';

// 1. Esquema de Validación con Zod
const formSchema = z.object({
    name: z.string().min(2, "El nombre debe tener al menos 2 caracteres."),
    document: z.string().min(6, "El documento debe tener al menos 6 dígitos."),
    email: z.string().email("Ingrese un correo electrónico válido."),
    phone: z.string().min(7, "El teléfono debe tener al menos 7 dígitos."),
    password: z.string().min(6, "La contraseña debe tener al menos 6 caracteres."),
    confirmPassword: z.string().min(6, "Debe confirmar la contraseña."),
}).refine(data => data.password === data.confirmPassword, {
    message: "Las contraseñas no coinciden.",
    path: ["confirmPassword"], // Indica dónde mostrar el error
});

// Tipo derivado del esquema
type RegisterFormValues = z.infer<typeof formSchema>;

export function RegisterPage() {
    const navigate = useNavigate();
    const [apiError, setApiError] = useState('');
    const [successMessage, setSuccessMessage] = useState('');

    const form = useForm<RegisterFormValues>({
        resolver: zodResolver(formSchema),
        defaultValues: {
            name: "",
            document: "",
            email: "",
            phone: "",
            password: "",
            confirmPassword: "",
        },
    });

    const isLoading = form.formState.isSubmitting;

    async function onSubmit(values: RegisterFormValues) {
        setApiError('');
        setSuccessMessage('');

        // Prepara el payload para la API
        const payload = {
            name: values.name,
            document: values.document,
            email: values.email,
            phone: values.phone,
            password: values.password,
            confirmPassword: values.confirmPassword,
        };

        try {
            // Llama al endpoint de registro
            await api.post('/Auth/register', payload);

            setSuccessMessage('¡Registro exitoso! Serás redirigido para iniciar sesión.');

            // Redirigir al login después de un breve mensaje de éxito
            setTimeout(() => {
                navigate('/login');
            }, 2000);

        } catch (error: any) {
            console.error('Error de registro:', error);
            const errorMessage = error.response?.data?.message ||
                error.response?.data?.error ||
                'Ocurrió un error al intentar registrarte. Por favor, verifica los datos.';
            setApiError(errorMessage);
        }
    }

    return (
        <div className="flex justify-center items-center min-h-screen min-w-screen bg-gray-100 p-4">
            <div className="bg-white shadow-2xl rounded-xl p-10 w-full max-w-2xl transition-all duration-300">
                <h2 className="text-3xl font-bold text-center mb-10 text-gray-800">
                    Crear Cuenta
                </h2>

                <Form {...form}>
                    <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-4">

                        {/* ⚠️ Mensaje de Éxito */}
                        {successMessage && (
                            <div className="bg-green-100 border border-green-400 text-green-700 px-4 py-3 rounded-xl relative text-center font-semibold" role="alert">
                                {successMessage}
                            </div>
                        )}

                        {/* ⚠️ Mensaje de Error de API/Backend */}
                        {apiError && (
                            <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded-xl relative" role="alert">
                                <span className="block sm:inline">{apiError}</span>
                            </div>
                        )}

                        {/* --- Campos del Formulario --- */}
                        <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                            <FormField
                                control={form.control}
                                name="name"
                                render={({ field }) => (
                                    <FormItem>
                                        <FormLabel className="text-gray-700">Nombre Completo</FormLabel>
                                        <FormControl><Input placeholder="John Doe" className="h-10 rounded-xl" {...field} /></FormControl>
                                        <FormMessage />
                                    </FormItem>
                                )}
                            />
                            <FormField
                                control={form.control}
                                name="document"
                                render={({ field }) => (
                                    <FormItem>
                                        <FormLabel className="text-gray-700">Documento/Cédula</FormLabel>
                                        <FormControl><Input placeholder="1020304050" className="h-10 rounded-xl" {...field} /></FormControl>
                                        <FormMessage />
                                    </FormItem>
                                )}
                            />
                        </div>

                        <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                            <FormField
                                control={form.control}
                                name="email"
                                render={({ field }) => (
                                    <FormItem>
                                        <FormLabel className="text-gray-700">Correo Electrónico</FormLabel>
                                        <FormControl><Input type="email" placeholder="user@example.com" className="h-10 rounded-xl" {...field} /></FormControl>
                                        <FormMessage />
                                    </FormItem>
                                )}
                            />
                            <FormField
                                control={form.control}
                                name="phone"
                                render={({ field }) => (
                                    <FormItem>
                                        <FormLabel className="text-gray-700">Teléfono</FormLabel>
                                        <FormControl><Input placeholder="3001234567" className="h-10 rounded-xl" {...field} /></FormControl>
                                        <FormMessage />
                                    </FormItem>
                                )}
                            />
                        </div>

                        <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                            <FormField
                                control={form.control}
                                name="password"
                                render={({ field }) => (
                                    <FormItem>
                                        <FormLabel className="text-gray-700">Contraseña</FormLabel>
                                        <FormControl><Input type="password" placeholder="********" className="h-10 rounded-xl" {...field} /></FormControl>
                                        <FormMessage />
                                    </FormItem>
                                )}
                            />
                            <FormField
                                control={form.control}
                                name="confirmPassword"
                                render={({ field }) => (
                                    <FormItem>
                                        <FormLabel className="text-gray-700">Confirmar Contraseña</FormLabel>
                                        <FormControl><Input type="password" placeholder="********" className="h-10 rounded-xl" {...field} /></FormControl>
                                        <FormMessage />
                                    </FormItem>
                                )}
                            />
                        </div>

                        <Button
                            type="submit"
                            disabled={isLoading || !!successMessage}
                            className="w-full h-12 rounded-xl text-base font-semibold transition-all duration-200 mt-6 bg-indigo-600 hover:bg-indigo-700"
                        >
                            {isLoading ? (
                                <><Loader2 className="mr-2 h-4 w-4 animate-spin" /> Registrando...</>
                            ) : (
                                'Crear Cuenta'
                            )}
                        </Button>
                    </form>
                </Form>

                <p className="text-center text-sm text-gray-500 pt-6 border-t mt-6 border-gray-100">
                    ¿Ya tienes cuenta? <a href="/login" className="text-indigo-600 hover:text-indigo-500 font-semibold transition duration-150">Inicia sesión aquí</a>
                </p>
            </div>
        </div>
    );
}