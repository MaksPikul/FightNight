import { useAuth } from "../../../Context/UseAuth"
import { useMessage } from "../../../Context/UseMessage"
import { Event } from "../../../Models/Event"
import { Message } from "../../../Models/Message"
import { Button } from "../../ui/button"
import { Card } from "../../ui/card"
import { ChatInput } from "./Messages/ChatInput"
import { ChatMessages } from "./Messages/ChatMessages"
import { ChatNav } from "./ChatNav"
import { HubConnection, HubConnectionBuilder , LogLevel} from '@microsoft/signalr'
import { useEffect, useState } from "react"
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { AddMessageApi, DeleteMessageApi, GetEventMessagesApi, UpdateMessageApi } from "../../../Services/MessageService"




interface EventChatProps {
    eventId: string
}


export const EventChat = ({
    eventId
}: EventChatProps) => {

    const [connection, setConnection] = useState<HubConnection>()
    //const [messages, setMessages] = useState<Message[]>([]);
    //const [event, setEvent] = useState<Event>();
    const { user } = useAuth()
    const queryClient = useQueryClient()

    const { isPending, error, data: messages, status } = useQuery({
        queryKey: ["Messages", eventId],
        queryFn: () => GetEventMessagesApi(eventId)
    })

    const addMsgMutation = useMutation({
        mutationFn: (
            msg: string,
        ) => AddMessageApi(msg, eventId, user?.userName, "user?.picture"),
        onSettled: async () => {
            
            //return await queryClient.invalidateQueries({ queryKey: ["Messages", eventId] })
        },
    })

    useEffect(() => {
        const conn = new HubConnectionBuilder()
            .withUrl("https://localhost:7161/chathub")
            .withAutomaticReconnect()
            .configureLogging(LogLevel.Information)
            .build();
        setConnection(conn);
    }, [])

    useEffect(() => {
        console.log("re-runs")
        if (connection) {
            connection.on("ConnectionRes", (message) => {

                console.log(message)
            });
            connection.on("SendMsgRes", (newMsg) => {

                
                queryClient.setQueryData(
                    ["Messages", eventId],
                    (oldMsgs: Message[]) => {
                    return [...oldMsgs, newMsg]
                })
                
                console.log(newMsg)

            });
            connection.on("EditMsgRes", (message) => {

                queryClient.setQueryData(
                    ["Messages", eventId],
                    (oldMsgs: Message[]) => {
                       return oldMsgs.map((oldMsg) => {
                            return oldMsg.id === message.id ? message : oldMsg
                        })
                    })

                console.log(message)
            });
            connection.on("DeleteMsgRes", (messageId) => {

                
                queryClient.setQueryData(
                    ["Messages", eventId],
                    (oldMsgs: Message[]) => {
                        return oldMsgs.filter((msg) => msg.id !== messageId)
                    })
                console.log(messageId)
            });
            connection.start()
                .then(() => {
                    connection.invoke("ConnectionReq",
                        {
                            userId: user?.userId,
                            eventId: eventId
                        })
                })
            
            }

            
        return () => {
            //connection?.invoke("DisconnectReq", roomName);
            connection?.stop();
        }
        
    }, [connection])




    if (isPending) return 'Loading...'

    if (error) return error.message
    
    if (status === "success") 
        return (
            <Card className="flex flex-col bg-red-800 h-[700px] w-[700px] p-2 gap-y-2">
                {/*<ChatNav />*/}
                <ChatMessages
                    addMsgMutation={addMsgMutation}
                    messages={messages}/>
                <ChatInput
                    mutate={addMsgMutation.mutate}/>
            </Card>
        )
}