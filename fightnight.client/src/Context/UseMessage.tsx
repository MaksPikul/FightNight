import { createContext, useContext, useEffect, useState } from "react"
import { Message } from "../Models/Message"
import { AddMessageApi, DeleteMessageApi, UpdateMessageApi } from "../Services/MessageService"
import { useToast } from "../Components/ui/use-toast"
import { useAuth } from "./UseAuth"

type MessageContextType = {
    messages: Message[] | null,
    SendMessage: (
        msg: string,
        eventId: string
    ) => void,
    DeleteMessage: (
        msgId: string,
        eventId: string
    ) => void,
    EditMessage: (
        msgId: string,
        eventId: string,
        newMsg: string
    ) => void,
    UpdateMessages: (
        eventId: string
    ) => void,
    GetMessages: (
        eventId: string
    ) => void
}

interface Props {
    children: React.ReactNode,
}

const MessageContext = createContext<MessageContextType>({} as MessageContextType)

export const MessageProvider = ({ children }: Props) => {
    const [messages, setMessages] = useState<Message[] | null>([]);
    const { user } = useAuth()


    const { toast } = useToast()


   
    const SendMessage = async (
        msg: string,
        eventId: string
    ) => {
        // Temporarily add message for user 
        const temp = messages
        setMessages((prev) => [...prev, {username:user?.username, message: msg, timeStamp:Date.now }])

        const res = await AddMessageApi(msg, eventId)
        if (res?.data) {
            //msg added to database, other clients alerted of message, temp message can be safetly added
            setMessages([...temp, res?.data])
        }
        else if (res?.response) {
            //msg not added, clients not alerted, remove temp message
            setMessages(temp)

            toast({
                title: "Error",
                description: res.response.data,
            })
            return res.response.data
        }
        else {
            toast({
                title: "Error",
                description: "App Broken.",
            })
        }
    }

    const DeleteMessage = async (
        msgId: string,
        eventId: string
    ) => {
        const res = await DeleteMessageApi(msgId, eventId)
        if (res?.data) {
            //msg added to database, other clients alerted of message, temp message can be safetly added



        }
        else if (res?.response) {
            //msg not deleted, clients not alerted, add back message

            toast({
                title: "Error",
                description: res.response.data,
            })
            return res.response.data
        }
        else {
            toast({
                title: "Error",
                description: "App Broken.",
            })
        }
    }

    const EditMessage = async (
        msgId: string,
        eventId: string,
        newMsg: string
    ) => {
        const res = await UpdateMessageApi(msgId, eventId, newMsg)
        if (res?.data) {
            //msg added to database, other clients alerted of message, temp message can be safetly added



        }
        else if (res?.response) {
            //msg not deleted, clients not alerted, add back message

            toast({
                title: "Error",
                description: res.response.data,
            })
            return res.response.data
        }
        else {
            toast({
                title: "Error",
                description: "App Broken.",
            })
        }
    }



    const UpdateMessages = (
        eventId: string
    ) => {
        //Message sent from other user, add it ontop of list of messages
        return null
    }

    const GetMessages = (
        eventId: string
    ) => {
        // Check cache,
        //
        return null
    }

    return (
        <MessageContext.Provider
            value={{
                messages, //might need to remove from here and add to a state cache
                SendMessage,
                DeleteMessage,
                EditMessage,
                UpdateMessages,
                GetMessages
            }}>
            {children}
        </MessageContext.Provider>
    )
}


export const useMessage = () => useContext(MessageContext);