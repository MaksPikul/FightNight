import { EventRole } from "./Event"

export type UserProfileToken = {
    email: string,
    username: string,
    token: string, 
    redirectUrl: string
}

export type UserProfile = {
    userId: string,
    userName: string,
    email: string,
    picture: string,
    isAuthed: boolean,
    role: string
}

export type UserEventProfile = {
    userId: string,
    username: string,
    email: string,
    picture: string,
    role: EventRole
}