import axios from "axios";
import { EventSchema } from "../Schemas";
import * as z from "zod"

const api = "https://localhost:7161/api/"

export const GetUserEvents = async (
) => {
    const res = await fetch(api + `event/user`, {
        method: "get",
        credentials: "include"
    })
    const x = await res.json()
    console.log( x)
    return x
}

// no need to send userId, user found in backend with cookie
export const CreateEventApi = async (title: string, date: Date) => {
    try {
        const data = await axios.post(api + "event", {
            title,
            date: date.toISOString()
        }, {
            withCredentials: true
        })
        return data
    }
    catch (err) {
        return err
    }
}

export const GetEventAndUsers = async (eventId: string) => {
    
    const res = await fetch(api + `event/${eventId}`,
        {
            method: "get",
            credentials: "include"
        })
        const x = res.json()
    console.log(x)
    return x

}

export const UpdateEvent = async (
    eventId: string,
    values: z.infer<typeof EventSchema>
) => {
    //UPDATE KEY ['Event', eventId]
    const res = await fetch(api + `event`,
        {
            body: JSON.stringify({ id: eventId, ...values }),
            headers: { 'Content-Type': 'application/json' },
            method: "patch",
            credentials: "include"
        })
    return await res.json()
}

export const DeleteEventApi = async (
    eventId: string
) => {

    const res = await fetch(api + `event/${eventId}`,
        {
            method: "delete",
            credentials: "include"
        })
    const x = res.json()
    console.log(x)
    return x
}

