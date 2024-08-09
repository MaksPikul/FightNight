
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

export const LoginForm = () => {

    const [error, setError] = useState<string | undefined>("")
    const [success, setSuccess] = useState<string | undefined>("")
    const [isPending, startTransition] = useTransition()

    const form = useForm<z.infer<typeof LoginSchema>>({
        resolver: zodResolver(LoginSchema),
        defaultValues: {
            email: "",
            password: "",
            pin: undefined
        }
    })

    const onSubmit = (values: z.infer<typeof LoginSchema>) => {
        
        async function postData () {
            const response = await fetch("/api/auth/login",{
                method: "POST",
                body: JSON.stringify(values)
            })
            return await response.json()
        }
        setError("")
        setSuccess("")

        startTransition(()=>{
            postData()
            .then((data)=>{
                console.log(data)

                if (data?.error){
                    form.reset();
                    setError(data?.error)
                }
                if (data?.success){
                    form.reset();
                    setSuccess(data?.success)
                    //router.refresh()
                }
                /*
                if (data?.twoFactor){
                    setShow2FA(true)
                }
                
                */
            })
        })
    }

return (
    <CardWrapper
        headerLabel="Sign in to Fight Night"
        backButtonHref="/auth/register"
        backButtonLabel="Don't have an account?"
        backButtonHrefLabel="Sign up to Fight Night"
        showSocial>
        
        <Form {...form}>
            <form 
            onSubmit={form.handleSubmit(onSubmit)}
            className="space-y-6"> 

            <div>
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
                )}/>
            </div>

                <FormError message={error} />
                <FormSuccess message={success} />

            <div className="flex flex-col text-center justify-center">
                <Button
                type="submit"
                disabled={isPending}>
                    Sign In
                </Button>
                
                <a
                href="/auth/reset"
                className="text-sm font-bold underline">
                    Forgot password?
                </a>
            </div>
                    
            </form>
        </Form>
    </CardWrapper>
)
}