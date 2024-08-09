import * as z from "zod"

export const LoginSchema = z.object({
    email: z.string()
        .email({message: "Email is required"}),
    password: z.string()
        .min(1, {message: "Password is required"}),
    pin: z.optional(z.string()),
})

export const RegisterSchema = z.object({
    name: z.string()
        .min(3,{message: "Minimum name length - 3 characters"})
        .max(20,{message: "Maximum name length - 20 characters"}),
    email: z.string()
        .email({message: "Email is required"}),
    password: z.string()
        .min(6, {message: "Minimum password length - 6 Chars"})
        .max(64, {message:"Maximum password length - 64 Chars" })
        .regex(/[a-z]/, 'Password must contain at least one lowercase letter')
        .regex(/[A-Z]/, 'Password must contain at least one uppercase letter')
        .regex(/[0-9]/, 'Password must contain at least one number')
        .regex(/[@$!%*?&]/, 'Password must contain at least one special character')
})
