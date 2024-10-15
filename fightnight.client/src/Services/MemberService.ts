import { EventRole } from "../Models/Event";

const api = "https://localhost:7161/api/"

export const AcceptInvite = async (
    inviteId: string
) => {
    const res = await fetch(api + "join-w-invite",
        {
            body: JSON.stringify({inviteId}),
            method: "post",
        })

    return await res.json()
}

export const AddThroughInvite = async (
    userId: string,
    allocatedRole: EventRole.Fighter | EventRole.Moderator | EventRole.Spectator,
    eventId: string
) => {
    fetch(api + "member",
        {
            body: JSON.stringify({ userId, eventId, allocatedRole }),
            method: "post",
            credentials: "include"
        }).then((res) => res.json())
}


export const SendInvite = async (
    email: string,
    allocatedRole: EventRole.Fighter | EventRole.Moderator | EventRole.Spectator,
    eventId: string
) => {
    const result = await fetch(api + "invite",
        {
            body: JSON.stringify({ email, eventId, allocatedRole }),
            method: "post",
            credentials: "include"
        })
    console.log(await result.json())
}

export const RemoveMemberFromEvent = async (
    userId: string,
    eventId: string
) => {
    fetch(api + "member",
        {
            body: JSON.stringify({ userId, eventId}),
            method: "delete",
            credentials: "include"
        }).then((res) => res.json())
}


export const GetEventMembers = async (
    eventId: string
) => {
    const res = await fetch(api + "member/" + eventId,
        {
            method: "get",
            credentials: "include"
        })
    return await res.json()
}

