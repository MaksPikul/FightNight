const api = "https://localhost:7161/api/"

export const AddMessageApi = async (
    msg: string,
    eventId: string,
    userId: string,
    username: string,
    userPicture: string
) => {
    fetch(api + "message",
        {
            body: JSON.stringify({ msg, eventId, userId, username, userPicture }),
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
    const res = await fetch(api + "message",
        {
            body: JSON.stringify({ msgId, eventId, newMsg }),
            method: "patch",
            credentials: "include"
        })
    return res
}

export const GetEventMessagesApi = async (
    eventId: string,
) => {
    const res = await fetch(api + `message/${eventId}`,
        {
            //body: JSON.stringify({ eventId }),
            method: "get",
            credentials: "include"
        })
    return res
}