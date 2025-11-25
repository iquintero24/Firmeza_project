import { zodResolver } from "@hookform/resolvers/zod"
import { useForm } from "react-hook-form"
import * as z from "zod"
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
import api from '@/api/axiosInstance';
import { useNavigate } from 'react-router-dom';

const formSchema = z.object({
    email: z.string().email("Ingrese un correo válido."),
    password: z.string().min(6, "Mínimo 6 caracteres."),
})

export function LoginForm() {
    const navigate = useNavigate();

    const form = useForm<z.infer<typeof formSchema>>({
        resolver: zodResolver(formSchema),
        defaultValues: { email: "", password: "" },
    });

    async function onSubmit(values: z.infer<typeof formSchema>) {
        try {
            const response = await api.post('/auth/login', values);
            const token = response.data.token;
            localStorage.setItem('jwt_token', token);
            navigate('/catalogo');
        } catch (error: any) {
            alert("Error al iniciar sesión: Credenciales incorrectas.");
        }
    }

    return (
        <Form {...form}>
            <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-5">

                <FormField
                    control={form.control}
                    name="email"
                    render={({ field }) => (
                        <FormItem>
                            <FormLabel className="text-gray-700">Correo Electrónico</FormLabel>
                            <FormControl>
                                <Input
                                    placeholder="cliente@firmeza.com"
                                    className="h-12 text-base rounded-xl"
                                    {...field}
                                />
                            </FormControl>
                            <FormMessage />
                        </FormItem>
                    )}
                />

                <FormField
                    control={form.control}
                    name="password"
                    render={({ field }) => (
                        <FormItem>
                            <FormLabel className="text-gray-700">Contraseña</FormLabel>
                            <FormControl>
                                <Input
                                    type="password"
                                    placeholder="********"
                                    className="h-12 text-base rounded-xl"
                                    {...field}
                                />
                            </FormControl>
                            <FormMessage />
                        </FormItem>
                    )}
                />

                <Button
                    type="submit"
                    className="w-full h-12 rounded-xl text-base font-semibold 
                    transition-all duration-200"
                >
                    Iniciar Sesión
                </Button>
            </form>
        </Form>
    );
}
