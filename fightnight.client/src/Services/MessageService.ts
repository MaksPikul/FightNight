const api = "https://localhost:7161/api/"

export const AddMessageApi = async (
    msg: string,
    eventId: string,
    username: string,
    picture: string
) => {
    const res = await fetch(api + "message",
        {
            body: JSON.stringify({ msg, eventId, username, picture }),
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
    pageParam: number,
) => {
    const limit = 20
    const res = await fetch(api + `message/${eventId}/${pageParam}/${limit}`,
        {
            //body: JSON.stringify({ eventId }),
            method: "get",
            credentials: "include"
        })
    const x = await res.json()
    return {
        data: x,
        currentPage: pageParam,
        nextCursor: limit === x.length ? pageParam + 1 : null
    }
}