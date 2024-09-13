"use client"

import { useForm } from "react-hook-form"
import { zodResolver } from "@hookform/resolvers/zod"

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
import { useState, useTransition } from "react"
import { CardWrapper } from "./card-wrapper"
import { FormError } from "../Misc/formError"
import { FormSuccess } from "../Misc/formSuccess"
import { PasswordChecks } from "./PasswordChecks"
import { RegisterSchema } from "../../Schemas"

import { useAuth } from "../../Context/UseAuth"

export const RegisterForm = () => {

    const [error, setError] = useState<string | undefined>()
    const [success, setSuccess] = useState<string | undefined>()
    const [loading, setLoading] = useState(false)
    const { registerUser } = useAuth()

    const form = useForm<z.infer<typeof RegisterSchema>>({
        resolver: zodResolver(RegisterSchema),
        defaultValues: {
            name: "",
            email: "",
            password: "",
        }
    })

    async function onSubmit(values: z.infer<typeof RegisterSchema>) {
        
        setError("")
        setSuccess("")

        setLoading(true)
        const res = await registerUser(
            values.email,
            values.name,
            values.password)
        setLoading(false)

        setError(res?.error)
        setSuccess(res?.success)
    }
    

    return (
        <CardWrapper
        headerLabel="Sign up to Fight Night"
        backButtonHref="/login"
        backButtonLabel="Already have an account?"
        backButtonHrefLabel="Sign in to Fight Night"
        showSocial={false}>

        <Form {...form}>
            <form 
            onSubmit={form.handleSubmit(onSubmit)}
            className="space-y-6 ">  
            
            <div>
                <FormField 
                control={form.control}
                name="name"
                render={({ field }) => (
                    <FormItem>
                        <FormLabel>Name</FormLabel>
                        <FormControl>
                            <Input 
                            {...field}
                            disabled={loading}
                            placeholder="name"
                            
                            />
                        </FormControl>
                        <FormMessage/>
                    </FormItem>
                )}/>

                <FormField 
                control={form.control}
                name="email"
                render={({ field }) => (
                    <FormItem>
                        <FormLabel>Email</FormLabel>
                        <FormControl>
                            <Input 
                            {...field}
                            
                            disabled={loading}
                            placeholder="example@fight.com"
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
                            type="password"
                            {...field}
                            disabled={loading}
                            placeholder="******"
                            />
                        </FormControl>
                        {/*<FormMessage/>*/}
                    </FormItem>
                )}/>

                <PasswordChecks password={form.getValues().password}/>
            </div>

                <FormError message={error} />
                <FormSuccess message={success} />

            <div className="flex justify-center">
                <Button
                className=""
                disabled={loading}
                type="submit">
                    Create Account
                </Button>
            </div>

            </form>
        </Form>

        </CardWrapper>
    )
}