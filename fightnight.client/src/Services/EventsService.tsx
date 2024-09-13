import axios from "axios";
import { EventSchema } from "../Schemas";
import * as z from "zod"

const api = "https://localhost:7161/api/"

export const GetUserEvents = async (
    userId : string
) => {
    try {
        const data = await axios.get(api + `event/user/${userId}`,{
            withCredentials: true
        });
        return data
    }
    catch (err) {
        return err
    }
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
    try {
        const data = await axios<Event>(api + `event/${eventId}`, {
            method: "get",
            withCredentials: true
        })
        console.log(data)
        return data
    }
    catch (err) {
        return err
    }
}

export const UpdateEvent = async (
    eventId: string,
    values: z.infer<typeof EventSchema>
) => {
    try {
        const data = await axios.patch(api + `event`, {id:eventId, ...values}, {
            withCredentials: true
        })
        console.log(data)
        return data
    }
    catch (err) {
        console.log("err")
        return err
    }
}
export const DeleteEvent = async (
    eventId: string
) => {
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