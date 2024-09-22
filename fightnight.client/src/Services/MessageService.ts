import { Message } from "../Models/Message"

const api = "https://localhost:7161/api/"

export const AddMessageApi = async (
    msg: string,
    eventId: string
) => {
    fetch(api + "message",
        {
            body: JSON.stringify({ msg, eventId }),
            method: "post",
            credentials: "include"
        }).then((res) => console.log(res))
}

export const DeleteMessageApi = async (
    msgId: string,
    eventId: string,
) => {
    fetch(api + "message",
        {
            body: JSON.stringify({ msgId, eventId }),
            method: "delete",
            credentials: "include"
        }).then((res) => res.json())
}

export const UpdateMessageApi = async (
    msgId: string,
    eventId: string,
    newMsg: string
) => {
    fetch(api + "message",
        {
            body: JSON.stringify({ msgId, eventId, newMsg }),
            method: "patch",
            credentials: "include"
        }).then((res) => res.json())
}

export const GetEventMessages = async (
    eventId: string,
) => {
    fetch(api + "message",
        {
            body: JSON.stringify({ eventId }),
            method: "get",
            credentials: "include"
        }).then((res) => res.json())
}