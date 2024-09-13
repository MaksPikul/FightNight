export type Event = {
    id: string,
    title: string,
    date: Date,
    time: string,
    eventDur: string,
    venue: string,
    venueAddress: string,
    desc: string,
    type: string,
    status: number,//EventStatus
    organizer: string,
    adminId: string,
    numMatches: number,
    numRounds: number,
    roundDur: number,
    role: number,//EventRole, 
    bannerUrl: string
}

export enum EventStatus {
    Planning,
    Launched,
    Ongoing,
    Completed
}

export enum EventRole {
    Spectator,
    Moderator,
    Fighter,
    Admin
}