import { useForm } from "react-hook-form"
import { CardWrapper } from "./card-wrapper"
import { Form, FormControl, FormField, FormItem, FormLabel, FormMessage } from "../ui/form"
import { Input } from "../ui/input"
import { FormError } from "../Misc/formError"
import { FormSuccess } from "../Misc/formSuccess"
import { Button } from "../ui/button"
import { useState } from "react"
import { EnterEmailSchema } from "../../Schemas"
import * as z from "zod"
import { zodResolver } from "@hookform/resolvers/zod"
import { SendForgotPasswordEmail } from "../../Services/EmailService"

export const ForgotPassword = () => {

    const [error, setError] = useState<string | undefined>("");
    const [success, setSuccess] = useState<string | undefined>("");
    const [loading,setLoading] = useState(false)


    const onSubmit = async (values: z.infer<typeof EnterEmailSchema>) => {
        setError("")
        setSuccess("")

        setLoading(true)
        const response = await SendForgotPasswordEmail(values.email)
        setLoading(false)

        if (response.data) {
            setError(response.data)
        }
        else if (response.response.data) {
            setError(response.response.data)
        }
        else {
            setError("BIG BAD")
        }
    }

    const form = useForm<z.infer<typeof EnterEmailSchema>>({
        resolver: zodResolver(EnterEmailSchema),
        defaultValues: {
            email: "",
        }
    })

    return (
        <CardWrapper
            headerLabel="Change password"
            backButtonHref="/login"
            backButtonHrefLabel="Back to login"
            backButtonLabel=""
            >
            <Form {...form}>
                <form
                    onSubmit={form.handleSubmit(onSubmit)}
                    className="space-y-6">
                                    
                    {success ?
                    <FormSuccess message={success} />
                    :
                    <div
                    className="space-y-4">
                    <FormField
                        control={form.control}
                        name="email"
                        render={({ field }) => (
                            <FormItem>
                                <FormLabel>Email</FormLabel>
                                <FormControl>
                                    <Input
                                        disabled={loading }
                                        {...field}
                                        placeholder="example@example.com"
                                        type="email"
                                    />
                                </FormControl>
                                <FormMessage />
                            </FormItem>
                        )} />
                    </div>}
                    
                    <FormError message={error} />
                    
                    <Button
                        disabled={loading }
                        type="submit"
                        className="w-full">
                        Send reset email
                    </Button>
                </form>
            </Form>
        </CardWrapper>
        )
}
