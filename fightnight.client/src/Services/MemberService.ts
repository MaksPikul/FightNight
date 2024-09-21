import { EventRole } from "../Models/Event";

const api = "https://localhost:7161/api/"

export const AddMemberToEvent = async (
    userId: string,
    allocatedRole: EventRole.Fighter | EventRole.Moderator | EventRole.Spectator,
    eventId: string
) => {
    fetch(api + "member",
        {
            body: JSON.stringify({userId, eventId, allocatedRole}),
            method: "post",
            credentials: "include"
        }).then((res) => res.json())
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
    fetch(api + "member",
        {
            body: JSON.stringify({ eventId }),
            method: "get",
            credentials: "include"
        }).then((res) => res.json())
}

