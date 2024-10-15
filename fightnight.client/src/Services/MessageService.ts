const api = "https://localhost:7161/api/"

export const AddMessageApi = async (
    msg: string,
    eventId: string,
    username: string,
    userPicture: string
) => {
    const res = await fetch(api + "message",
        {
            body: JSON.stringify({ msg, eventId, username, userPicture }),
            method: "post",
            headers: {
                'Content-Type': 'application/json'
            },
            credentials: "include"
        })
    const x = await res.json()
    return x
}

export const DeleteMessageApi = async (
    msgId: string,
    eventId: string,
) => {
    const res = await fetch(api + "message",
        {
            body: JSON.stringify({ msgId, eventId }),
            method: "delete",
            headers: {
                'Content-Type': 'application/json'
            },
            credentials: "include"
        })
    //const x = await res.json()
    //console.log(x)
    return res
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
    return await res.json()
}

export const GetEventMessagesApi = async (
    eventId: string,
    offset: number,
) => {
    const limit = 50
    const res = await fetch(api + `message/${eventId}/${offset}/${limit}`,
        {
            //body: JSON.stringify({ eventId }),
            method: "get",
            credentials: "include"
        })
    const x = await res.json()
    return x
}