export type Message = {

    id?: string,
    eventId: string,
    userId: string,
    username: string,
    message: string,
    timeStamp?: Date,
    isEdited?: boolean,
    isLoading: boolean

}