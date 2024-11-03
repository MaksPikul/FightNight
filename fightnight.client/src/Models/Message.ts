export type Message = {

    id?: string,
    eventId: string,
    userId: string,
    username: string,
    message: string,
    picture: string,
    timeStamp?: Date,
    isEdited?: boolean,
    isLoading: boolean

}