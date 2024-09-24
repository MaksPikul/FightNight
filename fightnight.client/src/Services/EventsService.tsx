import axios from "axios";
import { EventSchema } from "../Schemas";
import * as z from "zod"

const api = "https://localhost:7161/api/"

export const GetUserEvents = async (
    userId : string
) => {
    
    console.log(userId)
    console.log("i just ran")
    const res = await fetch(api + `event/user/${userId}`, {
        method: "get",
        credentials: "include"
    })
    return res.json()
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
        
    return res.json()

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

export const DeleteEvent = async (
    eventId: string
) => {

    //Delete from Key ['Event', eventId]

    try {
        const data = await axios.delete(api + `event/` + eventId, {
            withCredentials: true
        })
        return data
    }
    catch (err) {
        return err
    }
}

export const UploadEventBanner = async (
    file: File,
    eventId: string
) => {
    try {
        const data = await axios.patch(api + 'event/banner',
            {
                file,
                eventId
            },
            {
                withCredentials: true,
                headers: {
                    "Content-Type": "multipart/form-data", // Ensure that the request is sent as multipart/form-data
                }
            })
        return data
    }
    catch (err) {
        return err
    }
}