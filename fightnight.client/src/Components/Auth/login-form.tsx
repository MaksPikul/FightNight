
import { useForm } from "react-hook-form"
import { zodResolver } from "@hookform/resolvers/zod"
import { LoginSchema } from "../../Schemas"
import * as z from "zod"
import { Button } from "../ui/button"
//import { useRouter } from "next/navigation"
import {
    Form,
    FormControl,
    FormField,
    FormItem,
    FormLabel,
    FormMessage,
} from "../ui/form"

import { Input } from "../ui/input"
//import { signIn } from "next-auth/react"
//import Google from "next-auth/providers/google"
import { CardWrapper } from "./card-wrapper"
import { useState, useTransition } from "react"
import { FormError } from "../Misc/formError"
import { FormSuccess } from "../Misc/formSuccess"
import { getGoogleOAuthURL } from "../../Services/AuthService"
import { useAuth } from "../../Context/UseAuth"
import { Checkbox } from "../ui/checkbox"
import { useParams } from "react-router-dom"
//import { useNavigate } from "react-router-dom"

export const LoginForm = () => {

    const [error, setError] = useState<string | undefined>("")
    const [success, setSuccess] = useState<string | undefined>("")

    const [isPending, startTransition] = useTransition()
    const { loginUser } = useAuth()
    const { inviteId } = useParams()


    const form = useForm<z.infer<typeof LoginSchema>>({
        resolver: zodResolver(LoginSchema),
        defaultValues: {
            email: "",
            password: "",
            pin: undefined,
            rememberMe: true
        }
    })

    const onSubmit = async (values: z.infer<typeof LoginSchema>) => {

        setError("")
        const errMsg = await loginUser(
            values.email,
            values.password,
            values.rememberMe,
            inviteId
        )
        setError(errMsg)
    }

return (
    <CardWrapper
        
        headerLabel="Sign in to Fight Night"
        backButtonHref="/register"
        backButtonLabel="Don't have an account?"
        backButtonHrefLabel="Sign up to Fight Night"
        showSocial>
        
        <Form {...form}>
            <form 
            onSubmit={form.handleSubmit(onSubmit)}
            className="space-y-4"> 
                
                <FormField 
                control={form.control}
                name="email"
                render={({ field }) => (
                    <FormItem>
                        <FormLabel>Email</FormLabel>
                        <FormControl>
                            <Input 
                            {...field}
                            disabled={isPending}
                            placeholder="example@example.com"
                            type="email"
                            />
                        </FormControl>
                        <FormMessage/>
                    </FormItem>
                )}/>

                <FormField 
                control={form.control}
                name="password"
                render={({ field }) => (
                    <FormItem>
                        <FormLabel>Password</FormLabel>
                        <FormControl>
                            <Input 
                            {...field}
                            disabled={isPending}
                            placeholder="******"
                            type="password"
                            />
                        </FormControl>
                        
                        
                        <FormMessage/>
                    </FormItem>
                )} />

                <FormField
                control={form.control}
                name="rememberMe"
                render={({ field }) => (
                    <FormItem
                    className="space-x-2 ">
                        <FormLabel>Remember Me?</FormLabel>
                        <FormControl>
                            <Checkbox
                            checked={field.value}
                            onCheckedChange={field.onChange} />
                        </FormControl>
                        <FormMessage />
                    </FormItem>
                )} />
            

                <FormError message={error} />
                <FormSuccess message={success} />

            <div className="flex flex-col text-center justify-center">
                    <Button
                        
                type="submit"
                disabled={isPending}>
                    Sign In
                </Button>
                
                <a
                href="/forgot-password"
                className="text-sm font-bold underline">
                    Forgot password?
                </a>
            </div>
                    
            </form>
        </Form>

    </CardWrapper>
)
}