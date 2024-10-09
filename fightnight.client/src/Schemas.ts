import * as z from "zod"
import { EventRole } from "./Models/Event"

export const LoginSchema = z.object({
    email: z.string()
        .email({message: "Email is required"}),
    password: z.string()
        .min(1, {message: "Password is required"}),
    pin: z.optional(z.string()),
    rememberMe: z.boolean()
})

export const RegisterSchema = z.object({
    name: z.string()
        .min(3,{message: "Minimum name length - 3 characters"})
        .max(20,{message: "Maximum name length - 20 characters"}),
    email: z.string()
        .email({message: "Email is required"}),
    password: z.string()
        .min(8, {message: "Minimum password length - 8 Chars"})
        .max(64, {message:"Maximum password length - 64 Chars" })
        .regex(/[a-z]/, 'Password must contain at least one lowercase letter')
        .regex(/[A-Z]/, 'Password must contain at least one uppercase letter')
        .regex(/[0-9]/, 'Password must contain at least one number')
        .regex(/[@$!%*?&]/, 'Password must contain at least one special character')
})


export const EventSchema = z.object({
    title: z.string().min(1, "Title is required"),
    date: z.date(),
    time: z.string().min(1, "Time is required"), 
    venueAddress: z.string().min(1, "Venue Address is required"),
    desc: z.string(),
    numMatches: z.string().min(1, "Field is required"),
    numRounds: z.string().min(1, "Field is required"),
    roundDur: z.string().min(1, "Field is required"),
})

export const CreateEventSchema = z.object({
    title: z.string()
        .min(3, { message: "Minimum Title Length - 3 Characters" }),
    date: z.date(),
})


export const EnterEmailSchema = z.object({
    email: z.string().email({
        message: "Email is required"
    })
})

export const NewPasswordSchema = z.object({
    password: z.string()
        .min(8, { message: "Minimum password length - 8 Chars" })
        .max(64, { message: "Maximum password length - 64 Chars" })
        .regex(/[a-z]/, 'Password must contain at least one lowercase letter')
        .regex(/[A-Z]/, 'Password must contain at least one uppercase letter')
        .regex(/[0-9]/, 'Password must contain at least one number')
        .regex(/[@$!%*?&]/, 'Password must contain at least one special character'),
    confirmPassword: z.string()
}).refine(data => data.password === data.confirmPassword, {
    message: "Passwords don't match",
    path:["confimPassword"]
})


export const MessageSchema = z.object({
    message: z.string()
})


export const InviteSchema = z.object({
    email: z.string().email({ message: "Email is required" }),
    role: z.number()
})