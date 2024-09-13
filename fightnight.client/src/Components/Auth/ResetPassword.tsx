import { useState } from "react"
import { FormError } from "../Misc/formError"
import { FormSuccess } from "../Misc/formSuccess"
import { Button } from "../ui/button"
import { Form, FormControl, FormField, FormItem, FormLabel, FormMessage } from "../ui/form"
import { Input } from "../ui/input"
import { CardWrapper } from "./card-wrapper"
import { useForm } from "react-hook-form"
import { zodResolver } from "@hookform/resolvers/zod"
import { NewPasswordSchema } from "../../Schemas"
import { ChangeUserPassword } from "../../Services/AuthService"
import { PasswordChecks } from "./PasswordChecks"
import * as z from "zod"
import { useParams } from "react-router-dom"

export const ResetPassword = () => {

    const [error, setError] = useState<string | undefined>("");
    const [success, setSuccess] = useState<string | undefined>("");
    const [loading, setLoading] = useState(false)

    const { token } =  useParams()


    const form = useForm<z.infer<typeof NewPasswordSchema>>({
        resolver: zodResolver(NewPasswordSchema),
        defaultValues: {
            password: "",
            confirmPassword: ""
        }
    })

    const onSubmit = async (values: z.infer<typeof NewPasswordSchema>) => {
        setError("")
        setSuccess("")

        setLoading(true)
        if (token === undefined) {
            setError("No Reset Password Token Provided")
            return
        }
        const response = await ChangeUserPassword(values.password, token)
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

    return (
        <CardWrapper
            headerLabel="Password Reset"
            headerDescription="Enter your new password"
            backButtonHref="/login"
            backButtonHrefLabel="Back to login">
            <Form {...form}>
                <form
                    onSubmit={form.handleSubmit(onSubmit)}
                    className="space-y-6">

                    <div
                        className="space-y-4">
                        <FormField
                            control={form.control}
                            name="password"
                            render={({ field }) => (
                                <FormItem>
                                    <FormLabel>New Password</FormLabel>
                                    <FormControl>
                                        <Input
                                            {...field}
                                            disabled={loading}
                                            placeholder="********"
                                            type="password"
                                        />
                                    </FormControl>
                                    <FormMessage />
                                </FormItem>
                            )} />

                        <FormField
                            control={form.control}
                            name="password"
                            render={({ field }) => (
                                <FormItem>
                                    <FormLabel>Confirm Password</FormLabel>
                                    <FormControl>
                                        <Input
                                            {...field}
                                            disabled={loading}
                                            placeholder="********"
                                            type="password"
                                        />
                                    </FormControl>
                                    <FormMessage />
                                </FormItem>
                            )} />
                        <PasswordChecks password={form.getValues().password} />
                    </div>

                    <FormError message={error} />
                    <FormSuccess message={success} />
                    <Button
                        disabled={loading }
                        type="submit"
                        className="w-full">
                        Reset password
                    </Button>
                </form>
            </Form>
        </CardWrapper>
    )
}